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
    
    public partial class VoucherRejected
    {
        public int RV2Id { get; set; }
        public Nullable<int> V1id { get; set; }
        public string Remarks { get; set; }
    
        public virtual Voucher1 Voucher1 { get; set; }
    }
}
