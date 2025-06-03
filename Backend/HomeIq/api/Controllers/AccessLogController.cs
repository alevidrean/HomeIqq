using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Dto;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using api.Interfaces;
using api.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace api.Controllers
{
    [ApiController]
    [Route("api/accesslog")]
    public class AccessLogController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public AccessLogController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddLogFromDto([FromBody] AccessLogDto dto)
        {
            // Dacă primești CodBluetooth de la ESP32:
            // var user = await _context.Users.FirstOrDefaultAsync(u => u.CodBluetooth == dto.CodBluetooth);
            // if (user == null) return NotFound("User not found");
            // var userId = user.Id;

            // Dacă acțiunea vine de la frontend cu user logat:
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var log = new AccessLog
            {
                UserId = userId,
                Direction = dto.Direction,
                Timestamp = DateTime.UtcNow
            };
            _context.AccessLog.Add(log);
            await _context.SaveChangesAsync();
            return Ok(log);
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] string userId = null)
        {
            var logs = _context.AccessLog.AsQueryable();
            if (!string.IsNullOrEmpty(userId))
                logs = logs.Where(l => l.UserId == userId);
            return Ok(await logs.OrderByDescending(l => l.Timestamp).ToListAsync());
        }
    }
}