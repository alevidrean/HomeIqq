using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class EventLog
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } // <-- Adaugă această linie
        public string Details { get; set; }
    }
}