using ExpenseApproval.Model;
using ExpenseApproval.Services;
using System.Collections.Generic;

namespace ExpenseApproval.BLL
{
    public class BudgetDetails : IBudgetDetails
    {
        #region Private Members

        private IExpenseTypeServices _expenseTypeServices;
        private IParticipantServices _participantServices;

        #endregion

        #region Private Methods

        private IList<BudgetModel> TotalBudget()
        {
            IList<BudgetModel> budgetModel = new List<BudgetModel>();
            var budgets = _expenseTypeServices.GetAll();
            foreach (var budget in budgets)
            {
                budgetModel.Add(
                    new BudgetModel
                    {
                        ExpenseTypeId = budget.ExpenseTypeId,
                        Budget = budget.ExpenseAmount
                    });
            }
            return budgetModel;
        }

        private IList<BudgetModel> ApprovedBudget(int employeeId)
        {
            IList<BudgetModel> approvedBudget = _participantServices.GetApprovedAmount(employeeId);
            return approvedBudget;
        }

        private IList<BudgetModel> ConsumedBudget(int employeeId)
        {
            IList<BudgetModel> consumedBudget = _participantServices.GetClaimedAmount(employeeId);
            return consumedBudget;
        }

        #endregion

        #region Constructor

        public BudgetDetails(IExpenseTypeServices expenseTypeServices, IParticipantServices participantServices)
        {
            _expenseTypeServices = expenseTypeServices;
            _participantServices = participantServices;
        }

        #endregion

        public BudgetDetailModel BudgetInfo(int employeeId)
        {
            IList<BudgetModel> budget = TotalBudget();
            IList<BudgetModel> approvedBudget = ApprovedBudget(employeeId);
            IList<BudgetModel> consumedBudget = ConsumedBudget(employeeId);
            IList<BudgetModel> availableBudget = new List<BudgetModel>();

            for (int i = 0; i < approvedBudget.Count; i++)
            {
                availableBudget.Add(
                    new BudgetModel
                    {
                        ExpenseTypeId = approvedBudget[i].ExpenseTypeId,
                        Budget = budget[approvedBudget[i].ExpenseTypeId - 1].Budget - approvedBudget[i].Budget
                    });
            }

            BudgetDetailModel budgetDetails = new BudgetDetailModel
            {
                TotalBudget = budget,
                ApprovedBudget = approvedBudget,
                ConsumedBudget = consumedBudget,
                AvailableBudget = availableBudget
            };
            return budgetDetails;
        }
    }
}