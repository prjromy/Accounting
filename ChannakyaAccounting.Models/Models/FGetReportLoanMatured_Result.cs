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
    
    public partial class FGetReportLoanMatured_Result
    {
        public string PName { get; set; }
        public int IAccNo { get; set; }
        public string AccNo { get; set; }
        public string Name { get; set; }
        public decimal ApprovedAmt { get; set; }
        public Nullable<decimal> DisbursedAmt { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<System.DateTime> MDate { get; set; }
        public Nullable<int> MDays { get; set; }
        public Nullable<decimal> PrincipalPybl { get; set; }
        public Nullable<decimal> PrincipalPaid { get; set; }
        public Nullable<decimal> MaturedPrincipal { get; set; }
        public Nullable<decimal> InterestPybl { get; set; }
        public Nullable<decimal> InterestPaid { get; set; }
        public Nullable<decimal> MaturedInterest { get; set; }
        public Nullable<decimal> MaturedPrin { get; set; }
    }
}
