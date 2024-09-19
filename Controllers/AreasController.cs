using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Entities;
using DmlCafePos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DmlCafePos.Controllers
{
    [Authorize(Roles = "admin,demo,tam_surum")]
    public class AreasController : Controller
    {
        private UserManager<AppUser> _userManager;
        private readonly IAreaRepository _areaRepo;
        private readonly ITableRepository _tableRepo;
        public AreasController(IAreaRepository areaRepo, UserManager<AppUser> userManager,ITableRepository tableRepo)
        {
            _areaRepo = areaRepo;
            _userManager = userManager;
            _tableRepo = tableRepo;
        }
        public async Task<IActionResult> AreasList()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var areaModel = new AreaViewModel
                    {
                        Areas = _areaRepo.Entites.Where(a => a.MandantID == user.MandantID).ToList(),
                        User = user,
                        Area = new Area()
                    };
                    return View(areaModel);
                }
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> CreateArea(Area entity)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    if (entity != null)
                    {
                        entity.MandantID = user.MandantID;
                        _areaRepo.Create(entity);
                        return RedirectToAction("AreasList");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }

            return NotFound();


        }

        public async Task<IActionResult> AreaById(int id)
        {
            if (id != 0)
            {
                var area = _areaRepo.Entites.FirstOrDefault(a => a.AreaId == id);
                if (area != null)
                {
                    var userId = _userManager.GetUserId(User);
                    if (userId != null)
                    {
                        var user = await _userManager.FindByIdAsync(userId);
                        if(user != null)
                        {
                            ViewBag.Areas = new SelectList(_areaRepo.Entites.Where(a => a.MandantID == user.MandantID).ToList(),"AreaId","AreaName");
                            var areaModel = new AreaViewModel
                            {
                                User = user,
                                Area = area,
                                Areas = _areaRepo.Entites.Where(a => a.MandantID == user.MandantID)
                                                            .Include(a => a.Tables)
                                                            .ToList(),
                                Tables = _tableRepo.Entites.Where(a => a.Area.AreaId == id).ToList(),    
                            };
                            return View(areaModel);
                        }
                    }
                    
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteArea(int id)
        {
            if(id != 0)
            {
                var area = _areaRepo.Entites.FirstOrDefault(a => a.AreaId == id);
                if(area != null)
                {
                    _areaRepo.Delete(id);
                    return RedirectToAction("AreasList");
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> AddTableByArea(Table entity , int areaId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    if(entity != null)
                    {
                        ViewBag.TableByMandant = _tableRepo.Entites.Where(t => t.MandantID == user.MandantID).ToList();
                        entity.MandantID = user.MandantID;

                        var area = _areaRepo.Entites.FirstOrDefault(a => a.AreaId == areaId);
                        if(area == null)
                        {
                            return NotFound();
                        }
                        entity.Area = area;
                        _tableRepo.Create(entity);
                        return RedirectToAction("AreasList");

                    }
                }
            }    
            return View();
        }
        [HttpPost]
        public IActionResult DeleteTable(int id)
        {
            if(id >=1)
            {
                var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId == id);
                if(table != null){
                    _tableRepo.Delete(id);
                    return RedirectToAction("AreasList");
                }return NotFound();
            }return NotFound();
        }

    }
}