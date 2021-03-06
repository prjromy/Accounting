//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChannakyaAccounting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UnverifiedVoucherMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnverifiedVoucherMaster()
        {
            this.UnverifiedVoucherDetails = new HashSet<UnverifiedVoucherDetail>();
        }
    
        public int UnVerifiedid { get; set; }
        public Nullable<int> VoucherNo { get; set; }
        public Nullable<int> BId { get; set; }
        public Nullable<System.DateTime> VouDate { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string AssignedBy { get; set; }
        public Nullable<System.DateTime> AssignedOn { get; set; }
        public Nullable<int> FiYId { get; set; }
        public Nullable<int> CTId { get; set; }
        public Nullable<int> TotalTransaction { get; set; }
        public Nullable<int> VTypeID { get; set; }
        public string CompanyId { get; set; }
        public string Narration { get; set; }
    
        public virtual BatchDescription BatchDescription { get; set; }
        public virtual CurrencyType CurrencyType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnverifiedVoucherDetail> UnverifiedVoucherDetails { get; set; }
        public virtual VoucherType VoucherType { get; set; }
    }
}
