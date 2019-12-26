using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.VoucherReport
{
    public class VoucherReportsService
    {
        private readonly GenericUnitOfWork uow = new GenericUnitOfWork();

        public List<VoucherReportViewModel> GetVerifiedVoucherList(DateTime StartDate, DateTime EndDate, Nullable<int> BranchId, Nullable<int> VType, Nullable<int> PostedBy, string BatchNo, int pageNo = 1, int pageSize = 5)
        {

            //var startDate = StartDate == null ? DateTime.MinValue : StartDate;
            //var endDate = EndDate == null ? DateTime.MinValue : EndDate;
            //  var branchId = BranchId == null ? 0 : BranchId;
            var branchId = Loader.Models.Global.BranchId;
            var postedBy = PostedBy == null ? 0 : PostedBy;
            //var batchNo = BatchNo == null ? 0 : BatchNo;
            var vType = VType == null ? 0 : VType;
            //Nullable<int> branchId = BranchId == null ? DBNull.Value : BranchId;
            // modified
            var str = "select * from [acc].[GetVerifiedVoucherList]('" + StartDate + "', '" + EndDate + "', " + branchId + "," + vType + ", " + postedBy + ", " + BatchNo + "," + pageNo + "," + pageSize + "," + Loader.Models.Global.SelectedFYID + ")";
            List<VoucherReportViewModel> voucherList = uow.Repository<VoucherReportViewModel>()
                .SqlQuery("select * from [acc].[GetVerifiedVoucherList]('" + StartDate + "', '" + EndDate + "', " + branchId + "," + vType + ", " + postedBy + ", '" + BatchNo + "'," + pageNo + "," + pageSize + "," + Loader.Models.Global.SelectedFYID + ")").ToList();
            return voucherList;
        }

        public List<VoucherReportViewModel> GetUnVerifiedVoucherList(DateTime StartDate, DateTime EndDate, Nullable<int> BranchId, Nullable<int> VType, Nullable<int> PostedBy, string BatchNo, int pageNo = 1, int pageSize = 5)
        {

            //var startDate = StartDate == null ? DateTime.MinValue : StartDate;
            //var endDate = EndDate == null ? DateTime.MinValue : EndDate;
            // var branchId = BranchId == null ? 0 : BranchId;
            var branchId = Loader.Models.Global.BranchId;
            var postedBy = PostedBy == null ? 0 : PostedBy;
            // var batchNo = BatchNo == null ? 0 : BatchNo;
            var vType = VType == null ? 0 : VType;
            //Nullable<int> branchId = BranchId == null ? DBNull.Value : BranchId;
            var str = "select * from [acc].[GetUnVerifiedVoucherList]('" + StartDate + "', '" + EndDate + "', " + branchId + "," + vType + ", " + postedBy + ", '" + BatchNo + "'," + pageNo + "," + pageSize + "," + Loader.Models.Global.SelectedFYID + ")";
            List<VoucherReportViewModel> voucherList = uow.Repository<VoucherReportViewModel>()
                .SqlQuery(str).ToList();
            return voucherList;
        }
         
        public string CompanyLogo()
        {
            return  uow.Repository<ChannakyaAccounting.Models.Models.ParamValue>().GetSingle(x => x.PId == 8).PValue;
        }

        #region TrialBalance
        /// <summary>
        /// For Report Generation of Transaction Trial Balance
        /// </summary>
        /// <returns></returns>
        public List<Models.ViewModel.TrialBalanceViewModel> GetTransactionTrialBalanceReport(DateTime? fromDate, DateTime? tillDate, List<int> branchIdList, int pageNo = 1, int pageSize = 10)
        {
            //List<Models.ViewModel.TrialBalanceViewModel> TBReport = uow.Repository<Models.ViewModel.TrialBalanceViewModel>()
            //    .ExecWithStoreProcedure("select COUNT(*) OVER  () AS TotalCount,(select sum(ISNULL(a.DebitAmount,0)) from dbo.GetTrialBalanceReport() a ) as TotalDebitAmount,(select sum(ISNULL(a.CreditAmount,0)) from dbo.GetTrialBalanceReport() a ) as TotalCreditAmount , * from dbo.gettrialbalancereport() ORDER BY FId OFFSET((" + pageNo + @" -1) * " + pageSize + @")ROWS FETCH NEXT
            //        " + pageSize + " ROWS ONLY").ToList();
            string branchIdListInString = "";
            for (int i = 0; i < branchIdList.Count(); i++)
            {
                if (i != (branchIdList.Count - 1))
                {
                    branchIdListInString = branchIdListInString + branchIdList[i].ToString() + ",";
                }
                else
                {
                    branchIdListInString = branchIdListInString + branchIdList[i].ToString();
                }
            }

            var query = "select PId,(select sum(ISNULL(a.DebitAmount, 0)) from dbo.[TESTGetTransactionTrialBalanceReport]('" + fromDate + "', '" + tillDate + "', '" + branchIdListInString + "') a where PId = 0) as TotalDebitAmount,(select sum(ISNULL(a.CreditAmount, 0)) from dbo.[TESTGetTransactionTrialBalanceReport]('" + fromDate + "','" + tillDate + "','" + branchIdListInString + "') a where PId = 0 ) as TotalCreditAmount , *from dbo.[TESTGetTransactionTrialBalanceReport]('" + fromDate + "','" + tillDate + "','" + branchIdListInString + "')";

            Nullable<decimal> totalDebitAmount = uow.Repository<Models.ViewModel.TrialBalanceViewModel>().SqlQuery("select sum(ISNULL(a.DebitAmount,0)) as TotalDebitAmount from dbo.[TESTGetTransactionTrialBalanceReport]('" + fromDate + "','" + tillDate + "','" + branchIdListInString + "') a where PId=0").ToList().First().TotalDebitAmount;
            Nullable<decimal> totalCreditAmount = uow.Repository<Models.ViewModel.TrialBalanceViewModel>().SqlQuery("select sum(ISNULL(a.CreditAmount,0)) as TotalCreditAmount from dbo.[TESTGetTransactionTrialBalanceReport]('" + fromDate + "','" + tillDate + "','" + branchIdListInString + "') a where PId=0").ToList().First().TotalCreditAmount;
            
            List<Models.ViewModel.TrialBalanceViewModel> TBReport = uow.Repository<Models.ViewModel.TrialBalanceViewModel>().ExecWithStoreProcedure("select PId,(" + totalDebitAmount.ToString() + ") as TotalDebitAmount,(" + totalCreditAmount.ToString() + ") as TotalCreditAmount , * from dbo.[TESTGetTransactionTrialBalanceReport]('" + fromDate + "','" + tillDate + "','" + branchIdListInString + "')").ToList();
            return TBReport;
        }

        /// <summary>
        /// For Report Generation Of Normal Trial Balance
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Models.ViewModel.TrialBalanceViewModel> GetNormalTrialBalanceReport(DateTime? toDate, List<int> branchIdList, int pageNo = 1, int pageSize = 10)
        {
            string branchIdListInString = "";
            for (int i = 0; i < branchIdList.Count(); i++)
            {
                if (i != (branchIdList.Count - 1))
                {
                    branchIdListInString = branchIdListInString + branchIdList[i].ToString() + ",";
                }
                else
                {
                    branchIdListInString = branchIdListInString + branchIdList[i].ToString();
                }
            }

            //List<Models.ViewModel.TrialBalanceViewModel> TBReport = uow.Repository<Models.ViewModel.TrialBalanceViewModel>()
            //    .ExecWithStoreProcedure("select COUNT(*) OVER  () AS TotalCount,(select sum(ISNULL(a.DebitAmount,0)) from dbo.GetTrialBalanceReport() a ) as TotalDebitAmount,(select sum(ISNULL(a.CreditAmount,0)) from dbo.GetTrialBalanceReport() a ) as TotalCreditAmount , * from dbo.gettrialbalancereport() ORDER BY FId OFFSET((" + pageNo + @" -1) * " + pageSize + @")ROWS FETCH NEXT
            //        " + pageSize + " ROWS ONLY").ToList();

            Nullable<decimal> totalDebitAmount = uow.Repository<Models.ViewModel.TrialBalanceViewModel>().SqlQuery("select sum(ISNULL(a.DebitAmount, 0)) as TotalDebitAmount from dbo.GetNormalTrialBalanceReportTest('" + toDate + "','" + branchIdListInString + "') a where PId=0").ToList().First().TotalDebitAmount;
            Nullable<decimal> totalCreditAmount = uow.Repository<Models.ViewModel.TrialBalanceViewModel>().SqlQuery("select sum(ISNULL(a.CreditAmount, 0)) as TotalCreditAmount  from dbo.GetNormalTrialBalanceReportTest('" + toDate + "','" + branchIdListInString + "') a where PId=0 ").ToList().First().TotalCreditAmount;

            List<Models.ViewModel.TrialBalanceViewModel> TBReport = uow.Repository<Models.ViewModel.TrialBalanceViewModel>().ExecWithStoreProcedure("select PId, ("+ totalDebitAmount.ToString()+ ") as TotalDebitAmount, (" + totalCreditAmount.ToString()+ ") as TotalCreditAmount, * from dbo.GetNormalTrialBalanceReportTest('" + toDate + "','" + branchIdListInString + "')").ToList();
            return TBReport;
        }

        public Models.ViewModel.ReportTreeView GenerateTrialBalanceTree(List<TrialBalanceViewModel> trialBalance, DateTime? fromDate = null, DateTime? toDate = null, bool withPrevYr = false)
        {
            Models.ViewModel.ReportTreeView tree = this.GenerateTree(trialBalance, 0, fromDate, toDate, withPrevYr);
            return tree;
        }

        private Models.ViewModel.ReportTreeView GenerateTree(List<Models.ViewModel.TrialBalanceViewModel> list, int? parentMenuId, DateTime? fromDate = null, DateTime? toDate = null, bool withPrevYr = false)
        {
            //int prevFYear = Loader.Models.Global.SelectedFYID - 1;

            var parent = list.Where(x => x.PId == parentMenuId).ToList();

            var finaccList = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Pid == parentMenuId).ToList();
            Models.ViewModel.ReportTreeView tree = new Models.ViewModel.ReportTreeView();
            tree.Title = "Reports";

            //var prevYrList = uow.Repository<Models.ViewModel.TrialBalanceViewModel>().ExecWithStoreProcedure("select PId, (select sum(ISNULL(a.DebitAmount, 0)) from dbo.GetNormalTrialBalanceReport(" + prevFYear + ") a where PId=0) as TotalDebitAmount,(select sum(ISNULL(a.CreditAmount, 0)) from dbo.GetNormalTrialBalanceReport(" + prevFYear + ") a where PId=0 ) as TotalCreditAmount , *from dbo.GetNormalTrialBalanceReport(" + prevFYear + ")").ToList();


            foreach (var item in parent)
            {

                var count = tree.TreeData.Count;

                //if (count > 0)
                //{
                //    foreach (var item2 in tree.TreeData)
                //    {
                //        if (item2.Id == item.FId)
                //        {
                //            item2.PrevYrDebitAmount = item.PrevYrDebitAmount + item2.PrevYrDebitAmount;
                //            item2.PrevYrCreditAmount = item.PrevYrCreditAmount + item2.PrevYrCreditAmount;
                //            item2.CreditAmount = item.CreditAmount + item2.CreditAmount;
                //            item2.DebitAmount = item.DebitAmount + item2.DebitAmount;
                //        }
                //        else
                //        {
                //            tree.TreeData.Add(new Models.ViewModel.ReportTreeDTO
                //            {
                //                Id = item.FId,
                //                Text = item.Fname,
                //                PId = parentMenuId,
                //                PrevYrDebitAmount = item.PrevYrDbAmnt == null ? 0 : item.PrevYrDbAmnt,
                //                PrevYrCreditAmount = item.PrevYrCrAmnt == null ? 0 : item.PrevYrCrAmnt,
                //                CreditAmount = item.CreditAmount,
                //                DebitAmount = item.DebitAmount,
                //                TotalCount = 1,
                //                TotalCreditAmount = item.TotalCreditAmount,
                //                TotalDebitAmount = item.TotalDebitAmount
                //                //TDate = trialbalObj.TDate
                //            });
                //        }
                //    }
                //}
                //else
                //{
                    tree.TreeData.Add(new Models.ViewModel.ReportTreeDTO
                    {
                        Id = item.FId,
                        Text = item.Fname,
                        PId = parentMenuId,
                        PrevYrDebitAmount = item.PrevYrDbAmnt == null ? 0 : item.PrevYrDbAmnt,
                        PrevYrCreditAmount = item.PrevYrCrAmnt == null ? 0 : item.PrevYrCrAmnt,
                        CreditAmount = item.CreditAmount,
                        DebitAmount = item.DebitAmount,
                        TotalCount = 1,
                        TotalCreditAmount = item.TotalCreditAmount,
                        TotalDebitAmount = item.TotalDebitAmount
                        //TDate = trialbalObj.TDate
                    });
                //}
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateTree(list, itm.Id, fromDate, toDate, withPrevYr).TreeData.ToList();
            }
            return tree;
        }

        public TaskViewModel GetSingleTask(int task1Id)
        {
            int UserId = Loader.Models.Global.UserId;

            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.FGetTaskDetails(" + 0 + ") where Task1id=" + task1Id).FirstOrDefault();

            //NotificationSeenOn(task1Id);
            return taskList;
        }

        public decimal GetAmount(TrialBalanceViewModel trialObj, DateTime? fromdate = null, DateTime? todate = null, bool isdebit = true)
        {
            decimal amount = 0;        //private Models.ViewModel.ReportTreeView GenerateTree(List<Models.ViewModel.TrialBalanceViewModel> list, int? parentMenuId, DateTime? fromDate = null, DateTime? toDate = null, bool withPrevYr = false)
                                       //{
                                       //    Models.ViewModel.ReportTreeView tree = new ReportTreeView();
                                       //    var finaccList = uow.Repository<Models.Models.FinAcc>().FindBy(x=>x.Pid==parentMenuId).ToList();
                                       //    foreach (var item in finaccList)
                                       //    {

            //    }



            //}
            decimal finalAmnt = 0;
            if (isdebit)
            {
                if (trialObj.PrevYrDebitAmount != null)
                {
                    finalAmnt = (decimal)trialObj.PrevYrDebitAmount;
                }

            }
            else
            {
                if (trialObj.PrevYrCreditAmount != null)
                {
                    finalAmnt = (decimal)trialObj.PrevYrCreditAmount;
                }

            }
            if (fromdate != null && todate != null)
            {
                if (trialObj.TDate >= fromdate && trialObj.TDate <= todate)
                {

                    amount = finalAmnt;
                }
            }
            else
            {
                if (fromdate != null)
                {
                    if (trialObj.TDate >= fromdate)
                    {
                        amount = finalAmnt;
                    }
                }
                if (todate != null)
                {
                    if (trialObj.TDate <= todate)
                    {
                        amount = finalAmnt;
                    }
                }
            }
            return amount;
        }

        public List<Models.ViewModel.ProfitAndLossViewModel> GetProfitAndLoss(int pageNo = 1, int pageSize = 10)
        {

            List<Models.ViewModel.ProfitAndLossViewModel> TBReport = uow.Repository<Models.ViewModel.ProfitAndLossViewModel>().ExecWithStoreProcedure("select (select sum(ISNULL(a.DebitAmount,0)) from dbo.GetProfitAndLossReport() a ) as TotalDebitAmount,(select sum(ISNULL(a.CreditAmount,0)) from dbo.GetProfitAndLossReport() a ) as TotalCreditAmount , * from dbo.GetProfitAndLossReport()").ToList();

            return TBReport;
        }

        public List<Models.ViewModel.BalanceSheetViewModel> GetBalanceSheetReport(int pageNo = 1, int pageSize = 10)
        {

            List<Models.ViewModel.BalanceSheetViewModel> TBReport = uow.Repository<Models.ViewModel.BalanceSheetViewModel>().ExecWithStoreProcedure("select (select sum(ISNULL(a.DebitAmount,0)) from dbo.GetBalanceSheetReport() a ) as TotalDebitAmount,(select sum(ISNULL(a.CreditAmount,0)) from dbo.GetBalanceSheetReport() a ) as TotalCreditAmount , * from dbo.GetBalanceSheetReport()").ToList();

            return TBReport;
        }
        #endregion

        #region ProfitAndLoss
        private Models.ViewModel.ReportTreeView GenerateTreeProfitAndLoss(List<Models.ViewModel.ProfitAndLossViewModel> list, int? parentMenuId)
        {

            var parent = list.Where(x => x.PId == parentMenuId);
            Models.ViewModel.ReportTreeView tree = new Models.ViewModel.ReportTreeView();
            tree.Title = "Reports";
            foreach (var itm in parent)
            {
                tree.TreeData.Add(new Models.ViewModel.ReportTreeDTO
                {
                    Id = itm.FId,
                    Text = itm.Fname,
                    PId = itm.PId,
                    CreditAmount = itm.CreditAmount,
                    DebitAmount = itm.DebitAmount,
                    TotalCount = itm.TotalCount,
                    TotalCreditAmount = itm.TotalCreditAmount,
                    TotalDebitAmount = itm.TotalDebitAmount,
                    TDate = itm.TDate
                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateTreeProfitAndLoss(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }
        public Models.ViewModel.ReportTreeView GenerateProfitAndLossTree(List<ProfitAndLossViewModel> profitAndLoss)
        {
            Models.ViewModel.ReportTreeView tree = this.GenerateTreeProfitAndLoss(profitAndLoss, 0);
            return tree;
        }
        #endregion

        #region BalanceSheet
        private Models.ViewModel.ReportTreeView GenerateBalanceSheetTree(List<Models.ViewModel.BalanceSheetViewModel> list, int? parentMenuId)
        {

            var parent = list.Where(x => x.PId == parentMenuId);
            Models.ViewModel.ReportTreeView tree = new Models.ViewModel.ReportTreeView();
            tree.Title = "Reports";
            foreach (var itm in parent)
            {
                tree.TreeData.Add(new Models.ViewModel.ReportTreeDTO
                {
                    Id = itm.FId,
                    Text = itm.Fname,
                    PId = itm.PId,
                    CreditAmount = itm.CreditAmount,
                    DebitAmount = itm.DebitAmount,
                    TotalCount = itm.TotalCount,
                    TotalCreditAmount = itm.TotalCreditAmount,
                    TotalDebitAmount = itm.TotalDebitAmount,
                    TDate = itm.TDate
                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateBalanceSheetTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }
        public Models.ViewModel.ReportTreeView GenerateBalanceSheetTree(List<BalanceSheetViewModel> balanceSheet)
        {
            Models.ViewModel.ReportTreeView tree = this.GenerateBalanceSheetTree(balanceSheet, 0);
            return tree;
        }

        #endregion

        #region Date
        public DateTime GetFromDate()
        {
            DateTime fromDate = DateTime.Now;
            var dateObj = uow.Repository<Models.Models.FiscalYear>().FindBy(x => x.FYID == Loader.Models.Global.SelectedFYID).FirstOrDefault();
            if (dateObj != null)
            {
                fromDate = dateObj.StartDt;
            }
            return fromDate;
        }

        public DateTime GetToDate()
        {
            DateTime toDate = DateTime.Now;
            var dateObj = uow.Repository<Models.Models.FiscalYear>().FindBy(x => x.FYID == Loader.Models.Global.SelectedFYID).FirstOrDefault();
            if (dateObj != null)
            {
                toDate = dateObj.EndDt;
            }
            return toDate;
        }

        public List<LedgerStatementViewModel> GenerateLedgerReport(int FId, DateTime? fromDate = null, DateTime? toDate = null, bool withallBranch = false, int pageNo = 1, int pagesize = 10)
        {
            Nullable<int> branchId = Loader.Models.Global.BranchId;
            if (withallBranch)
            {
                branchId = null;
            }
            var teststr = "select * from acc.FGetLedgerStmnt(" + FId + "," + branchId + ",'" + fromDate + "', '" + toDate + "', " + Loader.Models.Global.SelectedFYID + "," + pageNo + "," + pagesize + ")";
            List<LedgerStatementViewModel> ledgerList = uow.Repository<LedgerStatementViewModel>().SqlQuery("select * from acc.FGetLedgerStmnt(" + FId + "," + branchId + ",'" + fromDate + "', '" + toDate + "', " + Loader.Models.Global.SelectedFYID + "," + pageNo + "," + pagesize + ")").ToList();
            return ledgerList;
        }
        //to display all content without pagination
        public List<LedgerStatementViewModel> GenerateLedgerReportWithOutPagination(int FId, DateTime? fromDate = null, DateTime? toDate = null, bool withallBranch = false, int pageNo = 1, int pagesize = 10)
        {
            Nullable<int> branchId = Loader.Models.Global.BranchId;
            if (withallBranch)
            {
                branchId = null;
            }
            //    var teststr = "select * from acc.FGetLedgerStmnt(" + FId + "," + branchId + ",'" + fromDate + "', '" + toDate + "', " + Loader.Models.Global.SelectedFYID + "," + pageNo + "," + pagesize + ")";
            List<LedgerStatementViewModel> ledgerList = uow.Repository<LedgerStatementViewModel>()
                .SqlQuery("select * from acc.[FgetLedgerStmntWithOutPaging](" + FId + "," + branchId + ",'" + fromDate + "', '" + toDate + "', " + Loader.Models.Global.SelectedFYID + ")").ToList();
            return ledgerList;
        }


        public List<LedgerStatementViewModel> GenerateCashStatement(DateTime? fromDate = null, DateTime? toDate = null, bool withallBranch = false, int pageNo = 1, int pagesize = 10)
        {
            Nullable<int> branchId = Loader.Models.Global.BranchId;
            if (withallBranch)
            {
                branchId = null;
            }
            var teststr = "select * from acc.FGetCashStmnt(" + branchId + ",'" + fromDate + "', '" + toDate + "', " + Loader.Models.Global.SelectedFYID + "," + pageNo + "," + pagesize + ")";
            List<LedgerStatementViewModel> ledgerList = uow.Repository<LedgerStatementViewModel>()
                .SqlQuery("select * from acc.FGetCashStmnt(" + branchId + ",'" + fromDate + "', '" + toDate + "', " + Loader.Models.Global.SelectedFYID + "," + pageNo + "," + pagesize + ")").ToList();
            return ledgerList;
        }



        #endregion

        #region mausham
        public string[] GetCompanyDetail()
        {
            Loader.Repository.GenericUnitOfWork loaderUOW = new Loader.Repository.GenericUnitOfWork();

            //  List<Loader.Models.ParamValue> companyList = loaderUOW.Repository<Loader.Models.ParamValue>().FindBy(x => x.PId == (int)Loader.Helper.EnumValue.Parameter.CompanyName || x.PId == (int)Loader.Helper.EnumValue.Parameter.CompanyAddress || x.PId == (int)Loader.Helper.EnumValue.Parameter.PhoneNumber).ToList();
            List<Loader.Models.ParamValue> companyList = new List<Loader.Models.ParamValue>();
            string CompanyName = loaderUOW.Repository<Loader.Models.ParamValue>().GetSingle(x => x.PId == 26).PValue;
            string CompanyAddress = loaderUOW.Repository<Loader.Models.ParamValue>().GetSingle(x => x.PId == 5).PValue;
            string CompanyPhoneNumber = loaderUOW.Repository<Loader.Models.ParamValue>().GetSingle(x => x.PId == 6).PValue;
            string[] CompanyDetail = { CompanyName, CompanyAddress, CompanyPhoneNumber };

            return CompanyDetail;

        }
        public SelectList GetSubsiByVoucherType(int FID)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<SubsiListViewModel> SubsiNameList = uow.Repository<SubsiListViewModel>().ExecWithStoreProcedure("select * from [acc].[FGetSubsiAccListFromFId](" + FID + ")").ToList();
            foreach (var item in SubsiNameList)
            {
                selectList.Add(new SelectListItem { Text = item.AccName, Value = item.CId.ToString() });
            }
            return new SelectList(selectList, "Value", "Text");
        }
        public List<SubsiStatementViewModel> GetSubsiStatementReport(int FId, string fromDate, string toDate, bool withallBranch, int SID, int pageNo = 1, int pageSize = 10)
        {
            int branchid = Loader.Models.Global.BranchId;
            string nsns = "select * from [acc].[FgetSubsiStmnt] (" + FId + "," + branchid + "," + SID + ",'" + fromDate + "','" + toDate + "'," + pageNo + "," + pageSize + ")";
            List<SubsiStatementViewModel> SubsiDisplayList = uow.Repository<SubsiStatementViewModel>().ExecWithStoreProcedure("select * from [acc].[FgetSubsiStmnt] (" + FId + "," + branchid + "," + SID + ",'" + fromDate + "','" + toDate + "'," + pageNo + "," + pageSize + ")").ToList();
            List<SubsiStatementViewModel> SubsiDisplayList1 = uow.Repository<SubsiStatementViewModel>().ExecWithStoreProcedure("select * from [acc].[FgetSubsiStatement] (" + SID + ",'" + fromDate + "','" + toDate + "'," + pageNo + "," + pageSize + ")").ToList();
            return SubsiDisplayList;
        }
        public List<SubsiBalanceViewModel> GenerateSubsiBalReport(int FId, DateTime tillDate, bool withallBranch, int pageNo = 1, int pageSize = 10)
        {
            int branchId = Loader.Models.Global.BranchId;
            string query = "select * from [acc].[FGetSubsiBalReport](" + FId + ",'" + tillDate + "'," + branchId + "," + pageNo + "," + pageSize + ")";
            List<SubsiBalanceViewModel> BalList = uow.Repository<SubsiBalanceViewModel>().ExecWithStoreProcedure(query).ToList();
            return BalList;
        }
        public SelectList UserDetail(int vouchertype, int[] BatchList, DateTime FDate, DateTime TDate)
        {
            string batchlist = string.Join(",", BatchList);
            // batchlist.TrimEnd()
            List<TransactionReportUserViewModel> UserList = uow.Repository<TransactionReportUserViewModel>().SqlQuery("select * from acc.FgetUserDetailByVoucherAndBatch(" + vouchertype + ",'" + batchlist + "'," + Loader.Models.Global.BranchId + ",'" + FDate.ToShortDateString() + "','" + TDate.ToShortDateString() + "')").ToList();
            return new SelectList(UserList, "UserId", "UserName");
        }
        #endregion
    }
}
