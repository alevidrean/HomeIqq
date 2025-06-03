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
    public class TemperatureController : ControllerBase
    {

        private readonly ITemperatureProgramService _programService;

        public TemperatureController(ITemperatureProgramService programService)
        {
            _programService = programService;
        }


        [HttpPost("set")]
        public async Task<IActionResult> SetTemperature([FromBody] TemperatureRequest request)
        {
            if (request.Temperature < 5 || request.Temperature > 35)
                return BadRequest("Temperatura trebuie să fie între 5 și 35 grade.");

            string message = $"WantedTemperature:{request.Temperature}";
            await WebSocketHandler.BroadcastMessageAsync(message);

            return Ok(new { message = $"Temperatura setată la {request.Temperature}°C" });
        }




        /// <summary>
        /// Noul endpoint: verifică în ce interval orar suntem în programul activ și trimite temperatura corespunzătoare.
        /// Dacă nu există program activ sau nu suntem într-un interval, răspunde cu 404.
        /// </summary>
        [HttpPost("evaluate")]
        public async Task<IActionResult> EvaluateCurrentTemperature()
        {
            // 1. Obținem temperatura curentă (din programul activ)
            var tempCurenta = await _programService.GetCurrentTemperatureAsync();

            if (tempCurenta == null)
            {
                // nu există program activ sau ora nu se încadrează în niciun interval
                return NotFound(new { message = "Nu există interval curent în programul activ." });
            }

            // 2. Construim mesajul "WantedTemperature:<val>" și trimitem către ESP prin WebSocket
            string message = $"WantedTemperature:{tempCurenta.Value}";
            await WebSocketHandler.BroadcastMessageAsync(message);

            return Ok(new { message = $"Și s-a trimis ESP => WantedTemperature:{tempCurenta.Value}" });
        }




    }
}
