using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto
{
    public class SetProgramRequest
    {
        public string ProgramName { get; set; } // "weekday", "weekend", "concediu"
    }
}