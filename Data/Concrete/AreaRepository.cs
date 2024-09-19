using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Data.Concrete.EfCoreIdentity;
using Dmlcafepos.Entities;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DmlCafePos.Data.Concrete
{
    public class AreaRepository : IAreaRepository
    {
        private readonly EfPosContext _context;
        private UserManager<AppUser> _userManager;
        public AreaRepository(EfPosContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IQueryable<Area> Entites => _context.Areas;

        public void Create(Area entity)
        {

            if (entity != null)
            {
                _context.Areas.Add(entity);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var area = _context.Areas.FirstOrDefault(a => a.AreaId == id);
            if (area != null)
            {
                _context.Areas.Remove(area);
                _context.SaveChanges();
            }
        }

        public void Update(Area entity)
        {
            if (entity != null)
            {
                _context.Areas.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}