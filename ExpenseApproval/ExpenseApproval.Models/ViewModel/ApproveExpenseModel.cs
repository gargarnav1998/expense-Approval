using System.ComponentModel.DataAnnotations;
using ExpenseApproval.Utility;

namespace ExpenseApproval.Model
{
    public class ApproveExpenseModel
    {
        [Required]
        public string ExpenseRemark { get; set; }

        [Range(Constant.MinimumValue, Constant.MaximumValue)]
        public int ExpenseStatusId { get; set; }

        public decimal? AmountApproved { get; set; }
    }
}