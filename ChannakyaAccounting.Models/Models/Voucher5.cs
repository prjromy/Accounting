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
    
    public partial class Voucher5
    {
        public int V5Id { get; set; }
        public string ChequeNo { get; set; }
        public System.DateTime ChequeDate { get; set; }
        public decimal ChequeAmount { get; set; }
        public string Payees { get; set; }
        public int V2Id { get; set; }
    
        public virtual Voucher2 Voucher2 { get; set; }
    }
}
