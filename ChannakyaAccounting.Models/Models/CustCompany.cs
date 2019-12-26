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
    
    public partial class CustCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustCompany()
        {
            this.RemittanceCustomers = new HashSet<RemittanceCustomer>();
        }
    
        public decimal CID { get; set; }
        public string CCName { get; set; }
        public Nullable<short> CCGroupID { get; set; }
        public string CCPerson { get; set; }
        public string CCno { get; set; }
    
        public virtual CustCompGroup CustCompGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RemittanceCustomer> RemittanceCustomers { get; set; }
        public virtual CustInfo CustInfo { get; set; }
    }
}
