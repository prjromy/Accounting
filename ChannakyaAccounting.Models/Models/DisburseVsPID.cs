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
    
    public partial class DisburseVsPID
    {
        public int DisVsPID { get; set; }
        public int DisburseId { get; set; }
        public int FID { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    
        public virtual DisbursementMain DisbursementMain { get; set; }
    }
}
