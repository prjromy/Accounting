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
    
    public partial class DenoBal
    {
        public int DenoBalId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> UserLevel { get; set; }
        public int DenoId { get; set; }
        public int Piece { get; set; }
    
        public virtual Denosetup Denosetup { get; set; }
    }
}
