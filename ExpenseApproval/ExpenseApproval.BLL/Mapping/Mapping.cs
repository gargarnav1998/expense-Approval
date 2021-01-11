using System;
using System.Collections.Generic;
using ExpenseApproval.Model;
using ExpenseApproval.Entity;
using System.Web;
using ExpenseApproval.Utility;

namespace ExpenseApproval.BLL
{
    public class Mapping
    {
        public static void MapEmployeeModel(IEnumerable<Employee> employees, IList<EmployeeModel> employeeModels)
        {
            foreach (Employee employee in employees)
            {
                EmployeeModel employeeModel = new EmployeeModel
                {
                    Email = employee.EmployeeEmail,
                    Name = employee.EmployeeName,
                    EmployeeId = employee.EmployeeId,
                    EmployeeRoleId = employee.RoleId,
                    EmployeeRoleName = employee.Role.RoleName
                };
                employeeModels.Add(employeeModel);
            }
        }

        public static void MapExpenseTypeModel(IEnumerable<ExpenseType> expenseTypes, IList<ExpenseTypeModel> expenseTypeModels)
        {
            foreach (ExpenseType expenseType in expenseTypes)
            {
                ExpenseTypeModel expenseTypeModel = new ExpenseTypeModel
                {
                    ExpenseAmount = expenseType.ExpenseAmount,
                    ExpenseName = expenseType.ExpenseName,
                    ExpenseTypeId = expenseType.ExpenseTypeId,
                    IsIndividual = expenseType.IsIndividual
                };
                expenseTypeModels.Add(expenseTypeModel);
            }
        }

        public static void MapExpenseViewModel(ExpenseViewModel expenseViewModel, Expense expense)
        {
            expenseViewModel.BillImage = expense.BillImage;
            expenseViewModel.CreationDate = expense.CreationDate;
            expenseViewModel.ExpenseCreatorName = expense.CreatorEmployee.EmployeeName;
            expenseViewModel.ExpenseCreatorId = expense.ExpenseCreatorId;
            expenseViewModel.ExpenseDate = expense.ExpenseDate;
            expenseViewModel.ExpenseTypeName = expense.ExpenseType.ExpenseName;
            expenseViewModel.ImagePath = expense.ImagePath;
            expenseViewModel.ExpenseId = expense.ExpenseId;
            expenseViewModel.ApproverName = expense.ApproverEmployee.EmployeeName;
            expenseViewModel.ApproverId = expense.ApproverId;
        }

        public static void MapExpense(ExpenseDetailModel expenseDetailModel, Expense expense)
        {
            expense.ApproverId = expenseDetailModel.ApproverId;
            expense.ExpenseTypeId = expenseDetailModel.ExpenseTypeId;
            expense.BillImage = expenseDetailModel.BillImage;
            expense.ExpenseDate = Convert.ToDateTime(expenseDetailModel.ExpenseDate);
            expense.ExpensePurpose = expenseDetailModel.Purpose;
        }

        public static ExpenseDetailModel HttpRequestMapping(HttpRequest httpRequest)
        {
            ExpenseDetailModel expenseDetailModel = new ExpenseDetailModel();
            expenseDetailModel.ApproverId = int.TryParse(httpRequest.Form[Constant.Approver], out int ApproverId) ? ApproverId : throw new ExpenseException(Constant.ErrorMessageInvalidApproverId, httpRequest.Form[Constant.Approver]);
            expenseDetailModel.ExpenseTypeId = int.TryParse(httpRequest.Form[Constant.ExpenseTypeId], out int ExpenseTypeId) ? ExpenseTypeId : throw new ExpenseException(Constant.ErrorMessageInvalidExpenseType);
            expenseDetailModel.Purpose = httpRequest.Form[Constant.ExpensePurpose];
            expenseDetailModel.AmountClaimed = int.TryParse(httpRequest.Form[Constant.AmountClaimed], out int AmountClaimed) ? AmountClaimed : throw new ExpenseException(Constant.ErrorMessageAmountNotInteger);
            expenseDetailModel.ExpenseDate = DateTime.Parse(httpRequest.Form[Constant.ExpenseDate]);
            IList<ParticipantDetailModel> participantDetailModels = new List<ParticipantDetailModel>();
            int count = default(int);
            foreach (string key in httpRequest.Form.AllKeys)
            {
                if (key.EndsWith(Constant.KeyEnd))
                {
                    ParticipantDetailModel participantDetailModel = new ParticipantDetailModel
                    {
                        AmountClaimed = int.TryParse(httpRequest.Form[Constant.ParticipantDetails + count + Constant.ParticipantAmountClaimed], out int ParticipantAmountClaimed) ? ParticipantAmountClaimed : throw new ExpenseException(Constant.ErrorMessageAmountNotInteger),
                        EmployeeId = int.TryParse(httpRequest.Form[Constant.ParticipantDetails + count + Constant.ParticipantEmployeeId], out int ParticipantEmployeeId) ? ParticipantEmployeeId : throw new ExpenseException(Constant.ErrorMessageInvalidParticipantId, httpRequest.Form[Constant.ParticipantDetails + count + Constant.ParticipantEmployeeId])
                    };
                    participantDetailModels.Add(participantDetailModel);
                    count++;
                }
            }
            expenseDetailModel.ParticipantDetails = participantDetailModels;
            return expenseDetailModel;
        }

        public static void MapExpenseDetailModel(ExpenseDetailModel expenseDetailModel, Expense expense)
        {
            expenseDetailModel.ApproverId = expense.ApproverId;
            expenseDetailModel.ApproverName = expense.ApproverEmployee.EmployeeName;
            expenseDetailModel.BillImage = expense.BillImage;
            expenseDetailModel.ExpenseDate = expense.ExpenseDate;
            expenseDetailModel.TypeName = expense.ExpenseType.ExpenseName;
            expenseDetailModel.Purpose = expense.ExpensePurpose;
            expenseDetailModel.ExpenseTypeId = expense.ExpenseTypeId;
            expenseDetailModel.ImagePath = expense.ImagePath;
            expenseDetailModel.ExpenseId = expense.ExpenseId;
        }
    }
}
