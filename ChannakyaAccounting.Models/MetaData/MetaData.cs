using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ChannakyaAccounting.Models.MetaData
{

    public partial class FinSys2MetaData
    {

        [Display(Name = "Node Group")]
        [Required]
        public int F1id { get; set; }
        [Display(Name = "Node Name")]
        [Required]
        public string F2Desc { get; set; }

        [Display(Name = "Parent Name")]
        public Nullable<int> Pid { get; set; }
        [Display(Name = "Ledger Type")]
        [Required]
        public int F1Type { get; set; }
        [Required]
        public bool IsFixed { get; set; }

        public int Count { get; set; }
        [Display(Name = "Image")]
        public byte[] Content { get; set; }
    }
    public partial class Voucher5MetaData
    {

        [Display(Name = "Cheque/Slip No.")]
        [Required]
        public string ChequeNo { get; set; }

        [Display(Name = "Cheque Date")]
        [Required]
        public System.DateTime ChequeDate { get; set; }

        [Display(Name = "Cheque/Slip Amount")]
        [Required]
        public decimal ChequeAmount { get; set; }

        [Display(Name = "Depositor/Payee")]
        [Required]
        public string Payees { get; set; }

    }
    public partial class FinAccMetaData
    {



        public int Fid { get; set; }
        [Required]
        [Display(Name = "Ledger Name")]
        public string Fname { get; set; }
        [Display(Name = "Parent Ledger")]
        public Nullable<int> Pid { get; set; }
        [Display(Name = "Ledger Type")]
        [Required]
        public int F2Type { get; set; }
        [Display(Name = "Image")]
        public byte[] Content { get; set; }
        public Nullable<bool> DebitRestriction { get; set; }
        public Nullable<bool> CreditRestriction { get; set; }

        [Remote("CheckIfCodeDuplicates", "FinAcc", AdditionalFields = "Fid", ErrorMessage = "Code Already Exists")]

        public string Code { get; set; }
        [Required]
        public string Alias { get; set; }
    }
    public partial class DimensionDefinationMetaData
    {




    }
    public partial class BankInfoMetaData
    {
        public int Bid { get; set; }
        public int Fid { get; set; }
        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNo { get; set; }
        [Display(Name = "Branch")]
        [Required]
        public string Branch { get; set; }
        [Required]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Required]

        public bool IsFixed { get; set; }
        [Required]
        [Display(Name = "Interest Rate")]
        public Nullable<double> InterestRate { get; set; }
        [Required]
        [Display(Name = "Open Date")]
        public System.DateTime OpenDate { get; set; }
        [Display(Name = "Mature Date")]
        public Nullable<System.DateTime> MatureDate { get; set; }
        [Required]
        [Display(Name = "Calculation Rule")]
        public Nullable<int> InterestRuleId { get; set; }
        [Required]
        [Display(Name = "Minimum Balance")]
        public Nullable<decimal> MinimumBalance { get; set; }



    }
    public partial class SubsiTitleMetaData
    {

        public int STid { get; set; }
        public int Fid { get; set; }
        [Display(Name = "Subsi Title")]
        [Required]
        public string STitleName { get; set; }
        [Display(Name = "Title Prefix")]
        [Required]
        public string STPrefix { get; set; }
        [Required]
        [Display(Name = "Length")]
        public int Slength { get; set; }
        [Display(Name = "Current Number")]
        [Required]
        public int CurrentNo { get; set; }


    }
    public partial class SubsiVSFIdMetaData
    {
        [Display(Name = "Subsi Name")]
        [Required]
        public Nullable<int> SSId { get; set; }

    }
    public partial class SchmDetailMetaData
    {
        public byte SDID { get; set; }

        [Display(Name = "Scheme Name")]
        [Remote("CheckExistingSchemeName", "Scheme",AdditionalFields = "SDID", ErrorMessage = "Scheme name exists!")]
        public string SDName { get; set; }

        [Display(Name = "Duration")]
        public bool HasDuration { get; set; }

        [Display(Name = "Overdraw")]
        public bool HasOverdraw { get; set; }

        [Display(Name = "Cheque")]
        public bool HasCheque { get; set; }
        [Display(Name = "Card")]
        public bool HasCard { get; set; }
        [Display(Name = "Certificate")]
        public bool HasCertificate { get; set; }
        [Display(Name = "Limit")]
        public bool HasIndivLimit { get; set; }
        [Display(Name = "Individual Rate")]
        public bool HasIndivRate { get; set; }
        [Display(Name = "Dormancy")]
        public bool HasDormancy { get; set; }
        [Display(Name = "Renewable")]
        public bool IsRenewable { get; set; }
        [Display(Name = "Scheme Type")]
        public byte SType { get; set; }
        [Display(Name = "Enable")]
        public bool SEnable { get; set; }
        [Display(Name = "Mode of Int. Capitalization")]

        public Nullable<byte> ICBID { get; set; }
        [Display(Name = "Movement Rule")]

        public Nullable<byte> MID { get; set; }

        public Nullable<bool> Revolving { get; set; }

        public bool Nomianable { get; set; }
        [Display(Name = "Multiple Deposit")]
        public Nullable<bool> MultiDeposit { get; set; }
        [Display(Name = "Multiple Withdrawl")]
        public Nullable<bool> Withdrawal { get; set; }
        [Display(Name = "Insurance Required")]
        public Nullable<bool> IsInsured { get; set; }
        [Display(Name = "SMS")]
        public Nullable<bool> HASSMS { get; set; }

    }
    public partial class ProductDetailMetaData
    {
        [Display(Name = "Parent Scheme")]
        public byte SDID { get; set; }
        [Display(Name = "Product Name")]
        //  [Required]
        [Remote("CheckExistingProductName", "Product", AdditionalFields = "PID", ErrorMessage = "Product name already exists!")]
        public string PName { get; set; }
        [Display(Name = "Duration")]
        //[Required]
        public Nullable<float> Duration { get; set; }
        [Display(Name = "Has Cheque")]

        public bool HasCheque { get; set; }
        [Display(Name = "Card")]
        public bool HasCard { get; set; }
        [Display(Name = "Certificate")]
        public bool HasCertificate { get; set; }
        [Display(Name = "Limit")]
        public bool HasIndivLimit { get; set; }
        [Display(Name = "Individual Rate")]
        public bool HasIndivRate { get; set; }

        [Display(Name = "Dormant Period")]
        //[Required]
        public Nullable<byte> DormantPeriod { get; set; }
        [Display(Name = "Interest Rate")]
        public Nullable<float> IRate { get; set; }
        [Display(Name = "Overdraft Interest Rate")]
        public Nullable<float> OIRate { get; set; }
        [Display(Name = "Principle Penalty Rate")]
        public Nullable<float> PPRate { get; set; }
        [Display(Name = "Penalty Interest Rate")]
        public Nullable<float> PIRate { get; set; }
        [Display(Name = "Movement Rule")]
        public byte MID { get; set; }
        [Display(Name = "Renewable")]
        public Nullable<byte> RNWID { get; set; }
        [Display(Name = "Limit Amount")]
        //[Required]
        public decimal LAmt { get; set; }
        [Display(Name = "Grace Period")]
        // [Required]
        public Nullable<float> PGRACE { get; set; }
        [Display(Name = "Overdraft Duration")]
        //[Required]
        public Nullable<float> ODuration { get; set; }

        public bool Nomianable { get; set; }
        [Display(Name = "Enable")]
        public bool enabled { get; set; }
        [Display(Name = "Multiple Deposit")]
        public Nullable<bool> MultiDep { get; set; }
        [Display(Name = "Multiple Withdrawl")]
        public Nullable<bool> Withdrawal { get; set; }
        [Display(Name = "Inter Branch Trans")]
        public Nullable<bool> IntraBrnhTrnx { get; set; }

        [Display(Name = "Prefix")]
        [Remote("IsPrefixExists", "Product", AdditionalFields = "PID", ErrorMessage = "Prefix already exists")]
        public string Apfix { get; set; }

        [Display(Name = "Actr Unknown")]
        public decimal aCtr { get; set; }
        [Display(Name = "Insured")]
        public bool IsInsured { get; set; }
        [Display(Name = "Sector")]
        public Nullable<int> NSId { get; set; }

        public Nullable<bool> Schedule { get; set; }
        [Display(Name = "Penalty Grace Days")]
        //[Required]
        public Nullable<short> penGDys { get; set; }
        [Display(Name = "Duration State")]
        public Nullable<byte> durState { get; set; }
        public Nullable<bool> Revolving { get; set; }
        [Display(Name = "SMS")]
        public Nullable<bool> HASSMS { get; set; }
    }
    public partial class DimensionValueMetaData
    {


        public int DVId { get; set; }
        [Display(Name = "Dimension Name")]
        // [Required]
        public int DDId { get; set; }
        [Display(Name = "Value")]
        //[Required]
        public string DimensionValue1 { get; set; }
        [Display(Name = "Code")]
        //[Required]
        public string CodeNo { get; set; }


    }
    public partial class DimensionDefinationMetaData
    {

        public int DDId { get; set; }
        [Display(Name = "Dimension Name")]
        public string DefName { get; set; }
        [Display(Name = "Type")]
        public Nullable<byte> IsManual { get; set; }
        [Display(Name = "Table Name")]
        public Nullable<int> TId { get; set; }
    }
    public partial class SubsiSetupMetaData
    {
        public int SSId { get; set; }
        [Display(Name = "Subsi Type")]

        //[Required]
        public int STBId { get; set; }

        [Display(Name = "Subsi Title")]
        // [Required]
        [Remote("CheckExistingSubsiTitle", "SubsiSetup", AdditionalFields = "SSId", ErrorMessage = "Subsi title already exists!")]
        public string Title { get; set; }

        // [Required]
        [Remote("CheckExistingSubsiPrefix", "SubsiSetup", AdditionalFields = "SSId", ErrorMessage = "Subsi prefix already exists!")]
        public string Prefix { get; set; }
        public Nullable<int> Length { get; set; }
        public Nullable<int> CurrentNo { get; set; }
    }

    public partial class SubsiDetailMetaData
    {
        public int SDID { get; set; }
        [Required]
        public int CId { get; set; }
        public int FId { get; set; }
        [Required]
        public string AccNo { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> Enable { get; set; }
        [Required]
        public Nullable<decimal> DebitLimit { get; set; }
        [Required]
        public Nullable<decimal> CreditLimit { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        public int BranchId { get; set; }
    }
    public partial class VoucherTypeMetaData
    {
        public int VTypeID { get; set; }
        [Remote("CheckExistingVoucherName", "VoucherType", AdditionalFields = "VTypeID", ErrorMessage = "Voucher Name already exists!")]
        public string VoucherName { get; set; }
        [Remote("CheckExistingVoucherPrefix", "VoucherType", AdditionalFields = "VTypeID", ErrorMessage = "Voucher Prefix already exists!")]
        public string Prefix { get; set; }
    }
    public partial class BatchDescriptionMetaData
    {
        public int BId { get; set; }
        [Remote("CheckExistingBatchName", "BatchDescription", AdditionalFields = "BId", ErrorMessage = "Batch Name already exists!")]
        public string BatchName { get; set; }
        [Remote("CheckExistingBatchPrefix", "BatchDescription", AdditionalFields = "BId", ErrorMessage = "Batch Prefix already exists!")]
        public string Prefix { get; set; }
    }

}
