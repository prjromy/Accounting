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
    
    public partial class RulePay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RulePay()
        {
            this.ProductPays = new HashSet<ProductPay>();
        }
    
        public byte PAYID { get; set; }
        public string PRule { get; set; }
        public bool active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPay> ProductPays { get; set; }
    }
}