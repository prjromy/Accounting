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
    
    public partial class FGetReportUnverifiedTransaction_Result
    {
        public Nullable<byte> PID { get; set; }
        public string ProductName { get; set; }
        public string accountnumber { get; set; }
        public string AccountName { get; set; }
        public Nullable<int> CurrID { get; set; }
        public System.DateTime tdate { get; set; }
        public string notes { get; set; }
        public decimal cramt { get; set; }
        public decimal dramt { get; set; }
        public int ttype { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<decimal> tno { get; set; }
        public string flag { get; set; }
        public Nullable<int> SType { get; set; }
    }
}