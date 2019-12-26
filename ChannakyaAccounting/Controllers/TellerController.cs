using ChannakyaBase.BLL;
using ChannakyaBase.BLL.Service;
using ChannakyaBase.Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaAccounting.Controllers
{
    public class TellerController : Controller
    {
        TellerService tellerService = null;
        private CommonService commonService = null;
        public TellerController()
        {
            tellerService = new TellerService();
            commonService = new CommonService();
        }
        // GET: Teller

        #region Account Search


        public ActionResult AccountNumberSearch(string accountNumber = "", string filterAccount = "", string accountType = "")
        {
            AccountSearchViewModel aAccountSearch = new AccountSearchViewModel();
            AccountDetailsViewModel code = new AccountDetailsViewModel();
            if (accountNumber != "")
            {
                code = tellerService.GetCodeByAccountNumber(accountNumber);
                if (code != null)
                {

                    aAccountSearch.ProductCode = TellerUtilityService.GetProductCode(code.PID);
                }
                else
                {
                    code = new AccountDetailsViewModel();
                    code.BrchID = commonService.GetBranchIdByEmployeeUserId(Loader.Models.Global.UserId);
                    code.CurrID = 82;
                }

            }
            else
            {
                code.BrchID = commonService.GetBranchIdByEmployeeUserId(Loader.Models.Global.UserId);
                code.CurrID = 82;
            }
            aAccountSearch.FilterAction = filterAccount;
            aAccountSearch.AccountType = accountType;
            aAccountSearch.BranchId = Convert.ToInt32(code.BrchID);
            aAccountSearch.CurrencyId = code.CurrID;
            var accountList = tellerService.GetAccountNumberList(aAccountSearch.BranchId, code.PID, aAccountSearch.CurrencyId, "", filterAccount, accountType, 1, 10);
            aAccountSearch.AccountList = new StaticPagedList<AccountSearchViewModel>(accountList, 1, 10, (accountList.Count == 0) ? 0 : accountList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(aAccountSearch);
        }
        public ActionResult _AccountNumberSearch(int branchCode = 0, int productCode = 0, int currencyCode = 0, string customerName = "", string filterAccount = "", string accountType = "", int pageNo = 1, int pageSize = 10)
        {

            AccountSearchViewModel aAccountSearch = new AccountSearchViewModel();
            var accountList = tellerService.GetAccountNumberList(branchCode, productCode, currencyCode, customerName, filterAccount, accountType, pageNo, pageSize);
            aAccountSearch.AccountList = new StaticPagedList<AccountSearchViewModel>(accountList, pageNo, pageSize, (accountList.Count == 0) ? 0 : accountList.Select(x => x.TotalCount).FirstOrDefault());
            //aAccountSearch.BranchId = branchCode;
            //aAccountSearch.CurrencyId = currencyCode;
            return PartialView(aAccountSearch);
        }
        public ActionResult GetSelectAccountNumber(Int64 accountNumber)
        {
            var singleAccountNumber = tellerService.GetSelectAccountNumber(accountNumber);
            return Json(singleAccountNumber, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccountNumber(string accountNumber)
        {
            var manyAccountNumber = tellerService.GetAccountNumber(accountNumber);
            return Json(manyAccountNumber, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductList(string productCode, int pageNo = 1, int pageSize = 5)
        {
            ProductViewModel pModel = new ProductViewModel();
            pModel.ProductList = tellerService.GetAllProductList(productCode, pageNo, pageSize);
            return PartialView(pModel);

        }
        public ActionResult _ProductList(string procuctName, int pageNo = 1, int pageSize = 5)
        {
            ProductViewModel pModel = new ProductViewModel();
            pModel.ProductList = tellerService.GetAllProductListByName(procuctName, pageNo, pageSize);
            return PartialView(pModel);
        }

        public ActionResult GetProductByCode(string productCode)
        {
            var productList = tellerService.GetAllProductList(productCode);
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSelectProduct(int productId)
        {
            ProductViewModel pViewMpdel = new ProductViewModel();
            pViewMpdel = tellerService.GetSingleProduct(productId);
            if (pViewMpdel.IntraBrnhTrnx == false)
            {
                pViewMpdel.BranchList = TellerUtilityService.CurrentBranch(commonService.GetBranchIdByEmployeeUserId(Loader.Models.Global.UserId));
            }
            else
            {
                pViewMpdel.BranchList = TellerUtilityService.GetBranchList();
            }
            return Json(pViewMpdel, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetSelectCurrency(int currencyId)
        //{
        //    return Json(tellerService.GetSingleCurrency(currencyId), JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult CurrencyList(string currencyCode = "")
        //{
        //    CurrencyModel currencyModel = new CurrencyModel();
        //    currencyModel.CurrencyList = tellerService.GetAllCurrencyList(currencyCode, "");
        //    return PartialView(currencyModel);
        //}
        //public ActionResult _CurrencyList(string currencyName)
        //{
        //    CurrencyModel currencyModel = new CurrencyModel();
        //    currencyModel.CurrencyList = tellerService.GetAllCurrencyList("", currencyName);
        //    return PartialView(currencyModel);
        //}
        //public ActionResult GetCurrencyByCode(string currencyCode)
        //{
        //    var productList = tellerService.GetAllCurrencyList(currencyCode);
        //    return Json(productList, JsonRequestBehavior.AllowGet);
        //}


        #endregion
    }
}