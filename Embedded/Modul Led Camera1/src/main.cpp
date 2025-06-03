#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ArduinoWebsockets.h>
#include <ArduinoJson.h>

using namespace websockets;

const char* ssid = "SALA_LECTURA_ETAJ_3";
const char* password = "";
const char* websockets_server_host = "192.168.100.154"; // Change to your ESP32 IP
const uint16_t websockets_server_port = 8080;
String ws_url = String("ws://") + websockets_server_host + ":" + String(websockets_server_port) + "/";
#define RELAY_PIN 0  // GPIO0 (D3 on some boards, check your wiring)
bool heaterStatus = false;
bool lastHeaterStatus = false; // Add this line
unsigned long lastSend = 0;
const unsigned long sendInterval = 5000;
WebsocketsClient client;

void transmitHeaterStatus() {
  StaticJsonDocument<64> doc;
  doc["EspId"]=3;
  doc["Camera"]=1;
  doc["ledstate"] = heaterStatus;
  String json;
  serializeJson(doc, json);
  client.send(json);
  Serial.println("Sent: " + json);
}

void setup() {
  Serial.begin(9600);
  pinMode(RELAY_PIN, OUTPUT);
  digitalWrite(RELAY_PIN, HIGH); // Relay off by default

  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nWiFi connected");

  client.onMessage([](WebsocketsMessage message){
    String msg = message.data();
    Serial.println("Received: " + msg);
    if (msg == "led_on") {
      heaterStatus = true;
      digitalWrite(RELAY_PIN, LOW); // Turn relay ON
      transmitHeaterStatus();
    } else if (msg == "led_off") {
      heaterStatus = false;
      digitalWrite(RELAY_PIN, HIGH); // Turn relay OFF
      transmitHeaterStatus();
    }
  });

  client.onEvent([](WebsocketsEvent event, String data){
    if(event == WebsocketsEvent::ConnectionOpened) {
      Serial.println("WebSocket Connected");
      transmitHeaterStatus();
    } else if(event == WebsocketsEvent::ConnectionClosed) {
      Serial.println("WebSocket Disconnected");
    }
  });

  client.connect(ws_url);
  transmitHeaterStatus(); // Initial status transmission
}

void loop() {
  if(!client.available()) {
    client.connect(ws_url);
  }

  // Only send status if it changed
  if (heaterStatus != lastHeaterStatus && client.available()) {
    transmitHeaterStatus();
    lastHeaterStatus = heaterStatus;
  }

  client.poll();
}