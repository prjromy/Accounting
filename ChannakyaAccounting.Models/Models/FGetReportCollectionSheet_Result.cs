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
    
    public partial class FGetReportCollectionSheet_Result
    {
        public long collSheetId { get; set; }
        public System.DateTime TDate { get; set; }
        public int SheetNo { get; set; }
        public int CollectorId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> LoanAmount { get; set; }
        public Nullable<decimal> DepositAmount { get; set; }
        public Nullable<int> LoanCount { get; set; }
        public Nullable<int> DepositCount { get; set; }
    }
}
