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
using Microsoft.VisualBasic;

namespace DmlCafePos.Controllers
{
    [Authorize(Roles = "admin,demo,tam_surum")]
    public class CustomerController:Controller
    {
        private readonly ICustomerRepository _customRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPaymentRepository _paymentRepo;

        public CustomerController(ICustomerRepository customRepo,UserManager<AppUser> userManager,IPaymentRepository paymentRepo)
        {
            _customRepo = customRepo;
            _userManager = userManager;
            _paymentRepo = paymentRepo;
        }
        public async Task<IActionResult> CustomerListIndex()
        {   
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    
                    var customerViewModel = new CustomerListViewModel 
                    {
                        Customers = _customRepo.Entites.Where(c => c.MandantID == user.MandantID).ToList()
                    };
                    return View(customerViewModel);
                }return NotFound();
            }return NotFound();
            
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer entity)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    if(entity != null)
                    {
                        entity.MandantID = user.MandantID;
                        _customRepo.Create(entity);
                        var customerViewModel = new CustomerListViewModel
                        {
                            Customers = _customRepo.Entites.Where(c => c.MandantID == user.MandantID).ToList()
                        };
                        return RedirectToAction("CustomerListIndex",new { CustomerListViewModel = customerViewModel});
                        
                    }return NotFound();
                }return NotFound();
            }return NotFound();
        } 
        
        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
             var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var customer = _customRepo.Entites.FirstOrDefault(c => c.CustomerId == customerId && c.MandantID == user.MandantID);
                    if(customer != null)
                    {
                        _customRepo.Delete(customerId);
                        var customerViewModel = new CustomerListViewModel
                        {
                            Customers = _customRepo.Entites.Where(c => c.MandantID == user.MandantID).ToList()
                        };
                        return RedirectToAction("CustomerListIndex",new { CustomerListViewModel = customerViewModel});
                    }return NotFound();
                }return NotFound();
            }return NotFound();
        }
    
        public async Task<IActionResult> CustomerDetail(int customerId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null){
                var user = await  _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var customer = _customRepo.Entites.FirstOrDefault(c => c.CustomerId == customerId && c.MandantID == user.MandantID);
                    if(customer != null)
                    {
                        return View(customer);
                    }
                }
            }return NotFound();  
        }

        [HttpPost]
        public async Task<IActionResult> PayingToBalance(double price,string paymentMethod,int customerId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null){
                var user = await  _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var customer = _customRepo.Entites.FirstOrDefault(c => c.CustomerId == customerId && c.MandantID == user.MandantID);
                    if(customer != null)
                    {
                        if(price >= 0)
                        {
                            if(paymentMethod != null)
                            {
                                var payment = new Payment{
                                PaymentPrice = price,
                                MandantID = user.MandantID,
                                PaymentMethod = "Kredi Kartı ile Müşteri Borç Ödemesi",
                                PaymentDate = DateTime.Today
                                };
                                _paymentRepo.Create(payment);
                                customer.CustomerBalance -= price;
                                _customRepo.Update(customer);
                                return RedirectToAction("CustomerDetail",customer);
                            }else{
                                var payment = new Payment{
                                PaymentPrice = price,
                                MandantID = user.MandantID,
                                PaymentMethod = "Nakit ile Müşteri Borç Ödemesi",
                                PaymentDate = DateTime.Today
                                };
                                _paymentRepo.Create(payment);
                                customer.CustomerBalance -= price;
                                _customRepo.Update(customer);
                                return RedirectToAction("CustomerDetail",customer);
                            }      
                        }return NotFound();
                    }return NotFound();
                }return NotFound();
            }return NotFound();
        }
    }   
}