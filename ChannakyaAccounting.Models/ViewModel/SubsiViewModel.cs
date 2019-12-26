using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Models.ViewModel
{
    public class SubsiViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmployeeId { get; set; }
        //public string Employ { get; set; }
        public int PhoneNumber { get; set; }
        public string Accno { get; set; }
        public decimal OpeningBalance { get; set; }
        public int AmountType { get; set; }
        public int ParentId { get; set; }
        public int IAccno { get; set; }
        public string AName { get; set; }
        public int TotalCount { get; set;}

        //public int MyProperty { get; set; }
    }

    public class DepositAndLoanAccountDetail
    {
        public int FId { get; set; }
        public string FName { get; set; }
        public string Accno { get; set; }
        public int IAccno { get; set; }
        public string AName { get; set; }
        public decimal OpeningBal { get; set; }
    }
    public class SubsiStatementMainViewModel
    {
        public SubsiStatementMainViewModel()
        {

        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> VType { get; set; }
        public Nullable<int> PostedBy { get; set; }
        public Nullable<int> BatchNo { get; set; }

        public IPagedList<SubsiStatementViewModel> LedgerReport { get; set; }
    }


    public class SubsiStatementViewModel
    {
        public int V2id { get; set; }
        public string VNo { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public string SubsiName { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal Balance { get; set; }
        public DateTime TrDate { get; set; }
        public Nullable<decimal> TotalDbAmt { get; set; }
        public Nullable<decimal> TotalCrAmt { get; set; }
        public int TotalDBCount { get; set; }
        public int TotalCrCount { get; set; }
        public Nullable<int> FId { get; set; }
        public Nullable<int> Id { get; set; }
    }
    public class SubsiBalanceViewModel
    {
        public string SubsiId { get; set; }
        public string OpeningBalance { get; set; }
        public string FId { get; set; }
        public string AmountType { get; set; }
        public string AccNo { get; set; }
        public Nullable<int> SID { get; set; }
        public string SubsiName { get; set; }
        public Nullable<decimal> OpBal { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
        public Nullable<decimal> ClBal { get; set; }
        public Nullable<decimal> TotalDbAmt { get; set; }
        public Nullable<decimal> TotalCrAmt { get; set; }
        public Nullable<int> TotalDBCount { get; set; }
        public Nullable<int> TotalCrCount { get; set; }
    }

    public  class FGetSubsiNameListModel
    {
        public decimal CustId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
    #region mausham
    public class uservsvoucherViewModel
    {
        public int userid { get; set; }
        public int Vtype { get; set; }
        public List<Batch> BatchList { get; set; }
        public string userName { get; set; }
        public string VoucherType { get; set; }

    }
    public class Batch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }

    }
    public class usertovoucherreturnViewModel
    {
        public int ID { get; set; }
        public int userid { get; set; }
        public int Vtype { get; set; }
        public int Batchtype { get; set; }
        public string BatchList { get; set; }
        public List<usertovoucherreturnViewModel> voucherreturnViewModel { get; set; }
    }
    public class SubsiListViewModel
    {
        public Nullable<int> AccId { get; set; }
        public string AccName { get; set; }
        public string AccNo { get; set; }
        public int CId { get; set; }

    }
    #endregion
}
