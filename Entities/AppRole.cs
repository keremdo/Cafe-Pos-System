using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Dmlcafepos.Entities
{
    public class AppRole : IdentityRole
    {
         public AppRole() : base() { }
         public AppRole(string roleName) : base(roleName) { }
    }
}