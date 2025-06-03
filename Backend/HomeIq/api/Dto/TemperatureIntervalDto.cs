using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Dto;
namespace api.Dto
{
    public class TemperatureIntervalDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Ora de început, format "HH:mm" (ex: "08:00")
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// Ora de sfârșit, format "HH:mm" (ex: "12:00")
        /// </summary>
        public string End { get; set; }

        /// <summary>
        /// Temperatura pentru acest interval (int între 5 și 35)
        /// </summary>
        public int Temperature { get; set; }
    }
}