using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Models.ViewModel
{
    public class ReportTreeDTO
    {

        public int Id { get; set; }
        public Nullable<int> PId { get; set; }
        public string Text { get; set; }
        public int TotalCount { get; set; }
        public Nullable<decimal> TotalDebitAmount { get; set; }
        public Nullable<decimal> TotalCreditAmount { get; set; }
        public Nullable<DateTime> TDate { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
        public Nullable<decimal> PrevYrDebitAmount { get; set; }
        public Nullable<decimal> PrevYrCreditAmount { get; set; }
        public List<ViewModel.ReportTreeDTO> Children { get; set; }
    }



    public class ReportTreeView
    {
        public ReportTreeView()
        {
            TreeData = new List<ViewModel.ReportTreeDTO>();
            Title = "Treeview";
        }
        public List<ViewModel.ReportTreeDTO> TreeData { get; set; }
        public string Title { get; set; }
    }

    public class TrialBalanceViewModel
    {
        public TrialBalanceViewModel()
        {
            this.TreeData = new List<TrialBalanceViewModel>();
        }
        public int FId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string BranchIdString { get; set; }
        public string Fname { get; set; }
        public int PId { get; set; }
        public int TotalCount { get; set; }
        public Nullable<decimal> TotalDebitAmount { get; set; }
        public Nullable<decimal> TotalCreditAmount { get; set; }
        // public Nullable<decimal> PrYrBal { get; set; }

        public Nullable<decimal> PrevYrDbAmnt { get; set; }
        public Nullable<decimal> PrevYrCrAmnt { get; set; }
        public Nullable<DateTime> TDate { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
        public Nullable<decimal> PrevYrDebitAmount { get; set; }
        public Nullable<decimal> PrevYrCreditAmount { get; set; }
        public List<ViewModel.TrialBalanceViewModel> TreeData { get; set; }
    }

    public class ProfitAndLossViewModel
    {
        public int FId { get; set; }
        public string Fname { get; set; }
        public int PId { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalDebitAmount { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public Nullable<DateTime> TDate { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
    }

    public class BalanceSheetViewModel
    {
        public int FId { get; set; }
        public string Fname { get; set; }
        public int PId { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalDebitAmount { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public Nullable<DateTime> TDate { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
    }

    public class BranchCheckBox
    {
        public int BranchId { get; set; }
        public bool IsChecked { get; set; }
    }

}
