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
    
    public partial class ExpenseParticipant
    {
        public int ExpenseParticipantId { get; set; }
        public int EmployeeId { get; set; }
        public int ExpenseId { get; set; }
        public Nullable<decimal> AmountApproved { get; set; }
        public decimal AmountClaimed { get; set; }
        public string ExpenseRemark { get; set; }
        public int ExpenseStatusId { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Expense Expense { get; set; }
        public virtual ExpenseStatus ExpenseStatus { get; set; }
    }
}
