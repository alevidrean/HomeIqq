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

namespace api.Controllers
{
    [ApiController]
    [Route("api/device")]
    public class DeviceController : ControllerBase
    {
        private readonly HttpClient _http;

        public DeviceController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
        }

        private readonly string espIp = "http://192.168.1.123"; // <-- înlocuiește cu IP-ul ESP32

        [HttpPost("toggle-light")]
        public async Task<IActionResult> ToggleLight()
        {
            try
            {
                var response = await _http.GetAsync($"{espIp}/toggle-light");
                var content = await response.Content.ReadAsStringAsync();
                return Ok(new { status = content.Contains("on") ? "on" : "off" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare la comunicarea cu ESP32: {ex.Message}");
            }
        }

        [HttpGet("light-status")]
        public async Task<IActionResult> GetLightStatus()
        {
            try
            {
                var response = await _http.GetAsync($"{espIp}/light-status");
                var content = await response.Content.ReadAsStringAsync();
                return Ok(new { status = content.Contains("on") ? "on" : "off" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Eroare la citirea statusului: {ex.Message}");
            }
        }
    }


}
