namespace ExpenseApproval.Model
{
    public class BudgetModel
    {
        public int ExpenseTypeId { get; set; }

        public string ExpenseTypeName { get; set; }

        public decimal TotalBudget { get; set; }

        public decimal AvailableBudget { get; set; }

        public decimal? ApprovedBudget { get; set; }

        public decimal ClaimedBudget { get; set; }

    }
}