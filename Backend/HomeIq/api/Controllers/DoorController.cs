using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoorHandler : ControllerBase
    {


        // Constructorul nu mai are DeviceService

        [HttpPost("unlock")]
        public async Task<IActionResult> AprindeLumina()
        {
            // Doar trimitem mesajul prin WebSocket către clienți
            await WebSocketHandler.BroadcastMessageAsync("unlock");
            return Ok(new { message = "Lumina aprinsă" });
        }
    }
}