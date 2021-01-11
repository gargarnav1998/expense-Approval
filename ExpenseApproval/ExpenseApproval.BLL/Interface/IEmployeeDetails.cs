using System.Collections.Generic;
using ExpenseApproval.Model;

namespace ExpenseApproval.BLL
{
    public interface IEmployeeDetails
    {
        IList<EmployeeModel> GetAll();
        IList<BudgetModel> GetBudgetDetails(int employeeId);
    }
}