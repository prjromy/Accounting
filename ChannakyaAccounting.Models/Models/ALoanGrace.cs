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
    
    public partial class ALoanGrace
    {
        public int IAccno { get; set; }
        public Nullable<short> GDurP { get; set; }
        public Nullable<short> GDurI { get; set; }
        public Nullable<System.DateTime> GDateP { get; set; }
        public Nullable<System.DateTime> GDateI { get; set; }
        public Nullable<short> GDayP { get; set; }
        public Nullable<short> GDayI { get; set; }
        public int GraceOption { get; set; }
    
        public virtual ADetail ADetail { get; set; }
    }
}
