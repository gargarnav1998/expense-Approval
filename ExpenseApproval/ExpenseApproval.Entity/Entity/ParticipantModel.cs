using System.ComponentModel.DataAnnotations;

namespace ExpenseApproval.Model
{
    public class ParticipantModel
    {
        public int ExpenseParticipantId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int ExpenseId { get; set; }

        public decimal? AmountApproved { get; set; }

        [Required]
        public decimal AmountClaimed { get; set; }

        public string ExpenseRemark { get; set; }

        public bool? ExpenseStatus { get; set; }

        public decimal AmountAvailable { get; set; }

        public string EmployeeName { get; set; }
    }
}