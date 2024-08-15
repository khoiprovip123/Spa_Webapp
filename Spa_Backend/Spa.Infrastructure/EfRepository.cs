using Microsoft.EntityFrameworkCore;
using Spa.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class EfRepository<T> where T : class
    {
        public readonly SpaDbContext _spaDbContext;

        //dbcontext

        public EfRepository(SpaDbContext spaDbContext)
        {
            _spaDbContext = spaDbContext;
        }

        public IEnumerable<T> GetAll()
        {
            return _spaDbContext.Set<T>().ToList();
        }

        public void Add(T entity)
        {
            _spaDbContext.Set<T>().Add(entity);
            _spaDbContext.SaveChanges();
        }

        public T GetById(long id)
        {
            return _spaDbContext.Set<T>().Find(id);
        }
        public T GetByEmail(string email)
        {
            return _spaDbContext.Set<T>().Find(email);
        }

        public void Update(T entity)
        {
            _spaDbContext.Set<T>().Update(entity);
            _spaDbContext.SaveChanges();

        }

        public void DeleteById(T entity)
        {
            //  var entity = GetById(id);
            _spaDbContext.Set<T>().Remove(entity);
            _spaDbContext.SaveChanges();
        }


    }
}
