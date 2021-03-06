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
    
    public partial class FinAcc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FinAcc()
        {
            this.BankInfoes = new HashSet<BankInfo>();
            this.DimensionVSLedgers = new HashSet<DimensionVSLedger>();
            this.SubsiTitles = new HashSet<SubsiTitle>();
            this.SubsiVSFIds = new HashSet<SubsiVSFId>();
            this.VoucherDetails = new HashSet<VoucherDetail>();
        }
    
        public int Fid { get; set; }
        public string Fname { get; set; }
        public Nullable<int> Pid { get; set; }
        public int F2Type { get; set; }
        public byte[] Content { get; set; }
        public Nullable<bool> DebitRestriction { get; set; }
        public Nullable<bool> CreditRestriction { get; set; }
        public string Code { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankInfo> BankInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DimensionVSLedger> DimensionVSLedgers { get; set; }
        public virtual FinSys2 FinSys2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubsiTitle> SubsiTitles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubsiVSFId> SubsiVSFIds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VoucherDetail> VoucherDetails { get; set; }
    }
}
