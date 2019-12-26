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
    
    public partial class PolicyPenCalc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PolicyPenCalc()
        {
            this.ProductPCIDs = new HashSet<ProductPCID>();
        }
    
        public byte PCID { get; set; }
        public string PCNAME { get; set; }
        public byte RID { get; set; }
        public byte PBALID { get; set; }
        public byte DURID { get; set; }
    
        public virtual RuleDuration RuleDuration { get; set; }
        public virtual RulePenBalance RulePenBalance { get; set; }
        public virtual RuleRate RuleRate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPCID> ProductPCIDs { get; set; }
    }
}