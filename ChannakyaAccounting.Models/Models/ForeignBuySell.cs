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
    
    public partial class ForeignBuySell
    {
        public decimal TransNo { get; set; }
        public System.DateTime Tdate { get; set; }
        public short PostedBy { get; set; }
        public Nullable<short> InCurr { get; set; }
        public Nullable<decimal> DrAmt { get; set; }
        public Nullable<short> OutCurr { get; set; }
        public Nullable<decimal> CrAmt { get; set; }
        public decimal TNo { get; set; }
        public Nullable<int> IAccNo { get; set; }
        public int BrnhId { get; set; }
        public Nullable<decimal> IVno { get; set; }
        public Nullable<decimal> OVno { get; set; }
        public Nullable<int> slpno { get; set; }
        public string notes { get; set; }
        public Nullable<double> rate { get; set; }
    }
}
