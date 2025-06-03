#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ArduinoWebsockets.h>
#include <ArduinoJson.h>
#include <DHT.h>

using namespace websockets;

// WiFi credentials
const char* ssid = "SALA_LECTURA_ETAJ_3";
const char* password = "";

// WebSocket server address (ESP32 IP and port)
const char* websockets_server_host = "192.168.100.154"; // Change to ESP32 IP
const uint16_t websockets_server_port = 8080;         // Change to your port
String ws_url = String("ws://") + websockets_server_host + ":" + String(websockets_server_port) + "/";

// DHT11 setup
#define DHTPIN 2      // GPIO2 (D4 on NodeMCU/ESP-01)
#define DHTTYPE DHT11
DHT dht(DHTPIN, DHTTYPE);

WebsocketsClient client;

float lastTemp = NAN;
float lastHum = NAN;

void setup() {
  Serial.begin(9600);
  dht.begin();

  // Connect to WiFi
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("WiFi connected");

  // Connect to WebSocket server
 
  client.connect(ws_url);

  client.onEvent([](WebsocketsEvent event, String data){
    if(event == WebsocketsEvent::ConnectionOpened) {
      Serial.println("WebSocket Connected");
    } else if(event == WebsocketsEvent::ConnectionClosed) {
      Serial.println("WebSocket Disconnected");
    } else if(event == WebsocketsEvent::GotPing) {
      Serial.println("Got a Ping!");
    } else if(event == WebsocketsEvent::GotPong) {
      Serial.println("Got a Pong!");
    }
  });
}

void loop() {
  // Read sensor data
  float h = dht.readHumidity();
  float t = dht.readTemperature();

  // Check if any reads failed
  if (isnan(h) || isnan(t)) {
    Serial.println("Failed to read from DHT sensor!");
    delay(2000);
    return;
  }

  // Only send if temperature or humidity changed
  if (t != lastTemp || h != lastHum) {
    StaticJsonDocument<128> doc;
    doc["EspId"] = 2; 
    doc["Camera"] = 2;
    doc["temperature"] = t;
    doc["humidity"] = h;
    String json;
    serializeJson(doc, json);
    Serial.println(json);

    if(client.available()) {
      client.send(json);
      Serial.println("Sent: " + json);
      lastTemp = t;
      lastHum = h;
    } else {
      client.connect(ws_url);
      Serial.println("WebSocket not available");
    }
  }

  client.poll(); // Keep connection alive

  delay(2000); // Check every 2 seconds (adjust as needed)
}