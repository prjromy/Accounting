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
    
    public partial class ProductBrnch
    {
        public int PBId { get; set; }
        public int BrnchID { get; set; }
        public int PID { get; set; }
        public Nullable<int> StartAcNo { get; set; }
        public Nullable<int> LastAcNo { get; set; }
    
        public virtual ProductDetail ProductDetail { get; set; }
    }
}
