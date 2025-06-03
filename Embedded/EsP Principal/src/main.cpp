#include <Arduino.h>
#include <ArduinoWebsockets.h>
#include <ArduinoJson.h>
#include <WiFi.h>
#include <vector>
#include <BluetoothSerial.h>
#include <time.h>
#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

using namespace websockets;

#define LOCK_PIN 2
// WiFi credentials
const char* ssid = "SALA_LECTURA_ETAJ_3";
const char* password = "";

BLECharacteristic* pCharacteristic = nullptr;
// WebSocket server
WebsocketsServer server;
struct ClientInfo {
    WebsocketsClient client;
    int clientId; // or String, or any unique identifier
};

std::vector<ClientInfo> clients;

//Websocket Client
WebsocketsClient Bclient;
const char* external_ws_host = "192.168.100.168"; // Change to your external server IP
const uint16_t external_ws_port = 5033;         // Change to your external server port
String external_ws_url = String("ws://") + external_ws_host + ":" + String(external_ws_port) + "/ws";

unsigned long lastSend = 0;
const unsigned long sendInterval = 10000;
//Json variables
int id = 0;
float temperature1 = 0, humidity1 = 0, temperature2 = 0, humidity2 = 0;
bool ledstate = false;
float wantedTemperature = 30.0; // Default value
bool heaterOn = false;
const int HEATER_CLIENT_ID = 5; // Set this to your new client's ID

//Bluetooth setup

unsigned long unlockTime = 0;
bool isUnlocked = false;


// For change detection
float lastTemperature1 = 0, lastHumidity1 = 0, lastTemperature2 = 0, lastHumidity2 = 0;
bool lastLedstate = false;
bool lastIsUnlocked = false;

void sendToClient(int targetId, const String& message) {
    for (auto& c : clients) {
        if (c.clientId == targetId) {
            c.client.send(message);
            Serial.println("Sent to client ID " + String(targetId) + ": " + message);
            break;
        }
    }
}
bool isHeaterClientConnected() {
    for (const auto& c : clients) {
        if (c.clientId == HEATER_CLIENT_ID) {
            return true;
        }
    }
    return false;
}
void checkHeaterLogic() {
   
    if (isHeaterClientConnected()) {
        if (!heaterOn && temperature1 < wantedTemperature - 0.5) {
            sendToClient(HEATER_CLIENT_ID, "heater_on");
            heaterOn = true;
            Serial.println("Heater ON (temperature low)");
        } else if (heaterOn && temperature1 > wantedTemperature + 0.5) {
            sendToClient(HEATER_CLIENT_ID, "heater_off");
            heaterOn = false;
            Serial.println("Heater OFF (temperature high)");
        }
    }
}
void sendLockState(bool unlocked) {
    StaticJsonDocument<64> doc;
    doc["lockState"] = unlocked ? "unlocked" : "locked";
    String json;
    serializeJson(doc, json);
    Bclient.send(json);
    Serial.println("Sent lock state: " + json);
}


class MyCallbacks: public BLECharacteristicCallbacks {
    void onWrite(BLECharacteristic *pCharacteristic) {
      std::string value = pCharacteristic->getValue();
      if (value == "unlock") {
        digitalWrite(LOCK_PIN, HIGH);
        isUnlocked = true;
        unlockTime = millis();
        sendLockState(true);
        Serial.println("Unlocked via BLE!");
      }
    }
};
class MyServerCallbacks : public BLEServerCallbacks {
    void onConnect(BLEServer* pServer, esp_ble_gatts_cb_param_t *param) {
        char macStr[18];
        sprintf(macStr, "%02X:%02X:%02X:%02X:%02X:%02X",
            param->connect.remote_bda[0], param->connect.remote_bda[1], param->connect.remote_bda[2],
            param->connect.remote_bda[3], param->connect.remote_bda[4], param->connect.remote_bda[5]);
        Serial.print("BLE client connected: ");
        Serial.println(macStr);
        // You can store or use macStr as a "token" for this session
    }
    void onDisconnect(BLEServer* pServer) {
      Serial.println("BLE client disconnected");
      BLEDevice::getAdvertising()->start(); // Restart advertising
      Serial.println("Restarted BLE advertising");
    }
};
void BluetoothSetup() {
  BLEDevice::init("ESP32_Doorbell");
  BLEServer *pServer = BLEDevice::createServer();
  pServer->setCallbacks(new MyServerCallbacks()); // <-- Add this line
  BLEService *pService = pServer->createService("12345678-1234-1234-1234-1234567890ab");
  pCharacteristic = pService->createCharacteristic(
                      "abcdefab-1234-1234-1234-abcdefabcdef",
                      BLECharacteristic::PROPERTY_READ   |
                      BLECharacteristic::PROPERTY_WRITE
                    );
  pCharacteristic->setValue("Hello BLE");
  pService->start();
  BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
  pAdvertising->start();
  Serial.println("BLE service started!");
}
void onMessageCallback(WebsocketsMessage message, ClientInfo* sender) {
  String msg = message.data();
  //Serial.println("Received: " + msg);

  // Parse JSON
  StaticJsonDocument<128> doc;
  DeserializationError error = deserializeJson(doc, msg);

  if (!error) {
        int incomingId = doc["EspId"] | -1;
        if (sender->clientId == -1 && incomingId != -1) {
            sender->clientId = incomingId;
            Serial.println("Assigned client ID: " + String(incomingId));
        }
        if (doc.containsKey("HeaterState")) {
        heaterOn = (bool)doc["HeaterState"];
        //
        checkHeaterLogic(); // Check heater logic based on the received state
        
}
        else
        id = doc["Camera"] | 0;
    switch (id)
    {
    case 1:
      if (doc["temperature"] && doc["humidity"]) {
        temperature1 = doc["temperature"];
        humidity1 = doc["humidity"];
        checkHeaterLogic(); // Check heater logic based on the received temperature
      }
      else if(doc["ledstate"])
      ledstate=doc["ledstate"];
      else if(doc["command"])
      {
        if(doc["command"] == "led_change") {
          ledstate = !ledstate;
          if(ledstate) {
            sendToClient(3, "led_on");
            Serial.println("LED ON");
            ledstate = true; // Update the ledstate variable
          } else {
            sendToClient(3, "led_off"); // LED OFF
            Serial.println("LED OFF");
            ledstate = false; // Update the ledstate variable
          }
          
          
        }
      }
      break;
    
    case 2:
      if (doc["temperature"] && doc["humidity"]) {
        temperature2 = doc["temperature"];
        humidity2 = doc["humidity"];
      }
      break;
    default:Serial.println("Unknown camera ID: " + String(id));
      break;
    }
   
    
    
    // Find the client with id -1 and update it
    
  } else {
    Serial.println("Failed to parse JSON!");
  }
  String response;
  serializeJson(doc, response);
  Serial.println("Sending response: " + response);

}
void TimeConfig(){
  
    configTime(3 * 3600, 0, "pool.ntp.org"); // GMT+2, adjust for your timezone
    Serial.println("Waiting for NTP time sync...");
    time_t now = time(nullptr);
    while (now < 8 * 3600 * 2) {
      delay(500);
      Serial.print(".");
      now = time(nullptr);
    }
    Serial.println("\nTime synchronized!");
}


void webSocketEvent(const char* payload, size_t length) {
  String msg = String(payload).substring(0, length);
  Serial.println("Received via WebSocket: " + msg);
  
  if (msg == "led_on") {
    
    sendToClient(3, "led_on");
    ledstate = true; // Update the ledstate variable
  } else if (msg == "led_off") {
    
    sendToClient(3, "led_off");
    ledstate = false; // Update the ledstate variable
  }
  else if(msg == "unlock") {
    digitalWrite(LOCK_PIN, HIGH); // Unlock (LED ON)
    isUnlocked = true;
    unlockTime = millis();
    sendLockState(true); // Send unlocked state
    Serial.println("Lock UNLOCKED (LED ON)");
  }
  else if (msg.startsWith("WantedTemperature:")) {
    int sep = msg.indexOf(':');
    if (sep > 0) {
      float temp = msg.substring(sep + 1).toFloat();
      wantedTemperature = temp;
      Serial.println("Wanted temperature set to: " + String(wantedTemperature));
      checkHeaterLogic(); // Check heater logic based on the new wanted temperature
    }
  }
  
 
}


void setup() {
  Serial.begin(9600);
  pinMode(LOCK_PIN, OUTPUT);
  digitalWrite(LOCK_PIN, LOW);
  BluetoothSetup();
  if (pCharacteristic) {
    pCharacteristic->setCallbacks(new MyCallbacks());
  }
  
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nWiFi connected");
  TimeConfig(); // Configure time using NTP
  // Start WebSocket server
  server.listen(8080);
  Serial.println("WebSocket server started on port 8080");
  Serial.print("ESP32 IP address: ");
  Serial.println(WiFi.localIP());
  Bclient.onMessage([](websockets::WebsocketsMessage message) {
        webSocketEvent(message.data().c_str(), message.length());
    });
  Bclient.connect(external_ws_url);
}

void loop() {
  // Accept new clients
  /*if (SerialBT.available()) {
    String cmd = SerialBT.readStringUntil('\n');
    cmd.trim();
    Serial.println("Received via BT: " + cmd);
    if (cmd.equalsIgnoreCase("Unlock")) {
     digitalWrite(LOCK_PIN, HIGH); // Unlock (LED ON)
    isUnlocked = true;
    unlockTime = millis();
    sendLockState(true); // Send unlocked state
    Serial.println("Lock UNLOCKED (LED ON)");
    }
    
  }*/
  //
  if(server.poll()) {
    WebsocketsClient newClient = server.accept();
    // Capture pointer to the client in the lambda
    clients.push_back({newClient, -1});
    ClientInfo& clientInfo = clients.back();
    clientInfo.client.onMessage([&clientInfo](WebsocketsMessage message) {
        onMessageCallback(message, &clientInfo);
    });
    Serial.println("New client connected with temporary ID: -1");
  }
  //Incuietoarea
  if (isUnlocked && millis() - unlockTime >= 10000) {
    digitalWrite(LOCK_PIN, LOW); // Lock (LED OFF)
    isUnlocked = false;
    sendLockState(false); // Send locked state
    Serial.println("Lock LOCKED (LED OFF)");
  }
  // Poll all clients
  for (auto it = clients.begin(); it != clients.end(); ) {
    if(it->client.available()) {
      it->client.poll();
      ++it;
    } else {
      Serial.println("Client disconnected! ID: " + String(it->clientId));
      it = clients.erase(it);
    }
}
   if(Bclient.available()) {
    Bclient.poll();
    
  } else {
    // Try to reconnect if disconnected
    Bclient.connect(external_ws_url);
  }
  // Only send updates if values changed or at interval
  bool changed = false;
  if (temperature1 != lastTemperature1 || humidity1 != lastHumidity1 ||
      temperature2 != lastTemperature2 || humidity2 != lastHumidity2 ||
      ledstate != lastLedstate || isUnlocked != lastIsUnlocked) {
    changed = true;
    lastTemperature1 = temperature1;
    lastHumidity1 = humidity1;
    lastTemperature2 = temperature2;
    lastHumidity2 = humidity2;
    lastLedstate = ledstate;
    lastIsUnlocked = isUnlocked;
  }

  if ((changed || millis() - lastSend > sendInterval)) {
    if (Bclient.available()) {
        StaticJsonDocument<256> doc;
        JsonObject cam1 = doc.createNestedObject("camera1");
        cam1["temperature"] = temperature1;
        cam1["humidity"] = humidity1;
        cam1["ledstate"] = ledstate;
        JsonObject cam2 = doc.createNestedObject("camera2");
        cam2["temperature"] = temperature2;
        cam2["humidity"] = humidity2;
        time_t now = time(nullptr);
        struct tm* timeinfo = localtime(&now);
        char timeString[25];
        strftime(timeString, sizeof(timeString), "%Y-%m-%d %H:%M:%S", timeinfo);

        doc["datetime"] = timeString;
        doc["LockState"] = isUnlocked ? "unlocked" : "locked";
        doc["HeaterState"] = heaterOn; // <-- Add this line
        String json;
        serializeJson(doc, json);
        Bclient.send(json);
        Serial.println("Sent to external server: " + json);
        lastSend = millis(); // Only update if sent
    } else {
        Serial.println("Backend not connected, skipping JSON send.");
        // Do NOT update lastSend, so it will try again next loop
    }
}

  delay(5); // Small delay to avoid busy loop
}