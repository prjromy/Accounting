using ChannakyaAccounting.Models.ViewModel;
using Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaAccounting.Controllers
{
    [MyAuthorize]
    public class SchemeController : Controller
    {
        private Service.DepositScheme.DepositSchemeService DepositScheme = null;
        private Service.FinAcc.FinAccService FinAccService = null;

        public SchemeController()
        {
            DepositScheme = new Service.DepositScheme.DepositSchemeService();
            FinAccService = new Service.FinAcc.FinAccService();
        }

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                int  pageSize= 12;
                int page = 1;
                ViewBag.ViewType = 1;
                var pagedList = DepositScheme.GetSchmDetailList(page, pageSize, "").AsQueryable();
                ViewBag.CountPager = 0;
                if (pagedList.Count() > 0)
                {
                    ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
                }
            
                ViewBag.ActivePager = page;
                return View(pagedList);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(int ViewType = 1, string query = null, int? page = 1)
        {
             int pageSize = 12;
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetAll().OrderBy(x=>x.FID).AsQueryable();
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            var pagedList = DepositScheme.GetSchmDetailList(Convert.ToInt32( page), pageSize, query).AsQueryable();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            return PartialView(pagedList);
            //ViewBag.Address = DepositScheme.GetAddress(parentId);
        }
        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1)
        {
             int pageSize = 12;
            // var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetAll().OrderBy(x => x.FinAcc).AsQueryable();
            var pagedList = DepositScheme.GetSchmDetailList(Convert.ToInt32(page), pageSize, query).AsQueryable();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //ViewBag.Address = DepositScheme.GetAddress(parentId);
        }

        //public ActionResult Details(int id = 0)
        //{
        //    Models.Models.SchmDetail Models.Models.SchmDetail = DepositScheme.GetSingle(id);
        //    if (Models.Models.SchmDetail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.SchmDetail);
        //}

        public ActionResult GetSchemeDepositType()
        {
            ViewBag.F2Type = DepositScheme.GetSchemeTYpe();

            return PartialView();
        }

        public ActionResult GetSchemeDetails(int SchemeType)
        {
            Models.Models.SchmDetail schmDetail = new Models.Models.SchmDetail();
            schmDetail.SType = Convert.ToByte(SchemeType);
            return PartialView(schmDetail);
        }

        public class schemeDetailsClass
        {
            public int stype { get;set; }
            public int depositType { get; set; }
        }

        public JsonResult GetStypeFromSchemeId(int SchemeId)
        {
            int Stype = DepositScheme.GetStypeFromSchemeId(SchemeId);
            return Json(Stype, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepositTypeFromSchemeId(int SchemeId)
        {
            int DepositType = DepositScheme.GetDepositTypeFromSchemeId(SchemeId);
            return Json(DepositType, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create(string activeText, int schemeId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Models.Models.SchmDetail SchemeDTO = new Models.Models.SchmDetail();
                int? parentLedgerId;
               
                if (schemeId == 0)
                {
                    parentLedgerId = 0;
                }
                else
                {
                    var finaccId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeId).FID;
                    parentLedgerId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == finaccId).Pid;
                }
                if (schemeId != 0)
                {
                    var finaccId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeId).FID;
                    SchemeDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeId);

                    var finaccDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == finaccId);
                    //var F2List = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetAll().Where(x => x.F2id == FinAccDTO.F2Type).ToList();
                    //ViewBag.F2Type = new SelectList(F2List, "F2id", "F2Desc");
                    ViewBag.ActiveId = schemeId;
                    var f2Id = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == finaccId).F2Type;
                    ViewBag.IsGroup = IsFinAccGroup(f2Id);
                    var schemeDetails = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeId);
                    if (schemeDetails != null)
                    {
                        SchemeDTO = schemeDetails;
                    }
                    ViewBag.Alias = SchemeDTO.FinAcc.Alias;
                    ViewBag.DepositType = finaccDTO.Pid;
                }

                ViewBag.ActiveText = activeText;
                ViewData["param"] = new ChannakyaAccounting.Models.ViewModel.TreeViewParam(false, true, false, 0, 0, "Available FinAcc Group");
                return PartialView(SchemeDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public bool IsFinAccGroup(int ledgerId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == ledgerId).FinSys1.IsGroup;
            return isGroup;
        }

        public ActionResult CheckExistingSchemeName(string SDName, int SDID)
        {
            bool ifSchemeNameExists = false;
            try
            {
                ifSchemeNameExists = DepositScheme.CheckSchemenName(SDName,SDID);
                return Json(!ifSchemeNameExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CheckExistingAlias(string alias , int? schemeId)
        {
            try
            {
                var isExists = DepositScheme.GetAlias(alias, schemeId);
                return Json(isExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Create(Models.Models.SchmDetail SchmDetail, String Alias)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    if (SchmDetail.SDID == 0)
                    {
                        DepositScheme.Save(SchmDetail,Alias);
                        return RedirectToAction("Create", new { SchmDetail = 0 });
                    }
                    else
                    {
                        DepositScheme.Save(SchmDetail,Alias);
                        var result = true;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(SchmDetail);

        }


        public ActionResult _CreateAction()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult _UpdateFinAccTree(int selectedNode, bool allowSelectGroupNode, bool withImageIcon, bool withCheckBox, int withOutMe = 0, int rootNode = -1)
        {
            ViewData["AllowSelectGroup"] = allowSelectGroupNode;
            ViewData["WithImageIcon"] = withImageIcon;
            ViewData["WithCheckBox"] = withCheckBox;
            ViewData["param"] = new ChannakyaAccounting.Models.ViewModel.TreeViewParam(false, true, true, 0, selectedNode, "Available FinAcc Group");

            ChannakyaAccounting.Models.ViewModel.TreeView tree = new ChannakyaAccounting.Models.ViewModel.TreeView();
            if (rootNode < 0)
            {
                tree = FinAccService.GetFinAccGroupTree(withOutMe);

            }
            //else
            //{
            //    tree = FinAccService.GetFinAccGroupTree(rootNode, withOutMe);
            //}
            return PartialView("_AccountingTreeViewBody", tree);
        }


        //[HttpPost]
        //public ActionResult _GetSchmDetailTreePopup(Loader.ViewModel.TreeViewParam param)
        //{

        //    ViewData["param"] = param;

        //    var filter = param.Filter == null ? "" : param.Filter;

        //  Models.ViewModel.TreeView tree = newModels.ViewModels.TreeView();

        //    if (param.WithOutMe == 0)
        //    {
        //        if (param.Title == "Select Department")
        //        {
        //            tree = DepositScheme.GetDepartmentGroupTree(filter);
        //        }
        //        else if (param.Title == "Select Designation")
        //        {
        //            tree = DepositScheme.GetDesignationGroupTree(filter);
        //        }

        //    }
        //    else
        //    {
        //        // tree = DepositScheme.GetModels.Models.SchmDetailGroupTree(param.WithOutMe, filter);
        //    }
        //    return PartialView("_TreeViewPopup", tree);

        //}

        //public ActionResult PagerIndex(int page = 1)
        //   {
        //   int listCount = new Loader.Repository.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetAll().Count();
        //   const int PageSize = 1;
        //       var pageCount = (listCount + PageSize - 1) / PageSize;

        //       ViewData["page"] = page;
        //       ViewData["count"] = pageCount;

        //       return View(listCount((page - 1) * PageSize).Take(PageSize));
        //   }


        [HttpPost]
        public ActionResult _GetSchmDetailTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();

            //if (param.WithOutMe == 0)
            //{
            //    if (param.Title == "Select Department")
            //    {
            //        tree = DepositScheme.GetDepartmentGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = DepositScheme.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.SchmDetail Models.Models.SchmDetail = DepositScheme.GetSingle(id);
        //    if (Models.Models.SchmDetail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.SchmDetail);
        //}


        //[HttpPost]
        //public ActionResult Edit(Models.Models.SchmDetail Models.Models.SchmDetail)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        DepositScheme.Save(Models.Models.SchmDetail);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.SchmDetail);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            //Models.Models.SchmDetail SchmDetail = new Loader.Repository.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == id);
            bool deleteConfirm = true;

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.SchmDetail);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int SchemeId)
        {
            var result = DepositScheme.Delete(SchemeId);
            //return RedirectToAction("Index");
            return Json(result, JsonRequestBehavior.AllowGet);

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