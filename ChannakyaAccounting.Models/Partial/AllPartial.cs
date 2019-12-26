using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.Metadata;
using System.ComponentModel.DataAnnotations;
using ChannakyaAccounting.Models.MetaData;
using System.ComponentModel;

namespace ChannakyaAccounting.Models.Models
{
    [MetadataType(typeof(FinSys2MetaData))]
    public partial class FinSys2
    {

    }
    [MetadataType(typeof(SubsiTitleMetaData))]
    public partial class SubsiTitle
    {


    }
    [MetadataType(typeof(DimensionDefinationMetaData))]
    public partial class DimensionDefination
    {
        public bool isSelected { get; set; }
        public int DLId { get; set; }
        public int Order { get; set; }

    }
    [MetadataType(typeof(SubsiVSFIdMetaData))]
    public partial class SubsiVSFId
    {
        //public int fromVoucher { get; set; }

    }
    public partial class ProductDurationInt
    {
        public DateTime EffectiveFrom { get; set; }

    }
    public partial class ProductICBDur
    {
        public DateTime EffectiveFrom { get; set; }

    }
    public partial class Voucher3T
    {
        public int fromVoucher { get; set; }
        public int SFId { get; set; }
        public List<Voucher3T> childList { get; set; }
        public string SubsiName { get; set; }


    }
    [MetadataType(typeof(BankInfoMetaData))]
    public partial class BankInfo
    {

    }
    
    [MetadataType(typeof(FinAccMetaData))]
    public partial class FinAcc
    {

        //public FinAcc()
        //{

        //}
        public List<DimensionDefination> DimensionDefinationList { get; set; }
        //public List<SchmDetail> SchemeDetails { get; set; }
        //public List<ProductDetail> ProductDetails { get; set; }

    }
   
    [MetadataType(typeof(SchmDetailMetaData))]
    public partial class SchmDetail
    {
        public int F2Type { get; set; }
    }

    [MetadataType(typeof(ProductDetailMetaData))]
    public partial class ProductDetail
    {
        //public ProductDetail()
        //{
        //    this.DurationList = new List<Models.Duration>();
        //    this.InterestValueList = new List<InterestValue>();
        //    this.ProductDetails = new List<ProductDetail>();
        //}
        public SchmDetail SchemeofProduct { get; set; }
        public List<Duration> DurationList { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
        public List<InterestValue> InterestValueList { get; set; }
        public FixedDepositList FixedDepositList { get; set; }
        public LoanInterestList LoanInterestList { get; set; }

        public NormalDepositList NormalDepositList { get; set; }

        public int LicenseBranch { get; set; }
        [DisplayName("Start Account Number")]
        public int StartAcNo { get; set; }
        public string Alias { get; set; }

        //public int DurationType { get; set; }
    }
    public partial class NormalDepositList
    {
        public NormalDepositList()
        {
            this.CalculationRuleList = new List<PolicyIntCalc>();
            this.CapitalizationRuleList = new List<RuleICBDuration>();
        }
        public int ProductDurationIntId { get; set; }
        public List<PolicyIntCalc> CalculationRuleList { get; set; }
        public List<RuleICBDuration> CapitalizationRuleList { get; set; }
    }
    public partial class RuleICBDuration
    {
       
        public bool IsChecked{ get; set; }
        public bool IsDefault { get; set; }
        public int PICBId { get; set; }
        public double InterestRateICB { get; set; }
        public ProductDurationInt ProductDurationInt { get; set; }
    }
    public partial class PolicyIntCalc
    {
        public bool IsChecked { get; set; }
        public bool IsDefault { get; set; }
    }

    public partial class LoanInterestList
    {
        public LoanInterestList()
        {
            this.PolicyCalculationList = new List<PolicyIntCalc>();
            this.PolicyPenaltyList = new List<PolicyPenCalc>();
            this.RulePayList = new List<RulePay>();
        }
        public List<PolicyIntCalc> PolicyCalculationList { get; set; }
        public List<PolicyPenCalc> PolicyPenaltyList { get; set; }
        public List<RulePay> RulePayList { get; set; }
    }
    public partial class PolicyPenCalc
    {
        public bool IsChecked { get; set; }
        public bool IsDefault { get; set; }
    }
    public partial class RulePay
    {
        public bool IsChecked { get; set; }
        public bool IsDefault { get; set; }
    }

    public partial class FixedDepositList
    {
        public FixedDepositList()
        {
            this.DurationList = new List<Duration>();
        }
        public List<Duration> DurationList { get; set; }
        public int PolicyIntCalcId { get; set; }
    }
    [MetadataType(typeof(DimensionValueMetaData))]
    public partial class DimensionValue
    {
    }
    [MetadataType(typeof(SubsiSetupMetaData))]
    public partial class SubsiSetup
    {
    }
    [MetadataType(typeof(VoucherTypeMetaData))]
    public partial class VoucherType
    {
    }
    [MetadataType(typeof(BatchDescriptionMetaData))]
    public partial class BatchDescription
    {
    }
    public partial class VoucherLedgerDetails
    {
        public VoucherLedgerDetails()
        {
            this.DimensionVSLedgerList = new List<Voucher4T>();
            this.SubsiVSLedgerList = new List<Voucher3T>();
            this.BankInfoList = new List<Voucher5T>();

        }
        public List<Voucher4T> DimensionVSLedgerList { get; set; }
        public List<Voucher3T> SubsiVSLedgerList { get; set; }
        public List<Voucher5T> BankInfoList { get; set; }
        public int Fid { get; set; }
        public string Fname { get; set; }


    }

    public partial class Voucher1T
    {
        public int VTypeId { get; set; }
        public int FYID { get; set; }
        public int BatchNo { get; set; }
        public List<Voucher4T> DimensionVSLedgerList { get; set; }
        public List<Voucher3T> SubsiVSLedgerList { get; set; }
        public List<Voucher5T> BankInfoList { get; set; }
    }
    public partial class Voucher2
    {
        public string LedgerName { get; set; }
    }

    public partial class Voucher2T
    {
        public List<Voucher5T> BankInfoList { get; set; }
        public List<Voucher4T> DimensionVSLedgerList { get; set; }
        public List<Voucher3T> SubsiVSLedgerList { get; set; }
        //public string LedgerName { get; set; }
        public int VoucherTransType { get; set; }
        public List<VoucherLedgerDetails> VoucherLedgerDetails { get; set; }
            public string LedgerName { get; set; }

        }

    public partial class VoucherCashLedgerModel
    {
        public int V2TId { get; set; }
        public Nullable<int> Fid { get; set; }
        public Nullable<int> V1Tid { get; set; }

        public int VoucherTransType { get; set; }
        public string LedgerName { get; set; }
        public string Particulars { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
    }
    public class AllDesignationViewModel
    {
        public int UserId { get; set; }
        public int DesignationId { get; set; }


        [DisplayName("Departmet")]
        public string DeptName { get; set; }
        public int DepartmentId { get; set; }
        [DisplayName("Designation")]
        public string DGName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int BranchId { get; set; }
        public string UserName { get; set; }
        public int DegOrder { get; set; }
        public string FullName { get; set; }
        public List<string> ConcatName { get; set; }
    }

    public partial class Voucher4T
    {
        public int fromVoucher { get; set; }
        public int Id { get; set; }
    }
    [MetadataType(typeof(Voucher5MetaData))]

    public partial class Voucher5T
    {

        public int fromVoucher { get; set; }
        public int Id { get; set; }
    }
    public partial class Duration
    {
        public Duration()
        {
            this.ProductDurationInt = new ProductDurationInt();
            this.DepositBasisList = new List<DepositBasi>();
            this.CaptList = new List<RuleICBDuration>();
        }
        public List<DepositBasi> DepositBasisList { get; set; }
        public List<RuleICBDuration> CaptList { get; set; }
        public ProductDurationInt ProductDurationInt { get; set; }

    }
    public partial class DepositBasi
    {
        
        public ProductDurationInt ProductDurationInt { get; set; }
    }

    public partial class InterestValue
    {
        public InterestValue()
        {
            this.DurationList = new List<Duration>();
        }
        public string Amount { get; set; }
        public int PolicyIntCalcId { get; set; }
        public List<Duration> DurationList { get; set; }
    }

    [MetadataType(typeof(SubsiDetailMetaData))]
    public partial class SubsiDetail
    {

    }

}

