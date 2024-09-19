using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DmlCafePos.Controllers
{
    [Authorize(Roles = "admin,demo,tam_surum")]
    public class HomePageController : Controller
    {
        private UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepo;
        private readonly ITableRepository _tableRepo;
        private readonly IAreaRepository _areaRepo;
        private readonly IPaymentRepository _paymentRepo;
        public HomePageController(UserManager<AppUser> userManager, IOrderRepository orderRepo, ITableRepository tableRepo,IAreaRepository areaRepo,IPaymentRepository paymentRepo)
        {
            _userManager = userManager;
            _orderRepo = orderRepo;
            _tableRepo = tableRepo;
            _areaRepo = areaRepo;
            _paymentRepo = paymentRepo;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var viewModel = new UserViewModel
                    {
                        User = user
                    };
                    return View(viewModel);
                }
            }
            return NotFound();

        }

        public async Task<IActionResult> Statistics()
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var mainModel = new StatisticViewModel{
                        Tables = _tableRepo.Entites.Where(t => t.MandantID == user.MandantID).ToList(),
                        Orders = _orderRepo.Entites.Where(o => o.MandantID == user.MandantID).ToList(),
                        Payments = _paymentRepo.Entites.Where(p => p.MandantID == user.MandantID)
                                                        .OrderByDescending(p => p.PaymentId)
                                                        .ToList()
                    };
                    return View(mainModel);
                }return NotFound();
            }return NotFound();
            
        }
        
        public async Task<IActionResult> Orders()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var orderViewModel = new OrderViewModel
                    {
                        Tables = _tableRepo.Entites.Where(t => t.MandantID == user.MandantID)
                                                    .Include(t => t.Area)
                                                    .ToList(),
                        Orders = _orderRepo.Entites.Where(o => o.MandantID == user.MandantID).ToList(),
                        Areas = _areaRepo.Entites.Where(a => a.MandantID == user.MandantID).ToList()
                    };
                    return View(orderViewModel);
                }
            }
            return NotFound();


        }

        public async Task<IActionResult> TableListByArea(int id)
        {
            if(id != 0)
            {
                var userId = _userManager.GetUserId(User);
                if (userId != null)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user != null)
                    {
                        var area = _areaRepo.Entites.FirstOrDefault(a => a.AreaId == id);
                        if(area != null)
                        {
                            var tables = _tableRepo.Entites.Where(t => t.MandantID == user.MandantID || t.Area == area).ToList();
                            return PartialView("_tablePartial",tables);
                        }
                        return NotFound();
                    }
                    return NotFound();
                }
            }
            return NotFound();
        }
    }
}