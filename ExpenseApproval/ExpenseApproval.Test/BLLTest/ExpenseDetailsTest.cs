//using Moq;
//using NUnit.Framework;
//using ExpenseApproval.BLL;
//using ExpenseApproval.Entity;
//using System.Collections.Generic;
//using System;
//using ExpenseApproval.Model;
//using System.Linq;
//using ExpenseApproval.DAL;

//namespace ExpenseApproval.UnitTest
//{
//    [TestFixture]
//    public class ExpenseDetailsTest
//    {
//        #region Private Members

//        private static IList<ExpenseParticipant> _expenseParticipants = new List<ExpenseParticipant>
//        {
//            new ExpenseParticipant
//            {
//                 AmountApproved = 200,
//                 AmountClaimed = 200,
//                 EmployeeId = 1,
//                 ExpenseId = 1,
//                 ExpenseParticipantId = 1,
//                 ExpenseRemark = "Good",
//                 ExpenseStatusId = 2
//            },
//            new ExpenseParticipant
//            {
//                 AmountApproved = null,
//                 AmountClaimed = 200,
//                 EmployeeId = 2,
//                 ExpenseId = 1,
//                 ExpenseParticipantId = 2,
//                 ExpenseRemark = null,
//                 ExpenseStatusId = 1
//            },
//             new ExpenseParticipant
//            {
//                 AmountApproved = null,
//                 AmountClaimed = 200,
//                 EmployeeId = 3,
//                 ExpenseId = 1,
//                 ExpenseParticipantId = 3,
//                 ExpenseRemark = "Rejected",
//                 ExpenseStatusId = 3
//            },
//        };

//        private Expense _expense = new Expense
//        {
//            CreationDate = DateTime.Now,
//            ApproverId = 2,
//            BillImage = null,
//            ExpenseCreatorId = 3,
//            ExpenseDate = Convert.ToDateTime("2011/02/02"),
//            ExpenseId = 1,
//            ExpensePurpose = "Party",
//            ExpenseTypeId = 1,
//            ImagePath = "Bill.jpg",
//            ModifiedDate = null,
//            ExpenseParticipants = _expenseParticipants
//        };

//        private Expense _expenseForCreator = new Expense
//        {
//            CreationDate = DateTime.Now,
//            ApproverId = 2,
//            BillImage = null,
//            ExpenseCreatorId = 3,
//            ExpenseDate = Convert.ToDateTime("2011/02/02"),
//            ExpenseId = 1,
//            ExpensePurpose = "Party",
//            ExpenseTypeId = 1,
//            ImagePath = "Bill.jpg",
//            ModifiedDate = null,
//            ExpenseParticipants = _expenseParticipants
//        };

//        #endregion

//        [Test]
//        public void GetAllByEmployeeId()
//        {
//            //Arrange
//            Mock<IUnitOfWork> moqUnitOfWork = new Mock<IUnitOfWork>();

//            //Setup
//            moqUnitOfWork.Setup(x => x.Repository<ExpenseType>().GetAll()).Returns(_expenseTypes);
//            moqUnitOfWork.Setup(x => x.Repository<ExpenseParticipant>().FindBy(It.IsAny<Expression<Func<ExpenseParticipant, bool>>>())).Returns(_expenseParticipants.AsQueryable);

//            var budgetDetails = new ExpenseDetails(moqUnitOfWork.Object);
//        }

//    }
//}