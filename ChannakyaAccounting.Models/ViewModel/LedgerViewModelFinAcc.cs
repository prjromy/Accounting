using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Models.ViewModel
{
    public class LedgerViewModelFinAcc
    {

        public LedgerViewModelFinAcc()
        {
            this.ListOfBank = new List<BankLedgerViewModel>();
            this.SubsiTitleList = new List<SubsiTitleViewModel>();
            this.DimensionDefinationList = new List<DimensionDefinationViewModel>();
            this.SubsiVSFid = new List<SubsiVSFidViewModel>();
            //this.DimensionDefinationCheckBox = new List<DimensionCheckboxValues>();
            //this.BankInfoInput = new BankInfo();
        }
        //FinAcc
        public int Fid { get; set; }
        [DisplayName("Ledger Name")]
        public string Fname { get; set; }
        public int Pid { get; set; }
        [DisplayName("LedgerType")]
        public int F2Type { get; set; }
        public byte[] Content { get; set; }
        public Nullable<bool> DebitRestriction { get; set; }
        public Nullable<bool> CreditRestriction { get; set; }
        public string Code { get; set; }
        public List<BankLedgerViewModel> ListOfBank { get; set; }
        public List<SubsiTitleViewModel> SubsiTitleList { get; set; }
        public List<DimensionDefinationViewModel> DimensionDefinationList { get; set; }
        public List<SubsiVSFidViewModel> SubsiVSFid { get; set; }

        public int DDId { get; set; }
        [DisplayName("DimensionName")]
        public string DefName { get; set; }


    }
    public class BankLedgerViewModel

    {
        public int Bid { get; set; }
        public int Fid { get; set; }
        public string PhoneNo { get; set; }
        public string Branch { get; set; }
        public string ContactPerson { get; set; }
        public bool IsFixed { get; set; }
        public Nullable<double> InterestRate { get; set; }
        public System.DateTime OpenDate { get; set; }
        public Nullable<System.DateTime> MatureDate { get; set; }
        public Nullable<int> InterestRuleId { get; set; }
        public Nullable<decimal> MinimumBalance { get; set; }
        public ICollection<BankLedgerViewModel> BankDataList { get; set; }
    }
    public class SubsiTitleViewModel
    {
        public int STid { get; set; }
        public string STitleName { get; set; }
        public string STPrefix { get; set; }
        public int Slength { get; set; }
        public int CurrentNo { get; set; }
        public virtual SubsiVSFidViewModel Subsi { get; set; }

    }
    public class DimensionDefinationViewModel
    {
        public int DDId { get; set; }
        [DisplayName("Select Dimension")]
        public string DefName { get; set; }
        public bool isSelected { get; set; }
    }
    public class SubsiVSFidViewModel
    {
        public int SFId { get; set; }
        public Nullable<int> STId { get; set; }
        public Nullable<int> Fid { get; set; }
    }
    public class DimensionVSLedgerViewModel
    {
        public int Id { get; set; }
        public int Fid { get; set; }
        public int DDID { get; set; }
    }
    public class VoucherLedgerViewModel
    {
     
        public int Fid { get; set; }
        public string FName { get; set; }
        public bool HasDimension { get; set; }
        public bool HasBankInfo { get; set; }
        public bool HasSubsiList { get; set; }

    }

}
