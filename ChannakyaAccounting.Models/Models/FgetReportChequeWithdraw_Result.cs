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
    
    public partial class FgetReportChequeWithdraw_Result
    {
        public System.DateTime TransactionDate { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int branchId { get; set; }
        public Nullable<int> ChequeNo { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }
}
