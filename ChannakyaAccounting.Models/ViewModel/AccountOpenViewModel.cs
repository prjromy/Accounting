
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.Model.ViewModel
{
    public class SchemeModel
    {
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }

        public bool? HasDuration { get; set; }

        public bool? AllowWithdraw { get; set; }
        public bool? MultiDeposit { get; set; }
        public bool? MultiWithdraw { get; set; }
        public bool? Schedule { get; set; }
        public bool? IsCheque { get; set; }
        
    }
    public class PolicyModel
    {
        public int PloicyIntCalId { get; set; }
        public string PolicyIntCalName { get; set; }
       public bool? IsDefault { get; set; }
    }
    public class InterestCapitalModel
    {
        public int ProductDurationIntId { get; set; }
        public int InterestCapitalizeId { get; set; }
        public string InterestCapitalizeName { get; set; }

        public bool IsDefault { get; set; }
        public decimal InterestRate { get; set; }

        public double? contributionValue { get; set; }
    }

    public class FloatingInterestModel
    {
        public int FloatingId { get; set; }
        public string FloatingName { get; set; }
    }
    public class CurrencyModel
    {
        public int CTId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }

        public string Country { get; set; }

        public List<CurrencyModel> CurrencyList { get; set; }

    }
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public float? InterestRate { get; set; }
        public decimal MinimumAmount { get; set; }

        public float? Duration { get; set; }

        public bool Nomianable { get; set; }

        public int MovementId { get; set; }

        public string DurState { get; set; }

        public int? DurationType { get; set; }

        public bool HasIndividualDuration { get; set; }

        public bool HasIndividualRate { get; set; }

        public bool HasIndividualLimit { get; set; }

        public bool HasDuration { get; set; }

        public bool? IntraBrnhTrnx { get; set; }
        public IPagedList<ProductViewModel> ProductList { get; set; }

        public SelectList BranchList { get; set; }
    }

    public class AccountsViewModel
    {
        public List<CurrencyModel> Currency { get; set; }
        public List<ProductViewModel> Product { get; set; }
        public List<FloatingInterestModel> FloatingInterest { get; set; }
        public List<InterestCapitalModel> InterestCapital { get; set; }
        public List<PolicyModel> Policy { get; set; }
        //public List<ChargeDetail> ChargeDetails { get; set; }
        public ProductViewModel ProductDetails { get; set; }

        public CheckProductTypeModel ChkProductType { get; set; }

        public List<ProductDurationViewModel> productDurationList { get; set; }
        public SchemeModel schemeDetails { get; set; }
        public bool IsChargeAvailable { get; set; }

        public DurationViewModel duration { get; set; }
    }

    public class AccountDetailsViewModel
    {
        [Required]
        [Display(Name = "Product")]
        public byte PID { get; set; }
        public int IAccno { get; set; }

        [Display(Name = "Account No")]
        public string Accno { get; set; }
        [Required]
        [Display(Name = "Registration Date")]
        public System.DateTime RDate { get; set; }
        [Required]
        [Display(Name = "Currency")]
        public int CurrID { get; set; }

        [Display(Name = "Branch")]
        public Nullable<int> BrchID { get; set; }
        [Display(Name = "PostedBy")]
        public string PostedBy { get; set; }
        [Display(Name = "ApprovedBy")]
        public string ApprovedBy { get; set; }
        [Display(Name = "Status")]
        public byte AccState { get; set; }

        public byte SType { get; set; }
        [Required]
        [Display(Name = "Account Name")]
        public string Aname { get; set; }
        [Display(Name = "Old Account No")]
        public string OldAccno { get; set; }
        [Display(Name = "Balance")]
        public decimal Bal { get; set; }
        [Display(Name = "Interest on Bal")]
        public decimal IonBal { get; set; }

        [Required]
        [Display(Name = "Scheme Name")]
        public int SchemeId { get; set; }

        [Display(Name = "Agreement Amount")]
        public decimal AgreementAmount { get; set; }

        [Display(Name = "Contribution Amount")]
        public decimal ContributionAmount { get; set; }

        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Display(Name = "Movement A/c")]
        public int MovementAcc { get; set; }

        [Display(Name = "Int. Cal. Rule")]
        public int InterestCalRule { get; set; }

        [Display(Name = "Floating Interest")]
        public int? FloatingInterest { get; set; }
        [Display(Name = "Interest Capitalize")]
        public int InterestCapitalize { get; set; }
        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }
        [Display(Name = "Minimum Balance")]
        public decimal MinimumBalance { get; set; }
        [Display(Name = "Maturation Date")]
        public DateTime MaturationDate { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public IEnumerable<int> CustomerId { get; set; }
        [Required]
        [Display(Name = "ReferredBy")]
        public int[] ReferredBy { get; set; }
        [Required]
        [Display(Name = "Collector")]
        public int BroughtBy { get; set; }

        public int ReferenceId { get; set; }

        public int MovementAccId { get; set; }

        public int Days { get; set; }

        public int Month { get; set; }

        public int IRateMasterId { get; set; }

        public ANomineeViewModel AccountNominee { get; set; }

        public ICollection<ANomineeViewModel> ANominees { get; set; }

        public IPagedList<AccountDetailsViewModel> AccountOpenList { get; set; }

        public bool DateType { get; set; }

        public int Basic { get; set; }
    }
    public class ANomineeViewModel
    {
        public int NID { get; set; }
        public int IAccno { get; set; }
        [Required]
        [Display(Name = "Nominee Name")]
        public string NomNam { get; set; }
        [Required]
        [Display(Name = "Nominee Relation")]
        public string NomRel { get; set; }
        [Required]
        [Display(Name = "Certificate")]
        public byte? CCertID { get; set; }
        [Display(Name = "CertificateNo")]
        public string CCertno { get; set; }
        [Required]
        [Display(Name = "Share")]
        public float Share { get; set; }

        [Required]
        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }

        [Display(Name = "ContactAddress")]
        public string ContactAddress { get; set; }

        public string CertificateName { get; set; }
    }
    public class AccountNomineeViewModel
    {
        public string NomNam { get; set; }

        public string NomRel { get; set; }

        public byte CCertID { get; set; }

        public string CCertno { get; set; }

        public float Share { get; set; }


        public string ContactNo { get; set; }

        public string CertificateName { get; set; }
        public string ContactAddress { get; set; }
    }

    public class DurationViewModel
    {
        public string NepaliDate { get; set; }
        public string EnglishDate { get; set; }
        public string Date { get; set; }
    }

    public class AccountSearchViewModel
    {
        public string BranchCode { get; set; }

        public string ProductCode { get; set; }

        public string CurrencyCode { get; set; }
        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountId { get; set; }
        [DisplayName("Client Name")]
        public string AccountHolder { get; set; }
        [DisplayName("Account Name")]
        public string AccountName { get; set; }
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }
        [DisplayName("Contact Details")]
        public string ContactDetails { get; set; }
        [DisplayName("CustomerId")]
        public decimal CustomerId { get; set; }
        [DisplayName("A/c Status")]
        public string AccountStatus { get; set; }

        public string ContactPerson { get; set; }

        public string Mobile { get; set; }

        public int TotalCount { get; set; }

        public int BranchId { get; set; }
        public int CurrencyId { get; set; }
        public int ProductId { get; set; }

        public string FilterAction { get; set; }
        public string AccountType { get; set; }
        public IPagedList<AccountSearchViewModel> AccountList { get; set; }
    }

    public class CheckProductTypeModel
    {
        public bool IsDuration { get; set; }

        public bool IsFixed { get; set; }

        public bool IsRecurring { get; set; }

        public bool IsMovement { get; set; }

        public bool IsIndiviualInterestRate { get; set; }

        public bool IsIndividualLimit { get; set; }
    }
    public  class ProductDurationViewModel
    {
        public int Id { get; set; }
        public string Duration { get; set; }
        public double Value { get; set; }
    }
}
