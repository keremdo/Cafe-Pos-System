using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Dmlcafepos.Entities
{
    public class AppUser:IdentityUser
    {
        public string MandantID {get;set;} = null!;

        public string? FullName { get; set; } 
        public string? Adress { get; set; } 
        public string? CompanyName { get; set; }

    }
}