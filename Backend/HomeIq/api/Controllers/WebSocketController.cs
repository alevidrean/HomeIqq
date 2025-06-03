using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using api.Models;
using api.Service;
using api.Dto;

namespace api.Controllers
{
    [ApiController]
    [Route("/ws")]
    public class WebSocketController : ControllerBase
    {
        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                WebSocketHandler.Clients.Add(socket);
                Console.WriteLine("Client WebSocket conectat.");

                var buffer = new byte[4 * 1024];

                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    var mesaj = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Mesaj primit de la client: {mesaj}");

                    try
                    {
                        var payload = JsonSerializer.Deserialize<SmartHomePayload>(mesaj);
                        if (payload != null)
                        {
                            WebSocketHandler.LatestPayload = payload;
                            //  Console.WriteLine($"Temperatura camera1: {payload.Camera1.Temperature} °C");
                            //  Console.WriteLine($"Temperatura camera2: {payload.Camera2.Temperature} °C");

                        }
                    }
                    catch (JsonException)
                    {
                        Console.WriteLine("Mesaj JSON invalid.");
                    }
                }

                WebSocketHandler.Clients.TryTake(out _);
                Console.WriteLine("Client WebSocket deconectat.");
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}
