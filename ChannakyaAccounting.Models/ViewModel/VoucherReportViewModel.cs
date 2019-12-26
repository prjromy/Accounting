using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Models.ViewModel
{
   public class VoucherReportViewModel
    {
        public int V1Id { get; set; }
        public string VNo { get; set; }
        public Nullable<DateTime> TDate { get; set; }
        public string Narration { get; set; }
        public int CompanyId { get; set; }
        public string UserName { get; set; }
        public Nullable<DateTime> PostedOn { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedOn { get; set; }
        public Nullable<int> totalCount { get; set; }

        //public int FYID { get; set; }
        //public int VTypeID { get; set; }
        //public string VoucherName { get; set; }
        //public string Prefix { get; set; }
        //public int BId { get; set; }
        //public string BatchName { get; set; }


        //public int VNId { get; set; }
     
        //public int PostedBy { get; set; }
       
     
        //public decimal DebitAmount { get; set; }
        //public decimal CreditAmount { get; set; }
        
       

    }

    public class LedgerStatementViewModel
    {
        public int V1id { get; set; }
        public int V2id { get; set; }
        public DateTime TrDate { get; set; }
        public string VNo { get; set; }
        public string Particulars { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal Balance { get; set; }
        public int FId { get; set; }
        public Nullable<decimal> TotalDbAmt { get; set; }
        public Nullable<decimal> TotalCrAmt { get; set; }
        public int TotalDBCount { get; set; }
        public int TotalCrCount { get; set; }
        public IPagedList<LedgerStatementViewModel> LedgerStatement { get; set; }

        [NotMapped]
        public bool WithAllBrnch { get; set; }
    }

    public class VoucherReportMainViewModel
    {
        public VoucherReportMainViewModel()
        {
           // this.VoucherReport = new List<VoucherReportViewModel>();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> VType { get; set; }
        public Nullable<int> PostedBy { get; set; }
        public string BatchNo { get; set; }
       
         public bool VerifiedVoucher { get; set; }
        public bool UnVerifiedVoucher { get; set; }
        public IPagedList<VoucherReportViewModel> VoucherReport { get; set; }
    }


    //public class SubsiBalanceViewModel
    //{
    //    public Nullable<int> AccNo { get; set; }
    //    public Nullable<int> SID { get; set;}
    //    public string SubsiName { get;set; }
    //    public Nullable<decimal> OpBal { get; set; }
    //    public Nullable <decimal> DebitAmount { get; set; }
    //    public Nullable<decimal> CreditAmount { get; set; }
    //    public Nullable<int> ClBal { get; set; }
    //}

    public class TransactionReportUserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
    public class DenoInViewModel
    {
        public decimal Tno { get; set; }
        public System.DateTime vdate { get; set; }
        public int DenoID { get; set; }
        public short CurrID { get; set; }

        [DisplayName("Figure")]
        public double Deno { get; set; }
        public Nullable<int> DenoBalId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Piece { get; set; }
        public Nullable<int> UserLevel { get; set; }
        public long Id { get; set; }
        public int DenoNumber { get; set; }
        public decimal DenoNumberTotal { get; set; }


        public string Currency { get; set; }

        public List<DenoInViewModel> DenoInList { get; set; }
    }
    public class DenoOutViewModel
    {
        public decimal Tno { get; set; }
        public System.DateTime vdate { get; set; }
        public int DenoIDOut { get; set; }
        public short CurrIDOut { get; set; }

        public double DenoOut { get; set; }

        public int? DenoBalIdOut { get; set; }
        public int? UserIdOut { get; set; }

        public int? PieceOut { get; set; }
        public int? UserLevelOut { get; set; }

        public int DenoNumberOut { get; set; }
        public decimal DenoNumberTotalOut { get; set; }
        public List<DenoOutViewModel> DenoOutList { get; set; }
    }

    public class DenoInOutViewModel
    {
        public List<DenoOutViewModel> DenoOutList { get; set; }
        public List<DenoInViewModel> DenoInList { get; set; }

        public decimal BalanceIn { get; set; }
        public decimal BalanceOut { get; set; }
        public decimal Balance { get; set; }
        public Guid TrancactionId { get; set; }
        public decimal TransactionAmount { get; set; }

        public bool IsTransactionWithDeno { get; set; }

        public string DenoInOut { get; set; }

        public int Amount { get;  set; }
        public int V1TId { get; set; }
    }
    public class CurrencyRateViewModel
    {
        public bool IsTransWithDeno { get; set; }
        public decimal RatePerUnit { get; set; }
        public decimal BuyingRate { get; set; }
        public decimal Srate { get; set; }


    }
    public class ReturnSingleValueModdel
    {
        public DateTime? DateValue { get; set; }
        public int IdValue { get; set; }
        public byte ByteValue { get; set; }
        public decimal AmountValue { get; set; }
    }
}
