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
    
    public partial class ALSchHistry
    {
        public int AlSchHistryId { get; set; }
        public int IAccno { get; set; }
        public Nullable<System.DateTime> schDate { get; set; }
        public Nullable<decimal> schPrin { get; set; }
        public Nullable<decimal> schInt { get; set; }
        public Nullable<decimal> calcInt { get; set; }
        public Nullable<decimal> balPrin { get; set; }
        public Nullable<decimal> pdPrin { get; set; }
        public Nullable<decimal> pdInt { get; set; }
        public Nullable<int> reSchFreq { get; set; }
    }
}
