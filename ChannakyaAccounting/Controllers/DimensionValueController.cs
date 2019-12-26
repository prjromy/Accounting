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
    public class DimensionValueController : Controller
    {
        private Service.DimensionValue.DimensionValueService DimensionValue;

        public DimensionValueController()
        {
            DimensionValue = new Service.DimensionValue.DimensionValueService();
        }

        // GET: DimensionValue

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                ChannakyaAccounting.Service.DimensionDefination.DimensionDefinationService dimService = new ChannakyaAccounting.Service.DimensionDefination.DimensionDefinationService();
                var dimTypes = dimService.GetDimensionDefinationList().FirstOrDefault();
                ViewBag.ViewType = 1;
                int page = 1;
                int pageSize = 12;
                // var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionValue>().FindBy(x => x.DimensionDefination.IsManual == 0 && x.DDId == dimTypes.DDId).AsQueryable().OrderByDescending(x => x.DDId);
                var pagedList = DimensionValue.GetDimensionValueList(page, pageSize, "", dimTypes.DDId);
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


        public ActionResult _Details(int? DDid=0, int ViewType = 1, string query = null, int? page = 1)
        {

            int pageSize = 12;
           // var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionValue>().FindBy(x => x.DimensionDefination.IsManual == 0).AsQueryable();
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            var pagedList = DimensionValue.GetDimensionValueList((int)page, pageSize, "", (int)DDid);
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView("_Details", pagedList);



            //ViewBag.Address = DimensionValue.GetAddress(parentId);

        }
        public ActionResult _DetailPartial(int? DDId, int ViewType = 1, string query = null, int? page = 1)
        {

            int pageSize = 12;
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            //List<Models.Models.DimensionValue> list = new List<Models.Models.DimensionValue>();
            //IQueryable<Models.Models.DimensionValue> list;

            //list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionValue>().FindBy(x => x.DimensionDefination.IsManual == 0).AsQueryable();
            //if (!string.IsNullOrEmpty(query) && DDId != 0)
            //{
            //    list = list.Where(m => m.DimensionValue1.ToLower().Contains(query.ToLower()) && m.DDId == DDId);
            //    ViewBag.Query = query;
            //}

            //else
            //{
            //    list = list.Where(m => m.DDId == DDId);

            //}
            //list = list.OrderByDescending(x => x.DDId);
            var pagedList = DimensionValue.GetDimensionValueList((int)page, pageSize, "", (int)DDId);
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);

            //ViewBag.Address = DimensionValue.GetAddress(parentId);

        }
        [HttpGet]
        public ActionResult Create(int DimensionValueId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Models.Models.DimensionValue DimensionValueDTO = new Models.Models.DimensionValue();
                if (DimensionValueId != 0)
                {

                    DimensionValueDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionValue>().GetSingle(x => x.DVId == DimensionValueId);

                }


                return PartialView(DimensionValueDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //public ActionResult Details(int id = 0)
        //{
        //    Models.Models.DimensionValue Models.Models.DimensionValue = DimensionValue.GetSingle(id);
        //    if (Models.Models.DimensionValue == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.DimensionValue);
        //}

        //public ActionResult GetDimensionValues(int ProductType)
        //{
        //    Models.Models.DimensionValue DimensionValue = new Models.Models.DimensionValue();
        //    DimensionValue.SType = Convert.ToByte(ProductType);
        //    return PartialView(DimensionValue);
        //}





        public bool IsFinAccGroup(int ledgerId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == ledgerId).FinSys1.IsGroup;
            return isGroup;
        }

        [HttpPost]
        public ActionResult Create(Models.Models.DimensionValue DimensionDetail)
        {
            
            if (ModelState.IsValid)
            {

                try
                {
                    if (DimensionDetail.DVId == 0)
                    {
                        DimensionValue.Save(DimensionDetail);
                        return RedirectToAction("Create", new { DDId = 0 });
                    }
                    else
                    {
                        DimensionValue.Save(DimensionDetail);
                        return RedirectToAction("Create", new { DDId = DimensionDetail.DDId });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(DimensionDetail);

        }


        public ActionResult _CreateAction()
        {
            return PartialView();
        }




        //[HttpPost]
        //public ActionResult _GetDimensionValueTreePopup(Loader.ViewModel.TreeViewParam param)
        //{

        //    ViewData["param"] = param;

        //    var filter = param.Filter == null ? "" : param.Filter;

        //  Models.ViewModel.TreeView tree = newModels.ViewModels.TreeView();

        //    if (param.WithOutMe == 0)
        //    {
        //        if (param.Title == "Select DimensionValue")
        //        {
        //            tree = DimensionValue.GetDimensionValueGroupTree(filter);
        //        }
        //        else if (param.Title == "Select Designation")
        //        {
        //            tree = DimensionValue.GetDesignationGroupTree(filter);
        //        }

        //    }
        //    else
        //    {
        //        // tree = DimensionValue.GetModels.Models.DimensionValueGroupTree(param.WithOutMe, filter);
        //    }
        //    return PartialView("_TreeViewPopup", tree);

        //}

        //public ActionResult PagerIndex(int page = 1)
        //   {
        //   int listCount = new Loader.Repository.GenericUnitOfWork().Repository<Models.Models.DimensionValue>().GetAll().Count();
        //   const int PageSize = 1;
        //       var pageCount = (listCount + PageSize - 1) / PageSize;

        //       ViewData["page"] = page;
        //       ViewData["count"] = pageCount;

        //       return View(listCount((page - 1) * PageSize).Take(PageSize));
        //   }


        [HttpPost]
        public ActionResult _GetDimensionValueTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();

            //if (param.WithOutMe == 0)
            //{
            //    if (param.Title == "Select DimensionValue")
            //    {
            //        tree = DimensionValue.GetDimensionValueGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = DimensionValue.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.DimensionValue Models.Models.DimensionValue = DimensionValue.GetSingle(id);
        //    if (Models.Models.DimensionValue == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Models.Models.DimensionValue);
        //}


        //[HttpPost]
        //public ActionResult Edit(Models.Models.DimensionValue Models.Models.DimensionValue)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        DimensionValue.Save(Models.Models.DimensionValue);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.DimensionValue);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Models.Models.DimensionValue DimensionValue = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionValue>().GetSingle(x => x.DDId == id);
            bool deleteConfirm = true;

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.DimensionValue);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int DimensionValueId)
        {
            DimensionValue.Delete(DimensionValueId);
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

        //public JsonResult GetSpecificDimensionValue(int DDId)
        //{
        //    List<> 
        //    return JsonResult(JsonRequestBehavior.AllowGet)
        //}
    }
}