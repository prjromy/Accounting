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
    
    public partial class DenoTrxnLog
    {
        public long Id { get; set; }
        public decimal Trxno { get; set; }
        public System.DateTime vdate { get; set; }
        public int Denoid { get; set; }
        public int Pics { get; set; }
        public int UserId { get; set; }
    
        public virtual Denosetup Denosetup { get; set; }
    }
}
