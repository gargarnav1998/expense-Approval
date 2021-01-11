using System.Collections.Generic;
using ExpenseApproval.Entity;
using ExpenseApproval.Model;
using System;
using ExpenseApproval.DAL;
using System.Linq;

namespace ExpenseApproval.BLL
{
    public class EmployeeDetails : IEmployeeDetails
    {
        #region Private members

        private IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        public EmployeeDetails(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        /// <summary>
        /// Returning the Employee and Approver details
        /// </summary>
        /// <returns></returns>
        public IList<EmployeeModel> GetAll()
        {
            IEnumerable<Employee> employees = _unitOfWork.Repository<Employee>().GetAll();
            IList<EmployeeModel> employeeModels = new List<EmployeeModel>();
            Mapping.MapEmployeeModel(employees, employeeModels);
            return employeeModels;
        }

        /// <summary>
        /// Send calculated budget of an employee
        /// </summary>
        /// <param name="employeeId"></param>
        public IList<BudgetModel> GetBudgetDetails(int employeeId)
        {
            IList<BudgetModel> budgetModels = new List<BudgetModel>();
            IEnumerable<ExpenseType> expenseTypes = _unitOfWork.Repository<ExpenseType>().GetAll();
            IList<ExpenseParticipant> expenseParticipantDetails = _unitOfWork.Repository<ExpenseParticipant>().FindBy(x => x.EmployeeId == employeeId).ToList();

            foreach (ExpenseType expenseType in expenseTypes)
            {
                decimal approvedAmount = expenseParticipantDetails.Where(X => X.Expense.ExpenseTypeId == expenseType.ExpenseTypeId)
                    .Select(x => x.AmountApproved).ToList().Sum() ?? 0;
                decimal claimedAmount = expenseParticipantDetails.Where(X => X.Expense.ExpenseTypeId == expenseType.ExpenseTypeId)
                    .Select(x => x.AmountClaimed).ToList().Sum();

                budgetModels.Add(
                    new BudgetModel
                    {
                        ExpenseTypeId = expenseType.ExpenseTypeId,
                        ExpenseTypeName = expenseType.ExpenseName,
                        TotalBudget = expenseType.ExpenseAmount,
                        AvailableBudget = expenseType.ExpenseAmount - approvedAmount,
                        ApprovedBudget = approvedAmount,
                        ClaimedBudget = claimedAmount
                    });
            }
            return budgetModels;
        }
    }
}