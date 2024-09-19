using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using DmlCafePos.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DmlCafePos.Controllers
{
    public class AuthLogController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthLogController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager,SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);

                    if(result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains("admin") || roles.Contains("demo"))
                        {
                            return RedirectToAction("Index", "HomePage");
                        }else{
                            return RedirectToAction("DeniedPath", "AuthLog");
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("","Yanlış Şifre");
                    }
                }else
                {
                    ModelState.AddModelError("","E posta bulunamadı");
                }
            }
            return View(model);
            
            
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","AuthLog");
        }

        public IActionResult DeniedPath()
        {
            return View();
        }

        
    }
}