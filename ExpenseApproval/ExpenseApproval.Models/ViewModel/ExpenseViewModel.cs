using System;

namespace ExpenseApproval.Model
{
    public class ExpenseViewModel
    {
        public int ExpenseId { get; set; }

        public string ExpenseTypeName { get; set; }

        public int ExpenseCreatorId { get; set; }

        public string ExpenseCreatorName { get; set; }

        public decimal AmountClaimed { get; set; }

        public decimal? AmountApproved { get; set; }

        public decimal AmountAvailable { get; set; }

        public string ApprovalStatus { get; set; }

        public int ParticipantId { get; set; }

        public DateTime ExpenseDate { get; set; }

        public DateTime CreationDate { get; set; }

        public string EmployeeName { get; set; }

        public int ApproverId { get; set; }

        public string ApproverName { get; set; }

        public string ImagePath { get; set; }

        public byte[] BillImage { get; set; }
    }
}
