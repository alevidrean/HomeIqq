using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Service;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrentTemperature : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCurrentTemperature()
        {
            var payload = WebSocketHandler.LatestPayload;
            if (payload == null)
            {
                return NotFound("Nu s-au primit date de la ESP.");
            }

            return Ok(new
            {
                Datetime = payload.Datetime,
                Camera1 = new
                {
                    Temperature = payload.Camera1.Temperature,
                    Humidity = payload.Camera1.Humidity,
                    LedState = payload.Camera1.LedState
                },
                Camera2 = new
                {
                    Temperature = payload.Camera2.Temperature,
                    Humidity = payload.Camera2.Humidity
                },
                LockState = payload.LockState
            });
        }
    }
}