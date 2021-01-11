using System.Collections.Generic;
using System.Web;
using ExpenseApproval.Model;

namespace ExpenseApproval.BLL
{
    public interface IExpenseDetails
    {
        IList<ExpenseTypeModel> ExpenseTypes();
        IList<ExpenseViewModel> GetAllByEmployeeId(int employeeId, string role, bool isApprover);
        HttpResponse DownloadBillForExpense(HttpResponse response, int expenseId);
        ExpenseDetailModel Create(ExpenseDetailModel expense, HttpRequest httpRequest, int creatorId);
        ApproveExpenseModel Approve(ApproveExpenseModel participant, int expenseParticipantId, int approverId);
        ExpenseDetailModel GetById(int expenseId, int employeeId);
        ExpenseDetailModel Edit(ExpenseDetailModel editExpense, int creatorId);
        void Delete(int expenseId, int creatorId);
    }
}