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
    
    public partial class CashFlow
    {
        public int Brhid { get; set; }
        public Nullable<int> FUserid { get; set; }
        public Nullable<int> UserID { get; set; }
        public byte TType { get; set; }
        public System.DateTime TDate { get; set; }
        public string Tdesc { get; set; }
        public long TNO { get; set; }
        public short Currid { get; set; }
        public decimal Dramt { get; set; }
        public decimal Cramt { get; set; }
        public Nullable<System.DateTime> AcceptedOn { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<decimal> Vno { get; set; }
    }
}
