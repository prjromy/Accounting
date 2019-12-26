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
    public class BatchDescriptionController : Controller
    {
        private Service.BatchDescription.BatchDescriptionService BatchDescription;

        public BatchDescriptionController()
        {
            BatchDescription = new Service.BatchDescription.BatchDescriptionService();
        }

        // GET: BatchDescription

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                ViewBag.ViewType = 1;
                int page = 1;
                int pageSize = 12;
                var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().GetAll().AsQueryable();
                var pagedList = new Pagination<Models.Models.BatchDescription>(list,page,pageSize);
                ViewBag.CountPager = new Pagination<Models.Models.BatchDescription>(list,page,pageSize).TotalPages;
                ViewBag.ActivePager = page;
                return View(pagedList);
            }
            return RedirectToAction("Index", "Home");

        }


        public ActionResult _Details(int ViewType = 1, string query = null, int? page = 1)
        {
            int pageSize = 12;
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().GetAll().OrderBy(x=>x.BatchName).AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.BatchName.ToLower().Contains(query.ToLower()));
                ViewBag.Query = query;
            }
            var pagedList = new Pagination<Models.Models.BatchDescription>(list, page??0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.BatchDescription>(list, page??0, pageSize).TotalPages;
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
           // return PartialView("_Details", list);
            //ViewBag.Address = BatchDescription.GetAddress(parentId);

        }
        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1)
        {
            int pageSize = 12;
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().GetAll().OrderBy(x => x.BatchName).AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.BatchName.ToLower().Contains(query.ToLower()));
                ViewBag.Query = query;
            }
            var pagedList = new Pagination<Models.Models.BatchDescription>(list, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.Models.BatchDescription>(list, page ?? 0, pageSize).TotalPages;
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            // return PartialView("_Details", list);
            //ViewBag.Address = BatchDescription.GetAddress(parentId);

        }

        [HttpGet]
        public ActionResult Create(int BatchDescriptionId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Models.Models.BatchDescription BatchDescriptionDTO = new Models.Models.BatchDescription();
                if (BatchDescriptionId != 0)
                {

                    BatchDescriptionDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().GetSingle(x => x.BId == BatchDescriptionId);

                }


                return PartialView(BatchDescriptionDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //public ActionResult Details(int id = 0)
        //{
        //    Models.Models.BatchDescription Models.Models.BatchDescription = BatchDescription.GetSingle(id);
        //    if (Models.Models.BatchDescription == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.BatchDescription);
        //}

        //public ActionResult GetBatchDescriptions(int ProductType)
        //{
        //    Models.Models.BatchDescription BatchDescription = new Models.Models.BatchDescription();
        //    BatchDescription.SType = Convert.ToByte(ProductType);
        //    return PartialView(BatchDescription);
        //}
        public ActionResult CheckExistingBatchName(string BatchName, int BId)
        {
            bool ifVoucherNameExists = false;
            try
            {
                ifVoucherNameExists = BatchDescription.CheckExistingVoucherName(BatchName, BId);
                return Json(!ifVoucherNameExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CheckExistingBatchPrefix(string Prefix, int BId)
        {
            bool ifSchemeNameExists = false;
            try
            {
                ifSchemeNameExists = BatchDescription.CheckExistingVoucherPrefix(Prefix, BId);
                return Json(!ifSchemeNameExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }




        public bool IsFinAccGroup(int ledgerId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == ledgerId).FinSys1.IsGroup;
            return isGroup;
        }

        [HttpPost]
        public ActionResult Create(Models.Models.BatchDescription BatchDescriptionDetail)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    if (BatchDescriptionDetail.BId == 0)
                    {
                        BatchDescription.Save(BatchDescriptionDetail);
                        return RedirectToAction("Create", new { BatchDescriptionId = 0 });
                    }
                    else
                    {
                        BatchDescription.Save(BatchDescriptionDetail);
                        return RedirectToAction("Create", new { BatchDescriptionId = BatchDescriptionDetail.BId });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(BatchDescriptionDetail);

        }


        public ActionResult _CreateAction()
        {
            return PartialView();
        }



        [HttpPost]
        public ActionResult _GetBatchDescriptionTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();

            //if (param.WithOutMe == 0)
            //{
            //    if (param.Title == "Select BatchDescription")
            //    {
            //        tree = BatchDescription.GetBatchDescriptionGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = BatchDescription.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.BatchDescription Models.Models.BatchDescription = BatchDescription.GetSingle(id);
        //    if (Models.Models.BatchDescription == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.BatchDescription);
        //}


        //[HttpPost]
        //public ActionResult Edit(Models.Models.BatchDescription Models.Models.BatchDescription)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        BatchDescription.Save(Models.Models.BatchDescription);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.BatchDescription);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            var totalCount = BatchDescription.IfExistsinVoucher(id);
            bool deleteConfirm = false;

            if (totalCount == 0)
            {
                deleteConfirm = true;
            }

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.BatchDescription);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int BatchDescriptionId)
        {
            BatchDescription.Delete(BatchDescriptionId);
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