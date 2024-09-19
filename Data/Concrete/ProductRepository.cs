
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Data.Concrete.EfCoreIdentity;
using Dmlcafepos.Entities;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace DmlCafePos.Data.Concrete
{
    
    public class ProductRepository : IProductRepository
    {
        private UserManager<AppUser> _userManager;
        private readonly EfPosContext _context;
        public ProductRepository(UserManager<AppUser> userManager,EfPosContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IQueryable<Product> Entites => _context.Products;

        public void Create(Product entity)
        {
            if(entity != null)
            {
                _context.Products.Add(entity);
                _context.SaveChanges();
            }
            
        }


        public void Delete(int id)
        {
            var entity = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if(entity != null)
            {
                _context.Products.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(Product updatedProduct)
        {
            var entity = _context.Products.FirstOrDefault(p=> p.ProductId == updatedProduct.ProductId);
            if(entity != null){
                entity.ProductName = updatedProduct.ProductName;
                entity.Description = updatedProduct.Description;
                entity.ProductPurchasePrice = updatedProduct.ProductPurchasePrice;
                entity.ProductPrice = updatedProduct.ProductPrice;
                entity.ImageUrl = updatedProduct.ImageUrl;
                _context.SaveChanges();
            }    

        } 
    }
}