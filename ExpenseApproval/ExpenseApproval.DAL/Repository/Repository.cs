using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System;
using ExpenseApproval.Entity;

namespace ExpenseApproval.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Private members

        private readonly ExpenseApprovalContext _dbContext;
        private IDbSet<T> _dbEntity;

        #endregion

        #region Constructor

        public Repository() { }

        public Repository(ExpenseApprovalContext dbContext)
        {
            _dbContext = dbContext;
            _dbEntity = _dbContext.Set<T>();
        }

        #endregion

        public virtual IEnumerable<T> GetAll()
        {
            return _dbEntity.ToList();
        }

        public T GetById(int Id)
        {
            return _dbEntity.Find(Id);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbEntity.Where(predicate);
            return query;
        }

        public void Insert(T model)
        {
            _dbEntity.Add(model);
        }

        public void Update(T model)
        {
            _dbContext.Entry(model).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            T model = _dbEntity.Find(id);
            _dbEntity.Remove(model);
        }
    }
}