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
    
    public partial class CustContactPerson
    {
        public int CPId { get; set; }
        public decimal CID { get; set; }
        public string CPName { get; set; }
        public string CPCNo { get; set; }
    
        public virtual CustInfo CustInfo { get; set; }
    }
}
