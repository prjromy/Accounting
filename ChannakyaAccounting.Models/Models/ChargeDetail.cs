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
    
    public partial class ChargeDetail
    {
        public int ChgID { get; set; }
        public string ChgName { get; set; }
        public int FID { get; set; }
        public byte Modules { get; set; }
        public string Product { get; set; }
        public Nullable<int> ModEveID { get; set; }
        public byte Triggertype { get; set; }
        public byte ChrType { get; set; }
        public Nullable<float> Ratio { get; set; }
        public Nullable<decimal> CAmount { get; set; }
        public Nullable<int> ChrTempID { get; set; }
        public bool Chngble { get; set; }
        public bool ChrDirect { get; set; }
        public bool Status { get; set; }
    
        public virtual Event Event { get; set; }
        public virtual Module Module { get; set; }
    }
}
