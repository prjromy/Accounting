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
    
    public partial class ProductDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductDetail()
        {
            this.ADetails = new HashSet<ADetail>();
            this.ALRegistrations = new HashSet<ALRegistration>();
            this.ProductBrnches = new HashSet<ProductBrnch>();
            this.ProductCurrencies = new HashSet<ProductCurrency>();
            this.ProductDurationInts = new HashSet<ProductDurationInt>();
            this.ProductICBDurs = new HashSet<ProductICBDur>();
            this.ProductOPSIDs = new HashSet<ProductOPSID>();
            this.ProductPays = new HashSet<ProductPay>();
            this.ProductPCIDs = new HashSet<ProductPCID>();
            this.ProductPSIDs = new HashSet<ProductPSID>();
            this.ProductTIDs = new HashSet<ProductTID>();
        }
    
        public byte SDID { get; set; }
        public int PID { get; set; }
        public int FID { get; set; }
        public string PName { get; set; }
        public Nullable<float> Duration { get; set; }
        public bool HasOverdraw { get; set; }
        public bool HasCheque { get; set; }
        public bool HasCard { get; set; }
        public bool HasCertificate { get; set; }
        public Nullable<bool> HasIndivLimit { get; set; }
        public Nullable<bool> HasIndDuration { get; set; }
        public Nullable<byte> DormantPeriod { get; set; }
        public Nullable<bool> HasIndivRate { get; set; }
        public Nullable<float> IRate { get; set; }
        public Nullable<float> OIRate { get; set; }
        public Nullable<float> PPRate { get; set; }
        public Nullable<float> PIRate { get; set; }
        public byte MID { get; set; }
        public Nullable<byte> RNWID { get; set; }
        public decimal LAmt { get; set; }
        public Nullable<float> PGRACE { get; set; }
        public Nullable<float> ODuration { get; set; }
        public bool Nomianable { get; set; }
        public bool enabled { get; set; }
        public Nullable<bool> MultiDep { get; set; }
        public Nullable<bool> Withdrawal { get; set; }
        public Nullable<bool> IntraBrnhTrnx { get; set; }
        public string Apfix { get; set; }
        public Nullable<decimal> aCtr { get; set; }
        public bool IsInsured { get; set; }
        public Nullable<int> NSId { get; set; }
        public Nullable<bool> Schedule { get; set; }
        public Nullable<short> penGDys { get; set; }
        public Nullable<byte> durState { get; set; }
        public Nullable<bool> Revolving { get; set; }
        public Nullable<bool> HASSMS { get; set; }
    
        public virtual FinAcc FinAcc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADetail> ADetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALRegistration> ALRegistrations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductBrnch> ProductBrnches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductCurrency> ProductCurrencies { get; set; }
        public virtual RuleRenew RuleRenew { get; set; }
        public virtual SchmDetail SchmDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductDurationInt> ProductDurationInts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductICBDur> ProductICBDurs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOPSID> ProductOPSIDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPay> ProductPays { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPCID> ProductPCIDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPSID> ProductPSIDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTID> ProductTIDs { get; set; }
    }
}
