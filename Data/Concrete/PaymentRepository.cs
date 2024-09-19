using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Data.Concrete.EfCoreIdentity;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Entities;

namespace DmlCafePos.Data.Concrete
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly EfPosContext _context;
        public PaymentRepository(EfPosContext context)
        {
            _context = context;
        }
        public IQueryable<Payment> Entites => _context.Payments;

        public void Create(Payment entity)
        {
            if(entity != null)
            {
                _context.Payments.Add(entity);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var entity = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
            if(entity != null)
            {
                _context.Payments.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(Payment entity)
        {
            throw new NotImplementedException();
        }
    }
}