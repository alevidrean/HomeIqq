using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto
{
    public class SelectProgramRequest
    {
        // <summary>
        /// Numele programului care trebuie activat
        /// </summary>
        public string ProgramName { get; set; }
    }
}