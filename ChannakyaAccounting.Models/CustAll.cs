//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChannakyaAccounting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CustAll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustAll()
        {
            this.SubLedgers = new HashSet<SubLedger>();
        }
    
        public int CustAllId { get; set; }
        public string CustName { get; set; }
        public string PANNo { get; set; }
        public string CustNo { get; set; }
    
        public virtual Info Info { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubLedger> SubLedgers { get; set; }
    }
}
