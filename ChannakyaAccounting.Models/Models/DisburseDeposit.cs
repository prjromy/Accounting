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
    
    public partial class DisburseDeposit
    {
        public int DisDepositId { get; set; }
        public int DisburseId { get; set; }
        public int DepositIaccno { get; set; }
        public Nullable<decimal> Amount { get; set; }
    
        public virtual ADetail ADetail { get; set; }
        public virtual DisbursementMain DisbursementMain { get; set; }
    }
}
