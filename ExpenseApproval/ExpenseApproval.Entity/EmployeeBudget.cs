//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpenseApproval.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeBudget
    {
        public int EmployeeBudgetId { get; set; }
        public int EmployeeId { get; set; }
        public int ExpenseTypeId { get; set; }
        public decimal AmountAllotted { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual ExpenseType ExpenseType { get; set; }
    }
}
