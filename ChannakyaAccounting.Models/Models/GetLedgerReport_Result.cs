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
    
    public partial class GetLedgerReport_Result
    {
        public int V1id { get; set; }
        public int V2Id { get; set; }
        public Nullable<int> fid { get; set; }
        public System.DateTime TDate { get; set; }
        public System.DateTime PostedOn { get; set; }
        public string VNo { get; set; }
        public string Particulars { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
        public int Balance { get; set; }
    }
}
