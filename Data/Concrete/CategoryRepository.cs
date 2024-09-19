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
    public class CategoryRepository : ICategoryRepository
    {
        private UserManager<AppUser> _userManager;
        private readonly EfPosContext _context;
        public CategoryRepository(UserManager<AppUser> userManager,EfPosContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IQueryable<Category> Entites => _context.Categories;

        public void Create(Category entity)
        {
            if(entity != null)
            {
                _context.Categories.Add(entity);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
             var category = _context.Categories.FirstOrDefault(a => a.CategoryId == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public void Update(Category entity)
        {
            if(entity != null)
            {
                _context.Categories.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}