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
    
    public partial class ProductPSID
    {
        public int PID { get; set; }
        public byte PSID { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public int PCalCId { get; set; }
    
        public virtual PolicyIntCalc PolicyIntCalc { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
    }
}
