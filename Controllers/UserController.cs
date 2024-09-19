using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using Dmlcafepos.ViewModels;
using DmlCafePos.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dmlcafepos.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController:Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private string _mandantID;
        
        public UserController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager,string mandantID)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mandantID = mandantID;
        }
        private string GenerateUniqueID()
        {
            // Burada benzersiz bir kimlik oluşturma mantığını yazabilirsiniz
            // Örneğin, GUID (Globally Unique Identifier) kullanabilirsiniz
            return Guid.NewGuid().ToString();
        }
        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }
        public IActionResult UserCreate()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> UserCreate(CreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new AppUser{UserName = model.Email,Email=model.Email,FullName = model.FullName!,MandantID = GenerateUniqueID()};
                IdentityResult result = await _userManager.CreateAsync(user,model.Password!);
                if(result.Succeeded)
                {
                    return RedirectToAction("UserList");
                }   
                foreach(IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new AppUser{FullName = model.FullName,UserName = model.Email,Email = model.Email,Adress = model.Address,PhoneNumber = model.Phone,CompanyName = model.CompanyName,MandantID = GenerateUniqueID()};
                
                IdentityResult result = await _userManager.CreateAsync(user,model.Password!);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user,"demo");
                    return RedirectToAction("Login","AuthLog");
                }
                foreach(IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
                
            }return View(model);
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = _roleManager.Roles.Select(r=> r.Name).ToList();
            return View(new EditViewModel{
                Id = user.Id,
                FullName = user.FullName,
                
                Email = user.Email,
                SelectedRoles = await _userManager.GetRolesAsync(user),
                AppUser = user
            });
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(string id,EditViewModel model)
        {
            if(id != model.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if(user != null)
                {
                    // user.FullName = model.FullName;
                    // user.Email = model.Email;

                    var result = await _userManager.UpdateAsync(user);

                    if(result.Succeeded && !string.IsNullOrEmpty(model.Password))
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user,model.Password);
                    }

                    if(result.Succeeded)
                    {
                        await _userManager.RemoveFromRolesAsync(user,await _userManager.GetRolesAsync(user));
                        if(model.SelectedRoles != null)
                        {
                            await _userManager.AddToRolesAsync(user,model.SelectedRoles);
                        }
                        return RedirectToAction("UserList");
                    }


                    foreach(IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("",err.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserDelete(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("UserList");
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
    }
}