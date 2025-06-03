using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class TemperatureProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Marchează dacă e programul care trebuie folosit acum
        /// </summary>
        public bool IsActive { get; set; } = false;

        public ICollection<TemperatureInterval> Intervals { get; set; }
    }
}