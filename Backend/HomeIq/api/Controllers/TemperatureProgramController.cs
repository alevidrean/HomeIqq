using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Service;
using api.Dto;


namespace api.Controllers
{
    [ApiController]
    [Route("api/temperature-programs")]
    public class TemperatureProgramController : ControllerBase
    {
        private readonly ITemperatureProgramService _programService;

        public TemperatureProgramController(ITemperatureProgramService programService)
        {
            _programService = programService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var programs = await _programService.GetAllProgramsAsync();
            return Ok(programs);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] TemperatureProgramDto dto)
        {
            var result = await _programService.UpsertProgramAsync(dto);
            return Ok(result);
        }

        [HttpPost("select/{name}")]
        public async Task<IActionResult> SelectProgram(string name)
        {
            var result = await _programService.SelectActiveProgramAsync(name);
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var active = await _programService.GetActiveProgramAsync();
            if (active == null)
                return NotFound();
            return Ok(active);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Update(string name, [FromBody] List<TemperatureIntervalDto> intervals)
        {
            var result = await _programService.UpdateProgramIntervalsAsync(name, intervals);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

    }
}