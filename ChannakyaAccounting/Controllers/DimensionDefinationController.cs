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
    public class DimensionDefinationController : Controller
    {
        private Service.DimensionDefination.DimensionDefinationService dimensionDefination;
        public DimensionDefinationController()
        {
            dimensionDefination = new Service.DimensionDefination.DimensionDefinationService();
        }

        // GET: DimensionDefination

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                 int page = 1;
                ViewBag.ViewType = 1;
                 int pageSize = 12  ;
                //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductDetail>().GetAll().AsQueryable();
                // var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().GetAll().AsQueryable();
                // var pagedList = new Pagination<Models.Models.DimensionDefination>(list, page , pageSize);
                //  ViewBag.CountPager = new Pagination<Models.Models.DimensionDefination>(list, page, pageSize).TotalPages;       
                // return View(dimensionDefination.GetAll().ToList());
                var pagedList = dimensionDefination.GetDimensionDefinationList(page,pageSize,null).ToList();
                ViewBag.CountPager = 0;
                if (pagedList.Count()> 0)
                {
                    ViewBag.CountPager=Math.Ceiling((pagedList.FirstOrDefault().totalCount)/(pageSize*1.0));
                }
                ViewBag.ActivePager = page;
                return View(pagedList);
            }
            return RedirectToAction("Index", "Home");

        }


        public ActionResult _Details(int ViewType = 1, string query = null, int? page = 1)
        {
             int pageSize = 12;
          //  var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().GetAll().OrderBy(X=>X.DDId).AsQueryable();
            ViewBag.ViewTYpe = ViewType;
            //if (!string.IsNullOrEmpty(query))
            //{
            //    list = list.Where(m => m.DefName.ToLower().Contains(query.ToLower()));
            //    ViewBag.Query = query;
            //}
            var pagedList = dimensionDefination.GetDimensionDefinationList(Convert.ToInt32( page), pageSize, query).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView("_Details", pagedList);
            //ViewBag.Address = dimensionDefination.GetAddress(parentId);

        }
        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1)
        {
             int pageSize = 12;
              ViewBag.ViewTYpe = ViewType;
            var pagedList = dimensionDefination.GetDimensionDefinationList(Convert.ToInt32(page), pageSize, query).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //ViewBag.Address = dimensionDefination.GetAddress(parentId);

        }
        [HttpGet]
        public ActionResult Create(int DimensionDefinationId = 0)
        {
            //asdfghjhgfggfgghgfdsdfghjkjhgfddfghj
            if (Request.IsAjaxRequest())
            {
                Models.Models.DimensionDefination DimensionDefinationDTO = new Models.Models.DimensionDefination();
                if (DimensionDefinationId != 0)
                {

                    DimensionDefinationDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().GetSingle(x => x.DDId == DimensionDefinationId);

                }
        
        
                return PartialView(DimensionDefinationDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //public ActionResult Details(int id = 0)
        //{
        //    Models.Models.ProductDetail Models.Models.ProductDetail = dimensionDefination.GetSingle(id);
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

        [HttpPost]
        public ActionResult Create(Models.Models.DimensionDefination DimensionDetail)
        {

            if (ModelState.IsValid)
            {
                
                try
                {
                    
                    if (DimensionDetail.DDId == 0 /*&& *//*dimensionDetailExists.DefName==DimensionDetail.Equals(x,DimensionDetail,StringComparison.CurrentCultureIgnoreCase*//*String.Equals(DimensionDetail.DefName, Convert.ToString(dimensionNameAll),StringComparison.OrdinalIgnoreCase)*/)
                    {
                        dimensionDefination.Save(DimensionDetail);
                        return RedirectToAction("Create", new { DimensionDefinationId = 0 });
                    }
                    else
                    {
                        dimensionDefination.Save(DimensionDetail);
                        return RedirectToAction("Create", new { DimensionDefinationId = DimensionDetail.DDId });
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
        //public ActionResult _GetProductDetailTreePopup(Loader.ViewModel.TreeViewParam param)
        //{

        //    ViewData["param"] = param;

        //    var filter = param.Filter == null ? "" : param.Filter;

        //  Models.ViewModel.TreeView tree = newModels.ViewModels.TreeView();

        //    if (param.WithOutMe == 0)
        //    {
        //        if (param.Title == "Select DimensionDefination")
        //        {
        //            tree = dimensionDefination.GetDimensionDefinationGroupTree(filter);
        //        }
        //        else if (param.Title == "Select Designation")
        //        {
        //            tree = dimensionDefination.GetDesignationGroupTree(filter);
        //        }

        //    }
        //    else
        //    {
        //        // tree = dimensionDefination.GetModels.Models.ProductDetailGroupTree(param.WithOutMe, filter);
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
            //    if (param.Title == "Select DimensionDefination")
            //    {
            //        tree = dimensionDefination.GetDimensionDefinationGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = dimensionDefination.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.ProductDetail Models.Models.ProductDetail = dimensionDefination.GetSingle(id);
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
        //        dimensionDefination.Save(Models.Models.ProductDetail);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.ProductDetail);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Models.Models.DimensionDefination ProductDetail = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().GetSingle(x => x.DDId == id);
            bool deleteConfirm = true;

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.ProductDetail);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int DimensionDefinationId)
        {
            dimensionDefination.Delete(DimensionDefinationId);
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