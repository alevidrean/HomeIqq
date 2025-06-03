#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ArduinoWebsockets.h>
#include <ArduinoJson.h>

using namespace websockets;

const char* ssid = "SALA_LECTURA_ETAJ_3";
const char* password = "";
const char* websockets_server_host = "192.168.100.154"; // ESP32 IP
const uint16_t websockets_server_port = 8080;
String ws_url = String("ws://") + websockets_server_host + ":" + String(websockets_server_port) + "/";

#define BUTTON_PIN 2  // GPIO2 (D4 on NodeMCU/ESP-01)


WebsocketsClient client;

void setup() {
  Serial.begin(9600);
  

  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nWiFi connected");

  client.onEvent([](WebsocketsEvent event, String data){
    if(event == WebsocketsEvent::ConnectionOpened) {
      Serial.println("WebSocket Connected");
    } else if(event == WebsocketsEvent::ConnectionClosed) {
      Serial.println("WebSocket Disconnected");
    }
  });

  client.connect(ws_url);
  pinMode(BUTTON_PIN,INPUT);
}

void loop() {
  bool buttonState = digitalRead(BUTTON_PIN);
  if(!client.available()) {
    Serial.println("WebSocket not available");
    client.connect(ws_url);
  }
  // Detect button press (active LOW)
  if (buttonState == LOW) {
    Serial.println("Button pressed");
    StaticJsonDocument<64> doc;
    doc["EspId"] = 4;      // Set your ESP ID
    doc["Camera"] = 1;     // Set your Camera ID
    doc["command"] = "led_change";
    String json;
    serializeJson(doc, json);
    client.send(json);
    delay(200); // debounce
  }


  

  client.poll();
  delay(10);
}