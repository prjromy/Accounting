using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Models.ViewModel
{
    
        public class AccountSearchViewModelAcc
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
            public IPagedList<AccountSearchViewModelAcc> AccountList { get; set; }
        }
    public class AccountDetailsViewModelAcc
    {
        [Required]
        [Display(Name = "Product")]
        public int PID { get; set; }
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

        public ANomineeViewModelAcc AccountNominee { get; set; }

        public ICollection<ANomineeViewModelAcc> ANominees { get; set; }

        public IPagedList<AccountDetailsViewModelAcc> AccountOpenList { get; set; }

        public bool DateType { get; set; }

        public int Basic { get; set; }
    }
    public class ANomineeViewModelAcc
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
    

}
