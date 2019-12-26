using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Service.Voucher1;
using ChannakyaAccounting.Service.VoucherReport;
using Loader;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataEntities = ChannakyaAccounting.Repository.UnitOfWork;

namespace ChannakyaAccounting.Controllers
{
    [MyAuthorize]
    public class VoucherReportController : Controller
    {
        //private TaskVerificationService taskVerification = null;

        VoucherReportsService _voucherReportService = new VoucherReportsService();
        Voucher1Service _voucher1Service = new Voucher1Service();
        Loader.Service.UsersService _usersService = new Loader.Service.UsersService();

        // GET: VoucherReport
        public ActionResult Index(int url = 0)
        {
            ViewData["ReportType"] = url;
            return View();
        }

        public ActionResult VoucherIndex()
        {
            ViewData["VTypeList"] = _voucher1Service.GetVoucherType();
            ViewData["BatchList"] = _voucher1Service.GetBatch();
            return View(new ChannakyaAccounting.Models.ViewModel.VoucherReportMainViewModel());
        }

        public ActionResult GetBatchForCheckBox(int VType)
        {
            var BatchList = _voucher1Service.GetBatchNumberForCheckBox(VType);
            return PartialView(BatchList);
        }

        public ActionResult _VerificationModal(int v1Id)
        {
            //   string query = "select * from [Mast].[Task] where EventValue= " + v1Id; change from EventValue to Task1id
            var task1Obj = new Loader.Repository.GenericUnitOfWork().Repository<TaskViewModel>().SqlQuery("select * from [Mast].[Task] where Task1id= " + v1Id).FirstOrDefault();
            ViewBag.EventValue = v1Id;
            TaskViewModel tsk = null;
            if (task1Obj != null)
            {
                tsk = _voucherReportService.GetSingleTask(task1Obj.Task1Id);
            }
            var voucherObj = new DataEntities.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher1>().GetSingle(x => x.V1Id == v1Id);
            if (voucherObj != null && tsk == null)
            {
                ViewBag.ApprovedOn = voucherObj.ApprovedOn;
                ViewBag.ApprovedBy = voucherObj.ApprovedBy;
                ViewBag.PostedOn = voucherObj.PostedOn;
                ViewBag.PostedBy = (int)voucherObj.PostedBy;
            }

            return PartialView(tsk);

        }

        public ActionResult GetUserDetails(string query)
        {

            int? page = 1;
            var list = new Loader.Repository.GenericUnitOfWork().Repository<Loader.Models.ApplicationUser>().GetAll().Where(x => x.EmployeeId != 0 && x.IsActive == true).AsQueryable();
            //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.CustIndividual>().GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
                ViewBag.Query = query;
            }
            int pageSize = 8;
            var dataList = list.OrderBy(m => m.UserName);
            ViewBag.ActivePager = page;
            //var pagedList = new Loader.ViewModel.Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize);
            var pagedList = new Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
            return PartialView(pagedList);
        }

        public ActionResult _GetUserDetails(string query = null, int? fId = 0, int? page = 1)
        {
            var list = new Loader.Repository.GenericUnitOfWork().Repository<Loader.Models.ApplicationUser>().GetAll().Where(x => x.EmployeeId != 0 && x.IsActive == true).AsQueryable();
            //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.CustIndividual>().GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
                ViewBag.Query = query;
            }
            const int pageSize = 8;
            var dataList = list.OrderBy(m => m.UserName);
            ViewBag.ActivePager = page;
            //var pagedList = new Loader.ViewModel.Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize);
            var pagedList = new Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
            return PartialView(pagedList);
        }

        #region transcation user mapped with voucher and batch
        public ActionResult GetUserDetailsForTrans(int VoucherType, string BatchList, DateTime FDate, DateTime TDate)
        {
            int[] BList = BatchList.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var list = _voucherReportService.UserDetail(VoucherType, BList, FDate, TDate);
            return Json(list, JsonRequestBehavior.AllowGet);
            // return PartialView();

        }
        //public ActionResult _GetUserDetailsForTrans(int VoucherType, string BatchList, string query = null,  int? fId = 0, int? page = 1)
        //{
        //    int PageSize = 12;
        //    int[] BList = BatchList.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
        //    var list = _voucherReportService.UserDetail(query, VoucherType, BList, Convert.ToInt32(page), PageSize).AsQueryable();
        //    var dataList = list.OrderBy(m => m.UserName);
        //    ViewBag.ActivePager = page;
        //    ViewBag.VoucherType = VoucherType;
        //    ViewBag.BatchList = BatchList;
        //    var pagedList = new Pagination<Models.ViewModel.TransactionReportUserViewModel>(dataList, page ?? 0, PageSize);
        //    ViewBag.CountPager = new Pagination<Models.ViewModel.TransactionReportUserViewModel>(dataList, page ?? 0, PageSize).TotalPages;
        //    return PartialView(pagedList);
        //}

        #endregion

        public ActionResult SearchUserList(string query)
        {
            int? page = 1;
            var list = new Loader.Repository.GenericUnitOfWork().Repository<Loader.Models.ApplicationUser>().FindBy(x => x.UserName.Contains(query)).Where(x => x.IsActive == true && x.EmployeeId != 0);
            ViewBag.Query = query;
            //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<M>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).AsQueryable(); ;
            //if (!string.IsNullOrEmpty(query))
            //{
            //    list = list.Where(m => m..Contains(query) || m.Code.Contains(query));

            //}
            const int pageSize = 12;
            var dataList = list.OrderBy(m => m.UserName).AsQueryable();


            var pagedList = new Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);

        }

        public ActionResult GetVoucherList(DateTime? StartDate, DateTime? EndDate, int? VType, int? PostedBy, string BatchNo, string VerifiedVoucher)
        {
            int pageSize = 12;
            Models.ViewModel.VoucherReportMainViewModel voucherMainReport = new VoucherReportMainViewModel();
            voucherMainReport.BatchNo = BatchNo;
            voucherMainReport.StartDate = (DateTime)StartDate;
            voucherMainReport.EndDate = (DateTime)EndDate;
            voucherMainReport.VType = VType;
            voucherMainReport.PostedBy = PostedBy;
            voucherMainReport.VerifiedVoucher = Convert.ToBoolean(VerifiedVoucher);
            //get base64 and add in viewbag 

            //string base64 = "";
            string Logo = _voucherReportService.CompanyLogo(); //  companyLogo 
            //ViewBag.LogoPath = Path;
            //Path = Server.MapPath("~") + Path;
            //string base64String = "";
            //using (System.Drawing.Image image = System.Drawing.Image.FromFile(Path))
            //{
            //    using (System.IO.MemoryStream m = new System.IO.MemoryStream())
            //    {
            //        image.Save(m, image.RawFormat);
            //        byte[] imageBytes = m.ToArray();

            //        // Convert byte[] to Base64 String
            //        base64String = Convert.ToBase64String(imageBytes);
            //    }
            //}
            ViewBag.Logo = Logo;

            var companyDetail = _voucherReportService.GetCompanyDetail();
            //if (companyDetail.Count > 3)
            //{
                ViewBag.CompanyName = companyDetail;
                ViewBag.CompanyAddress = companyDetail;
                ViewBag.CompanyPhoneNo = companyDetail;
            //}
            ViewBag.Title = "Voucher List Report";

            Models.ViewModel.VoucherReportMainViewModel voucherMain = new Models.ViewModel.VoucherReportMainViewModel();
            if (voucherMainReport.VerifiedVoucher)
            {
                var voucherList = _voucherReportService.GetVerifiedVoucherList(Convert.ToDateTime(voucherMainReport.StartDate), Convert.ToDateTime(voucherMainReport.EndDate), voucherMainReport.BranchId, voucherMainReport.VType, voucherMainReport.PostedBy, voucherMainReport.BatchNo, 1, pageSize);
                voucherMain.VoucherReport = voucherList.ToPagedList(1, pageSize);
                ViewBag.Title = "Verified Voucher";
                ViewBag.IsVerified = true;
            }
            else
            {
                var voucherList = _voucherReportService.GetUnVerifiedVoucherList(Convert.ToDateTime(voucherMainReport.StartDate), Convert.ToDateTime(voucherMainReport.EndDate), voucherMainReport.BranchId, voucherMainReport.VType, voucherMainReport.PostedBy, voucherMainReport.BatchNo, 1, pageSize);
                voucherMain.VoucherReport = voucherList.ToPagedList(1, pageSize);
                ViewBag.Title = "UnVerified Voucher";
                ViewBag.IsVerified = false;
            }
            voucherMain.StartDate = voucherMainReport.StartDate;
            voucherMain.EndDate = voucherMainReport.EndDate;
            voucherMain.BatchNo = voucherMainReport.BatchNo;
            voucherMain.VType = voucherMainReport.VType;
            voucherMain.PostedBy = voucherMainReport.PostedBy;
            voucherMain.VerifiedVoucher = voucherMainReport.VerifiedVoucher;
            ViewBag.ActivePager = 1;

            if (voucherMain.VoucherReport.Count > 0)
            {
                ViewBag.PagerCount = Math.Ceiling(Convert.ToDecimal(voucherMain.VoucherReport.FirstOrDefault().totalCount) / pageSize);
                return View(voucherMain);
            }

            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetVoucherList(Models.ViewModel.VoucherReportMainViewModel voucherMainReport, int pageno = 1)
        {
            int pageSize = 12;
            Models.ViewModel.VoucherReportMainViewModel voucherMain = new Models.ViewModel.VoucherReportMainViewModel();
            if (voucherMainReport.VerifiedVoucher)
            {
                var voucherList = _voucherReportService.GetVerifiedVoucherList(Convert.ToDateTime(voucherMainReport.StartDate), Convert.ToDateTime(voucherMainReport.EndDate), voucherMainReport.BranchId, voucherMainReport.VType, voucherMainReport.PostedBy, voucherMainReport.BatchNo, pageno, pageSize);
                voucherMain.VoucherReport = voucherList.ToPagedList(1, pageSize);
                ViewBag.Title = "Verified Voucher";
                ViewBag.IsVerified = true;
            }
            else
            {
                var voucherList = _voucherReportService.GetUnVerifiedVoucherList(Convert.ToDateTime(Loader.Models.Global.TransactionDate).AddMonths(-12), Convert.ToDateTime(Loader.Models.Global.TransactionDate).AddMonths(12), voucherMainReport.BranchId, voucherMainReport.VType, voucherMainReport.PostedBy, voucherMainReport.BatchNo, pageno, pageSize);
                voucherMain.VoucherReport = voucherList.ToPagedList(1, pageSize);
                ViewBag.Title = "UnVerified Voucher";
                ViewBag.IsVerified = false;
            }
            voucherMain.StartDate = voucherMainReport.StartDate;
            voucherMain.EndDate = voucherMainReport.EndDate;
            voucherMain.PostedBy = voucherMainReport.PostedBy;
            voucherMain.VType = voucherMainReport.VType;
            voucherMain.BatchNo = voucherMainReport.BatchNo;
            ViewBag.ActivePager = pageno;
            //ViewBag.PagerCount = Math.Ceiling(Convert.ToDecimal(voucherMain.VoucherReport.SingleOrDefault().totalCount) / pageSize);
            return PartialView(voucherMain);
        }
        public ActionResult TransReportPagination(ChannakyaAccounting.Models.ViewModel.VoucherReportMainViewModel voucherMainReport, int totalPage, int pageno = 1)
        {
            //   const int PageSize = 2;
            ViewBag.ActivePager = pageno;
            ViewBag.totalPage = totalPage;
            // ViewBag.total = totalPage;
            return PartialView(voucherMainReport);


        }

        public ActionResult TrialBalance(List<Models.ViewModel.BranchCheckBox> branchList, int page = 1, DateTime? fromDate = null, DateTime? toDate = null, bool reportType = false)
        {

            var companyDetail = _voucherReportService.GetCompanyDetail();
            //if (companyDetail.Count > 2)
            //{
                ViewBag.CompanyName = companyDetail[0];
                ViewBag.CompanyAddress = companyDetail[1];
                ViewBag.CompanyPhoneNo = companyDetail[2];
            //}

            string Logo = _voucherReportService.CompanyLogo(); //  companyLogo 
            //ViewBag.LogoPath = Path;
            //Path = Server.MapPath("~") + Path;
            //string base64String = "";
            //using (System.Drawing.Image image = System.Drawing.Image.FromFile(Path))
            //{
            //    using (System.IO.MemoryStream m = new System.IO.MemoryStream())
            //    {
            //        image.Save(m, image.RawFormat);
            //        byte[] imageBytes = m.ToArray();

            //        // Convert byte[] to Base64 String
            //        base64String = Convert.ToBase64String(imageBytes);
            //    }
            //}
            ViewBag.Logo = Logo;

            ViewBag.Title = "Trial Balance Report";

            List<int> branchIdList = new List<int>();
            foreach (var item in branchList)
            {
                if (item.IsChecked == true)
                {
                    branchIdList.Add(item.BranchId);
                }
            }

            if (reportType)
            {
                var trialBalanceReport = _voucherReportService.GetTransactionTrialBalanceReport(fromDate, toDate, branchIdList, 0);//.Where(x => branchList.Select(y => y.BranchId).ToList().Contains(Convert.ToInt32(x.BranchId))).ToList();
                ViewData["TrialBalanceTree"] = _voucherReportService.GenerateTrialBalanceTree(trialBalanceReport, fromDate, toDate, false);
                ViewData["WithPrevYr"] = true;
                ViewData["ReportType"] = 1;
                return View(trialBalanceReport);
            }
            else
            {
                var trialBalanceReport = _voucherReportService.GetNormalTrialBalanceReport(toDate, branchIdList);//.Where(x => branchList.Select(y => y.BranchId).ToList().Contains(Convert.ToInt32(x.BranchId))).ToList(); ;
                ViewData["TrialBalanceTree"] = _voucherReportService.GenerateTrialBalanceTree(trialBalanceReport, fromDate, toDate, true);
                ViewData["WithPrevYr"] = true;
                ViewData["ReportType"] = 1;
                return View(trialBalanceReport);
            }

        }
        public ActionResult ProfitAndLoss(List<Models.ViewModel.BranchCheckBox> branchList, int page = 1, DateTime? fromDate = null, DateTime? toDate = null, bool reportType = false)
        {
            var companyDetail = _voucherReportService.GetCompanyDetail();
            //if (companyDetail.Count > 2)
            //{
                ViewBag.CompanyName = companyDetail[0];
                ViewBag.CompanyAddress = companyDetail[1];
                ViewBag.CompanyPhoneNo = companyDetail[2];
            //}
            List<int> branchIdList = new List<int>();
            foreach (var item in branchList)
            {
                if (item.IsChecked == true)
                {
                    branchIdList.Add(item.BranchId);
                }
            }

            ViewBag.Title = "Profit And Loss Report";
            if (reportType)
            {
                var trialBalanceReport = _voucherReportService.GetTransactionTrialBalanceReport(fromDate, toDate, branchIdList, 0)
                    .Where(x => branchList.Where(y => y.IsChecked == true).Select(y => y.BranchId).ToList().Contains(Convert.ToInt32(x.BranchId)))
                    .Where(x => ExecuteMe(x.FId) == 1 || ExecuteMe(x.FId) == 2).ToList();/*.Where(x => branchList.Where(y => y.IsChecked == true).Select(y => y.BranchId).ToList().Contains(x.BranchId) && (ExecuteMe(x.BranchId) == 1 || ExecuteMe(x.BranchId) == 2)).ToList();*/

                ViewData["TrialBalanceTree"] = _voucherReportService.GenerateTrialBalanceTree(trialBalanceReport, fromDate, toDate, false);
                ViewData["WithPrevYr"] = false;
                ViewData["ReportType"] = 2;
                return View("TrialBalance", trialBalanceReport);
            }
            else
            {
                var trialBalanceReport = _voucherReportService.GetNormalTrialBalanceReport(toDate, branchIdList)
                    .Where(x => branchList.Where(y => y.IsChecked == true).Select(y => y.BranchId).ToList().Contains(Convert.ToInt32(x.BranchId)))
                    .Where(x => ExecuteMe(x.FId) == 1 || ExecuteMe(x.FId) == 2).ToList();/*.Where(x => branchList.Where(y => y.IsChecked == true).Select(y => y.BranchId).ToList().Contains(x.BranchId) && (ExecuteMe(x.BranchId) == 1 || ExecuteMe(x.BranchId) == 2)).ToList();*/

                ViewData["TrialBalanceTree"] = _voucherReportService.GenerateTrialBalanceTree(trialBalanceReport, fromDate, toDate, true);
                ViewData["WithPrevYr"] = true;
                ViewData["ReportType"] = 2;
                return View("TrialBalance", trialBalanceReport);
            }

        }
        public ActionResult BalanceSheet(List<Models.ViewModel.BranchCheckBox> branchList, int page = 1, DateTime? fromDate = null, DateTime? toDate = null, bool reportType = false)
        {
            var companyDetail = _voucherReportService.GetCompanyDetail();
            //if (companyDetail.Count > 2)
            //{
                ViewBag.CompanyName = companyDetail[0];
                ViewBag.CompanyAddress = companyDetail[1];
                ViewBag.CompanyPhoneNo = companyDetail[2];
            //}
            List<int> branchIdList = new List<int>();
            foreach (var item in branchList)
            {
                if (item.IsChecked == true)
                {
                    branchIdList.Add(item.BranchId);
                }
            }
            ViewBag.Title = " BalanceSheet Report";
            if (reportType)
            {
                var trialBalanceReport = _voucherReportService.GetTransactionTrialBalanceReport(fromDate, toDate, branchIdList, 0).Where(x => branchList.Where(y => y.IsChecked == true).Select(y => y.BranchId).ToList().Contains(Convert.ToInt32(x.BranchId))).Where(x => ExecuteMe(x.FId) == 3 || ExecuteMe(x.FId) == 4).ToList();/*.Where(x => branchList.Where(y => y.IsChecked == true).Select(y => y.BranchId).ToList().Contains(x.BranchId) && (ExecuteMe(x.BranchId) == 1 || ExecuteMe(x.BranchId) == 2)).ToList();*/
                ViewData["TrialBalanceTree"] = _voucherReportService.GenerateTrialBalanceTree(trialBalanceReport, fromDate, toDate, false);
                ViewData["WithPrevYr"] = false;
                ViewData["ReportType"] = 3;
                return View("TrialBalance", trialBalanceReport);
            }
            else
            {
                var trialBalanceReport = _voucherReportService.GetNormalTrialBalanceReport(toDate, branchIdList).Where(x => branchList.Where(y => y.IsChecked == true).Select(y => y.BranchId).ToList().Contains(Convert.ToInt32(x.BranchId))).Where(x => ExecuteMe(x.FId) == 3 || ExecuteMe(x.FId) == 4).ToList();/*.Where(x => branchList.Where(y => y.IsChecked == true).Select(y => y.BranchId).ToList().Contains(x.BranchId) && (ExecuteMe(x.BranchId) == 1 || ExecuteMe(x.BranchId) == 2)).ToList();*/
                ViewData["TrialBalanceTree"] = _voucherReportService.GenerateTrialBalanceTree(trialBalanceReport, fromDate, toDate, true);
                ViewData["WithPrevYr"] = true;
                ViewData["ReportType"] = 3;
                return View("TrialBalance", trialBalanceReport);
            }

        }
        public ActionResult DisplayLedgerList(string query = null, int? fId = 0, int? page = 1)
        {
            const int pageSize = 12;
            var finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).AsQueryable();
            if (fId != 0)
            {
                finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fId && x.FinSys2.FinSys1.IsGroup == false).AsQueryable();
            }
            if (!string.IsNullOrEmpty(query))
            {
                finaccList = finaccList.Where(m => m.Fname.Contains(query));
                ViewBag.Query = query;
            }

            var dataList = finaccList.OrderBy(m => m.Fname);
            var pagedList = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;


            return PartialView(pagedList);

        }
        public ActionResult SearchLedgerList(string query, int? page = 1)
        {
            //int? page = 1;
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.Fname.Contains(query) || m.Code.Contains(query));
                ViewBag.Query = query;
            }
            const int pageSize = 12;
            var dataList = list.OrderBy(m => m.Fname);


            var pagedList = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }
        public ActionResult DisplayLedgerListOfBank(string query = null, int? fId = 0, int? page = 1)
        {
            const int pageSize = 12;
            var finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false && x.FinSys2.FinSys1.F1id == (int)EnumValue.FinSys1.Bank).AsQueryable();
            if (fId != 0)
            {
                finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fId && x.FinSys2.FinSys1.IsGroup == false && x.FinSys2.FinSys1.F1id == (int)EnumValue.FinSys1.Bank).AsQueryable();
            }
            if (!string.IsNullOrEmpty(query))
            {
                finaccList = finaccList.Where(m => m.Fname.Contains(query) || m.Code.Contains(query));
                ViewBag.Query = query;
            }

            var dataList = finaccList.OrderBy(m => m.Fname);
            var pagedList = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;


            return PartialView(pagedList);

        }
        public ActionResult _DisplayLedgerListOfBank(string query = null, int? fId = 0, int? page = 1)
        {
            const int pageSize = 12;
            var finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false && x.FinSys2.FinSys1.F1id == (int)EnumValue.FinSys1.Bank).AsQueryable();
            if (fId != 0)
            {
                finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fId && x.FinSys2.FinSys1.IsGroup == false && x.FinSys2.FinSys1.F1id == (int)EnumValue.FinSys1.Bank).AsQueryable();
            }
            if (!string.IsNullOrEmpty(query))
            {
                finaccList = finaccList.Where(m => m.Fname.Contains(query) || m.Code.Contains(query));
                ViewBag.Query = query;
            }

            var dataList = finaccList.OrderBy(m => m.Fname);
            var pagedList = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;


            return PartialView(pagedList);

        }
        public ActionResult SearchBankList(string query = null)
        {
            int? page = 1;
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false && x.FinSys2.FinSys1.F1id == (int)EnumValue.FinSys1.Bank).AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.Fname.Contains(query) || m.Code.Contains(query));
                ViewBag.Query = query;
            }
            const int pageSize = 12;
            var dataList = list.OrderBy(m => m.Fname);


            var pagedList = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }
        //[HttpPost]
        public ActionResult LedgerReportIndex()
        {
            return PartialView("~/Views/VoucherReport/LedgerReport/Index.cshtml");
        }

        public ActionResult CashStatementIndex()
        {
            return PartialView("~/Views/VoucherReport/CashStatement/Index.cshtml");
        }
        public ActionResult BankStatementIndex()
        {
            return PartialView("~/Views/VoucherReport/Bank Statement/Index.cshtml");
        }
        public ActionResult SubsiStatementIndex()
        {
            return PartialView("~/Views/VoucherReport/SubsiStatement/Index.cshtml");
        }
        public ActionResult SubsiBalanceIndex()
        {
            return PartialView("~/Views/VoucherReport/SubsiBalanceReport/Index.cshtml");
        }
        public ActionResult LedgerReport(int FId, int page = 1, DateTime? fromDate = null, DateTime? toDate = null/*,bool withallBranch=false*/ )
        {
            const int PageSize = 12;
            var ledgerList = _voucherReportService.GenerateLedgerReport(FId, fromDate, toDate, false, page, PageSize).ToPagedList(page, PageSize + 1);
            var companyDetail = _voucherReportService.GetCompanyDetail();
            //if (companyDetail.Count > 2)
            //{
                ViewBag.CompanyName = companyDetail[0];
                ViewBag.CompanyAddress = companyDetail[1];
                ViewBag.CompanyPhoneNo = companyDetail[2];
            //}
            ViewBag.Title = "Ledger Report";
            ViewBag.FId = FId;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.PageSize = PageSize;
            ViewBag.ActivePager = 1;

            string Logo = _voucherReportService.CompanyLogo(); //  companyLogo 
            //ViewBag.LogoPath = Logo;
            //Path = Server.MapPath("~") + Path;
            //string base64String = "";
            //using (System.Drawing.Image image = System.Drawing.Image.FromFile(Path))
            //{
            //    using (System.IO.MemoryStream m = new System.IO.MemoryStream())
            //    {
            //        image.Save(m, image.RawFormat);
            //        byte[] imageBytes = m.ToArray();

            //        // Convert byte[] to Base64 String
            //        base64String = Convert.ToBase64String(imageBytes);
            //    }
            //}
            ViewBag.Logo = Logo;

            if (ledgerList.Count > 0)
                return View(ledgerList);
            else
                return Json(false, JsonRequestBehavior.AllowGet);

        }
        public ActionResult _LedgerReport(int FId, int page = 1, DateTime? fromDate = null, DateTime? toDate = null/*,bool withallBranch=false*/ )
        {
            int PageSize = 12;
            var ledgerList = _voucherReportService.GenerateLedgerReport(FId, fromDate, toDate, false, page, PageSize).ToPagedList(1, PageSize + 1);
            ViewBag.FId = FId;
            ViewBag.ActivePager = page;
            ViewBag.PageSize = PageSize;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            return PartialView(ledgerList);

        }
        public ActionResult LedgerReportPagination(int FId, int totalPage, int page, DateTime? fromDate = null, DateTime? toDate = null)
        {
            //   const int PageSize = 2;
            ViewBag.ActivePager = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.FId = FId;
            return PartialView();


        }
        //check without pagination
        public ActionResult LedgerReportWithoutPagination(int FId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var ledgerList = _voucherReportService.GenerateLedgerReportWithOutPagination(FId, fromDate, toDate, false).ToList();
            int count = ledgerList.Count();
            if (ledgerList.Count > 0)
            {

            }
            var LedgerList = ledgerList.ToPagedList(1, count);
            var companyDetail = _voucherReportService.GetCompanyDetail();
            //if (companyDetail.Count > 2)
            //{
                ViewBag.CompanyName = companyDetail[0];
                ViewBag.CompanyAddress = companyDetail[1];
                ViewBag.CompanyPhoneNo = companyDetail[2];
            //}
            ViewBag.Title = "Ledger Report";
            ViewBag.FId = FId;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            return PartialView(LedgerList);
        }
        public ActionResult ViewLedgerStatement(int ledgerId)
        {
            const int PageSize = 12;
            ViewBag.PageSize = PageSize;
            ViewBag.ActivePager = 1;
            var finaccObj = new DataEntities.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == ledgerId);
            if (finaccObj != null)
            {
                ViewBag.FId = ledgerId;
                var finsys1Obj = finaccObj.FinSys2.FinSys1.F1id;
                if (finsys1Obj == (int)EnumValue.FinSys1.SubsiAccount)
                {
                    List<ChannakyaAccounting.Models.ViewModel.SubsiBalanceViewModel> subsiBalance = _voucherReportService.GenerateSubsiBalReport(ledgerId, (DateTime)Loader.Models.Global.TransactionDate, true);
                    ViewData["_SubsiBalance"] = subsiBalance;
                    if (subsiBalance.Count > 0)
                        return PartialView("_LedgerStatementPopUp", subsiBalance);
                    else
                        return Json(JsonRequestBehavior.AllowGet);
                }

                else if (finsys1Obj == (int)EnumValue.FinSys1.Bank)
                {
                    List<Models.ViewModel.LedgerStatementViewModel> bankStatement = _voucherReportService.GenerateLedgerReport(ledgerId, Loader.Models.Global.TransactionDate.Value.AddDays(-30), Loader.Models.Global.TransactionDate, false, 1, PageSize);
                    //return PartialView("_BankStatementPopUp", bankStatement);
                    ViewData["_BankStatement"] = bankStatement;
                    if (bankStatement.Count > 0)
                        return PartialView("_LedgerStatementPopUp", bankStatement);
                    else
                        return Json(JsonRequestBehavior.AllowGet);
                }
                else if (finsys1Obj == (int)EnumValue.FinSys1.Cash)
                {
                    List<Models.ViewModel.LedgerStatementViewModel> cashStatement = _voucherReportService.GenerateCashStatement(Loader.Models.Global.TransactionDate.Value.AddDays(-30), Loader.Models.Global.TransactionDate, false, 1, PageSize);
                    ViewData["_CashStatement"] = cashStatement;
                    if (cashStatement.Count > 0)
                        return PartialView("_LedgerStatementPopUp", cashStatement);
                    else
                        return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<Models.ViewModel.LedgerStatementViewModel> ledgerStatement = _voucherReportService.GenerateLedgerReport(ledgerId, Loader.Models.Global.TransactionDate.Value.AddDays(-30), Loader.Models.Global.TransactionDate, false, 1, PageSize);
                    //return PartialView("_BankStatementPopUp", bankStatement);
                    ViewData["_LedgerStatement"] = ledgerStatement;
                    if (ledgerStatement.Count > 0)
                        return PartialView("_LedgerStatementPopUp", ledgerStatement);
                    else
                        return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }
        public ActionResult BankStatementReport(int FId, int page = 1, DateTime? fromDate = null, DateTime? toDate = null/*,bool withallBranch=false*/ )
        {

            const int PageSize = 12;
            var ledgerList = _voucherReportService.GenerateLedgerReport(FId, fromDate, toDate, false, page, PageSize).ToPagedList(page, PageSize + 1);
            var companyDetail = _voucherReportService.GetCompanyDetail();
            //if (companyDetail.Count > 2)
            //{
                ViewBag.CompanyName = companyDetail[0];
                ViewBag.CompanyAddress = companyDetail[1];
                ViewBag.CompanyPhoneNo = companyDetail[2];
            //}
            ViewBag.Title = "Bank Statement Report";
            ViewBag.FId = FId;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.ActivePager = page;
            ViewBag.PageSize = PageSize;

            string Logo = _voucherReportService.CompanyLogo(); //  companyLogo 
            //ViewBag.LogoPath = Logo;
            //Path = Server.MapPath("~") + Path;
            //string base64String = "";
            //using (System.Drawing.Image image = System.Drawing.Image.FromFile(Path))
            //{
            //    using (System.IO.MemoryStream m = new System.IO.MemoryStream())
            //    {
            //        image.Save(m, image.RawFormat);
            //        byte[] imageBytes = m.ToArray();

            //        // Convert byte[] to Base64 String
            //        base64String = Convert.ToBase64String(imageBytes);
            //    }
            //}
            ViewBag.Logo = Logo;

            if (ledgerList.Count > 0)
                return View(ledgerList);
            else
                return Json(false, JsonRequestBehavior.AllowGet);

        }
        //[HttpPost]
        public ActionResult _BankStatementReport(int FId, int page = 1, string fromDate = null, string toDate = null, bool withallBranch = false)
        {
            const int PageSize = 12;
            DateTime FromDate = fromDate == "" ? DateTime.Now : Convert.ToDateTime(fromDate);
            DateTime ToDate = toDate == "" ? DateTime.Now : Convert.ToDateTime(toDate);
            var ledgerList = _voucherReportService.GenerateLedgerReport(FId, FromDate, ToDate, false, page, PageSize).ToPagedList(1, PageSize + 1);
            ViewBag.FId = FId;
            ViewBag.FromDate = DateTime.Now;
            ViewBag.ToDate = DateTime.Now;
            ViewBag.ActivePager = page;
            return PartialView(ledgerList);
        }
        public ActionResult BankReportPagination(int FId, int totalPage, int page, DateTime? fromDate = null, DateTime? toDate = null)
        {
            //   const int PageSize = 2;
            ViewBag.ActivePager = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.FId = FId;
            return PartialView();


        }
        public ActionResult CashStatementReport(int page = 1, DateTime? fromDate = null, DateTime? toDate = null/*,bool withallBranch=false*/ )
        {
            const int PageSize = 12;
            var ledgerList = _voucherReportService.GenerateCashStatement(fromDate, toDate, false, page, PageSize).ToPagedList(page, PageSize + 1);

            var companyDetail = _voucherReportService.GetCompanyDetail();
            //if (companyDetail.Count > 2)
            //{
                ViewBag.CompanyName = companyDetail[0];
                ViewBag.CompanyAddress = companyDetail[1];
                ViewBag.CompanyPhoneNo = companyDetail[2];
            //}
            ViewBag.Title = "Cash Statement Report";
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.PageSize = PageSize;
            ViewBag.ActivePager = page;

            string Logo = _voucherReportService.CompanyLogo(); //  companyLogo 
            //ViewBag.LogoPath = Logo;
            //Path = Server.MapPath("~") + Path;
            //string base64String = "";
            //using (System.Drawing.Image image = System.Drawing.Image.FromFile(Path))
            //{
            //    using (System.IO.MemoryStream m = new System.IO.MemoryStream())
            //    {
            //        image.Save(m, image.RawFormat);
            //        byte[] imageBytes = m.ToArray();

            //        // Convert byte[] to Base64 String
            //        base64String = Convert.ToBase64String(imageBytes);
            //    }
            //}
            ViewBag.Logo = Logo;


            if (ledgerList.Count > 0)
                return View(ledgerList);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        public ActionResult _CashStatementReport(int page = 1, string fromDate = null, string toDate = null, bool withallBranch = false)
        {
            const int PageSize = 12;
            DateTime FromDate = fromDate == "" ? DateTime.Now : Convert.ToDateTime(fromDate);
            DateTime ToDate = toDate == "" ? DateTime.Now : Convert.ToDateTime(toDate);
            var ledgerList = _voucherReportService.GenerateCashStatement(FromDate, ToDate, false, page, PageSize).ToPagedList(1, PageSize);
            //var ledgerList = _voucherReportService.GenerateLedgerReport(FId, fromDate, toDate, false, page, 2).ToPagedList(page, 2);
            //ViewBag.FId = FId;
            //ViewBag.FromDate = fromDate;
            //ViewBag.ToDate = toDate;
            ViewBag.FromDate = DateTime.Now;
            ViewBag.ToDate = DateTime.Now;
            ViewBag.ActivePager = page;
            return PartialView(ledgerList);

        }
        public ActionResult CashReportPagination(int totalPage, int page, DateTime? fromDate = null, DateTime? toDate = null)
        {
            //   const int PageSize = 2;
            ViewBag.ActivePager = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            return PartialView();


        }
        public ActionResult SubsiStatementReport(int FId, int page = 1, string fromDate = null, string toDate = null, bool withallBranch = false, int SubsiValue = 0)
        {
            const int PageSize = 12;
            try
            {
                if (FId == 0 || SubsiValue == 0)
                {
                    //  return null;
                    throw new Exception("Please select Subsi Type");
                }
                var SubsiReport = _voucherReportService.GetSubsiStatementReport(FId, fromDate, toDate, withallBranch, SubsiValue, page, PageSize).ToPagedList(page, PageSize);
                var companyDetail = _voucherReportService.GetCompanyDetail();
                //if (companyDetail.Count > 2)
                //{
                    ViewBag.CompanyName = companyDetail[0];
                    ViewBag.CompanyAddress = companyDetail[1];
                    ViewBag.CompanyPhoneNo = companyDetail[2];
                //}
                ViewBag.Title = "Subsi Statement Report";
                ViewBag.FId = FId;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.ActivePager = page;

                string Logo = _voucherReportService.CompanyLogo(); //  companyLogo 
                                                                   
                ViewBag.Logo = Logo;

                if (SubsiReport.Count > 0)
                    return PartialView(SubsiReport);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return JavaScript(ex.Message);
            }

        }
        //Under Construction
        public ActionResult _SubsiStatementReport(int FId, int page = 1, string fromDate = null, string toDate = null, bool withallBranch = false, int SubsiValue = 0)
        {
            const int PageSize = 12;
            var SubsiReport = _voucherReportService.GetSubsiStatementReport(FId, fromDate, toDate, withallBranch, SubsiValue, page, PageSize).ToPagedList(page, PageSize);
            ViewBag.FId = FId;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.ActivePager = page;
            return PartialView(SubsiReport);

        }
        public ActionResult SubsiReportPagination(int FId, int totalPage, int page, DateTime? fromDate = null, DateTime? toDate = null)
        {
            //   const int PageSize = 2;
            ViewBag.ActivePager = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.FId = FId;
            return PartialView();


        }
        public ActionResult SubsiBalanceReport(int FId, int page = 1, string tillDate = null, bool withallBranch = false)
        {
            const int PageSize = 12;
            try
            {
                if (FId == 0)
                {
                    throw new Exception("Please Enter Ledger Name");
                }

                var SBalList = _voucherReportService.GenerateSubsiBalReport(FId, Convert.ToDateTime(tillDate), withallBranch, page, PageSize).ToPagedList(1, PageSize);
                var companyDetail = _voucherReportService.GetCompanyDetail();
                //if (companyDetail.Count > 2)
                //{
                    ViewBag.CompanyName = companyDetail[0];
                    ViewBag.CompanyAddress = companyDetail[1];
                    ViewBag.CompanyPhoneNo = companyDetail[2];
                //}
                ViewBag.Title = "Subsi Balance Report";
                ViewBag.FId = FId;
                ViewBag.PageSize = PageSize;
                ViewBag.TillDate = tillDate;
                ViewBag.ActivePager = page;
                if (SBalList.Count > 0)
                    return PartialView(SBalList);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                //throw JavaScript(ex.Message);
                throw ex;

            }

        }
        public ActionResult _SubsiBalanceReport(int FId, int page = 1, string tillDate = null, bool withallBranch = false)
        {
            const int PageSize = 12;
            var SBalList = _voucherReportService.GenerateSubsiBalReport(FId, Convert.ToDateTime(tillDate), withallBranch, page, PageSize).ToPagedList(1, PageSize);
            ViewBag.FId = FId;
            ViewBag.PageSize = PageSize;
            ViewBag.TillDate = tillDate;
            return PartialView(SBalList);


        }
        public ActionResult SubsiBalReportPagination(int FId, int totalPage, int page, string tillDate = null, bool withallBranch = false)
        {
            //   const int PageSize = 2;
            ViewBag.ActivePager = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.TillDate = tillDate;
            ViewBag.FId = FId;
            return PartialView();


        }
        public ActionResult GetSubsiType(int FID)
        {
            SelectList SubsiList = _voucherReportService.GetSubsiByVoucherType(FID);
            return Json(SubsiList, JsonRequestBehavior.AllowGet);
        }
        public int ExecuteMe(int ledgerId)
        {
            int finalId = 0;
            var context = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().GetContext();
            string connectionString = context.Database.Connection.ConnectionString;
            string query = "select [dbo].[GetLedgerType](" + ledgerId + ")";
            var returnedObj = context.Database.SqlQuery<Nullable<int>>(
                       query).ToList();
            if (returnedObj[0] != null)
            {
                finalId = Convert.ToInt32(returnedObj[0]);
            }
            return finalId;
        }

    }
}