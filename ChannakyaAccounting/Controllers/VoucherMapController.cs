using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loader.ViewModel;
using Loader;

namespace ChannakyaAccounting.Controllers
{
    [MyAuthorize]
    public class VoucherMapController : Controller
    {
        private Service.VoucherMap.VoucherMapService VoucherMap;

        public VoucherMapController()
        {
            VoucherMap = new Service.VoucherMap.VoucherMapService();
        }

        // GET: VoucherMap

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.ViewType = 1;
                int page = 1;
                int pageSize = 12;
                //return RedirectToAction("Create", "VoucherMap",new { VoucherMapId =0});
                //  var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherNo>().GetAll().OrderBy(x => x.VNId).AsQueryable();
                var pagedList = VoucherMap.GetVoucherNoList(page, pageSize, "").AsEnumerable();
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
            const int pageSize = 12;
            ViewBag.ViewTYpe = ViewType;
            var pagedList = VoucherMap.GetVoucherNoList(Convert.ToInt32(page), pageSize, query).AsEnumerable();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView("_Details", pagedList);
            //ViewBag.Address = VoucherMap.GetAddress(parentId);

        }

        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1)
        {
            const int pageSize = 12;
            ViewBag.ViewTYpe = ViewType;
            var pagedList = VoucherMap.GetVoucherNoList(Convert.ToInt32(page), pageSize, query).AsEnumerable();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //ViewBag.Address = VoucherMap.GetAddress(parentId);

        }

        [HttpGet]
        public ActionResult Create(int VoucherMapId = 0, int batchNo = 0, int vTypeId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Models.Models.VoucherNo VoucherMapDTO = new Models.Models.VoucherNo();
                if (VoucherMapId != 0 && VoucherMapId != -1)
                {

                    VoucherMapDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherNo>().GetSingle(x => x.VNId == VoucherMapId);
                    //ViewBag.CustomerName = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.CustIndividual>().GetSingle(x => x.CID == VoucherMapDTO.CId).Fname;

                }
                if (VoucherMapId == -1)
                {

                    VoucherMapDTO.BId = batchNo;
                    VoucherMapDTO.VTypeId = vTypeId;
                    //ViewBag.CustomerName = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.CustIndividual>().GetSingle(x => x.CID == customerId).Fname;
                }


                return PartialView(VoucherMapDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult EditVoucherMapDropDown(int vtypeId, int batchNo)
        {

            var checkifVoucherExists = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherNo>().FindBy(x => x.BId == batchNo && x.VTypeId == vtypeId);
            if (checkifVoucherExists.Count() > 0)
            {
                return RedirectToAction("Create", new { VoucherMapId = checkifVoucherExists.FirstOrDefault().VNId });
            }

            return RedirectToAction("Create", new { VoucherMapId = -1, batchNo = batchNo, vTypeId = vtypeId });
        }

        public bool IsFinAccGroup(int ledgerId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == ledgerId).FinSys1.IsGroup;
            return isGroup;
        }

        [HttpPost]
        public ActionResult Create(Models.Models.VoucherNo VoucherMapDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (VoucherMapDTO.VNId == 0)
                    {
                        VoucherMap.Save(VoucherMapDTO);
                        return RedirectToAction("Create", new { VoucherMapId = 0 });
                    }
                    else
                    {
                        VoucherMap.Save(VoucherMapDTO);
                        return RedirectToAction("Create", new { VoucherMapId = VoucherMapDTO.VNId });
                    }
                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }
            }
            return View(VoucherMap);
        }

        public ActionResult _CreateAction()
        {
            return PartialView();
        }

        //[HttpPost]
        //public ActionResult _GetProductDetailTreePopup(Loader.ViewModel.TreeViewParam param)
        //{

        //    ViewData["param"] = param;

        //    var filter = param.Filter == null ? "" : param.Filter;

        //  Models.ViewModel.TreeView tree = newModels.ViewModels.TreeView();

        //    if (param.WithOutMe == 0)
        //    {
        //        if (param.Title == "Select VoucherMap")
        //        {
        //            tree = VoucherMap.GetVoucherMapGroupTree(filter);
        //        }
        //        else if (param.Title == "Select Designation")
        //        {
        //            tree = VoucherMap.GetDesignationGroupTree(filter);
        //        }

        //    }
        //    else
        //    {
        //        // tree = VoucherMap.GetModels.Models.ProductDetailGroupTree(param.WithOutMe, filter);
        //    }
        //    return PartialView("_TreeViewPopup", tree);

        //}

        //public ActionResult PagerIndex(int page = 1)
        //   {
        //   int listCount = new Loader.Repository.GenericUnitOfWork().Repository<Models.Models.ProductDetail>().GetAll().Count();
        //   const int PageSize = 1;
        //       var pageCount = (listCount + PageSize - 1) / PageSize;

        //       ViewData["page"] = page;
        //       ViewData["count"] = pageCount;

        //       return View(listCount((page - 1) * PageSize).Take(PageSize));
        //   }

        [HttpPost]
        public ActionResult _GetProductDetailTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();

            //if (param.WithOutMe == 0)
            //{
            //    if (param.Title == "Select VoucherMap")
            //    {
            //        tree = VoucherMap.GetVoucherMapGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = VoucherMap.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.ProductDetail Models.Models.ProductDetail = VoucherMap.GetSingle(id);
        //    if (Models.Models.ProductDetail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.ProductDetail);
        //}


        //[HttpPost]
        //public ActionResult Edit(Models.Models.ProductDetail Models.Models.ProductDetail)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        VoucherMap.Save(Models.Models.ProductDetail);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.ProductDetail);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0, int bType = 0, int vType = 0, int FDate = 0)
        {
            bool deleteConfirm = true;
            //var voucher1Data = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher1>().FindBy(x => x.VNId == id).Count() ;
            //var voucher1TData = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher1T>().FindBy(x => x.VNId == id).Count();
            //if(voucher1Data>0 || voucher1TData>0)
            //{
            //    deleteConfirm = false;
            //}
            int userid = Loader.Models.Global.UserId;
            int count = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.UserVSVoucher>().GetAll().Where(x => x.UserId == userid && x.BatchId == bType && x.VTypeId == vType).ToList().Count();
            int currentNo = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherNo>().FindBy(x => x.VNId == id && x.BId == bType && x.VTypeId == vType).SingleOrDefault().CurrentNo;
            if (count > 0)
            {
                deleteConfirm = false;
            }
            if (currentNo > 0)
            {
                deleteConfirm = false;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.ProductDetail);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int VoucherMapId)
        {
            VoucherMap.Delete(VoucherMapId);
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