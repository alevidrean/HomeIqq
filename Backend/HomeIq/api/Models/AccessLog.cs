using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class AccessLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // FK către AspNetUsers(Id)
        public DateTime Timestamp { get; set; }
        public string Direction { get; set; }  // "In" sau "Out"
    }
}