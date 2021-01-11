using System.Collections.Generic;
using ExpenseApproval.Entity;

namespace ExpenseApproval.Services
{
    public interface IParticipantServices
    {
        void Add(ExpenseParticipant participant);
        IEnumerable<ExpenseParticipant> GetParticipantByEmployeeIdAndExpenseTypeId(int employeeId, int expenseTypeId);
        IEnumerable<ExpenseParticipant> GetAllByExpenseId(int expenseId);
        void Edit(ExpenseParticipant participant);
        void Delete(int expenseParticipantId);
        ExpenseParticipant GetById(int expenseParticipantId);
        IEnumerable<ExpenseParticipant> GetAll();
        bool CheckIfParticipant(int expenseParticipantId);
        IEnumerable<ExpenseParticipant> GetParticipantByApproverId(int approverId);
    }
}

