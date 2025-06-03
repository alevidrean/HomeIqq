using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace api.Models
{
    public class TemperatureInterval
    {
        public int Id { get; set; }

        /// <summary>
        /// Foreign key către TemperatureProgram
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// Ora de început a intervalului (ex: 08:00)
        /// </summary>
        public TimeSpan Start { get; set; }

        /// <summary>
        /// Ora de sfârșit a intervalului (ex: 12:00)
        /// </summary>
        public TimeSpan End { get; set; }

        /// <summary>
        /// Temperatura setată în acest interval (5–35)
        /// </summary>
        public int Temperature { get; set; }

        [JsonIgnore]
        public TemperatureProgram Program { get; set; }
    }

}
