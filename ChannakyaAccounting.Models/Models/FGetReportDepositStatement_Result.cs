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
    
    public partial class FGetReportDepositStatement_Result
    {
        public Nullable<int> Tno { get; set; }
        public Nullable<System.DateTime> Tdate { get; set; }
        public Nullable<System.DateTime> Vdate { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string DC { get; set; }
        public Nullable<bool> IsMarked { get; set; }
    }
}
