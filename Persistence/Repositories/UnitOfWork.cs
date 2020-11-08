using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DataAccess;
using Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : BaseModel;

        void Save();
    }

    public class UnitOfWork : IDisposable, IUnitOfWork
    {

        private readonly RSDbContext _dbContext;

        public UnitOfWork(RSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       


        public IRepository<T> Repository<T>() where T : BaseModel
        {
            return new Repository<T>(_dbContext);
        }//factory method - design pattern

        public void Save()
        {
           _dbContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                    //...
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
