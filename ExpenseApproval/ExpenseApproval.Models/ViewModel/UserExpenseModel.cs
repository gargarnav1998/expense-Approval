using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseApproval.Model
{
   
    public class UserExpenseModel
    {
        public string EmployeeName { get; set; }
        public string ExpenseTypeName { get; set; }
        public decimal? AmountApproved { get; set; }
        public decimal AmountClaimed { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime ExpenseCreationDate { get; set; }
        public string ExpenseCreatedBy { get; set; }
        public decimal ExpenseTypeAmount { get; set; }
        public bool ExpenseStatus { get; set; }
        public int ExpenseId { get; set; }
    } 
}
