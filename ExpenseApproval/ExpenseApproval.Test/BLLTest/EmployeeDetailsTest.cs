using ExpenseApproval.BLL;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using ExpenseApproval.Entity;
using System.Linq;
using ExpenseApproval.DAL;
using System;
using System.Linq.Expressions;
using ExpenseApproval.Model;

namespace ExpenseApproval.UnitTest
{
    [TestFixture]
    public class EmployeeDetailsTest
    {
        #region Mock Data

        private IList<Employee> _employees = new List<Employee>
        {
            new Employee
            {
                EmployeeId = 1,
                EmployeeName = "Nikhil Yadav",
                EmployeeEmail = "nikhil.yadav@optimusinfo.com",
                Role = new Role{ RoleId = 3, RoleName = "Admin" },
                RoleId = 3
            },
            new Employee
            {
                EmployeeId = 2,
                EmployeeName = "Arnav Garg",
                EmployeeEmail = "arnav.garg@optimusinfo.com",
                Role = new Role{ RoleId = 2, RoleName = "Approver" },
                RoleId = 2
            },
            new Employee
            {
                EmployeeId = 3,
                EmployeeName = "Ashutosh Tiwari",
                EmployeeEmail = "ashutosh.tiwari@optimusinfo.com",
                Role = new Role{ RoleId = 1, RoleName = "Employee" },
                RoleId = 1
            }
        };

        private IList<ExpenseType> _expenseTypes = new List<ExpenseType>
        {
            new ExpenseType
            {
                 ExpenseTypeId = 1,
                 ExpenseAmount = 4000,
                 ExpenseName = "Recreational",
                 IsIndividual = false
            },
            new ExpenseType
            {
                 ExpenseTypeId = 2,
                 ExpenseAmount = 4000,
                 ExpenseName = "Travel",
                 IsIndividual = true
            },
            new ExpenseType
            {
                 ExpenseTypeId = 3,
                 ExpenseAmount = 5000,
                 ExpenseName = "Certification",
                 IsIndividual = true
            },
            new ExpenseType
            {
                 ExpenseTypeId = 4,
                 ExpenseAmount = 6000,
                 ExpenseName = "Sport",
                 IsIndividual = false
            }
        };

        private IList<ExpenseParticipant> _expenseParticipants = new List<ExpenseParticipant>
        {
            new ExpenseParticipant
            {
                 AmountApproved = 200,
                 AmountClaimed = 200,
                 EmployeeId = 1,
                 ExpenseId = 1,
                 ExpenseParticipantId = 1,
                 ExpenseStatusId = 2,
                 ExpenseRemark = "good",
                 Expense = new Expense{ ExpenseId = 1, ExpenseTypeId = 1, ExpenseType = new ExpenseType{ ExpenseTypeId = 1 } }
            },
            new ExpenseParticipant
            {
                 AmountApproved = null,
                 AmountClaimed = 200,
                 EmployeeId = 1,
                 ExpenseId = 2,
                 ExpenseParticipantId = 2,
                 ExpenseStatusId = 1,
                 ExpenseRemark = null,
                 Expense = new Expense{ ExpenseId = 2, ExpenseTypeId = 1, ExpenseType = new ExpenseType{ ExpenseTypeId = 1 } }
            },
            new ExpenseParticipant
            {
                 AmountApproved = null,
                 AmountClaimed = 200,
                 EmployeeId = 1,
                 ExpenseId = 3,
                 ExpenseParticipantId = 3,
                 ExpenseStatusId = 3,
                 ExpenseRemark = "bad",
                 Expense = new Expense{ ExpenseId = 1, ExpenseTypeId = 1, ExpenseType = new ExpenseType{ ExpenseTypeId = 1 } }
            }
        };

        #endregion

        #region Private Variable

        private static Mock<IUnitOfWork> _moqUnitOfWork = new Mock<IUnitOfWork>();
        private EmployeeDetails _employeedetails = new EmployeeDetails(_moqUnitOfWork.Object);

        #endregion

        #region Test Methods

        [Test]
        public void GetAll()
        {
            //Setup
            _moqUnitOfWork.Setup(x => x.Repository<Employee>().GetAll()).Returns(_employees);

            //Act
            IList<EmployeeModel> employeeModels = _employeedetails.GetAll();

            //Assert
            _moqUnitOfWork.Verify(x => x.Repository<Employee>().GetAll(), Times.Once);
            Assert.AreEqual(employeeModels.Count, _employees.Count);
        }

        [Test]
        public void GetBudgetDetails()
        {
            //Setup
            _moqUnitOfWork.Setup(x => x.Repository<ExpenseType>().GetAll()).Returns(_expenseTypes);
            _moqUnitOfWork.Setup(x => x.Repository<ExpenseParticipant>().FindBy(It.IsAny<Expression<Func<ExpenseParticipant, bool>>>())).Returns(_expenseParticipants.AsQueryable);

            //Act
            IList<BudgetModel> budgetModels = _employeedetails.GetBudgetDetails(It.IsAny<int>());

            //Assert
            _moqUnitOfWork.Verify(x => x.Repository<ExpenseType>().GetAll(), Times.Once);
            _moqUnitOfWork.Verify(x => x.Repository<ExpenseParticipant>().FindBy(It.IsAny<Expression<Func<ExpenseParticipant, bool>>>()), Times.Once);
            Assert.AreEqual(budgetModels.Count, _expenseTypes.Count);
        }

        #endregion
    }
}