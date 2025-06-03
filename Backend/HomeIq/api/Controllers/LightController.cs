using api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LightController : ControllerBase
    {

        // Constructorul nu mai are DeviceService

        [HttpPost("on")]
        public async Task<IActionResult> AprindeLumina()
        {
            // Doar trimitem mesajul prin WebSocket către clienți
            await WebSocketHandler.BroadcastMessageAsync("led_on");
            return Ok(new { message = "Lumina aprinsă" });
        }

        [HttpPost("off")]
        public async Task<IActionResult> StingeLumina()
        {
            await WebSocketHandler.BroadcastMessageAsync("led_off");
            return Ok(new { message = "Lumina stinsă" });
        }
    }

}
