using System.Collections.Generic;
using ExpenseApproval.Model;
using ExpenseApproval.Entity;
using System.Linq;
using ExpenseApproval.Utility;
using System.Web;
using System;
using System.Configuration;
using ExpenseApproval.DAL;

namespace ExpenseApproval.BLL
{
    public class ExpenseDetails : IExpenseDetails
    {
        #region Private members

        private IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        public ExpenseDetails(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Private methods

        private void CreateExpenseForIndividual(ExpenseDetailModel expenseDetailModel, int creatorId, IList<ParticipantDetailModel> participantDetailModels)
        {
            if (expenseDetailModel.ParticipantDetails.Any())
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidParticipantCreated);
            }

            ExpenseParticipant participant = new ExpenseParticipant
            {
                EmployeeId = creatorId,
                AmountClaimed = expenseDetailModel.AmountClaimed,
                ExpenseStatusId = Constant.PendingStateId
            };
            _unitOfWork.Repository<ExpenseParticipant>().Insert(participant);

            ParticipantDetailModel participantDetailModel = new ParticipantDetailModel
            {
                EmployeeId = participant.EmployeeId,
                AmountClaimed = participant.AmountClaimed,
                EmployeeName = _unitOfWork.Repository<Employee>().GetById(creatorId).EmployeeName
            };
            participantDetailModels.Add(participantDetailModel);
        }

        private void CreateExpenseForShared(ExpenseDetailModel expenseDetailModel, IList<ParticipantDetailModel> participantDetailModels)
        {
            if (!expenseDetailModel.ParticipantDetails.Any())
            {
                throw new ExpenseException(Constant.ErrorMessageParticipantNotAdded);
            }

            if (expenseDetailModel.ParticipantDetails.GroupBy(x => x.EmployeeId).Any(y => y.Count() > Constant.CountofEmployee))
            {
                throw new ExpenseException(Constant.ErrorMessageDuplicateRecord);
            }

            decimal sumOfAmount = expenseDetailModel.ParticipantDetails.Select(x => x.AmountClaimed).Sum();
            if (expenseDetailModel.AmountClaimed != sumOfAmount)
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidAmount);
            }

            foreach (ParticipantDetailModel participant in expenseDetailModel.ParticipantDetails)
            {
                Employee employee = _unitOfWork.Repository<Employee>().GetById(participant.EmployeeId);
                if (employee == null)
                {
                    throw new ExpenseException(Constant.ErrorMessageInvalidParticipantId, participant.EmployeeId);
                }

                ExpenseParticipant expenseParticipant = new ExpenseParticipant
                {
                    AmountClaimed = participant.AmountClaimed,
                    EmployeeId = participant.EmployeeId,
                    ExpenseStatusId = Constant.PendingStateId
                };
                _unitOfWork.Repository<ExpenseParticipant>().Insert(expenseParticipant);
                participantDetailModels.Add(new ParticipantDetailModel
                {
                    EmployeeId = participant.EmployeeId,
                    EmployeeName = employee.EmployeeName,
                    AmountClaimed = participant.AmountClaimed
                });
            }
        }

        private IList<ExpenseViewModel> ExpensesForEmployee(IList<Expense> expenses, int employeeId)
        {
            IList<ExpenseViewModel> expenseViewModels = new List<ExpenseViewModel>();
            IEnumerable<ExpenseParticipant> allExpenseParticipants = _unitOfWork.Repository<ExpenseParticipant>().GetAll();
            foreach (Expense expense in expenses)
            {
                ExpenseViewModel expenseViewModel = new ExpenseViewModel();
                Mapping.MapExpenseViewModel(expenseViewModel, expense);
                ExpenseParticipant participant = expense.ExpenseParticipants.FirstOrDefault(x => x.EmployeeId == employeeId);
                
                if (participant != null)
                {
                    decimal amountApproved = allExpenseParticipants
                    .Where(x => (x.Expense.ExpenseTypeId == participant.Expense.ExpenseTypeId) && (x.EmployeeId == participant.EmployeeId))
                    .Select(x => x.AmountApproved).Sum() ?? 0;
                    decimal availableBudget = participant.Expense.ExpenseType.ExpenseAmount - amountApproved;

                    expenseViewModel.EmployeeName = participant.Employee.EmployeeName;
                    expenseViewModel.AmountApproved = participant.AmountApproved;
                    expenseViewModel.AmountClaimed = participant.AmountClaimed;
                    expenseViewModel.ApprovalStatus = participant.ExpenseStatus.ExpenseApproval;
                    expenseViewModel.ParticipantId = participant.ExpenseParticipantId;
                    expenseViewModel.AmountAvailable = availableBudget;
                }
                else
                {
                    expenseViewModel.EmployeeName = Constant.NotIncludedInExpense;
                    expenseViewModel.AmountApproved = null;
                    expenseViewModel.AmountClaimed = Constant.InitialAmount;
                    expenseViewModel.ApprovalStatus = Constant.NotIncludedInExpense;
                    expenseViewModel.ParticipantId = Constant.InitialId;
                }
                expenseViewModels.Add(expenseViewModel);
            }
            return expenseViewModels;
        }

        private IList<ExpenseViewModel> ExpensesForApproval(IList<ExpenseParticipant> expenseParticipants, int employeeId)
        {
            IList<ExpenseViewModel> expenseViewModels = new List<ExpenseViewModel>();
            IEnumerable<ExpenseParticipant> allExpenseParticipants = _unitOfWork.Repository<ExpenseParticipant>().GetAll();

            foreach (ExpenseParticipant participant in expenseParticipants)
            {
                decimal amountApproved = allExpenseParticipants
                    .Where(x => (x.Expense.ExpenseTypeId == participant.Expense.ExpenseTypeId) && (x.EmployeeId == participant.EmployeeId))
                    .Select(x => x.AmountApproved).Sum() ?? 0;
                decimal availableBudget = participant.Expense.ExpenseType.ExpenseAmount - amountApproved;
                ExpenseViewModel expenseViewModel = new ExpenseViewModel();
                Mapping.MapExpenseViewModel(expenseViewModel, participant.Expense);
                expenseViewModel.EmployeeName = participant.Employee.EmployeeName;
                expenseViewModel.AmountApproved = participant.AmountApproved;
                expenseViewModel.AmountClaimed = participant.AmountClaimed;
                expenseViewModel.ApprovalStatus = participant.ExpenseStatus.ExpenseApproval;
                expenseViewModel.ParticipantId = participant.ExpenseParticipantId;
                expenseViewModel.AmountAvailable = availableBudget;
                expenseViewModels.Add(expenseViewModel);
            }
            return expenseViewModels;
        }

        private void EditExpenseForIndividual(ExpenseDetailModel expenseDetailModel, Expense expense, int creatorId, IList<ParticipantDetailModel> participantDetailModels, ExpenseType expenseTypeForOldExpense)
        {
            if (expenseDetailModel.ParticipantDetails.Any())
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidParticipantCreated);
            }

            if (!expenseTypeForOldExpense.IsIndividual)
            {
                IList<ExpenseParticipant> expenseParticipants = expense.ExpenseParticipants.ToList();
                foreach (ExpenseParticipant expenseParticipant in expenseParticipants)
                {
                    _unitOfWork.Repository<ExpenseParticipant>().Delete(expenseParticipant.ExpenseParticipantId);
                }

                ExpenseParticipant participant = new ExpenseParticipant()
                {
                    EmployeeId = creatorId,
                    AmountClaimed = expenseDetailModel.AmountClaimed,
                    ExpenseId = expenseDetailModel.ExpenseId,
                    ExpenseStatusId = Constant.PendingStateId
                };
                _unitOfWork.Repository<ExpenseParticipant>().Insert(participant);
            }
            else
            {
                ExpenseParticipant expenseParticipant = expense.ExpenseParticipants.FirstOrDefault();
                expenseParticipant.EmployeeId = creatorId;
                expenseParticipant.AmountClaimed = expenseDetailModel.AmountClaimed;
                _unitOfWork.Repository<ExpenseParticipant>().Update(expenseParticipant);
            }
            Employee creatorEmployee = _unitOfWork.Repository<Employee>().GetById(creatorId);
            ParticipantDetailModel participantDetailModel = new ParticipantDetailModel
            {
                EmployeeName = creatorEmployee.EmployeeName,
                EmployeeId = creatorId,
                AmountClaimed = expenseDetailModel.AmountClaimed
            };
            participantDetailModels.Add(participantDetailModel);
        }

        private void EditExpenseForShared(ExpenseDetailModel expenseDetailModel, Expense expense, int creatorId, IList<ParticipantDetailModel> participantDetailModels)
        {
            if (!expenseDetailModel.ParticipantDetails.Any())
            {
                throw new ExpenseException(Constant.ErrorMessageParticipantNotAdded);
            }

            if (expenseDetailModel.ParticipantDetails.GroupBy(x => x.EmployeeId).Any(y => y.Count() > Constant.CountofEmployee))
            {
                throw new ExpenseException(Constant.ErrorMessageDuplicateRecord);
            }

            decimal sumOfAmount = expenseDetailModel.ParticipantDetails.Select(x => x.AmountClaimed).Sum();
            if (expenseDetailModel.AmountClaimed != sumOfAmount)
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidAmount);
            }

            IList<int> idsToInsert = expenseDetailModel.ParticipantDetails.Select(x => x.EmployeeId).Except(expense.ExpenseParticipants.Select(x => x.EmployeeId)).ToList();
            IList<int> idsToDelete = expense.ExpenseParticipants.Select(x => x.EmployeeId).Except(expenseDetailModel.ParticipantDetails.Select(x => x.EmployeeId)).ToList();
            IList<int> idsToUpdate = expenseDetailModel.ParticipantDetails.Select(x => x.EmployeeId).Intersect(expense.ExpenseParticipants.Select(x => x.EmployeeId)).ToList();

            foreach (ParticipantDetailModel participantDetailModel in expenseDetailModel.ParticipantDetails)
            {
                Employee employee = _unitOfWork.Repository<Employee>().GetById(participantDetailModel.EmployeeId);
                if (employee == null)
                {
                    throw new ExpenseException(Constant.ErrorMessageInvalidParticipantId, participantDetailModel.EmployeeId);
                }

                if (idsToUpdate.Contains(participantDetailModel.EmployeeId))
                {
                    ExpenseParticipant expenseParticipant = expense.ExpenseParticipants.Where(x => x.EmployeeId == participantDetailModel.EmployeeId).FirstOrDefault();
                    expenseParticipant.AmountClaimed = participantDetailModel.AmountClaimed;
                    _unitOfWork.Repository<ExpenseParticipant>().Update(expenseParticipant);
                }

                if (idsToInsert.Contains(participantDetailModel.EmployeeId))
                {
                    ExpenseParticipant expenseParticipant = new ExpenseParticipant
                    {
                        AmountClaimed = participantDetailModel.AmountClaimed,
                        EmployeeId = participantDetailModel.EmployeeId,
                        ExpenseId = expenseDetailModel.ExpenseId,
                        ExpenseStatusId = Constant.PendingStateId
                    };
                    _unitOfWork.Repository<ExpenseParticipant>().Insert(expenseParticipant);
                }

                participantDetailModels.Add(new ParticipantDetailModel
                {
                    EmployeeId = participantDetailModel.EmployeeId,
                    EmployeeName = employee.EmployeeName,
                    AmountClaimed = participantDetailModel.AmountClaimed
                });
            }

            foreach (int idToDelete in idsToDelete)
            {
                int participantId = expense.ExpenseParticipants.Where(x => x.EmployeeId == idToDelete).Select(x => x.ExpenseParticipantId).FirstOrDefault();
                _unitOfWork.Repository<ExpenseParticipant>().Delete(participantId);
            }
        }

        #endregion

        /// <summary>
        /// Sending Expense Details to front end
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="role"></param>
        /// <param name="forApproval"></param>
        /// <returns></returns>
        public IList<ExpenseViewModel> GetAllByEmployeeId(int employeeId, string role, bool forApproval)
        {
            IList<Expense> expenses;
            IList<Expense> expensesCreatedByEmployee;
            IList<Expense> expensesForEmployee;
            IList<ExpenseViewModel> expenseViewModels = new List<ExpenseViewModel>();

            if (forApproval)
            {
                IList<ExpenseParticipant> expenseParticipants = _unitOfWork.Repository<ExpenseParticipant>().FindBy(x => x.Expense.ApproverId == employeeId).ToList();
                if(expenseParticipants == null)
                {
                    return expenseViewModels;
                }
                expenseViewModels = ExpensesForApproval(expenseParticipants, employeeId);
            }
            else
            {
                expensesForEmployee = _unitOfWork.Repository<Expense>().FindBy(x => x.ExpenseParticipants.Where(y => y.EmployeeId == employeeId).Select(y => y.EmployeeId).Contains(employeeId)).ToList();
                expensesCreatedByEmployee = _unitOfWork.Repository<Expense>().FindBy(x => x.ExpenseCreatorId == employeeId).ToList();
                IEnumerable<Expense> expensesForCreator = expensesCreatedByEmployee.Except(expensesForEmployee);
                expenses = expensesForEmployee.Concat(expensesForCreator).ToList();
                expenseViewModels = ExpensesForEmployee(expenses, employeeId);
            }

            if (expenseViewModels.Any())
            {
                return expenseViewModels.OrderByDescending(x => x.ExpenseDate).ToList();
            }
            else
            {
                throw new ExpenseException(Constant.ErrorMessageNoRecordFound, employeeId);
            }
        }

        /// <summary>
        /// Return the details for expense for Edit
        /// </summary>
        /// <param name="expenseId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public ExpenseDetailModel GetById(int expenseId, int employeeId)
        {
            Expense expense = _unitOfWork.Repository<Expense>().GetById(expenseId);
            if (expense == null)
            {
                throw new ExpenseException(Constant.ErrorMessageNoExpenseFound, expenseId);
            }

            if (!expense.ExpenseParticipants.Select(x => x.EmployeeId).Contains(employeeId) && employeeId != expense.ExpenseCreatorId && employeeId != expense.ApproverId)
            {
                throw new ExpenseException(Constant.ErrorMessageUnauthorized, employeeId);
            }

            decimal totalAmountClaimed = Constant.InitialAmount;
            IList<ParticipantDetailModel> participantModels = new List<ParticipantDetailModel>();
            ExpenseDetailModel expenseDetailModel = new ExpenseDetailModel();
            Mapping.MapExpenseDetailModel(expenseDetailModel, expense);

            foreach (ExpenseParticipant expenseParticipant in expense.ExpenseParticipants)
            {
                participantModels.Add(new ParticipantDetailModel
                {
                    EmployeeId = expenseParticipant.EmployeeId,
                    EmployeeName = expenseParticipant.Employee.EmployeeName,
                    AmountClaimed = expenseParticipant.AmountClaimed
                }
                );
                expenseDetailModel.ParticipantDetails = participantModels;
                totalAmountClaimed = totalAmountClaimed + expenseParticipant.AmountClaimed;
            }
            expenseDetailModel.AmountClaimed = totalAmountClaimed;
            return expenseDetailModel;
        }

        /// <summary>
        /// Creates stream of byte array in response to download bill image
        /// </summary>
        /// <param name="response"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public HttpResponse DownloadBillForExpense(HttpResponse response, int expenseId)
        {
            string fileName = _unitOfWork.Repository<Expense>().GetById(expenseId).ImagePath;
            response.ContentType = Constant.ContentType;
            response.AppendHeader(Constant.ContentDisposition, Constant.Attachment);
            response.TransmitFile(ConfigurationManager.AppSettings[Constant.BillImagePath] + fileName);
            response.End();
            return response;
        }

        /// <summary>
        /// Writing Expense details provided by user into database
        /// </summary>
        /// <param name="expenseDetailModel"></param>
        /// <param name="creatorId"></param>
        public ExpenseDetailModel Create(ExpenseDetailModel expenseDetailModel, HttpRequest httpRequest, int creatorId)
        {
            if (creatorId == expenseDetailModel.ApproverId)
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidApprover);
            }

            string fileName = default(string);
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    HttpPostedFile postedFile = httpRequest.Files[file];
                    bool validFor = postedFile.FileName.Contains("");
                    if (Constant.ValidFormats.Contains(postedFile.FileName.Split('.')[1]))
                    {
                        fileName = DateTime.Now.ToFileTime() + postedFile.FileName;
                        string filePath = System.Web.Hosting.HostingEnvironment.MapPath((ConfigurationManager.AppSettings[Constant.BillImagePath] + fileName));
                        postedFile.SaveAs(filePath);
                    }
                    else
                    {
                        throw new ExpenseException(Constant.ErrorMessageInvalidBillImageFormat);
                    }
                }
            }
            else
            {
                throw new ExpenseException(Constant.ErrorMessageNoImage);
            }

            Employee approverEmployee = _unitOfWork.Repository<Employee>().GetById(expenseDetailModel.ApproverId);
            if (approverEmployee == null)
            {
                throw new ExpenseException(Constant.ErrorMessageValidApproverId, expenseDetailModel.ApproverId);
            }

            ExpenseType expenseType = _unitOfWork.Repository<ExpenseType>().GetById(expenseDetailModel.ExpenseTypeId);
            if (expenseType == null)
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidExpenseType);
            }

            if (Constant.Approvers.Contains(approverEmployee.Role.RoleName))
            {
                Expense expense = new Expense();
                Mapping.MapExpense(expenseDetailModel, expense);
                expense.CreationDate = DateTime.Now;
                expense.ExpenseCreatorId = creatorId;
                expense.ImagePath = fileName;

                _unitOfWork.Repository<Expense>().Insert(expense);

                IList<ParticipantDetailModel> participantDetailModels = new List<ParticipantDetailModel>();

                if (!expenseType.IsIndividual)
                {
                    CreateExpenseForShared(expenseDetailModel, participantDetailModels);
                }
                else
                {
                    CreateExpenseForIndividual(expenseDetailModel, creatorId, participantDetailModels);
                }
                _unitOfWork.Commit();

                expenseDetailModel.ParticipantDetails = participantDetailModels;
                expenseDetailModel.TypeName = expenseType.ExpenseName;
                expenseDetailModel.ImagePath = fileName;
                expenseDetailModel.ExpenseId = expense.ExpenseId;
                expenseDetailModel.ApproverName = approverEmployee.EmployeeName;
            }
            else
            {
                throw new ExpenseException(Constant.ErrorMessageValidApproverId, expenseDetailModel.ApproverId);
            }
            return expenseDetailModel;
        }

        /// <summary>
        /// Updating the already existing expense
        /// </summary>
        /// <param name="expenseDetailModel"></param>
        /// <param name="creatorId"></param>
        /// <returns></returns>
        public ExpenseDetailModel Edit(ExpenseDetailModel expenseDetailModel, int creatorId)
        {
            Expense expense = _unitOfWork.Repository<Expense>().GetById(expenseDetailModel.ExpenseId);
            if (expense == null)
            {
                throw new ExpenseException(Constant.ErrorMessageNoExpenseFound, expenseDetailModel.ExpenseId);
            }

            if (creatorId != expense.ExpenseCreatorId)
            {
                throw new ExpenseException(Constant.ErrorMessageUnauthorizedAction);
            }

            if (creatorId == expenseDetailModel.ApproverId)
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidApprover);
            }

            ExpenseType expenseType = _unitOfWork.Repository<ExpenseType>().GetById(expenseDetailModel.ExpenseTypeId);
            if (expenseType == null)
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidExpenseType);
            }

            Employee approverEmployee = _unitOfWork.Repository<Employee>().GetById(expenseDetailModel.ApproverId);
            if (approverEmployee == null)
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidApproverId);
            }

            if (!Constant.Approvers.Contains(approverEmployee.Role.RoleName))
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidApproverId);
            }

            foreach (ExpenseParticipant expenseParticipant in expense.ExpenseParticipants)
            {
                if (expenseParticipant.ExpenseStatusId != Constant.PendingStateId)
                {
                    throw new ExpenseException(Constant.ErrorMessageNotValid);
                }
            }

            IList<ParticipantDetailModel> participantDetailModels = new List<ParticipantDetailModel>();
            ExpenseType expenseTypeforOldExpense = _unitOfWork.Repository<ExpenseType>().GetById(expense.ExpenseTypeId);
            Mapping.MapExpense(expenseDetailModel, expense);
            expense.ModifiedDate = DateTime.Now;
            expense.ExpenseCreatorId = creatorId;
            _unitOfWork.Repository<Expense>().Update(expense);

            if (!expenseType.IsIndividual)
            {
                EditExpenseForShared(expenseDetailModel, expense, creatorId, participantDetailModels);
            }
            else
            {
                EditExpenseForIndividual(expenseDetailModel, expense, creatorId, participantDetailModels, expenseTypeforOldExpense);
            }

            _unitOfWork.Commit();

            expenseDetailModel.TypeName = expense.ExpenseType.ExpenseName;
            expenseDetailModel.ApproverName = expense.ApproverEmployee.EmployeeName;
            expenseDetailModel.ParticipantDetails = participantDetailModels;
            return expenseDetailModel;
        }

        /// <summary>
        /// Updates Approval Status of the Expense
        /// </summary>
        /// <param name="approveExpense"></param>
        /// <param name="expenseParticipantId"></param>
        /// <param name="approverId"></param>
        /// <returns></returns>
        public ApproveExpenseModel Approve(ApproveExpenseModel approveExpense, int expenseParticipantId, int approverId)
        {
            ExpenseParticipant expenseParticipant = _unitOfWork.Repository<ExpenseParticipant>().GetById(expenseParticipantId);
            if (expenseParticipant == null)
            {
                throw new ExpenseException(Constant.ErrorMessageParticipantNotFound, expenseParticipantId);
            }

            if (approverId != expenseParticipant.Expense.ApproverId)
            {
                throw new ExpenseException(Constant.ErrorMessageUnauthorized, approverId);
            }

            if (expenseParticipant.ExpenseStatusId != Constant.PendingStateId)
            {
                throw new ExpenseException(Constant.ErrorMessageNotValid);
            }

            if (approveExpense.AmountApproved > expenseParticipant.AmountClaimed)
            {
                throw new ExpenseException(Constant.ErrorMessageAmountException);
            }

            expenseParticipant.ExpenseStatusId = approveExpense.ExpenseStatusId;
            expenseParticipant.ExpenseRemark = approveExpense.ExpenseRemark;

            if (approveExpense.ExpenseStatusId == Constant.ApproveStateId)
            {
                expenseParticipant.AmountApproved = approveExpense.AmountApproved;
            }

            _unitOfWork.Repository<ExpenseParticipant>().Update(expenseParticipant);
            _unitOfWork.Commit();
            return approveExpense;
        }

        /// <summary>
        /// Deleting the expense record according to ExpenseId 
        /// </summary>
        /// <param name="expenseId"></param>
        /// <param name="creatorId"></param>
        public void Delete(int expenseId, int creatorId)
        {
            Expense expense = _unitOfWork.Repository<Expense>().GetById(expenseId);

            if (expense == null)
            {
                throw new ExpenseException(Constant.ErrorMessageNoExpenseFound, expenseId);
            }

            if (expense.ExpenseCreatorId != creatorId)
            {
                throw new ExpenseException(Constant.ErrorMessageUnauthorizedAction);
            }

            IList<ExpenseParticipant> expenseParticipants = expense.ExpenseParticipants.ToList();

            foreach (ExpenseParticipant expenseParticipant in expenseParticipants)
            {
                if (expenseParticipant.ExpenseStatus.ExpenseStatusId != Constant.PendingStateId)
                {
                    throw new ExpenseException(Constant.ErrorMessageNotValid);
                }
                _unitOfWork.Repository<ExpenseParticipant>().Delete(expenseParticipant.ExpenseParticipantId);
            }
            _unitOfWork.Repository<Expense>().Delete(expenseId);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Returning the detail of expense categories 
        /// </summary>
        public IList<ExpenseTypeModel> ExpenseTypes()
        {
            IEnumerable<ExpenseType> expenseTypes = _unitOfWork.Repository<ExpenseType>().GetAll();
            IList<ExpenseTypeModel> expenseTypeModels = new List<ExpenseTypeModel>();
            Mapping.MapExpenseTypeModel(expenseTypes, expenseTypeModels);
            return expenseTypeModels;
        }
    }
}
