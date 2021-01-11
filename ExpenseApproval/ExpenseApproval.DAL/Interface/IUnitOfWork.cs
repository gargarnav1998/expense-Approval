using System;

namespace ExpenseApproval.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Repository<T> Repository<T>() where T : class;
    }
}