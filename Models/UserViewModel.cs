using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;

namespace DmlCafePos.Models
{
    public class UserViewModel
    {
        public AppUser User {get;set;} = null!;
    }
}