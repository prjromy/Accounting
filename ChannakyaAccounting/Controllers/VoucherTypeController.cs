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
    public class VoucherTypeController : Controller
    {
        private Service.VoucherType.VoucherTypeService VoucherType;

        public VoucherTypeController()
        {
            VoucherType = new Service.VoucherType.VoucherTypeService();
        }

        // GET: VoucherType
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                int page = 1;
                const int pageSize = 12;
                ViewBag.ViewType = 1;
                var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherType>().GetAll().AsQueryable();
                var pagedList = new Pagination<Models.Models.VoucherType>(list, page, pageSize);
                ViewBag.CountPager = new Pagination<Models.Models.VoucherType>(list, page, pageSize).TotalPages;
                ViewBag.ActivePager = page;
                return View(pagedList);
            }
            return RedirectToAction("Index", "Home");

        }

        public ActionResult _Details(int ViewType = 1, string query = null, int? page = 1)
        {
            const int pageSize = 12;
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherType>().GetAll().OrderBy(x=>x.VoucherName).AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.VoucherName.ToLower().Contains(query.ToLower()));
                ViewBag.Query = query;
            }

            ViewBag.ViewTYpe = ViewType;
            var pagedList = new Pagination<Models.Models.VoucherType>(list, page?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.VoucherType>(list, page?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView("_Details", pagedList);
            //ViewBag.Address = VoucherType.GetAddress(parentId);

        }

        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1)
        {
            const int pageSize = 12;
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherType>().GetAll().OrderBy(x => x.VoucherName).AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.VoucherName.ToLower().Contains(query.ToLower()));
                ViewBag.Query = query;
            }
            ViewBag.ViewTYpe = ViewType;
            var pagedList = new Pagination<Models.Models.VoucherType>(list, page?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.VoucherType>(list, page?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //ViewBag.Address = VoucherType.GetAddress(parentId);

        }

        [HttpGet]
        public ActionResult Create(int VoucherTypeId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Models.Models.VoucherType VoucherTypeDTO = new Models.Models.VoucherType();
                if (VoucherTypeId != 0)
                {

                    VoucherTypeDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherType>().GetSingle(x => x.VTypeID == VoucherTypeId);

                }


                return PartialView(VoucherTypeDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //public ActionResult Details(int id = 0)
        //{
        //    Models.Models.VoucherType Models.Models.VoucherType = VoucherType.GetSingle(id);
        //    if (Models.Models.VoucherType == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.VoucherType);
        //}

        //public ActionResult GetVoucherTypes(int ProductType)
        //{
        //    Models.Models.VoucherType VoucherType = new Models.Models.VoucherType();
        //    VoucherType.SType = Convert.ToByte(ProductType);
        //    return PartialView(VoucherType);
        //}

        public bool IsFinAccGroup(int ledgerId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == ledgerId).FinSys1.IsGroup;
            return isGroup;
        }

        public ActionResult CheckExistingVoucherName(string VoucherName, int VTypeID)
        {
            bool ifVoucherNameExists = false;
            try
            {
                ifVoucherNameExists = VoucherType.CheckExistingVoucherName(VoucherName, VTypeID);
                return Json(!ifVoucherNameExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CheckExistingVoucherPrefix(string Prefix, int VTypeID)
        {
            bool ifSchemeNameExists = false;
            try
            {
                ifSchemeNameExists = VoucherType.CheckExistingVoucherPrefix(Prefix, VTypeID);
                return Json(!ifSchemeNameExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Create(Models.Models.VoucherType VoucherTypeDetail)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    if (VoucherTypeDetail.VTypeID == 0)
                    {
                        VoucherType.Save(VoucherTypeDetail);
                        return RedirectToAction("Create", new { VoucherTypeId = 0 });
                    }
                    else
                    {
                        VoucherType.Save(VoucherTypeDetail);
                        return RedirectToAction("Create", new { VoucherTypeId = VoucherTypeDetail.VTypeID });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(VoucherTypeDetail);

        }

        public ActionResult _CreateAction()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult _GetVoucherTypeTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();

            //if (param.WithOutMe == 0)
            //{
            //    if (param.Title == "Select VoucherType")
            //    {
            //        tree = VoucherType.GetVoucherTypeGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = VoucherType.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.VoucherType Models.Models.VoucherType = VoucherType.GetSingle(id);
        //    if (Models.Models.VoucherType == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.VoucherType);
        //}


        //[HttpPost]
        //public ActionResult Edit(Models.Models.VoucherType Models.Models.VoucherType)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        VoucherType.Save(Models.Models.VoucherType);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.VoucherType);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            var totalCount = VoucherType.IfExistsinVoucher(id);
            bool deleteConfirm = false;
          
            if (totalCount==0)
            {
                deleteConfirm = true;
            }

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.VoucherType);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int VoucherTypeId)
        {
            VoucherType.Delete(VoucherTypeId);
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