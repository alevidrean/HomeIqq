using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string CNP { get; set; }
        public string CodBluetooth { get; set; }
    }
}