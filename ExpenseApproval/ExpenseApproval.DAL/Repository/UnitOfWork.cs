using System;
using ExpenseApproval.Entity;
using System.Collections.Generic;

namespace ExpenseApproval.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ExpenseApprovalContext _dbContext;
        private bool disposed;
        private Dictionary<string, object> repositories;

        public UnitOfWork(ExpenseApprovalContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Repository<T> Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);
                repositories.Add(type, repositoryInstance);
            }
            return (Repository<T>)repositories[type];
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}