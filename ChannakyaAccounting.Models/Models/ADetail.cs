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
    
    public partial class ADetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ADetail()
        {
            this.AChqs = new HashSet<AChq>();
            this.AchqFGdPymts = new HashSet<AchqFGdPymt>();
            this.ALSches = new HashSet<ALSch>();
            this.ADSches = new HashSet<ADSch>();
            this.AIntExpenses = new HashSet<AIntExpens>();
            this.AintPayables = new HashSet<AintPayable>();
            this.ALColls = new HashSet<ALColl>();
            this.ALinkloans = new HashSet<ALinkloan>();
            this.ALRegistrations = new HashSet<ALRegistration>();
            this.AMClearanceHs = new HashSet<AMClearanceH>();
            this.ANomAccs = new HashSet<ANomAcc>();
            this.ANominees = new HashSet<ANominee>();
            this.AOfCusts = new HashSet<AOfCust>();
            this.ARates = new HashSet<ARate>();
            this.AWtdQueues = new HashSet<AWtdQueue>();
            this.ChqRqsts = new HashSet<ChqRqst>();
            this.DisburseDeposits = new HashSet<DisburseDeposit>();
            this.DisbursementMains = new HashSet<DisbursementMain>();
            this.Guarantors = new HashSet<Guarantor>();
            this.IchkBounces = new HashSet<IchkBounce>();
            this.IChkDeps = new HashSet<IChkDep>();
            this.IChkDeps1 = new HashSet<IChkDep>();
            this.IntLogs = new HashSet<IntLog>();
            this.ReferenceInfoes = new HashSet<ReferenceInfo>();
            this.TempALSches = new HashSet<TempALSch>();
        }
    
        public int IAccno { get; set; }
        public int PID { get; set; }
        public string Accno { get; set; }
        public System.DateTime RDate { get; set; }
        public int CurrID { get; set; }
        public Nullable<int> BrchID { get; set; }
        public string PostedBy { get; set; }
        public string ApprovedBy { get; set; }
        public byte AccState { get; set; }
        public string Aname { get; set; }
        public string OldAccno { get; set; }
        public decimal Bal { get; set; }
        public decimal IonBal { get; set; }
        public decimal IRate { get; set; }
        public byte DateType { get; set; }
        public Nullable<System.DateTime> LastTransDate { get; set; }
    
        public virtual CurrencyType1 CurrencyType1 { get; set; }
        public virtual AccountStatu AccountStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AChq> AChqs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AchqFGdPymt> AchqFGdPymts { get; set; }
        public virtual APolicyInterest APolicyInterest { get; set; }
        public virtual ADur ADur { get; set; }
        public virtual AMinBal AMinBal { get; set; }
        public virtual AOPolInt AOPolInt { get; set; }
        public virtual APolPen APolPen { get; set; }
        public virtual APRate APRate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALSch> ALSches { get; set; }
        public virtual ADrLimit ADrLimit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADSch> ADSches { get; set; }
        public virtual AICBDur AICBDur { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AIntExpens> AIntExpenses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AintPayable> AintPayables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALColl> ALColls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALinkloan> ALinkloans { get; set; }
        public virtual ALoan ALoan { get; set; }
        public virtual ALoanGrace ALoanGrace { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALRegistration> ALRegistrations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AMClearanceH> AMClearanceHs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ANomAcc> ANomAccs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ANominee> ANominees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AOfCust> AOfCusts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARate> ARates { get; set; }
        public virtual ATempFloatIntRate ATempFloatIntRate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AWtdQueue> AWtdQueues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChqRqst> ChqRqsts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisburseDeposit> DisburseDeposits { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisbursementMain> DisbursementMains { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guarantor> Guarantors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IchkBounce> IchkBounces { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IChkDep> IChkDeps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IChkDep> IChkDeps1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IntLog> IntLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceInfo> ReferenceInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TempALSch> TempALSches { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
    }
}
