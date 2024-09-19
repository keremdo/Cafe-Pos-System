using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Data.Concrete.EfCoreIdentity;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Entities;

namespace DmlCafePos.Data.Concrete
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly EfPosContext _context;
        public CustomerRepository(EfPosContext context)
        {
            _context = context;
        }
        public IQueryable<Customer> Entites => _context.Customers;

        public void Create(Customer entity)
        {
            if(entity != null)
            {
                _context.Customers.Add(entity);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var customer = _context.Customers.FirstOrDefault(a => a.CustomerId == id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        public void Update(Customer updatedCustomer)
        {
            var entity = _context.Customers.FirstOrDefault(c=> c.CustomerId == updatedCustomer.CustomerId);
            if(entity != null)
            {
                entity.CustomerName = updatedCustomer.CustomerName;
                entity.CustomerSurname = updatedCustomer.CustomerSurname;
                entity.CustomerPhone = updatedCustomer.CustomerPhone;
                entity.CustomerBalance = updatedCustomer.CustomerBalance;
                _context.SaveChanges();
            }

        }
    }
}