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
    
    public partial class CollSheetTran
    {
        public int Id { get; set; }
        public Nullable<long> CollSheetId { get; set; }
        public int IAccNo { get; set; }
        public long TNo { get; set; }
        public int SType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    
        public virtual CollSheet CollSheet { get; set; }
    }
}
