using System;

namespace ExpenseApproval.Model
{
    public class ExpenseException : Exception
    {
        public ExpenseException(string message) : base(message)
        {
        }

        public ExpenseException(string message, int id) : base(string.Format(message,id))
        {
        }

        public ExpenseException(string message, string id) : base(string.Format(message, id))
        {
        }
    }
}
