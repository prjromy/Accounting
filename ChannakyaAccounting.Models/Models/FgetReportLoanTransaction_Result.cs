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
    
    public partial class FgetReportLoanTransaction_Result
    {
        public decimal tno { get; set; }
        public System.DateTime vdate { get; set; }
        public decimal PriDr { get; set; }
        public decimal PriCr { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> POnPr { get; set; }
        public Nullable<decimal> POnInt { get; set; }
        public Nullable<decimal> IOnI { get; set; }
        public byte ttype { get; set; }
        public string ProductName { get; set; }
        public byte PID { get; set; }
        public string Accno { get; set; }
        public string AccountName { get; set; }
    }
}