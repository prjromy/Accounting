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
    
    public partial class FGetReportOverdraftBalance_Result
    {
        public string PName { get; set; }
        public string Accno { get; set; }
        public string Aname { get; set; }
        public decimal Bal { get; set; }
        public Nullable<double> ODInterest { get; set; }
        public decimal AppAmt { get; set; }
        public Nullable<System.DateTime> ValDat { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<float> Days { get; set; }
        public Nullable<System.DateTime> MatDat { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }
    }
}
