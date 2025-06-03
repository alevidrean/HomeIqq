using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Models;
using api.Data;
namespace api.Service
{
    public interface ITemperatureProgramService
    {
        Task<IEnumerable<TemperatureProgram>> GetAllProgramsAsync();
        Task<TemperatureProgram> UpsertProgramAsync(TemperatureProgramDto dto);
        Task<TemperatureProgram> SelectActiveProgramAsync(string programName);
        Task<TemperatureProgram> GetActiveProgramAsync();
        Task<TemperatureProgram?> UpdateProgramIntervalsAsync(string name, List<TemperatureIntervalDto> intervals);

        string BuildScheduleMessage(TemperatureProgram program);

        /// <summary>
        /// Verifică ora curentă în programul activ și returnează temperatura aferentă intervalului.
        /// Dacă nu există program activ sau nu suntem într-un interval, returnează null.
        /// </summary>
        Task<int?> GetCurrentTemperatureAsync();
    }
}