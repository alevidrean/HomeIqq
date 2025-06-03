using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // "ESP32", "TempSensor", etc.
        public string OwnerId { get; set; } // FK către AspNetUsers
        public string UniqueIdentifier { get; set; } // MAC, serial, etc.
    }
}