//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChannakyaAccounting.Models.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VoucherNo
    {
        public int VNId { get; set; }
        public int BId { get; set; }
        public int CurrentNo { get; set; }
        public short FYID { get; set; }
        public int VTypeId { get; set; }
        public Nullable<int> CompanyId { get; set; }
    
        public virtual BatchDescription BatchDescription { get; set; }
        public virtual BatchDescription BatchDescription1 { get; set; }
        public virtual FiscalYear FiscalYear { get; set; }
        public virtual FiscalYear FiscalYear1 { get; set; }
        public virtual VoucherType VoucherType { get; set; }
        public virtual VoucherType VoucherType1 { get; set; }
    }
}