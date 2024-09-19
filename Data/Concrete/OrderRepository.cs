using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Data.Concrete.EfCoreIdentity;
using Dmlcafepos.Entities;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Entities;
using Microsoft.AspNetCore.Identity;

namespace DmlCafePos.Data.Concrete
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EfPosContext _context;
        private UserManager<AppUser> _userManager;
        public OrderRepository(EfPosContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IQueryable<Order> Entites => _context.Orders;

        public void Create(Order entity)
        {
            if (entity != null)
            {   
                entity.isActive = true;
                _context.Orders.Add(entity);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var order = _context.Orders.FirstOrDefault(a => a.OrderId == id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public void Update(Order updatedOrder)
        {
            var entity = _context.Orders.FirstOrDefault(p=> p.OrderId == updatedOrder.OrderId);
            if(entity != null){
                entity.Table = updatedOrder.Table;
                entity.Price = updatedOrder.Price;
                entity.Spread = updatedOrder.Spread;
                entity.OrderItems = updatedOrder.OrderItems;
                entity.orderDate = updatedOrder.orderDate;
                _context.SaveChanges();
            } 
        }

       
    }
}