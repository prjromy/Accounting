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
    public class SubsiSetupController : Controller
    {

        private Service.Subsi_Setup.SubsiSetupService SubsiSetup;
        public SubsiSetupController()
        {
            SubsiSetup = new Service.Subsi_Setup.SubsiSetupService();
        }

        // GET: SubsiSetup

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                const int page = 1;
                 int pageSize =12;

                ViewBag.ViewType = 1;
                //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiSetup>().GetAll().AsQueryable().OrderByDescending(x=>x.SSId);
                //var pagedList = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize);
                //ViewBag.CountPager = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize).TotalPages;
                var pagedList = SubsiSetup.GetSubsiSetupList(page,pageSize,"").AsEnumerable();
                ViewBag.CountPager = 0;
                if (pagedList.Count() > 0)
                {
                    ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
                }
                ViewBag.ActivePager = page;
                return View(pagedList );
            }
            return RedirectToAction("Index", "Home");

        }


        public ActionResult _Details(int ViewType = 1, string query = null, int? page = 1)
        {
            ViewBag.ViewType = ViewType;
             int pageSize = 12;
            var pagedList = SubsiSetup.GetSubsiSetupList(Convert.ToInt32( page), pageSize, "").AsEnumerable();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView("_Details", pagedList);
          //ViewBag.Address = SubsiSetup.GetAddress(parentId);

        }
        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1)
        {
            ViewBag.ViewType = ViewType;
             int pageSize = 12;
            ViewBag.CountPager = 0;
            var pagedList = SubsiSetup.GetSubsiSetupList(Convert.ToInt32( page), pageSize, query).AsEnumerable();
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //ViewBag.Address = SubsiSetup.GetAddress(parentId);

        }

        [HttpGet]
        public ActionResult Create(int SubsiSetupId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Models.Models.SubsiSetup SubsiSetupDTO = new Models.Models.SubsiSetup();
                if (SubsiSetupId != 0)
                {

                    SubsiSetupDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiSetup>().GetSingle(x => x.SSId == SubsiSetupId);

                }


                return PartialView(SubsiSetupDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //public ActionResult Details(int id = 0)
        //{
        //    Models.Models.ProductDetail Models.Models.ProductDetail = SubsiSetup.GetSingle(id);
        //    if (Models.Models.ProductDetail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.ProductDetail);
        //}

        //public ActionResult GetProductDetails(int ProductType)
        //{
        //    Models.Models.ProductDetail ProductDetail = new Models.Models.ProductDetail();
        //    ProductDetail.SType = Convert.ToByte(ProductType);
        //    return PartialView(ProductDetail);
        //}





        public bool IsFinAccGroup(int ledgerId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == ledgerId).FinSys1.IsGroup;
            return isGroup;
        }
        public ActionResult CheckExistingSubsiTitle(string Title, int SSId)
        {
            bool ifSchemeNameExists = false;
            try
            {
                ifSchemeNameExists = SubsiSetup.CheckExistingSubsiTitle(Title, SSId);
                return Json(!ifSchemeNameExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckExistingSubsiPrefix(string Prefix, int SDID)
        {
            bool ifSchemeNameExists = false;
            try
            {
                ifSchemeNameExists = SubsiSetup.CheckExistingSubsiPrefix(Prefix, SDID);
                return Json(!ifSchemeNameExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Create(Models.Models.SubsiSetup SubsiDetail)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    if (SubsiDetail.SSId == 0)
                    {
                        SubsiSetup.Save(SubsiDetail);
                        return RedirectToAction("Create", new { SubsiSetupId = 0 });
                    }
                    else
                    {
                        SubsiSetup.Save(SubsiDetail);
                        return RedirectToAction("Create", new { SubsiSetupId = 0 });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(SubsiDetail);

        }


        public ActionResult _CreateAction()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult _GetProductDetailTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();

            //if (param.WithOutMe == 0)
            //{
            //    if (param.Title == "Select SubsiSetup")
            //    {
            //        tree = SubsiSetup.GetSubsiSetupGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = SubsiSetup.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.ProductDetail Models.Models.ProductDetail = SubsiSetup.GetSingle(id);
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
        //        SubsiSetup.Save(Models.Models.ProductDetail);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.ProductDetail);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Models.Models.SubsiSetup subsiSetup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiSetup>().GetSingle(x => x.SSId == id);
            bool deleteConfirm = true;
            if (subsiSetup != null)
            {
                deleteConfirm = false;
            }

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.ProductDetail);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int SubsiSetupId)
        {
            SubsiSetup.Delete(SubsiSetupId);
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