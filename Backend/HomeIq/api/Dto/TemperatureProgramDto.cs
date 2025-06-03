using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Dto;
namespace api.Dto
{
    public class TemperatureProgramDto
    {
        /// <summary>
        /// Numele programului: "weekday" / "weekend" / "concediu"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Lista celor 0–6 intervale orare
        /// </summary>
        public List<TemperatureIntervalDto> Intervals { get; set; }
    }
}