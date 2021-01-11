using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpenseApproval.DAL
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int Id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Insert(T model);
        void Update(T model);
        void Delete(int id);
    }
}