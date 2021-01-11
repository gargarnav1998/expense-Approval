using System.ComponentModel.DataAnnotations;
using ExpenseApproval.Utility;

namespace ExpenseApproval.Model
{
    public class ParticipantDetailModel
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        [Range(Constant.MinimumValue, int.MaxValue)]
        public decimal AmountClaimed { get; set; }
    }
}