using AutoMapper;
using ExpenseApproval.Model;
using ExpenseApproval.Entity;

namespace ExpenseApproval.BLL
{
    public class Automapping
    {
        public static void EmployeeMapping()
        {
            Mapper.CreateMap<Employee, EmployeeModel>();
        }

        public static void ExpenseTypeMapping()
        {
            Mapper.CreateMap<ExpenseType, ExpenseTypeModel>();
        }

        public static void ExpenseMapping()
        {
            Mapper.CreateMap<ExpenseDetailModel, Expense>();
        }

        public static void ParticipantMapping()
        {
            Mapper.CreateMap<ParticipantDetailModel, ExpenseParticipant>();
        }

        public static void ParticipantViewMapping()
        {
            Mapper.CreateMap<ExpenseParticipant, ParticipantModel>();
        }

        public static void ExpenseViewMapping()
        {
            Mapper.CreateMap<Expense, ExpenseViewModel>();
        }

        public static void EditExpenseMapping()
        {
            Mapper.CreateMap<ExpenseViewModel, Expense>();
        }

        public static void GetEditExpenseMapping()
        {
            Mapper.CreateMap<Expense, ExpenseModel>();
        }
    }
}