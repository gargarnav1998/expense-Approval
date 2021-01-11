using System;
using System.Collections.Generic;

namespace ExpenseApproval.Model
{
    public class ExpenseModel
    {
        public int ExpenseId { get; set; }

        public string ExpenseCreatedBy { get; set; }

        public DateTime ExpenseDate { get; set; }

        public DateTime ExpenseCreationDate { get; set; }

        public int ApproverId { get; set; }

        public int ExpenseTypeId { get; set; }

        public string ExpensePurpose { get; set; }

        public byte[] BillImage { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ImagePath { get; set; }

        public virtual IList<ParticipantModel> ExpenseParticipants { get; set; }
    }
}