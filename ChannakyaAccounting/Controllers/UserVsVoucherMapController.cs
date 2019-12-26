using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Service.Subsi_Setup;
using ChannakyaAccounting.Service.Voucher1;
using Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaAccounting.Controllers
{
    [MyAuthorize]
    public class UserVsVoucherMapController : Controller
    {
        Voucher1Service forconversion = new Voucher1Service();
        ReturnBaseMessageModel returnmessage = null;
        SubsiSetupService SubsiSetupService = new SubsiSetupService();

        private Service.VoucherMap.VoucherMapService VoucherMap;
        public UserVsVoucherMapController()
        {
          //  var companyName = Loader.ViewModel.en
            VoucherMap = new Service.VoucherMap.VoucherMapService();
        }
        // GET: UserVsVoucherMap
        public ActionResult Index()
        {
            return View();
        }

        #region 
        public ActionResult GetUserDetails(string query)
        {

            int? page = 1;
            var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.User>().GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
                ViewBag.Query = query;
            }
             int pageSize = 12;
            var dataList = list.OrderBy(m => m.UserName);

            var pagedList = new Loader.ViewModel.Pagination<Models.Models.User>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new ChannakyaAccounting.Models.ViewModel.Pagination<Models.Models.User>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);

        }

        public ActionResult UsertoVoucherMap(int uservsvoucherId=0, int vtype=0,int FDate=0)
        {
            uservsvoucherViewModel listofbatch = new uservsvoucherViewModel();
            if (vtype != 0)
            {
                listofbatch = VoucherMap.GetEditItem(uservsvoucherId,vtype,FDate);
                return PartialView(listofbatch);
            }
            listofbatch = VoucherMap.BatchList();
            return PartialView(listofbatch);
        }

        public ActionResult CreateUsertoVoucherMap(uservsvoucherViewModel data)
        {
          
            try
            {
                if (data.Vtype == 0)
                {
                    throw new Exception("Please Select Voucher Type");
                }
                if (data.userid == 0)
                {
                    throw new Exception("Please Select User Name");
                }
                int count = 0;
                if (data.BatchList != null)
                {

                    count = data.BatchList.Where(x => x.IsSelected).Count();
                }
                else
                {
                    throw new Exception("No Batch Is Mapped");
                }
               
                if (count == 0)
                {
                    throw new Exception("Please Select Batch");
                }

                returnmessage = VoucherMap.createUsertoVoucherMappng(data);
                return RedirectToAction("_DetailsOfVoucher");
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
        }

        public ActionResult _DetailsOfVoucher(int ViewType = 1, string query = null, int? page = 1)
        {
            usertovoucherreturnViewModel list = VoucherMap.GetUsersAndVoucherList();
            ViewBag.ViewTYpe = ViewType;
            ViewBag.ActivePager = page;
            return PartialView("_Details", list);
            //ViewBag.Address = Duration.GetAddress(parentId);

        }

        public ActionResult GetUserToVoucherList()
        {
            ViewBag.ViewType = 1;
            int page = 1;
             int pageSize = 12;
            usertovoucherreturnViewModel retrunuserList = VoucherMap.GetUsersAndVoucherList();
            var pagedList = new Pagination<Models.ViewModel.usertovoucherreturnViewModel>(retrunuserList.voucherreturnViewModel.AsQueryable(), page, pageSize);
            ViewBag.CountPager = new Pagination<Models.ViewModel.usertovoucherreturnViewModel>(retrunuserList.voucherreturnViewModel.AsQueryable(), page, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            retrunuserList.voucherreturnViewModel = pagedList;
            return PartialView(retrunuserList);
        }

        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1)
        {
            int type = ViewType;
            ViewBag.ViewType = type;
             int pageSize = 12;
            usertovoucherreturnViewModel retrunuserList = VoucherMap.GetUsersAndVoucherList();
            if (query != null && query != "")
            {
                Voucher1Service userid = new Voucher1Service();
               int[] Check= userid.GetIdFromUserNameforUserVsVoucher(query);
                retrunuserList.voucherreturnViewModel= retrunuserList.voucherreturnViewModel.Where(x => Check.Contains(x.userid)).ToList();
            }
            var pagedList = new Pagination<Models.ViewModel.usertovoucherreturnViewModel>(retrunuserList.voucherreturnViewModel.AsQueryable(), page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Models.ViewModel.usertovoucherreturnViewModel>(retrunuserList.voucherreturnViewModel.AsQueryable(), page ?? 0, pageSize).TotalPages;
            ViewBag.ActivePager = page;
            retrunuserList.voucherreturnViewModel = pagedList;
            return PartialView(retrunuserList);

        }
        [HttpGet]

        public JsonResult DeleteVoucher(int id = 0,int Vtype=0,int Btype = 0)
        {
            //Models.Models.UserVSVoucher Duration = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.UserVSVoucher>().GetSingle(x => x.ID == id );
            bool Status=VoucherMap.CheckMapping(id, Vtype,Btype);
            bool deleteConfirm;
            if (Status == true)
            {
                 deleteConfirm = true;
            }
            else
            {
              deleteConfirm = false;
            }
            
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.Duration);
        }

        [HttpPost]
        public ActionResult VoucherDeleteConfirm(int userId, int Vtype)
        {
            VoucherMap.DeleteUsertoVoucherMap(userId, Vtype);
            return RedirectToAction("GetUserToVoucherList");
        }

        public ActionResult SelectBatchFromVtype(int Vtype,int userID=0)
        {
            uservsvoucherViewModel batchlist = new uservsvoucherViewModel();
            batchlist = VoucherMap.BatchListFromVtype(Vtype,userID);
            int counts = batchlist.BatchList.Count();
            if (counts>0)
            {
                return PartialView(batchlist);

            }
            else
            {
                return Json(new
                {
                    message = "no data",
                    AjaxReturn = PartialView(batchlist)
                });
            }
            
        }
        #endregion

        #region DemoEdit
        public ActionResult Edit(List<Batch> batchList)
        {
            uservsvoucherViewModel listofbatch = new uservsvoucherViewModel();
            listofbatch = VoucherMap.BatchList(); //Ths is total List
            return PartialView();
        }
        #endregion

    }
}