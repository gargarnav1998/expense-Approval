namespace ExpenseApproval.Model
{
    public class ExpenseTypeModel
    {
        public int ExpenseTypeId { get; set; }

        public string ExpenseName { get; set; }

        public decimal ExpenseAmount { get; set; }

        public bool IsIndividual { get; set; }
    }
}