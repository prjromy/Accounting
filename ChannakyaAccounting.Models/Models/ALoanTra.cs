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
    
    public partial class ALoanTra
    {
        public int iaccno { get; set; }
        public decimal tno { get; set; }
        public System.DateTime vdate { get; set; }
        public Nullable<decimal> PriDr { get; set; }
        public Nullable<decimal> OthrDr { get; set; }
        public Nullable<decimal> PriCr { get; set; }
        public Nullable<decimal> othrCr { get; set; }
        public Nullable<decimal> RbtInt { get; set; }
        public Nullable<decimal> RbtPen { get; set; }
        public Nullable<decimal> intAPd { get; set; }
        public Nullable<decimal> intRPd { get; set; }
        public Nullable<decimal> IonIAPd { get; set; }
        public Nullable<decimal> IonIRPd { get; set; }
        public Nullable<decimal> IonCAPd { get; set; }
        public Nullable<decimal> IonCRPd { get; set; }
        public Nullable<decimal> PonPrAPd { get; set; }
        public Nullable<decimal> PonPrRPd { get; set; }
        public Nullable<decimal> PonIAPd { get; set; }
        public Nullable<decimal> PonIRPd { get; set; }
        public byte ttype { get; set; }
    }
}