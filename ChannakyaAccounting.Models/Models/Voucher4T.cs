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
    
    public partial class Voucher4T
    {
        public int V4TId { get; set; }
        public int V2TId { get; set; }
        public int DDID { get; set; }
        public Nullable<int> DVId { get; set; }
        public string Amount { get; set; }
    
        public virtual DimensionValue DimensionValue { get; set; }
        public virtual Voucher2T Voucher2T { get; set; }
    }
}
