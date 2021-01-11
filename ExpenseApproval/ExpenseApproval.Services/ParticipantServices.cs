using System.Collections.Generic;
using ExpenseApproval.DAL;
using ExpenseApproval.Entity;
using System.Linq;

namespace ExpenseApproval.Services
{
    public class ParticipantServices : IParticipantServices
    {
        #region Private members

        private IRepository<ExpenseParticipant> _participantRepository;

        #endregion 

        #region Constructor

        public ParticipantServices()
        {
            _participantRepository = new Repository<ExpenseParticipant>();
        }

        #endregion

        public IEnumerable<ExpenseParticipant> GetAll()
        {
            return _participantRepository.GetAll();
        }

        public IEnumerable<ExpenseParticipant> GetAllByExpenseId(int expenseId)
        {
            return _participantRepository.FindBy(x => x.ExpenseId == expenseId);
        }

        public ExpenseParticipant GetById(int expenseParticipantId)
        {
            return _participantRepository.GetById(expenseParticipantId);
        }

        public void Add(ExpenseParticipant participant)
        {
            _participantRepository.Insert(participant);
            _participantRepository.Save();
        }

        public void Edit(ExpenseParticipant participant)
        {
            _participantRepository.Update(participant);
            _participantRepository.Save();
        }

        public void Delete(int expenseParticipantId)
        {
            _participantRepository.Delete(expenseParticipantId);
            _participantRepository.Save();
        }

        public bool CheckIfParticipant(int expenseParticipantId)
        {
            return _participantRepository.GetAll().Select(x => x.ExpenseParticipantId).ToList().Contains(expenseParticipantId);
        }

        public IEnumerable<ExpenseParticipant> GetParticipantByEmployeeIdAndExpenseTypeId(int employeeId, int expenseTypeId)
        {
            return _participantRepository.FindBy(x => x.EmployeeId == employeeId)
                .Where(x => x.Expense.ExpenseTypeId.Equals(expenseTypeId));
        }

        public IEnumerable<ExpenseParticipant> GetParticipantByApproverId(int approverId)
        {
            return _participantRepository.FindBy(x => x.Expense.ApproverId == approverId);
        }
    }
}