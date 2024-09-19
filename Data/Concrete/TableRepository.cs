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
    public class TableRepository : ITableRepository
    {
        private readonly EfPosContext _context;
        private UserManager<AppUser> _userManager;

        public TableRepository(EfPosContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IQueryable<Table> Entites => _context.Tables;

        public void Create(Table entity)
        {
            if (entity != null)
            {
                _context.Tables.Add(entity);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var table = _context.Tables.FirstOrDefault(a => a.TableId == id);
            if (table != null)
            {
                _context.Tables.Remove(table);
                _context.SaveChanges();
            }
        }

        public void Update(Table updatedTable)
        {
            var entity = _context.Tables.FirstOrDefault(a => a.TableId == updatedTable.TableId);
           if (entity != null)
            {
                entity.isEmpty = updatedTable.isEmpty;
                _context.SaveChanges();
            }
        }

       
    }
}