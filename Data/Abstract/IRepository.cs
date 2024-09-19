using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DmlCafePos.Data.Abstract
{
    public interface IRepository<T>
    {
        IQueryable<T> Entites {get;}
        void Create(T entity);
        void Delete(int id);
        void Update(T entity);
    }
}