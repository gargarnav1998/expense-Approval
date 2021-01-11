using System;

namespace ExpenseApproval.Model
{
    public class Expenses
    {
        public string ExpenseTypeName { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public decimal? ExpenseTypeAmount { get; set; }
        public int ParticipantId { get; set; }
        public int EmployeeId { get; set; }
        public int ExpenseId { get; set; }
        public decimal? AmountApproved { get; set; }
        public decimal AmountClaimed { get; set; }
        public string ExpenseRemark { get; set; }
        public bool? ExpenseStatus { get; set; }
        public decimal AmountAvailable { get; set; }
        public string EmployeeName { get; set; }
        public string ApproverName { get; set; }
        public byte[] BillImage { get; set; }
        public string ImagePath {get ; set;}
    }
}
