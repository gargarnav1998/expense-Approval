using System.Collections.Generic;
using ExpenseApproval.Services;
using ExpenseApproval.Model;

namespace ExpenseApproval.BLL
{
    public class UserExpenseList:IUserExpenseList
    {
        #region private members

        private IUserExpenseService _userExpenseDetails;
        private IEmployeeServices _employeeServices;
        #endregion

        #region constructor

        public UserExpenseList(IUserExpenseService userExpenseList, IEmployeeServices employeeServices)
        {
            _userExpenseDetails = userExpenseList;
            _employeeServices = employeeServices;
        }

        #endregion

        #region API Method

        public IList<UserExpenseModel> GetExpenseList(int employeeId)
        {
            IList<int> employeeIds = _employeeServices.GetEmployeeIds();
            IList<UserExpenseModel> expenseList =_userExpenseDetails.ExpenseList(employeeId);
            if(employeeIds.Contains(employeeId))
            {
                if (expenseList.Count != 0)
                {
                    return expenseList;
                }
                else
                {
                    throw new NoRecordFound("in user expense details");
                }
            }
            else
            {
                throw new EmployeeNotFound("");
            }
            
        }

        #endregion
    }
}