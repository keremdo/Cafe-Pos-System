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
    [Authorize(Roles = "admin")]
    public class RolesController:Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManage;
        public RolesController(RoleManager<AppRole> roleManager,UserManager<AppUser> userManage)
        {
            _roleManager = roleManager;
            _userManage = userManage;
        }

        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(AppRole model)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(model);
                if(result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }

                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleDelete(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var user = await _roleManager.FindByIdAsync(id);
            if(user != null)
            {
                var result = await _roleManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }

                foreach(IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }else{
                return NotFound();
            }
            return View();
        }
        
        public async Task<IActionResult> RoleDetails(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if(role != null && role.Name != null)
            {
                ViewBag.Users = await _userManage.GetUsersInRoleAsync(role.Name);
                return View(role);
            }
            return RedirectToAction("RoleList");
        }
    }
}