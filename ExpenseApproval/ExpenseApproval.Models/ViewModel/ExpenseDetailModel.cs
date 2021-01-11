using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExpenseApproval.Utility;

namespace ExpenseApproval.Model
{
    public class ExpenseDetailModel
    {
        public int ExpenseId { get; set; }

        public int ExpenseTypeId { get; set; }

        public string TypeName { get; set; }

        public int ApproverId { get; set; }

        public string ApproverName { get; set; }

        [Range(Constant.MinimumValue, int.MaxValue)]
        public decimal AmountClaimed { get; set; }

        [Required]
        [ValidateDateRange]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public string Purpose { get; set; }

        public byte[] BillImage { get; set; }

        public string ImagePath { get; set; }

        [Required]
        public IList<ParticipantDetailModel> ParticipantDetails { get; set; }
    }
}