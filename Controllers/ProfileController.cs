using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DmlCafePos.Controllers
{
    [Authorize(Roles = "admin,demo,tam_surum")]
    public class ProfileController:Controller
    {
        private UserManager<AppUser> _userManager;
        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> MyProfile()
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null){
                    return View(user);
                }
            }
            return NotFound();
        }
        public IActionResult ProfileDetails()
        {
            return View();
        }
    }
}