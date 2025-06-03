using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using api.Dto;
using api.Models;
using api.Data;

namespace api.Controllers
{
    [ApiController]
    [Route("api/eventlog")]
    public class EventLogController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public EventLogController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        //[AllowAnonymous] // For testing purposes, remove in production
        public async Task<IActionResult> AddEvent([FromBody] EventLogDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var log = new EventLog
            {
                EventType = dto.EventType,
                Timestamp = DateTime.UtcNow,
                UserId = userId,
                Details = dto.Details
            };
            _context.EventLog.Add(log);
            await _context.SaveChangesAsync();
            return Ok(log);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("EventLogController is alive!");
        }





        [HttpGet("last")]
        //   [Authorize] // sau [AllowAnonymous] temporar pentru test
        [AllowAnonymous]
        public IActionResult GetLastEvents()
        {
            var events = _context.EventLog
                .OrderByDescending(e => e.Timestamp)
                .Take(15)
                .ToList();

            return Ok(events);
        }



    }
}