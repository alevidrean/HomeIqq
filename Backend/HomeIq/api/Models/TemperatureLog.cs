using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class TemperatureLog
    {
        public int Id { get; set; }

        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string Camera { get; set; }



    }
}