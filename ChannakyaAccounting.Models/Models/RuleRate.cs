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
    
    public partial class RuleRate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RuleRate()
        {
            this.PolicyIntCalcs = new HashSet<PolicyIntCalc>();
            this.PolicyPenCalcs = new HashSet<PolicyPenCalc>();
        }
    
        public byte RID { get; set; }
        public string RateRule { get; set; }
        public Nullable<bool> Enable { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PolicyIntCalc> PolicyIntCalcs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PolicyPenCalc> PolicyPenCalcs { get; set; }
    }
}
