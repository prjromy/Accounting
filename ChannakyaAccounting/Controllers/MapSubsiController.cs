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
    public class MapSubsiController : Controller
    {

        private Service.Subsi_Setup.SubsiDetailService subsiDetail;
        public MapSubsiController()
        {
            subsiDetail = new Service.Subsi_Setup.SubsiDetailService();
        }

        // GET: SubsiDetail

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                ViewBag.ViewType = 1;
                int page = 1;
                int pageSize = 12;
                // var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductDetail>().GetAll().AsQueryable();
                //var list = subsiDetail.GetAll().AsQueryable();
                List<Models.ViewModel.SubsiDetailViewModel> emptyList = new List<Models.ViewModel.SubsiDetailViewModel>();
                var pagedList = new Pagination<Models.ViewModel.SubsiDetailViewModel>(emptyList.AsQueryable(), page, pageSize);
                ViewBag.CountPager = new Pagination<Models.ViewModel.SubsiDetailViewModel>(emptyList.AsQueryable(), page, pageSize).TotalPages;
                ViewBag.ActivePager = page;
                return View(pagedList);
            }
            return RedirectToAction("Index", "Home");

        }


        //[ChildActionOnly]
        public JsonResult GetLedgerParentSubsiTable(int ledgerId)
        {
            
            int subsiTblId = ChannakyaAccounting.Helper.GenerateSubsiTypeList.GenerateSubsiTableFromLedgerId(ledgerId);
            return Json(subsiTblId, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public JsonResult GetDecodedHtml(string htmlContent)
        {
            string decoded = System.Net.WebUtility.HtmlDecode(htmlContent);
            //var finalStr = System.Net.WebUtility.HtmlDecode(decoded);
            return Json(decoded, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _Details(int ViewType = 1, string query = null, int? page = 1,int FId=0)
        {
            int pageSize = 12;
            var list =subsiDetail.GetAll().AsQueryable();
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            List<int> FidList1 = subsiDetail.GetSubsiLedgerIdByName(query);
            string Check = "";
            if (FidList1.Count > 0)
            {
                Check = String.Join(",", FidList1.Select(p => p.ToString()).ToArray());
                Check = Check + "," + FId.ToString();
            }
            else
            {
                if (FId != 0)
                    Check = FId.ToString();
            }
            var pagedList = subsiDetail.GetSubsiDetailList(Convert.ToInt32(page), pageSize, Check).AsEnumerable();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            return PartialView("_Details", pagedList);
            //ViewBag.Address = SubsiDetail.GetAddress(parentId);

        }
        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1,int FId= 0)
        {
            int pageSize = 12;
            //var list = subsiDetail.GetAll().AsQueryable();
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            List<int> FidList1 = subsiDetail.GetSubsiLedgerIdByName(query);
            string Check = "";
            if (FidList1.Count > 0)
            {
                Check = String.Join(",", FidList1.Select(p => p.ToString()).ToArray());
                Check = Check + "," + FId.ToString();
            }
            else
            {
                if (FId != 0)
                    Check = FId.ToString();
            }

            //if (!string.IsNullOrEmpty(query) && FId == 0)
            //{
            //    //list=list.Where(m=>m.FId)

            //    //List<int> FidList = subsiDetail.GetSubsiLedgerIdByName(query);
            //    // list = list.Where(x => FidList.Contains(x.FId));
            //    List<int> FidList = subsiDetail.GetSubsiLedgerIdByName(query);
            //    list = list.Where(x => FidList.Contains(x.FId));
            //    list = new List<Models.Models.SubsiDetail>().AsQueryable();
            //    ViewBag.Query = query;
            //}
            //else if (!string.IsNullOrEmpty(query) && FId != 0)
            //{
            //    //list=list.Where(m=>m.FId) 

            //    List<int> FidList = subsiDetail.GetSubsiLedgerIdByName(query);
            //    FidList = FidList.Where(x => FidList.Contains(FId)).ToList<int>();
            //    List<int> final = new List<int>();
            //    //final.Add(FidList.Select(item=>item==FId))
            //    foreach (var item in FidList)
            //    {
            //        if (item == FId)
            //            final.Add(item);
            //    }

            //    //FidList = FidList.RemoveAll(item=>FidList.Contains(FId));
            //    //FidList= FidList.Where(x =>==FId).ToList<int>();
            //    list = list.Where(x => final.Contains(x.FId));
            //    ViewBag.Query = query;
            //}
            //else if (string.IsNullOrEmpty(query) && FId != 0)
            //{
            //    //list=list.Where(m=>m.FId)         
            //    list = list.Where(x => x.FId == FId);
            //    ViewBag.Query = query;
            //}
            //else
            //{
            //    list = new List<Models.Models.SubsiDetail>().AsQueryable();
            //    ViewBag.Query = query;
            //}
            //list = list.OrderByDescending(x => x.FId);
            //var pagedList = new Pagination<Models.Models.SubsiDetail>(list, page ?? 0, pageSize);
            //ViewBag.CountPager = new Pagination<Models.Models.SubsiDetail>(list, page ?? 0, pageSize).TotalPages;
            var pagedList = subsiDetail.GetSubsiDetailList(Convert.ToInt32( page), pageSize, Check).AsEnumerable();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
           // ViewBag.Address = SubsiDetail.GetAddress(parentId);

        }

        public ActionResult GenerateLedgerList(int custId)
        {
            Models.Models.SubsiDetail subsi = new Models.Models.SubsiDetail();
            subsi.CId = custId;
            //var ledgerList = SubsiDetail.GetSubsiLedger(custId);
            return PartialView(subsi);
        }
        public JsonResult GenerateAccNumber(int ledgerId)
        {
            var accNo = subsiDetail.GenerateAccountNumber(ledgerId);
            return Json(accNo, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Create(int SubsiDetailId = 0,int customerId=0,int ledgerId=0)
        {
            if (Request.IsAjaxRequest())
            {
                Models.Models.SubsiDetail SubsiDetailDTO = new Models.Models.SubsiDetail();
                if (SubsiDetailId != 0 && SubsiDetailId!=-1)
                {

                    SubsiDetailDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiDetail>().GetSingle(x => x.SDID == SubsiDetailId);
                    ViewBag.CustomerName = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.CustIndividual>().GetSingle(x => x.CID == SubsiDetailDTO.CId).Fname;

                }
                if(SubsiDetailId==-1)
                {
                    
                    SubsiDetailDTO.CId = customerId;
                    SubsiDetailDTO.FId = ledgerId;
                    ViewBag.CustomerName = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.CustIndividual>().GetSingle(x =>x.CID==customerId).Fname;
                }


                return PartialView(SubsiDetailDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetCustomerDetails(string query)
        {
            
            int page = 1;
            const int pageSize = 12;
            var pagedList = subsiDetail.getCustList(page,pageSize , "").ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }
        // added for pagination
        public ActionResult SearchCustomerDetails(string query = null, int? fId = 0, int page = 1)
        { 
            const int pageSize = 12;
            var pagedList = subsiDetail.getCustList(page, pageSize, query).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);

        }

        public ActionResult GetEmployeeDetails(string query)
        {

            int page = 1;
            int pageSize = 12;
            var pagedList = subsiDetail.getempList(page, pageSize, "").ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }
        public ActionResult SearchEmployeeDetails(string query, int page = 1)
        {
            
            const int pageSize = 12;
            var pagedList = subsiDetail.getempList(page, pageSize, query).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
           
            return PartialView(pagedList);
        }

        public ActionResult GetUserDetails(string query)
        {

            int? page = 1;
            var list = new Loader.Repository.GenericUnitOfWork().Repository<Loader.Models.ApplicationUser>().GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
                ViewBag.Query = query;
            }
            const int pageSize = 12;
            var dataList = list.OrderBy(m => m.UserName);

            var pagedList = new Loader.ViewModel.Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }
        public ActionResult SearchUserDetails(string query, int? page = 1)
        {
            var list = new Loader.Repository.GenericUnitOfWork().Repository<Loader.Models.ApplicationUser>().GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
                ViewBag.Query = query;
            }
            const int pageSize = 12;
            var dataList = list.OrderBy(m => m.UserName);

            var pagedList = new Loader.ViewModel.Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Loader.Models.ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }

        public ActionResult EditSubsiDetailDropDown(int ledgerId, int customerId)
        {
            //var accDetails = from m in uow.Repository<Models.Models.SubsiVSFId>().FindBy(x => x.Fid == ledgerNo)
            //                 join n in uow.Repository<Models.Models.SubsiSetup>().GetAll()
            //                 on m.SSId equals n.SSId
            //                 select new Models.Models.SubsiSetup
            //                 {
            //                     Length = n.Length,
            //                     Prefix = n.Prefix

            //                 };
            //var initialNumber = 1;
            //var accNumber = "0";
            //if (accDetails.Count() > 0)
            //{
            //    accNumber = accDetails.FirstOrDefault().Prefix + initialNumber.ToString().PadLeft(Convert.ToInt32(accDetails.SingleOrDefault().Length), '0');
            //}
            //return accNumber;
            //var accNumber = accDetails.Select(c => new { c.Length, c.Prefix = c.Length + " " + c.Prefix });

            var checkifSubsiExists = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiDetail>().FindBy(x => x.CId == customerId && x.FId == ledgerId);
            if (checkifSubsiExists.Count() > 0)
            {
                return RedirectToAction("Create",new { SubsiDetailId = checkifSubsiExists.FirstOrDefault().SDID });
            }

            return RedirectToAction("Create", new { SubsiDetailId = -1,customerId=customerId,ledgerId=ledgerId });
        }

        //public ActionResult Details(int id = 0)
        //{
        //    Models.Models.ProductDetail Models.Models.ProductDetail = SubsiDetail.GetSingle(id);
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
        public ActionResult Create(Models.Models.SubsiDetail SubsiDetail)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    if (SubsiDetail.SDID == 0)
                    {
                        subsiDetail.Save(SubsiDetail);
                        return RedirectToAction("Create", new { SubsiDetailId = 0 });
                    }
                    else
                    {
                        subsiDetail.Save(SubsiDetail);
                        //return RedirectToAction("Create", new { SubsiDetailId = SubsiDetail.SDID });
                        return RedirectToAction("Index");
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




        //[HttpPost]
        //public ActionResult _GetProductDetailTreePopup(Loader.ViewModel.TreeViewParam param)
        //{

        //    ViewData["param"] = param;

        //    var filter = param.Filter == null ? "" : param.Filter;

        //  Models.ViewModel.TreeView tree = newModels.ViewModels.TreeView();

        //    if (param.WithOutMe == 0)
        //    {
        //        if (param.Title == "Select SubsiDetail")
        //        {
        //            tree = SubsiDetail.GetSubsiDetailGroupTree(filter);
        //        }
        //        else if (param.Title == "Select Designation")
        //        {
        //            tree = SubsiDetail.GetDesignationGroupTree(filter);
        //        }

        //    }
        //    else
        //    {
        //        // tree = SubsiDetail.GetModels.Models.ProductDetailGroupTree(param.WithOutMe, filter);
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
            //    if (param.Title == "Select SubsiDetail")
            //    {
            //        tree = SubsiDetail.GetSubsiDetailGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = SubsiDetail.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.ProductDetail Models.Models.ProductDetail = SubsiDetail.GetSingle(id);
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
        //        SubsiDetail.Save(Models.Models.ProductDetail);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.ProductDetail);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Models.Models.SubsiDetail ProductDetail = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiDetail>().GetSingle(x => x.SDID == id);
            bool deleteConfirm = true;
            if (subsiDetail.MappedCount(id) > 0)
                deleteConfirm = false;
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.ProductDetail);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int SubsiDetailId)
        {
            subsiDetail.Delete(SubsiDetailId);
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