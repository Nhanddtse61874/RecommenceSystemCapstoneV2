using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;
using Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IRepository<T> where T : BaseModel
    {
        IList<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includeProperties);
        T Get(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);

        void  Create(T model);

        void Update(T model);

        void Delete(int id);
    }
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public Repository()
        {

        }
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }
        public void Create(T model)
        {
             _dbContext.Add(model);
        }

        public void Delete(int id)
        {
             T entityToDelete = _dbSet.Find(id);
            _dbContext.Remove(entityToDelete);
        }

        public T Get(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties)
        {
            var result = IncludeProperties(includeProperties);
            return result.FirstOrDefault(filter);
        }


        public IList<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
        {
            var result = IncludeProperties(includeProperties);
            if (filter != null)
            {
                result = result.Where(filter);
            }
            if (orderBy != null)
            {
                result = orderBy(result);
            }
            return result.ToList();
        }

        public void Update(T model)
        {
            _dbContext.Attach(model);
            _dbContext.Entry(model).State = EntityState.Modified;
;        }

        private IQueryable<T> IncludeProperties(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> result = _dbSet;
            foreach (var includeProperty in includeProperties)
            {
                result = result.Include(includeProperty);
            }
            return result.AsExpandable();
        }
    }
}
