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
    
    public partial class Expense
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Expense()
        {
            this.ExpenseParticipants = new HashSet<ExpenseParticipant>();
        }
    
        public int ExpenseId { get; set; }
        public int ExpenseCreatorId { get; set; }
        public System.DateTime ExpenseDate { get; set; }
        public System.DateTime CreationDate { get; set; }
        public int ApproverId { get; set; }
        public int ExpenseTypeId { get; set; }
        public string ExpensePurpose { get; set; }
        public byte[] BillImage { get; set; }
        public string ImagePath { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual Employee ApproverEmployee { get; set; }
        public virtual Employee CreatorEmployee { get; set; }
        public virtual ExpenseType ExpenseType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExpenseParticipant> ExpenseParticipants { get; set; }
    }
}