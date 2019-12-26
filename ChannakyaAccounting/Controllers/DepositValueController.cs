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
    public class DurationController : Controller
    {
        private Service.Duration.DurationService Duration;

        public DurationController()
        {
            Duration = new Service.Duration.DurationService();
        }

        // GET: Duration

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                int page = 1;
                const int pageSize = 12;
                ViewBag.ViewType = 1;
                var pagedList = Duration.GetDurationList(page,pageSize,"").AsQueryable();
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
        public JsonResult IsManual(int DDID)
        {
            var id = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().GetSingle(x => x.DDId == DDID).IsManual;
            return Json(id, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _Details(int ViewType = 1, string query = null, int page = 1)
        {
            const int pageSize = 12;
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            var pagedList = Duration.GetDurationList(page, pageSize, query).AsQueryable();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            return PartialView( pagedList);
            //ViewBag.Address = Duration.GetAddress(parentId);

        }
        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int page = 1) 
        {
            const int pageSize = 12;
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            var pagedList = Duration.GetDurationList(page, pageSize, query).AsQueryable();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            return PartialView(pagedList);
            //ViewBag.Address = Duration.GetAddress(parentId);

        }
        [HttpGet]
        public ActionResult Create(int DurationId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Models.Models.Duration DurationDTO = new Models.Models.Duration();
                if (DurationId != 0)
                {

                    DurationDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetSingle(x => x.Id == DurationId);

                }


                return PartialView(DurationDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //public ActionResult Details(int id = 0)
        //{
        //    Models.Models.Duration Models.Models.Duration = Duration.GetSingle(id);
        //    if (Models.Models.Duration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.Duration);
        //}

        //public ActionResult GetDurations(int ProductType)
        //{
        //    Models.Models.Duration Duration = new Models.Models.Duration();
        //    Duration.SType = Convert.ToByte(ProductType);
        //    return PartialView(Duration);
        //}






        [HttpPost]
        public ActionResult Create(Models.Models.Duration depositData)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    if (depositData.Id == 0)
                    {
                        Duration.Save(depositData);
                        return RedirectToAction("Create", new { DurationId = 0 });
                    }
                    else
                    {
                        Duration.Save(depositData);
                        return RedirectToAction("Create", new { DurationId = depositData.Id });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(Duration);

        }


        public ActionResult _CreateAction()
        {
            return PartialView();
        }




        //[HttpPost]
        //public ActionResult _GetDurationTreePopup(Loader.ViewModel.TreeViewParam param)
        //{

        //    ViewData["param"] = param;

        //    var filter = param.Filter == null ? "" : param.Filter;

        //  Models.ViewModel.TreeView tree = newModels.ViewModels.TreeView();

        //    if (param.WithOutMe == 0)
        //    {
        //        if (param.Title == "Select Duration")
        //        {
        //            tree = Duration.GetDurationGroupTree(filter);
        //        }
        //        else if (param.Title == "Select Designation")
        //        {
        //            tree = Duration.GetDesignationGroupTree(filter);
        //        }

        //    }
        //    else
        //    {
        //        // tree = Duration.GetModels.Models.DurationGroupTree(param.WithOutMe, filter);
        //    }
        //    return PartialView("_TreeViewPopup", tree);

        //}

        //public ActionResult PagerIndex(int page = 1)
        //   {
        //   int listCount = new Loader.Repository.GenericUnitOfWork().Repository<Models.Models.Duration>().GetAll().Count();
        //   const int PageSize = 1;
        //       var pageCount = (listCount + PageSize - 1) / PageSize;

        //       ViewData["page"] = page;
        //       ViewData["count"] = pageCount;

        //       return View(listCount((page - 1) * PageSize).Take(PageSize));
        //   }


        [HttpPost]
        public ActionResult _GetDurationTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();

            //if (param.WithOutMe == 0)
            //{
            //    if (param.Title == "Select Duration")
            //    {
            //        tree = Duration.GetDurationGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = Duration.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.Duration Models.Models.Duration = Duration.GetSingle(id);
        //    if (Models.Models.Duration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.Duration);
        //}


        //[HttpPost]
        //public ActionResult Edit(Models.Models.Duration Models.Models.Duration)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Duration.Save(Models.Models.Duration);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.Duration);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Models.Models.Duration Duration = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetSingle(x => x.Id == id);
            bool deleteConfirm = true;

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.Duration);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int DurationId)
        {
            Duration.Delete(DurationId);
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