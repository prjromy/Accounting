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
    
    public partial class ShrSReturn
    {
        public decimal Regno { get; set; }
        public decimal SCrtNo { get; set; }
        public Nullable<decimal> SoldTo { get; set; }
        public decimal Tno { get; set; }
        public System.DateTime Sdate { get; set; }
        public decimal SQty { get; set; }
        public decimal Amt { get; set; }
        public string Note { get; set; }
        public int PostedBy { get; set; }
        public int ttype { get; set; }
        public int SType { get; set; }
        public Nullable<int> Vno { get; set; }
        public Nullable<byte> Brchid { get; set; }
        public bool Isdeleted { get; set; }
    }
}
