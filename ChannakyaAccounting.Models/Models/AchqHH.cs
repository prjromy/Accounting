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
    
    public partial class AchqHH
    {
        public int AchqHHId { get; set; }
        public int AchqHId { get; set; }
        public byte Cstate { get; set; }
        public System.DateTime Tdate { get; set; }
        public int Postedby { get; set; }
        public System.DateTime PostedOn { get; set; }
        public string Remarks { get; set; }
    
        public virtual AchqH AchqH { get; set; }
    }
}
