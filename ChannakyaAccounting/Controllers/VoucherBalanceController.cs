using ChannakyaAccounting.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoreLinq;
using DataEntities = ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using Loader;

namespace ChannakyaAccounting.Controllers
{
    [MyAuthorize]
    public class VoucherBalanceController : Controller
    {
        private Service.VoucherBalance.VoucherBalanceService VoucherBalance;

        public VoucherBalanceController()
        {
            VoucherBalance = new Service.VoucherBalance.VoucherBalanceService();
        }

        // GET: VoucherBalance

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == false)
            {
                if (Loader.Models.Global.SelectedFYID == VoucherBalance.GetFirstTransactionFYear())
                {
                    //ViewBag.ViewType = 1;
                    //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherBal>().GetAll().AsQueryable();
                    //return View(VoucherBalance.GetAll().ToList());
                    return RedirectToAction("Create", "VoucherBalance");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public JsonResult IsManual(int DDID)
        {
            var id = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().GetSingle(x => x.DDId == DDID).IsManual;
            return Json(id, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _Details(int ViewType = 1, string query = null, int? page = 1)
        {
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherBal>().GetAll().ToList();
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            return PartialView("_Details", list);
            //ViewBag.Address = VoucherBalance.GetAddress(parentId);

        }
        public ActionResult GetLedgerList(string query = null, int? fId = 0, int? page = 1)
        {
             int pageSize = 10;
            var finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).AsQueryable();
            if (fId != 0)
            {
                finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fId && x.FinSys2.FinSys1.IsGroup == false).AsQueryable();
            }


            var dataList = finaccList.OrderBy(m => m.Fname);
            var pagedList = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;


            return PartialView(pagedList);

        }

        public ActionResult AddSubsiUpdate(List<ChannakyaAccounting.Models.ViewModel.SubsiBalanceViewModel> subsiDataList)
        {
            try
            {
                //VoucherBalance.GetFirstTransactionFYear()
                if (Loader.Models.Global.SelectedFYID <= VoucherBalance.GetBranchEstFiscalYear())
                {
                    VoucherBalance.SaveSubsiOpeningBalance(subsiDataList);
                    return JavaScript("Saved");
                }
                else
                    return JavaScript("FY");//to choose intital fiscal year
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }

            //return View();
        }
        public ActionResult AddSubsiBalance(int ledgerId = 0, int? page = 1, string accNo = "", string AccName = "", int parentId = 0,int Status=1)
        {
             int withSubsi = 1;
             int pageSize = 10;
            ViewBag.pageSize = pageSize;
            ChannakyaAccounting.Service.Subsi_Setup.SubsiDetailService subsidetailService = new Service.Subsi_Setup.SubsiDetailService();
            int subsiTableId = Helper.GenerateSubsiTypeList.GenerateSubsiTableFromLedgerId(ledgerId);
            List<OpeningBalanceViewModel> opBalVM = new List<OpeningBalanceViewModel>();
            // var finalList= new ChannakyaAccounting.Models.ViewModel.OpeningBalanceViewModel();
            var finalList = VoucherBalance.GetMappedDataOfOpeningBalance(ledgerId, withSubsi, Convert.ToInt32(page), pageSize,accNo,AccName).DistinctBy(x => x.FId).ToList();
            ViewData["SubsiTableId"] = subsiTableId;
            ViewData["LedgerId"] = ledgerId;
            ViewData["Page"] = page;
            ViewData["Status"] = Status;

            return PartialView("SubsiBalanceChildList", finalList.ToList());

            //return PartialView("SubsiDetail", finalList.ToPagedList(Convert.ToInt32(page),1));
        }

        public JsonResult GetTotalBalanceOfSubsi(int ledgerId,int FYId=0)
        {
            var openingBalObject = new DataEntities.GenericUnitOfWork().Repository<Models.Models.SubsiVSOpeningBalance>().FindBy(x => x.FId == ledgerId /*&& x.FYID == item.FYId*/).ToList();

            decimal totalOpeningBal = 0;
            if (openingBalObject.Count() > 0)
            {
                foreach (var subitem in openingBalObject)
                {
                    totalOpeningBal = totalOpeningBal + subitem.OpeningBal;
                }

            }

            return Json(totalOpeningBal, JsonRequestBehavior.AllowGet);
          
        }

        [HttpGet]
        public ActionResult Create(int VoucherBalanceId = 0)
        {
            const int Status = 1; //for Subsi

            if (Request.IsAjaxRequest() == true)
            {
                if (Loader.Models.Global.SelectedFYID <= VoucherBalance.GetBranchEstFiscalYear())
                {

                   //  var finalList = VoucherBalance.GetMappedDataOfOpeningBalance(VoucherBalanceId,Status).DistinctBy(x => x.FId).OrderBy(x => x.LedgerName).Distinct().ToList();
                    var finalList = VoucherBalance.GetMappedDataOfOpeningBalance(VoucherBalanceId, Status).DistinctBy(x => x.FId).OrderBy(x => x.LedgerName).Distinct().ToList();
                    ViewBag.ActivePager = 1;
                    ViewBag.Edit = 1;// allow edit
                    return PartialView(finalList);
                }
                else
                {
                    var finalList = VoucherBalance.GetMappedDataOfOpeningBalance(VoucherBalanceId, Status).DistinctBy(x => x.FId).OrderBy(x => x.LedgerName).Distinct().ToList();
                    ViewBag.ActivePager = 1;
                    ViewBag.Edit = 0; // non editable
                    return PartialView(finalList);
                    //return RedirectToAction("Index", "Home");
                    //return Json("Please Select Initial Fiscal Year", JsonRequestBehavior.AllowGet);
                    //ViewBag.ErrorMessage = "Please Select Initial Fiscal Year";
                    //return PartialView("~/Views/Account/ErrorMessage.cshtml");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(List<ChannakyaAccounting.Models.ViewModel.OpeningBalanceViewModel> openingBalanceList)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    VoucherBalance.Save(openingBalanceList);
                    return RedirectToAction("Create", new { VoucherBalanceId = 0 });


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(openingBalanceList);

        }



        public ActionResult CreateGeneralBalance(int VoucherBalanceId = 0)
        {
            const int Status = 0; // for genereral 
            if (Request.IsAjaxRequest() == true)
            {
                if (Loader.Models.Global.SelectedFYID <= VoucherBalance.GetHeadBranchEstFiscalYear())
                {

                    var finalList = VoucherBalance.GetMappedDataOfOpeningBalance(VoucherBalanceId, Status).DistinctBy(x => x.FId).OrderBy(x => x.LedgerName).Distinct().ToList();

                    ViewBag.ActivePager = 1;
                    ViewBag.Edit = 1; // allow to edit
                    //ViewBag.FYId = new SelectList(.Repository<Models.Models.FiscalYear>().GetAll().ToList(),"","");

                    // ModelState.Clear();
                    return PartialView(finalList);
                }
                else {

                    var finalList = VoucherBalance.GetMappedDataOfOpeningBalance(VoucherBalanceId, Status).DistinctBy(x => x.FId).OrderBy(x => x.LedgerName).Distinct().ToList();

                    ViewBag.ActivePager = 1;
                    //ViewBag.FYId = new SelectList(.Repository<Models.Models.FiscalYear>().GetAll().ToList(),"","");
                    ViewBag.Edit = 0;// to display only no edit
                    return PartialView(finalList);
                }
                //else
                //{
                //    ViewBag.ErrorMessage = "Please Select Initial Fiscal Year";
                //    return PartialView("~/Views/Account/ErrorMessage.cshtml");
                //}
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CreateGeneralBalance(List<ChannakyaAccounting.Models.ViewModel.OpeningBalanceViewModel> openingBalanceList)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (Loader.Models.Global.SelectedFYID <= VoucherBalance.GetBranchEstFiscalYear())
                    {
                        VoucherBalance.Save(openingBalanceList);
                        return CreateGeneralBalance(0);
                        //return RedirectToAction("CreateGeneralBalance", new { VoucherBalanceId = 0 });
                    }
                    else
                        return JavaScript("FY"); 
                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return PartialView(openingBalanceList);

        }


        public bool IsFinAccGroup(int ledgerId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == ledgerId).FinSys1.IsGroup;
            return isGroup;
        }

        

        public ActionResult _CreateAction()
        {
            return PartialView();
        }




        //[HttpPost]
        //public ActionResult _GetVoucherBalanceTreePopup(Loader.ViewModel.TreeViewParam param)
        //{

        //    ViewData["param"] = param;

        //    var filter = param.Filter == null ? "" : param.Filter;

        //  Models.ViewModel.TreeView tree = newModels.ViewModels.TreeView();

        //    if (param.WithOutMe == 0)
        //    {
        //        if (param.Title == "Select VoucherBalance")
        //        {
        //            tree = VoucherBalance.GetVoucherBalanceGroupTree(filter);
        //        }
        //        else if (param.Title == "Select Designation")
        //        {
        //            tree = VoucherBalance.GetDesignationGroupTree(filter);
        //        }

        //    }
        //    else
        //    {
        //        // tree = VoucherBalance.GetModels.Models.VoucherBalGroupTree(param.WithOutMe, filter);
        //    }
        //    return PartialView("_TreeViewPopup", tree);

        //}

        //public ActionResult PagerIndex(int page = 1)
        //   {
        //   int listCount = new Loader.Repository.GenericUnitOfWork().Repository<Models.Models.VoucherBal>().GetAll().Count();
        //   const int PageSize = 1;
        //       var pageCount = (listCount + PageSize - 1) / PageSize;

        //       ViewData["page"] = page;
        //       ViewData["count"] = pageCount;

        //       return View(listCount((page - 1) * PageSize).Take(PageSize));
        //   }


        [HttpPost]
        public ActionResult _GetVoucherBalanceTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();

            //if (param.WithOutMe == 0)
            //{
            //    if (param.Title == "Select VoucherBalance")
            //    {
            //        tree = VoucherBalance.GetVoucherBalanceGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = VoucherBalance.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.VoucherBal Models.Models.VoucherBal = VoucherBalance.GetSingle(id);
        //    if (Models.Models.VoucherBal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.VoucherBal);
        //}


        //[HttpPost]
        //public ActionResult Edit(Models.Models.VoucherBal Models.Models.VoucherBal)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        VoucherBalance.Save(Models.Models.VoucherBal);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.VoucherBal);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Models.Models.VoucherBal VoucherBalance = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherBal>().GetSingle(x => x.Id == id);
            bool deleteConfirm = true;

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.VoucherBal);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int VoucherBalanceId)
        {
            VoucherBalance.Delete(VoucherBalanceId);
            return RedirectToAction("Index");
        }
        public ActionResult DisplayImage(HttpPostedFileBase file)
        {
            using (var reader = new System.IO.BinaryReader(file.InputStream))
            {
                byte[] ContentImage = reader.ReadBytes(file.ContentLength);
                var ImageContent = Convert.ToBase64String(ContentImage, Base64FormattingOptions.None);
                return Json(ImageContent, JsonRequestBehavior.AllowGet);
            }


        }
    }
}