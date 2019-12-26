using ChannakyaAccounting.Models.ViewModel;
using DataEntities = ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChannakyaAccounting.Helper;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using PagedList;
using Loader.Models;
using Loader;
using MoreLinq;

namespace ChannakyaAccounting.Controllers
{
    [MyAuthorize]
    public class Voucher1Controller : Controller
    {
        private Service.Voucher1.Voucher1Service Voucher1 = null;
        private Service.TaskVerificationAcc.TaskVerificationService _taskService = null;
        private readonly Service.Teller.TellerServiceAcc _tellerService = null;
        private GenericUnitOfWork uow = null;

        public Voucher1Controller()
        {
            Voucher1 = new Service.Voucher1.Voucher1Service();
            _taskService = new Service.TaskVerificationAcc.TaskVerificationService();
            _tellerService = new Service.Teller.TellerServiceAcc();
            uow = new GenericUnitOfWork();
        }

        // GET: Voucher1
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                ViewBag.ViewType = 1;
                var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher1T>().GetAll().AsQueryable();
                return View(list);
            }
            return RedirectToAction("Index");


        }

        //public JsonResult SetFiscalYear(int FYId)
        //{
        //    string fyName = "";
        //    if (FYId != 0)
        //    {
        //        Loader.Models.Global.SelectedFYID = FYId;
        //        Loader.Service.ParameterService paramService = new Loader.Service.ParameterService();
        //        fyName = paramService.GetCurrentFiscalYear(FYId);
        //    }
        //    return Json(fyName, JsonRequestBehavior.AllowGet);

        //}
        //public JsonResult SetTranscationDate(int FYId)
        //{
        //    string TDate = "";
        //    if (FYId != 0)
        //    {
        //        TDate = Voucher1.GetTranscationDate(FYId);

        //    }
        //    return Json(TDate, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult AppendTemporaryLogsValue(int v2Id)
        {
            var v1TId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher2T>().GetSingle(x => x.V2TId == v2Id).V1Tid;
            List<Models.Models.Voucher1T> v1TList = new List<Models.Models.Voucher1T>();
            Models.Models.Voucher1T v1TempModal = new Models.Models.Voucher1T();
            //var logsList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher2T>().GetSingle(x => x.V2TId == v2Id);
            //v1TempModal.Voucher2T.Add(logsList);

            //v1T.Add(v1TempModal);
            var v1T = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher1T>().GetSingle(x => x.V1TId == v1TId);
            var v2List = v1T.Voucher2T.Where(x => x.V2TId == v2Id).SingleOrDefault();
            //var sfId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiVSFId>().FindBy(x => x.Fid == v2List.Fid).FirstOrDefault().SFId;
            //if(v2List.Voucher3T!=null)
            //{
            //    v2List.Voucher3T.ToList()[0].SFId = sfId;
            //}
            v1TempModal.Voucher2T.Add(v2List);
            v1TList.Add(v1TempModal);
            return PartialView(v1TList);
        }

        public ActionResult GenerateSubsiTypeNameList(int SId, string Placeholder, string query, int ledgerId = 0, int page = 1)
        {
            int pageSize = 12;
            ViewBag.Placeholder = Placeholder;
            var finalSubsiList = GenerateSubsiTypeList.GenerateSubsiList(SId);
            int subsiTable = GenerateSubsiTypeList.GenerateSubsiTableFromLedgerId(ledgerId);
            var allSubsiList = finalSubsiList;
            //if (subsiTable == 4 || subsiTable == 5)
            //{
            var mappedSubsi = new DataEntities.GenericUnitOfWork().Repository<Models.Models.SubsiDetail>().FindBy(x => x.FId == ledgerId);
            allSubsiList = (from m in mappedSubsi join n in finalSubsiList on m.CId equals n.Id select n).AsQueryable();
            var finallist = allSubsiList.OrderBy(m => m.Accno).AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                finallist = finallist.Where(m => m.Name.ToUpper().Contains(query.ToUpper())).OrderBy(m => m.Accno);
                ViewBag.Query = query;
            }
            //var dataList = allSubsiList.OrderBy(m => m.Accno);
            var PagedList = new Pagination<Models.ViewModel.SubsiViewModel>(finallist, page, pageSize);
            ViewBag.CountPager = new Pagination<Models.ViewModel.SubsiViewModel>(finallist, page, pageSize).TotalPages;
            ViewBag.SubsiTableId = SId;
            ViewBag.ActivePager = page; 
            ViewBag.ledgerId = ledgerId;

            //var list = finalSubsiList.
            return View(PagedList);
        }

        public ActionResult SearchSubsiTypeNameList(int SId, string Placeholder, string query, int ledgerId = 0, int page = 1)
        {
            int pageSize = 12;
            ViewBag.Placeholder = Placeholder;
            var finalSubsiList = GenerateSubsiTypeList.GenerateSubsiList(SId);
            int subsiTable = GenerateSubsiTypeList.GenerateSubsiTableFromLedgerId(ledgerId);
            var allSubsiList = finalSubsiList;
            //if (subsiTable == 4 || subsiTable == 5)
            //{
            var mappedSubsi = new DataEntities.GenericUnitOfWork().Repository<Models.Models.SubsiDetail>().FindBy(x => x.FId == ledgerId);
            allSubsiList = (from m in mappedSubsi join n in finalSubsiList on m.CId equals n.Id select n).AsQueryable();
            var finallist = allSubsiList.OrderBy(m => m.Accno).AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                finallist = finallist.Where(m => m.Name.ToUpper().Contains(query.ToUpper())).OrderBy(m => m.Accno);
                ViewBag.Query = query;
            }
            //var dataList = allSubsiList.OrderBy(m => m.Accno);
            var PagedList = new Pagination<Models.ViewModel.SubsiViewModel>(finallist, page, pageSize);
            ViewBag.CountPager = new Pagination<Models.ViewModel.SubsiViewModel>(finallist, page, pageSize).TotalPages;
            ViewBag.SubsiTableId = SId;
            ViewBag.ActivePager = page;
            ViewBag.ledgerId = ledgerId;

            //var list = finalSubsiList.
            return PartialView(PagedList);
        }

        public ActionResult DisplayTemporaryLogsValue(int v1Id)
        {
            var logsList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher1T>().GetSingle(x => x.V1TId == v1Id);
            if(logsList.Voucher2T != null)
            {
                foreach (var item in logsList.Voucher2T)
                {
                    if(item.Voucher3T != null)
                    {
                        foreach (var item2 in item.Voucher3T)
                        {
                            var pid = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == item.Fid).FirstOrDefault().Pid;
                            if(pid != 262) //not saving deposit
                            {
                                var SSID = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiVSFId>().GetSingle(x => x.Fid == item.Fid).SSId;
                                var STBId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiSetup>().GetSingle(x => x.SSId == SSID).STBId;
                                if (STBId == 1)
                                {
                                    item2.SubsiName = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Employee>().GetSingle(x => x.EmployeeId == item2.SId).EmployeeName;
                                }
                                else if (STBId == 2)
                                {
                                    item2.SubsiName = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.User>().GetSingle(x => x.UserId == item2.SId).UserName;
                                }
                                else if (STBId == 3)
                                {
                                    item2.SubsiName = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.CustIndividual>().GetSingle(x => x.CID == item2.SId).Fname + new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.CustIndividual>().GetSingle(x => x.CID == item2.SId).Lname;
                                }
                            } //write code for account name for other than subsi
                           
                        }
                    }
                }
            }
            return PartialView(logsList);
        }

        public ActionResult DisplayVoucherVerifyDetails(int V1Id)
        {
            var logsList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher1>().GetSingle(x => x.V1Id == V1Id);

            return PartialView(logsList);
        }

        public ActionResult GetVoucherLogs()
        {
            var voucherNo = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherNo>().GetAll().Where(x => x.CompanyId == Loader.Models.Global.BranchId && x.FYID == Loader.Models.Global.SelectedFYID).Select(x => x.VNId).ToList();
            var logsList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher1T>().FindBy(x => voucherNo.Contains(x.VNId)).ToList();


            return PartialView(logsList);
        }

        public ActionResult GetAmount(int type)
        {

            Models.Models.Voucher1T vouchermaster = new Models.Models.Voucher1T();
            Models.Models.Voucher2T voucherDetail = new Models.Models.Voucher2T();
            vouchermaster.Voucher2T.Add(voucherDetail);
            ViewBag.Id = type;
            return PartialView("_AddAmount", vouchermaster);
        }

        public ActionResult GetBatch(int type)
        {
            Models.Models.Voucher1T voucherDetail = new Models.Models.Voucher1T();
            voucherDetail.VTypeId = type;
            return PartialView("_BatchNumber", voucherDetail);
        }

        public ActionResult SearchLedgerList(string query)
        {
            int? page = 1;
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).AsQueryable();
            list = list.Where(x => x.FinSys2.FinSys1.F1id != 3);
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.Fname.ToLower().Contains(query.ToLower()) || m.Code.ToLower().Contains(query.ToLower()));
                ViewBag.Query = query;
            }
            const int pageSize = 12;
            var dataList = list.OrderBy(m => m.Fname);


            var pagedList = new Pagination<Models.Models.FinAcc>(dataList.AsQueryable(), page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList.AsQueryable(), page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }

        public ActionResult SearchSubsiList(string query, int sId = 0)
        {
            Loader.Repository.GenericUnitOfWork gUOW = new Loader.Repository.GenericUnitOfWork();

            int? page = 1;
            var list = gUOW.Repository<Loader.Models.Employee>().GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.EmployeeName.Contains(query) || m.EmployeeNo.Contains(query));
                ViewBag.Query = query;
            }
            const int pageSize = 12;
            //var dataList = list.OrderBy(m => m.EmployeeName);
            var dataList = Helper.GenerateSubsiTypeList.GenerateSubsiList(sId, query).AsQueryable();

            var pagedList = new Pagination<ChannakyaAccounting.Models.ViewModel.SubsiViewModel>(dataList, page ?? 0, pageSize);
            var finalVMList = pagedList.Select(x => new SubsiViewModel
            {
                Id = x.Id,
                Name = x.Name

            });
            ViewBag.CountPager = new Pagination<ChannakyaAccounting.Models.ViewModel.SubsiViewModel>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(finalVMList);
        }

        public ActionResult AddVoucherDetails(string query = null, int? fId = 0, int? page = 1)
        {

            const int pageSize = 12;
            var finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).Where(x => x.FinSys2.FinSys1.F1id != 3).AsQueryable();
            if (fId != 0)
            {
                finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fId && x.FinSys2.FinSys1.IsGroup == false).Where(x => x.FinSys2.FinSys1.F1id != 3).AsQueryable();
            }
            ViewBag.ActivePager = page;
            if (!string.IsNullOrEmpty(query))
            {
                finaccList = finaccList.Where(m => m.Fname.Contains(query));
                ViewBag.Query = query;
            }
            var dataList = finaccList.OrderBy(m => m.Fname);
            var pagedList = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize);
            //ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.CountPager = pagedList.TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);

        }

        public ActionResult _AddVoucherDetails(string query = null, int? fId = 0, int? page = 1)
        {
            const int pageSize = 12;
            var finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).Where(x => x.FinSys2.FinSys1.F1id != 3).AsQueryable();
            if (fId != 0)
            {
                finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fId && x.FinSys2.FinSys1.IsGroup == false).Where(x => x.FinSys2.FinSys1.F1id != 3).AsQueryable();
            }
            if (!string.IsNullOrEmpty(query))
            {
                finaccList = finaccList.Where(m => m.Fname.Contains(query));
                ViewBag.Query = query;
            }
            var dataList = finaccList.OrderBy(m => m.Fname);
            var pagedList = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize);
            //ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.CountPager = pagedList.TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }

        public JsonResult SearchVoucherList(string text)
        {
            var ledgerName = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().GetAll().Where(x => x.Fname.Contains(text)).ToList();
            var countLedger = ledgerName.Count();

            try
            {
                var data = new
                {
                    suggestions = new[]
                        {
                          new { value = "United Arab Emirates", data = "AE" },
                        }
                };
                List<Models.ViewModel.VoucherLedgerViewModel> jsonResult = new List<Models.ViewModel.VoucherLedgerViewModel>();

                foreach (var item in ledgerName)
                {
                    jsonResult.Add(new Models.ViewModel.VoucherLedgerViewModel { Fid = item.Fid, FName = item.Fname, HasBankInfo = item.BankInfoes.Count() == 0 ? false : true, HasDimension = item.DimensionVSLedgers.Count() == 0 ? false : true, HasSubsiList = item.SubsiVSFIds.Count() == 0 ? false : true });

                }

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }

        public ActionResult AddTableRow()
        {
            ChannakyaAccounting.Models.Models.Voucher1T voucherMaster = new Models.Models.Voucher1T();
            voucherMaster.Voucher2T.Add(new Models.Models.Voucher2T());
            return PartialView(voucherMaster);
        }

        public ActionResult AddTableRowReadOnly(Models.Models.VoucherLedgerDetails voucher2)
        {
            //if (voucher2 == null)
            //{
            //    return JavaScript("voucher 2 is null");
            //}
            //if(voucher2.BankInfoList == null)
            //{
            //    return JavaScript("only bankinfo is null");
            //}
            //if(voucher2.BankInfoList.FirstOrDefault().ChequeAmount == 0)
            //{
            //    return JavaScript("The ChequeAmount is 0");
            //}
           
            Models.Models.Voucher2T voucher2T = new Models.Models.Voucher2T();
            //voucher2T.Voucher4T = voucher2.DimensionVSLedgerList;
            voucher2T.Voucher3T = voucher2.SubsiVSLedgerList;
            voucher2T.Voucher5T = voucher2.BankInfoList;
            //if (FromVoucher > 0 )
            //{
            //    //voucher2T.Voucher3T.ForEach(x => x.fromVoucher = FromVoucher);
            //    voucher2T.Voucher5T.ForEach(x => x.fromVoucher = FromVoucher);

            //}
            ViewBag.WithButtons = false;
            return PartialView("AddTableRowReadOnly", voucher2T);
        }

        public ActionResult SaveTablePermanent(ChannakyaAccounting.Models.ViewModel.TaskVerificationList taskverificationModel)
        {
            GenericUnitOfWork uow = new GenericUnitOfWork();
            //IEnumerable<Models.Models.Voucher1T> voucher1T = null;
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {
                int voucherTempId = taskverificationModel.VoucherTempId;
                var voucherParam = new SqlParameter();
                voucherParam.SqlDbType = SqlDbType.Int;
                voucherParam.ParameterName = "@V1Id";
                voucherParam.Direction = ParameterDirection.Output;
                if (taskverificationModel.Narration == null) { taskverificationModel.Narration = ""; }
                try
                {

                    uow.GetContext().Database.ExecuteSqlCommand
                    ("exec [dbo].[PSetToPermanentTableVoucher] @TV1Id,@Narration,@FYId,@BranchId,@V1Id out", new SqlParameter("@TV1Id", voucherTempId), new SqlParameter("@Narration", taskverificationModel.Narration), new SqlParameter("@FYId", Loader.Models.Global.SelectedFYID), new SqlParameter("@BranchId", Loader.Models.Global.BranchId), voucherParam);

                    int voucher1Id = Convert.ToInt32(voucherParam.Value);

                    _taskService.SaveTaskNotification(taskverificationModel, voucher1Id, taskverificationModel.EventId);
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return JavaScript(ex.Message);

                }
            }


            return RedirectToAction("Create");
        }

        public ActionResult GetVoucherHeader()
        {
            return PartialView("_GetVoucherHeader");
        }

        public JsonResult Delete(int V2TId)
        {
            var voucher2Details = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher2T>().GetSingle(x => x.V2TId == V2TId);
            bool isSaved = Voucher1.DeleteVoucher(V2TId);
            return Json(isSaved, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTemporaryLogs(int V1TId)
        {
            var voucher2Details = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher1T>().GetSingle(x => x.V1TId == V1TId);
            bool isSaved = Voucher1.DeleteTemporaryLogsValue(V1TId);
            return RedirectToAction("Create", "Voucher1");
        }

        public ActionResult GetVerifiedVoucherPopUp(int V1Id)
        {
            var voucher1Model = new DataEntities.GenericUnitOfWork().Repository<Models.Models.Voucher1>().FindBy(x => x.V1Id == V1Id).FirstOrDefault();
            return PartialView(voucher1Model);
        }

        /// <summary>
        /// Gets all The Verified Vouchers List - Besides Rejected Vouchers
        /// </summary>
        /// <returns></returns>
        public ActionResult GetVerifiedVoucherReport(int pageNo = 1, int pageSize = 10, string batchText = "", string voucherType = "", int vouchernoText = 0)
        {
            var voucherverifiedList = new DataEntities.GenericUnitOfWork().Repository<Models.Models.Voucher1>().
                FindBy(x => x.ApprovedBy != null && x.ApprovedBy != null
                && (x.VoucherRejecteds.Select(y => y.V1id).ToList().Contains(x.V1Id) == false)).ToList();
            if (vouchernoText != 0)
            {
                voucherverifiedList = voucherverifiedList.Where(x => x.VNo == vouchernoText).ToList();
            }
            var finalList = voucherverifiedList;
            ViewBag.BatchText = batchText;
            ViewBag.VoucherType = voucherType;

            if (voucherType != "" && batchText != "")
            {
                var voucherTypeList = new DataEntities.GenericUnitOfWork().Repository<Models.Models.VoucherType>().FindBy(x => x.VoucherName.Contains(voucherType)).ToList();
                var batchTypeList = new DataEntities.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().FindBy(x => x.BatchName.Contains(batchText)).ToList();

                var vouchernoList = from m in new DataEntities.GenericUnitOfWork().Repository<Models.Models.VoucherNo>().GetAll()
                                    join n in voucherTypeList on m.VTypeId equals n.VTypeID
                                    join o in batchTypeList on m.BId equals o.BId
                                    select m;

                finalList = (from m in voucherverifiedList join n in vouchernoList on m.VNId equals n.VNId select m).ToList();
            }
            else
            {
                if (voucherType != "")
                {
                    var voucherTypeList = new DataEntities.GenericUnitOfWork().Repository<Models.Models.VoucherType>().FindBy(x => x.VoucherName.Contains(voucherType)).ToList();
                    var vouchernoList = from m in new DataEntities.GenericUnitOfWork().Repository<Models.Models.VoucherNo>().GetAll()
                                        join n in voucherTypeList on m.VTypeId equals n.VTypeID
                                        select m;

                    finalList = (from m in voucherverifiedList join n in vouchernoList on m.VNId equals n.VNId select m).ToList();
                }
                if (batchText != "")
                {
                    var voucherTypeList = new DataEntities.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().FindBy(x => x.BatchName.Contains(batchText)).ToList();
                    var vouchernoList = from m in new DataEntities.GenericUnitOfWork().Repository<Models.Models.VoucherNo>().GetAll()
                                        join n in voucherTypeList on m.BId equals n.BId
                                        select m;

                    finalList = (from m in voucherverifiedList join n in vouchernoList on m.VNId equals n.VNId select m).ToList();
                }
            }

            //var voucherTypeList = new DataEntities.GenericUnitOfWork().Repository<Models.Models.voucherN>

            var voucherLst = new StaticPagedList<Models.Models.Voucher1>(finalList, pageNo, pageSize, (voucherverifiedList.Count == 0) ? 0 : voucherverifiedList.Count());
            return PartialView("VoucherReport", voucherLst);
        }

        public JsonResult GetCIdFromName(string CName, int fid)
        {
            int Sid = Voucher1.GetSTBIDfromFid(fid);

            var fullName = CName.Remove(CName.Length - 1, 1);
            fullName = fullName.Remove(0, 1);
            fullName = fullName.Replace('"', ' ');
            fullName = fullName.Replace(" ", "");
            var fullName2 = fullName.Split(',');
            int CId = Voucher1.GetCIdFromName(fullName2, Sid);

            return Json(CId, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save In Temporary Table of VOucher - Voucher1T , 2T and so on.
        /// </summary>
        /// <param name="voucher1"></param>
        /// <param name="SubsiListInVoucher"></param>
        /// <returns></returns>
        public ActionResult SaveTableTemporary(Models.Models.Voucher1T voucher1, IEnumerable<Models.Models.Voucher3T> SubsiListInVoucher, IEnumerable<Models.Models.Voucher5T> BankListInVoucher, int VoucherTransType, IEnumerable<string> SId, string ledgerName, string debitAmount, string creditAmount, string FID2)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                #region exception handling and save temprary service call

                //if(voucher1.BankInfoList == null)
                //{
                //    return JavaScript("Bank Info is null");
                //}
                //if (BankListInVoucher == null)
                //{
                //    return JavaScript("BankList Javascript is null");
                //}
                //if (voucher1.SubsiVSLedgerList == null)
                //{
                //}
                //if (SubsiListInVoucher != null)
                //{
                //    voucher1.SubsiVSLedgerList = SubsiListInVoucher.ToList();
                //}
                //else
                //{
                //    //return JavaScript("SubsiListInVoucher Null");
                //}

                int voucherMasterId;
                if (voucher1.Voucher2T == null)
                {
                    Models.Models.Voucher2T voucher2 = new Models.Models.Voucher2T();
                    List<Models.Models.Voucher2T> voucher2List = new List<Models.Models.Voucher2T>();

                    voucher2.Fid = Convert.ToInt32(FID2);
                    voucher2.LedgerName = ledgerName;
                    decimal debitAmountt;
                    decimal creditAmountt;
                    voucher2.VoucherTransType = VoucherTransType;
                    if (decimal.TryParse(debitAmount, out debitAmountt))
                    {
                        voucher2.DebitAmount = decimal.Parse(debitAmount);
                    }
                    if (decimal.TryParse(creditAmount, out creditAmountt))
                    {
                        voucher2.CreditAmount = decimal.Parse(creditAmount);
                    }
                    if (voucher2.Fid == null || voucher2.Fid == 0)
                    {
                        return JavaScript("voucher2.fid is empty");
                    }
                    if (voucher2.DebitAmount == null || voucher2.DebitAmount == 0)
                    {
                        //return JavaScript("voucher2.DebitAmount is empty");
                    }
                    try
                    {
                        voucher2List.Add(voucher2);
                        ICollection<Models.Models.Voucher2T> voucher2Collection = voucher2List;
                        voucher1.Voucher2T = voucher2Collection;
                    }
                    catch (Exception ex)
                    {
                        return JavaScript(ex.Message + " voucher1.voucher2T.add(voucher2) error");
                    }
                }
                try
                {
                    if (voucher1 == null)
                    {
                        return JavaScript("voucher1 null error");
                    }
                    if (voucher1.Voucher2T == null)
                    {
                        return JavaScript("voucher2T null error");
                    }
                    foreach (var item in voucher1.Voucher2T)
                    {
                        item.VoucherTransType = VoucherTransType;
                    }
                    voucherMasterId = Voucher1.SaveTemporary(voucher1);

                    if (voucherMasterId == 9999)
                    {
                        return JavaScript("vouchermaster.voucher2T == null (inside savetemp)");
                    }
                    if (voucherMasterId == 8888)
                    {
                        return JavaScript("voucher1.voucher2T == null (inside savetemp)");
                    }
                    if (voucherMasterId == 7777)
                    {
                        return JavaScript("commit error (inside savetemp)");
                    }
                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message + "Save Temporary ma error");
                }
                #endregion

                //int voucherMasterId = Voucher1.SaveTemporary(voucher1);

                if (voucher1.Voucher2T.First().V2TId == 0)
                {
                    var voucher2List = voucher1.Voucher2T.First();
                    //voucher2List.Voucher4T = voucher1.DimensionVSLedgerList;
                    if (voucher1.SubsiVSLedgerList != null)
                    {
                        voucher2List.Voucher3T = voucher1.SubsiVSLedgerList;
                    }
                    if (voucher1.BankInfoList != null)
                    {
                        voucher2List.Voucher5T = voucher1.BankInfoList;
                    }
                    if (SubsiListInVoucher != null)
                    {
                        voucher2List.SubsiVSLedgerList = SubsiListInVoucher.ToList();
                    }
                    if (voucher2List.Voucher3T == null)
                    {
                        return JavaScript("voucher2List.Voucher3T is null (end ma)");
                    }
                    voucher2List.V2TId = voucherMasterId;
                    ViewBag.V1Id = voucherMasterId;
                    ViewData["IsSaved"] = true;
                    return PartialView("AddTableRowReadOnly", voucher2List);
                }
                else
                {
                    var voucher2TId = voucher1.Voucher2T.ToList()[0].V2TId;
                    var voucher2List = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher2T>().FindBy(x => x.V2TId == voucher2TId).ToList();
                    if (voucher2List[0].Voucher3T != null)
                    {
                        if (voucher2List.FirstOrDefault().Voucher3T.Count() > 0)
                        {
                            voucher2List[0].Voucher3T.ToList().ForEach(x => x.SFId = ChannakyaAccounting.Helper.GenerateSubsiTypeList.GenerateSubsiTableFromLedgerId(Convert.ToInt32(voucher2List[0].Fid)));
                        }
                    }
                    return PartialView("AddTableRowReadOnly", voucher2List.ToList()[0]);
                }
            }
            catch (Exception ex)
            {
                return JavaScript("Check!!!! " + voucher1.Voucher2T.First().DebitAmount + " test data" + ex.Message + ledgerName + FID2 + debitAmount + creditAmount);
            }
            //}
            //return View(voucher1);
        }

        /// <summary>
        /// Appends the Voucher Data from Entry Form in The Voucher Form
        /// </summary>
        /// <param name="voucher2"></param>
        /// <returns></returns>
        public ActionResult AppendVoucherInTextBox(Models.Models.Voucher2T voucher2)
        {
            //if(voucher2.BankInfoList == null)
            //{
            //    return JavaScript("AppendVoucherInTextBox BankInfo Null");
            //}

            ChannakyaAccounting.Models.Models.Voucher1T voucherMaster = new Models.Models.Voucher1T();
            voucherMaster.Voucher2T.Add(voucher2);

            Models.Models.VoucherLedgerDetails jsonResult = new Models.Models.VoucherLedgerDetails();
            jsonResult.Fid = Convert.ToInt32(voucher2.Fid);
            jsonResult.Fname = voucher2.LedgerName;
            //if (voucher2.DimensionVSLedgerList != null)
            //{
            //    jsonResult.DimensionVSLedgerList = voucher2.DimensionVSLedgerList;
            //}
            if (voucher2.SubsiVSLedgerList != null)
            {
                jsonResult.SubsiVSLedgerList = voucher2.SubsiVSLedgerList;
            }
            if (voucher2.BankInfoList != null)
            {
                jsonResult.BankInfoList = voucher2.BankInfoList;
            }
            return Json(new { data = jsonResult }, JsonRequestBehavior.AllowGet);
            //return PartialView("AddTableRow",voucherMaster);
        }

        /// <summary>
        /// Adds the Ledger Details to the Voucher-Form
        /// </summary>
        /// <param name="voucher2"></param>
        /// <returns></returns>
        public ActionResult AddVoucherLedgerDetails(Models.Models.VoucherLedgerDetails voucher2)
        {
            Models.Models.Voucher1T voucher2DTO = new Models.Models.Voucher1T();

            try
            {
                if (voucher2 == null)
                {
                    return JavaScript("Voucher 2 null");
                }
                if (voucher2.SubsiVSLedgerList == null)
                {
                    return JavaScript("AddVoucherLedgerDetails subsiVSledgerlist null");
                }
                if (voucher2.BankInfoList == null)
                {
                    return JavaScript("AddVoucherLedgerDetails bankInfo null");
                }
                if (voucher2.SubsiVSLedgerList != null && voucher2.SubsiVSLedgerList.Count() > 0)
                {
                    voucher2.SubsiVSLedgerList.ToList().ForEach(x => x.fromVoucher = 1);
                    voucher2DTO.SubsiVSLedgerList = voucher2.SubsiVSLedgerList;
                }
                if (voucher2.BankInfoList != null && voucher2.BankInfoList.Count() > 0)
                {
                    voucher2.BankInfoList.ToList().ForEach(x => x.fromVoucher = 1);
                    voucher2DTO.BankInfoList = voucher2.BankInfoList;
                }
            }
            catch (Exception ex)
            {
                return JavaScript("Tala ko part bata");
            }
            //voucher2DTO.DimensionVSLedgerList = voucher2.DimensionVSLedgerList;
            //voucher2DTO.SubsiVSLedgerList = voucher2.SubsiVSLedgerList;
            //if (voucher2.DimensionVSLedgerList != null && voucher2.DimensionVSLedgerList.Count() > 0)
            //{
            //    voucher2.DimensionVSLedgerList.ToList().ForEach(x => x.fromVoucher = 1);
            //    voucher2DTO.DimensionVSLedgerList = voucher2.DimensionVSLedgerList;
            //}

            return PartialView("_AddLedgerVoucherDetails", voucher2DTO);
        }

        /// <summary>
        /// Form to enter the Ledger Details Selecting the Ledger in the Voucher Form
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <returns>EnterLedger-PartialView</returns>
        public ActionResult EnterLedgerDetails(int ledgerId)
        {
            ChannakyaAccounting.Models.Models.Voucher2T voucherDetail = new Models.Models.Voucher2T();
            //voucherDetail.DimensionVSLedgerList = new List<Models.Models.Voucher4T>();
            voucherDetail.SubsiVSLedgerList = new List<Models.Models.Voucher3T>();
            voucherDetail.BankInfoList = new List<Models.Models.Voucher5T>();
            var ledgerText = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == ledgerId).Fname;
            //var dimensionList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionVSLedger>().FindBy(x => x.Fid == ledgerId).ToList();
            var subsiList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiVSFId>().FindBy(x => x.Fid == ledgerId).ToList();
            var bankList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.BankInfo>().FindBy(x => x.Fid == ledgerId).ToList();

            var otherSubsiListOfLedger = Voucher1.GetAccountInfoFromLedgerId(ledgerId);
            //if (dimensionList.Count() > 0)
            //{
            //    foreach (var item in dimensionList)
            //    {
            //        Models.Models.Voucher4T voucher4 = new Models.Models.Voucher4T();
            //        voucher4.DDID = item.DDID;
            //        voucher4.Id = item.Id;
            //        voucherDetail.DimensionVSLedgerList.Add(voucher4);
            //    }

            //}
            if (subsiList.Count() > 0)
            {
                foreach (var item in subsiList)
                {
                    Models.Models.Voucher3T voucher3 = new Models.Models.Voucher3T();

                    voucher3.SFId = Convert.ToInt32(item.SubsiSetup.STBId);
                    voucherDetail.SubsiVSLedgerList.Add(voucher3);
                }

            }
            if (otherSubsiListOfLedger.Count() > 0)
            {

                foreach (var item in otherSubsiListOfLedger)
                {
                    Models.Models.Voucher3T voucher3 = new Models.Models.Voucher3T();

                    voucher3.SFId = Convert.ToInt32((int)EnumValue.Subsi.Deposit);
                    voucherDetail.SubsiVSLedgerList.Add(voucher3);
                }
            }
            if (bankList.Count() > 0)
            {
                foreach (var item in bankList)
                {
                    Models.Models.Voucher5T voucher5 = new Models.Models.Voucher5T();
                    voucherDetail.BankInfoList.Add(voucher5);
                }
            }

            voucherDetail.LedgerName = ledgerText;
            voucherDetail.Fid = ledgerId;

            return PartialView(voucherDetail);
        }

        public ActionResult AddSearchedVoucherDetails(string query = null, int? page = 1)
        {

            const int pageSize = 10;

            var finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.Fname.Contains(query)).AsQueryable();
            if (finaccList.Count() == 0)
            {
                finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.Code.Contains(query)).AsQueryable();
            }
            var dataList = finaccList.OrderBy(m => m.Fname);
            var pagedList = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView("AddVoucherDetails", pagedList);


        }

        public ActionResult AddSubsiInVoucher(int SFId, string SubsiName)
        {
            Models.Models.Voucher3T voucher3 = new Models.Models.Voucher3T();
            voucher3.SFId = SFId;
            voucher3.SubsiName = SubsiName;
            return PartialView("AddMoreSubsi", voucher3);
        }

        [HttpGet]
        public ActionResult AddCashLedger(int vType, int V1Tid = 0, decimal finalCreditAmount = 0, decimal finalDebitAmount = 0)
        {
            //if (vType == 0)
            //{
            //    return JavaScript("Vtype is 0");
            //}
            //if (V1Tid == 0)
            //{
            //    return JavaScript("V1tid is 0");
            //}
            //if (finalCreditAmount == 0 && finalDebitAmount == 0)
            //{
            //    return JavaScript("Final Amounts are 0");
            //}

            Models.Models.VoucherCashLedgerModel voucherCashLedgerModel = new Models.Models.VoucherCashLedgerModel();

            voucherCashLedgerModel.V1Tid = V1Tid;
            if (vType == 2)
            {
                voucherCashLedgerModel.VoucherTransType = 1;
                voucherCashLedgerModel.CreditAmount = finalDebitAmount;
            }
            else
            {
                voucherCashLedgerModel.VoucherTransType = 0;
                voucherCashLedgerModel.DebitAmount = finalCreditAmount;
            }

            bool addCashStatus = Voucher1.AddCashLedger(voucherCashLedgerModel);

            //if (addCashStatus == false)
            //{
            //    return JavaScript("AddCashLedger ko service error");
            //}

            return PartialView(voucherCashLedgerModel);
        }

        [HttpGet]
        public ActionResult Create(/*string activeText, int ProductId = 0*/)
        {
            //if (Request.IsAjaxRequest())
            //{
            try
            {
                Models.Models.Voucher1T VoucherDTO = new Models.Models.Voucher1T();
                VoucherDTO.Voucher2T.Add(new Models.Models.Voucher2T());
                ViewBag.CTId = new SelectList(new DataEntities.GenericUnitOfWork().Repository<Models.Models.CurrencyType>().GetAll().OrderBy(x => x.CurrencyName), "CTId", "CurrencyName", 8);
                ViewBag.UserID = Global.UserId;
                //ViewBag.ActiveText = activeText;
                if(ViewBag.CTID == null)
                {
                    return JavaScript("CTID null");
                }
                if (ViewBag.UserID == null || ViewBag.UserID == 0)
                {
                    return JavaScript("UserID null");
                }
                return PartialView(VoucherDTO);

            }
            catch(Exception ex){
                return JavaScript(ex.Message);
            }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}
        }

        #region Deno Setup for transaction
        public ActionResult UseDenoList(Int64 utno)
        {
            DenoInViewModel denoInModel = new DenoInViewModel();
            denoInModel.DenoInList = Voucher1.GetDenoByTransactionNumber(utno);

            return PartialView(denoInModel);
        }
        public ActionResult DenoTransaction(int amount, int V1TId, int currencyId = 1, string denoInOut = "DenoIn")
        {

            if(amount == 0)
            {
                return JavaScript("amount is 0");
            }

            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            try
            {
                if (currencyId == 0)
                {
                    currencyId = Voucher1.DefultCurrency();
                }
                denoInOutModel = Voucher1.DenoList(currencyId);
                denoInOutModel.DenoInOut = denoInOut;
                denoInOutModel.Amount = amount;
                ViewBag.Amount = amount;
                ViewBag.V1TId = V1TId;
                denoInOutModel.IsTransactionWithDeno = Voucher1.IsTransactionWithDeno();
            }
            catch (Exception Ex)
            {
                return JavaScript("bhitra ta ayo");
            }
            return PartialView("_DenoTransaction", denoInOutModel);
        }

        public ActionResult DenoTransactionByUser(int currencyId, string denoInOut, int UserId)///for transaction edit to getdeno of posted by 
        {
            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            if (currencyId == 0)
            {
                currencyId = Voucher1.DefultCurrency();
            }

            denoInOutModel = Voucher1.DenoListByUser(currencyId, UserId);
            denoInOutModel.DenoInOut = denoInOut;
            denoInOutModel.IsTransactionWithDeno = Voucher1.IsTransactionWithDeno();
            return PartialView("_DenoTransaction", denoInOutModel);
        }

        public ActionResult UpdateDeno(DenoInOutViewModel denoInOutModel)
        {
            if(denoInOutModel == null)
            {
                return JavaScript("DenoInOut is null");
            }
            var returnmessage = Voucher1.UpdateDeno(denoInOutModel, denoInOutModel.Amount, denoInOutModel.V1TId);
            return Json(returnmessage.Success, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Account Search

        public ActionResult AccountNumberSearch(string accountNumber = "", string filterAccount = "", string accountType = "", int ledgerId = 0)
        {
            AccountSearchViewModelAcc aAccountSearch = new AccountSearchViewModelAcc();
            AccountDetailsViewModelAcc code = new AccountDetailsViewModelAcc();
            if (accountNumber != "")
            {
                code = _tellerService.GetCodeByAccountNumber(accountNumber);
                if (code != null)
                {

                    aAccountSearch.ProductCode = _tellerService.GetProductCode(code.PID);
                }
                else
                {
                    code = new AccountDetailsViewModelAcc();
                    //code.BrchID = _tellerService.GetBranchIdByEmployeeUserId(Loader.Models.Global.UserId);
                    code.CurrID = 1;
                }

            }
            else
            {
                //code.BrchID = _tellerService.GetBranchIdByEmployeeUserId(Loader.Models.Global.UserId);
                code.CurrID = 1;
            }
            aAccountSearch.FilterAction = filterAccount;
            aAccountSearch.AccountType = accountType;
            aAccountSearch.BranchId = Convert.ToInt32(code.BrchID);
            aAccountSearch.CurrencyId = code.CurrID;
            var accountList = _tellerService.GetAccountNumberList(aAccountSearch.BranchId, code.PID, aAccountSearch.CurrencyId, "", filterAccount, accountType, 1, 5, ledgerId);
            aAccountSearch.AccountList = new StaticPagedList<AccountSearchViewModelAcc>(accountList, 1, 5, (accountList.Count == 0) ? 0 : accountList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView("AccountNumberSearch", aAccountSearch);
        }
        public ActionResult _AccountNumberSearch(int branchCode = 0, int productCode = 0, int currencyCode = 0, string customerName = "", string filterAccount = "", string accountType = "", int pageNo = 1, int pageSize = 5)
        {

            AccountSearchViewModelAcc aAccountSearch = new AccountSearchViewModelAcc();
            var accountList = _tellerService.GetAccountNumberList(branchCode, productCode, currencyCode, customerName, filterAccount, accountType, pageNo, pageSize);
            aAccountSearch.AccountList = new StaticPagedList<AccountSearchViewModelAcc>(accountList, pageNo, pageSize, (accountList.Count == 0) ? 0 : accountList.Select(x => x.TotalCount).FirstOrDefault());
            //aAccountSearch.BranchId = branchCode;
            //aAccountSearch.CurrencyId = currencyCode;
            return PartialView("_AccountNumberSearch", aAccountSearch);
        }
        #endregion
    }
}