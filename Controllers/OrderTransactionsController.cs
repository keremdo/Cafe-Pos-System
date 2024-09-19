using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Entities;
using DmlCafePos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DmlCafePos.Controllers
{
    [Authorize(Roles = "admin,demo,tam_surum")]
    public class OrderTransactionsController:Controller
    {
        private UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepo;
        private readonly ITableRepository _tableRepo;
        private readonly IAreaRepository _areaRepo;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IPaymentRepository _paymentRepo;
        public OrderTransactionsController(UserManager<AppUser> userManager,IOrderRepository orderRepo,ITableRepository tableRepo,IAreaRepository areaRepo,IProductRepository productRepo,ICategoryRepository categoryRepo,ICustomerRepository customerRepo,IPaymentRepository paymentRepo)
        {
            _userManager = userManager;
            _orderRepo = orderRepo;
            _tableRepo = tableRepo;
            _areaRepo = areaRepo;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _customerRepo = customerRepo;
            _paymentRepo = paymentRepo;
        } 

        public async Task<IActionResult> OrderDetails()
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var orderList = _orderRepo.Entites.Where(o => o.MandantID == user.MandantID)
                                                .Include(o => o.Table)
                                                .OrderByDescending(o => o.OrderId)
                                                .ToList();
                    return View(orderList);
                }
                return NotFound();
            }
            return NotFound();
        }

        public async Task<IActionResult> OrderDetail(int orderId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var order = _orderRepo.Entites.FirstOrDefault(o => o.OrderId == orderId && o.MandantID == user.MandantID);
                    if(order != null)
                    {
                        return View(order);
                    }return NotFound();
                }return NotFound();
            }
            return NotFound();
        }
        public async Task<IActionResult> CustomizeOrder(int id,int? orderId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null){
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId == id);
                    if(table != null){
                        if(table.isEmpty == false)
                        {
                            var orderViewModel = new OrderViewModel{
                            Table = table,
                            Products = _productRepo.Entites.Where(p => p.MandantID == user.MandantID).ToList(),
                            Areas = _areaRepo.Entites.Where(p => p.MandantID == user.MandantID).ToList(),
                            Categories = _categoryRepo.Entites.Where(p => p.MandantID == user.MandantID).ToList()
                            };
                            return View(orderViewModel);
                        }else{
                            var order = _orderRepo.Entites.FirstOrDefault(o => o.OrderId == orderId && o.MandantID == user.MandantID);
                            if(order != null)
                            {
                                var orderViewModel = new OrderViewModel{
                                Table = table,
                                Order = order,
                                Products = _productRepo.Entites.Where(p => p.MandantID == user.MandantID).ToList(),
                                Areas = _areaRepo.Entites.Where(p => p.MandantID == user.MandantID).ToList(),
                                Categories = _categoryRepo.Entites.Where(p => p.MandantID == user.MandantID).ToList()
                                };
                                return View(orderViewModel);
                            }return NotFound();
                        }     
                    }return NotFound();
                        
                } return NotFound();
                   
            } return NotFound();
        } 
        
        

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int tableId,List<string> orderItems,double totalPrice,double totalSpread)
        {   
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId == tableId && t.MandantID == user.MandantID);
                    if(table != null)
                    {
                        if(totalSpread == 0)
                        {
                            var order = new Order{
                            Table = table,
                            MandantID = user.MandantID,
                            OrderItems = orderItems,
                            Price = totalPrice,
                            orderDate = DateTime.Today
                        };
                        _orderRepo.Create(order);
                        table.isEmpty = true;
                        _tableRepo.Update(table);
                        return RedirectToAction("Orders","HomePage");
                        }
                        var orderHasSpread = new Order{
                            Table = table,
                            MandantID = user.MandantID,
                            OrderItems = orderItems,
                            Price = totalPrice,
                            Spread = totalSpread,  
                            orderDate = DateTime.Today
                        };
                        _orderRepo.Create(orderHasSpread);
                        table.isEmpty = true;
                        _tableRepo.Update(table);
                        return RedirectToAction("Orders","HomePage");
                    }return NotFound();

                }return NotFound();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int tableId,List<string> orderItems,double totalPrice,int orderId,double totalSpread)
        {   
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId == tableId && t.MandantID == user.MandantID);
                    if(table != null)
                    {
                        var order = _orderRepo.Entites.FirstOrDefault(o=> o.OrderId == orderId && o.MandantID == user.MandantID);
                        if(order != null)
                        {
                            if(totalSpread ==0)
                            {
                                order.Price = order.Price + totalPrice;
                                order.OrderItems.AddRange(orderItems);
                                order.orderDate = DateTime.Now;
                                _orderRepo.Update(order);
                                return RedirectToAction("Orders","HomePage");  
                            }
                            else{
                                order.Spread = order.Spread + totalSpread;
                                order.Price = order.Price + totalPrice;
                                order.OrderItems.AddRange(orderItems);
                                order.orderDate = DateTime.Now;
                                _orderRepo.Update(order);
                                return RedirectToAction("Orders","HomePage");
                            }
                            
                        }
                        
                    }return NotFound();

                }return NotFound();
            }
            return NotFound();
        }
        

        public async Task<IActionResult> OrderClosing(int tableId,int orderId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId==tableId && t.MandantID== user.MandantID);
                    var order = _orderRepo.Entites.FirstOrDefault(o => o.OrderId == orderId && o.MandantID == user.MandantID);
                    if(order != null && table !=null)
                    {
                        var closingModel = new OrderClosingViewModel
                        {
                            Table = table,
                            Order = order,
                            OrderItems = order.OrderItems,
                            Customers = _customerRepo.Entites.Where(c=> c.MandantID == user.MandantID).ToList()
                        };
                        return View(closingModel);
                    }return NotFound();

                }return NotFound();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PayingToCash(int tableId,int orderId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId==tableId && t.MandantID== user.MandantID);
                    var order = _orderRepo.Entites.FirstOrDefault(o => o.OrderId == orderId && o.MandantID == user.MandantID);
                    if(order != null && table !=null)
                    {
                        table.isEmpty = false;
                        order.isActive = false;
                        _tableRepo.Update(table);
                        _orderRepo.Update(order);
                        var payment = new Payment{
                            MandantID = user.MandantID,
                            PaymentPrice = order.Price,
                            PaymentMethod = "Nakit Ödeme",
                            PaymentDate = DateTime.Today
                        };
                        _paymentRepo.Create(payment);
                        return RedirectToAction("Orders","HomePage");
                    }return NotFound();
                }return NotFound();
            }
            return NotFound();
        } 

        [HttpPost]
        public async Task<IActionResult> PayingToCard(int tableId,int orderId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId==tableId && t.MandantID== user.MandantID);
                    var order = _orderRepo.Entites.FirstOrDefault(o => o.OrderId == orderId && o.MandantID == user.MandantID);
                    if(order != null && table !=null)
                    {
                        table.isEmpty = false;
                        order.isActive = false;
                        _tableRepo.Update(table);
                        _orderRepo.Update(order);
                        var payment = new Payment{
                            MandantID = user.MandantID,
                            PaymentPrice = order.Price,
                            PaymentMethod = "Kredi Kartı ile Ödeme",
                            PaymentDate = DateTime.Today
                        };
                        _paymentRepo.Create(payment);
                        return RedirectToAction("Orders","HomePage");
                    }return NotFound();
                }return NotFound();
            }
            return NotFound();
        } 
    
        [HttpPost]
        public async Task<IActionResult> SplitByOrder(double price,int orderid,int tableid,string? paymentMethod)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var order = _orderRepo.Entites.FirstOrDefault(o => o.OrderId == orderid && o.MandantID == user.MandantID);
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId == tableid && t.MandantID == user.MandantID);
                   
                    if(order != null && table != null)
                    {
                        if(paymentMethod != null)
                        {
                            if(order.Price <= 0)
                            {
                                order.Price = 0;    
                                order.isActive = false;
                                table.isEmpty = false;
                                _orderRepo.Update(order);
                                _tableRepo.Update(table);
                                return RedirectToAction("Orders","HomePage"); 
                            }
                            order.Price = order.Price - price;
                            _orderRepo.Update(order);   
                            var payment = new Payment
                            {
                                MandantID = user.MandantID,
                                PaymentPrice = price,
                                PaymentMethod = "Kredi Kartı ile Ödeme",
                                PaymentDate = DateTime.Today
                            };
                            _paymentRepo.Create(payment);
                                
                            return RedirectToAction("OrderClosing",new {tableId = tableid,orderId = orderid}); 
                        }else{
                            if(order.Price <= 0)
                            {
                                order.Price = 0;    
                                order.isActive = false;
                                table.isEmpty = false;
                                _orderRepo.Update(order);
                                _tableRepo.Update(table);
                                return RedirectToAction("Orders","HomePage"); 
                            }
                            order.Price = order.Price - price;
                            _orderRepo.Update(order);   
                            var payment = new Payment
                            {
                                MandantID = user.MandantID,
                                PaymentPrice = price,
                                PaymentMethod = "Nakit Ödeme",
                                PaymentDate = DateTime.Today
                            };
                            _paymentRepo.Create(payment);
                                
                            return RedirectToAction("OrderClosing",new {tableId = tableid,orderId = orderid});
                        }  
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DebitToCustomer(int tableId,int orderId,int customerId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId == tableId && t.MandantID == user.MandantID);
                    var order = _orderRepo.Entites.FirstOrDefault(t => t.OrderId == orderId && t.MandantID == user.MandantID);
                    var customer = _customerRepo.Entites.FirstOrDefault(t => t.CustomerId == customerId && t.MandantID == user.MandantID);
                    if(table != null && order != null && customer != null)
                    {
                        customer.CustomerBalance += order.Price;
                        _customerRepo.Update(customer);
                        order.isActive = false;
                        _orderRepo.Update(order);
                        table.isEmpty = false;
                        _tableRepo.Update(table);
                        return RedirectToAction("Orders","HomePage");
                    }return NotFound();
                }return NotFound();
            }return NotFound();
        }
    
    
        public async Task<IActionResult> ChangeOrder(int tableId,int orderId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId == tableId && t.MandantID == user.MandantID);
                    var order = _orderRepo.Entites.FirstOrDefault(t => t.OrderId == orderId && t.MandantID == user.MandantID);
                    if(table != null && order != null)
                    {
                        var changeViewModel = new ChangeOrderViewModel{
                            Order = order,
                            Table = table,
                            Tables = _tableRepo.Entites.Where(t => t.isEmpty == false && t.MandantID == user.MandantID).ToList(),    
                            Areas = _areaRepo.Entites.Where(t => t.MandantID == user.MandantID).ToList()
                        };  
                        return View(changeViewModel);
                    }return NotFound();
                }return NotFound();
            }return NotFound();
           
        }

        public async Task<IActionResult> ChangeOrderDoing(int tableId,int orderId,int updatedTableId)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    var table = _tableRepo.Entites.FirstOrDefault(t => t.TableId == tableId && t.MandantID == user.MandantID);
                    var updatedTable = _tableRepo.Entites.FirstOrDefault(t => t.TableId == updatedTableId && t.MandantID == user.MandantID);
                    var order = _orderRepo.Entites.FirstOrDefault(t => t.OrderId == orderId && t.MandantID == user.MandantID);
                    if(table != null && order != null && updatedTable != null)
                    {
                        order.Table = table;
                        _orderRepo.Update(order);
                        table.isEmpty = true;
                        updatedTable.isEmpty = false;
                        _tableRepo.Update(table);
                        _tableRepo.Update(updatedTable);
                        return RedirectToAction("Statistics","HomePage");
                    }return NotFound();
                }return NotFound();
            }return NotFound();
            
        }  
    }
}