using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Dto;
using api.Models;

namespace api.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class SmartHomeController : ControllerBase
    {

        // variabilă statică pentru demo, la producție folosește DB
        private static SmartHomePayload latestPayload;

        [HttpPost("update")]
        public IActionResult Update([FromBody] SmartHomePayload payload)
        {
            latestPayload = payload;
            return Ok(new { message = "Date primite și salvate." });
        }

        // 3. GET pentru frontend să ia temperatura curentă
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            if (latestPayload == null)
                return NotFound("Nu există date încă.");

            return Ok(new
            {
                camera1 = new { latestPayload.Camera1.Temperature, latestPayload.Camera1.Humidity },
                camera2 = new { latestPayload.Camera2.Temperature, latestPayload.Camera2.Humidity },
                datetime = latestPayload.Datetime,
                lockState = latestPayload.LockState
            });
        }
    }
}