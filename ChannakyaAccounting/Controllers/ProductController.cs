using ChannakyaAccounting.Models.Models;
using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using Loader;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaAccounting.Controllers
{
    [MyAuthorize]
    public class ProductController : Controller
    {
        private Service.DepositProduct.DepositProductService DepositProduct = null;
        private Service.FinAcc.FinAccService FinAccService = null;

        public ProductController()
        {
            DepositProduct = new Service.DepositProduct.DepositProductService();
            FinAccService = new Service.FinAcc.FinAccService();
        }

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                int pageSize = 12;
                int page = 1;
                ViewBag.ViewType = 1;
                //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductDetail>().GetAll().AsQueryable();
                //var pagedList = new Pagination<Models.Models.ProductDetail>(list, page, pageSize);
                var pagedList = DepositProduct.GetProductDetailList(page, pageSize, "");
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

        public ActionResult AddDepositProduct(int schemeId)
        {
            Models.Models.SchmDetail schemeDetails = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().FindBy(x => x.SDID == schemeId).FirstOrDefault();
            Models.Models.ProductDetail modelproduct = new Models.Models.ProductDetail();
            modelproduct.SchmDetail = schemeDetails;
            modelproduct.SchemeofProduct = schemeDetails;

            return PartialView(modelproduct);
        }

        public ActionResult _Details(int ViewType = 1, string query = null, int? page = 1)
        {
            int pageSize = 12;
            ViewBag.ViewTYpe = ViewType;
            var pagedList = DepositProduct.GetProductDetailList(Convert.ToInt32(page), pageSize, query);
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //ViewBag.Address = DepositProduct.GetAddress(parentId);

        }

        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1)
        {
            int pageSize = 12;
            ViewBag.ViewTYpe = ViewType;
            var pagedList = DepositProduct.GetProductDetailList(Convert.ToInt32(page), pageSize, query);
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((pagedList.FirstOrDefault().totalCount) / (pageSize * 1.0));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //ViewBag.Address = DepositProduct.GetAddress(parentId);

        }

        //public ActionResult Details(int id = 0)
        //{
        //    Models.Models.ProductDetail Models.Models.ProductDetail = DepositProduct.GetSingle(id);
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

        [HttpGet]
        public ActionResult Create(string activeText, int ProductId = 0)
        {

            #region CreateGet
            if (Request.IsAjaxRequest())
            {
                Models.Models.ProductDetail ProductDTO = new Models.Models.ProductDetail();

                if (ProductId != 0)
                {
                    #region WithProduct

                    var finaccId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductDetail>().GetSingle(x => x.PID == ProductId).FID;
                    ProductDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductDetail>().GetSingle(x => x.PID == ProductId);

                    var finaccDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == finaccId);

                    ViewBag.ActiveId = ProductId;
                    var f2Id = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == finaccId).F2Type;
                    ViewBag.IsGroup = IsFinAccGroup(f2Id);
                    var schemeDetail = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == ProductDTO.SDID);
                    ProductDTO.SchmDetail = schemeDetail;
                    ProductDTO.SchemeofProduct = schemeDetail;
                    ViewBag.Alias = ProductDTO.FinAcc.Alias;

                    var productDetails = ProductDTO;

                    if (productDetails != null)
                    {
                        var totalDuration = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetAll().ToList();
                        var totalBasis = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DepositBasi>().GetAll().ToList();
                        var ProductDurationIntList = new GenericUnitOfWork().Repository<ProductDurationInt>().FindBy(x => x.PId == productDetails.PID).DistinctBy(x => x.Value).ToList();

                        var startACNo = new GenericUnitOfWork().Repository<Models.Models.ProductBrnch>().GetSingle(x => x.PID == productDetails.PID).StartAcNo;

                        ProductDTO.StartAcNo = Convert.ToInt32(startACNo);


                        if (productDetails.ProductDurationInts.Count() > 0 && productDetails.ProductPSIDs.Count() == 1)
                        {

                            ProductDTO.InterestValueList = new List<InterestValue>();
                            var valueList = productDetails.ProductDurationInts.DistinctBy(x => x.Value).ToList();

                            if (productDetails.ProductDurationInts.First().DBId != null)
                            {
                                foreach (var item in valueList)
                                {
                                    InterestValue interestValue = new InterestValue();
                                    interestValue.Amount = Convert.ToString(item.Value);
                                    interestValue.PolicyIntCalcId = productDetails.ProductPSIDs.First().PSID;
                                    foreach (var subitem in totalDuration)
                                    {
                                        Duration duration = new Duration();
                                        duration.Duration1 = subitem.Duration1;
                                        duration.Id = subitem.Id;

                                        foreach (var item2 in totalBasis)
                                        {
                                            DepositBasi depositBasis = new DepositBasi();
                                            depositBasis.Id = item2.Id;
                                            depositBasis.Name = item2.Name;
                                            depositBasis.ProductDurationInt = new ProductDurationInt();
                                            var interestRate = new GenericUnitOfWork().Repository<ProductDurationInt>().FindBy(x => x.Value == item.Value).Where(x => x.DBId == item2.Id && x.DVId == subitem.Id && x.PId == item.PId && x.Value == item.Value).ToList();
                                            if (interestRate.Count() > 0)
                                            {
                                                depositBasis.ProductDurationInt.ICBId = interestRate.FirstOrDefault().ICBId;
                                                depositBasis.ProductDurationInt.InterestRate = interestRate.FirstOrDefault().InterestRate;
                                            }
                                            else
                                            {
                                                depositBasis.ProductDurationInt.InterestRate = 0;
                                            }
                                            duration.DepositBasisList.Add(depositBasis);
                                        }
                                        interestValue.DurationList.Add(duration);
                                    }
                                    ProductDTO.InterestValueList.Add(interestValue);
                                }
                            }
                            else
                            {
                                //This is Fixed Deposit Product

                                FixedDepositList fdList = new FixedDepositList();
                                fdList.PolicyIntCalcId = productDetails.ProductPSIDs.First().PSID;

                                var durationList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetAll().ToList();
                                foreach (var subitem in durationList)
                                {
                                    var captList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.RuleICBDuration>().GetAll();

                                    foreach (var subitem1 in captList)
                                    {
                                        subitem1.ProductDurationInt = new ProductDurationInt();

                                        var doesExists = productDetails.ProductDurationInts.Where(x => x.ICBId == Convert.ToInt32(subitem1.ICBDurID) && x.DVId == subitem.Id && x.PId == productDetails.PID).Count();
                                        if (doesExists > 0)
                                        {
                                            var elementinLoan = productDetails.ProductDurationInts.Where(x => x.ICBId == Convert.ToInt32(subitem1.ICBDurID) && x.DVId == subitem.Id && x.PId == productDetails.PID).Single();
                                            ProductDurationInt product = new ProductDurationInt();
                                            product.DVId = subitem.Id;
                                            product.InterestRate = elementinLoan.InterestRate;
                                            product.ICBId = subitem1.ICBDurID;
                                            product.PId = productDetails.PID;

                                            subitem1.ProductDurationInt = product;
                                        }

                                        subitem.CaptList.Add(subitem1);



                                    }
                                    fdList.DurationList.Add(subitem);
                                }

                                ProductDTO.FixedDepositList = fdList;

                            }


                        }

                        if (productDetails.ProductICBDurs.Count() > 0 && productDetails.ProductPSIDs.Count() > 0)
                        {
                            var totalRuleIcb = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.RuleICBDuration>().GetAll();
                            var totalRulePsid = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.PolicyIntCalc>().GetAll();
                            //This is Normal Deposit Product Without Duration
                            NormalDepositList normalDeposit = new NormalDepositList();
                            foreach (var item in totalRuleIcb)
                            {
                                RuleICBDuration ruleIcb = new RuleICBDuration();
                                var ruleList = productDetails.ProductICBDurs.Where(x => x.ICBDurID == item.ICBDurID && x.PID == productDetails.PID);
                                if (ruleList.Count() > 0)
                                {
                                    ruleIcb.ICBDurID = item.ICBDurID;
                                    ruleIcb.ICBDurNam = item.ICBDurNam;
                                    ruleIcb.IsChecked = true;
                                    ruleIcb.IsDefault = ruleList.First().IsDefault;
                                    ruleIcb.InterestRateICB = Convert.ToDouble(DepositProduct.getInterestFromICBDurID(item.ICBDurID,ProductDTO.PID));
                                }
                                else
                                {
                                    ruleIcb.ICBDurID = item.ICBDurID;
                                    ruleIcb.ICBDurNam = item.ICBDurNam;
                                    ruleIcb.IsChecked = false;
                                    ruleIcb.IsDefault = false;
                                    ruleIcb.InterestRateICB = 0;
                                }
                                normalDeposit.CapitalizationRuleList.Add(ruleIcb);
                            }
                            foreach (var item in totalRulePsid)
                            {
                                PolicyIntCalc pentIcb = new PolicyIntCalc();
                                var psRule = productDetails.ProductPSIDs.Where(x => x.PSID == item.PSID && x.PID == productDetails.PID);
                                if (psRule.Count() > 0)
                                {
                                    pentIcb.PSName = item.PSName;
                                    pentIcb.IsChecked = true;
                                    pentIcb.IsDefault = Convert.ToBoolean(psRule.FirstOrDefault().IsDefault);
                                    pentIcb.PSID = item.PSID;
                                }
                                else
                                {
                                    pentIcb.PSName = item.PSName;
                                    pentIcb.IsChecked = false;
                                    pentIcb.IsDefault = false;
                                    pentIcb.PSID = item.PSID;
                                }
                                normalDeposit.CalculationRuleList.Add(pentIcb);
                            }
                            ProductDTO.NormalDepositList = normalDeposit;
                        }

                        if (productDetails.ProductPCIDs.Count() > 0 && productDetails.ProductPSIDs.Count() > 0)
                        {
                            LoanInterestList loanInterest = new LoanInterestList();

                            //This is Loan Deposit Product
                            var totalPenalty = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.PolicyPenCalc>().GetAll();
                            var totalRulePay = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.RulePay>().GetAll();
                            var totalPolicyCalc = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.PolicyIntCalc>().GetAll();
                            foreach (var item in totalPenalty)
                            {
                                PolicyPenCalc policyPent = new PolicyPenCalc();
                                var policyModel = productDetails.ProductPCIDs.Where(x => x.PCID == item.PCID && x.PID == productDetails.PID);
                                if (policyModel.Count() > 0)
                                {
                                    policyPent.PCID = item.PCID;
                                    policyPent.PCNAME = item.PCNAME;
                                    policyPent.IsChecked = true;
                                    policyPent.IsDefault = Convert.ToBoolean(policyModel.First().IsDefault);
                                    loanInterest.PolicyPenaltyList.Add(policyPent);
                                }
                                else
                                {
                                    policyPent.PCID = item.PCID;
                                    policyPent.PCNAME = item.PCNAME;
                                    policyPent.IsChecked = false;
                                    policyPent.IsDefault = false;
                                    loanInterest.PolicyPenaltyList.Add(policyPent);
                                }


                            }

                            //var totalRulePay = from m in totalRulePay join n in productDetails.ProductPays on m.PAYID equals n.PAYID join o in productDetails.PID on n.PID equals o select new ProductPay;

                            foreach (var item in totalRulePay)
                            {
                                RulePay rulePay = new RulePay();
                                var rulePayList = productDetails.ProductPays.Where(x => x.PAYID == item.PAYID && x.PID == productDetails.PID);
                                if (rulePayList.Count() > 0)
                                {
                                    rulePay.PAYID = item.PAYID;
                                    rulePay.PRule = item.PRule;
                                    rulePay.IsChecked = true;
                                    rulePay.IsDefault = Convert.ToBoolean(rulePayList.First().IsDefault);

                                }
                                else
                                {
                                    rulePay.PAYID = item.PAYID;
                                    rulePay.PRule = item.PRule;
                                    rulePay.IsChecked = false;
                                    rulePay.IsDefault = false;

                                }

                                loanInterest.RulePayList.Add(rulePay);
                            }
                            foreach (var item in totalPolicyCalc)
                            {
                                PolicyIntCalc policyIntCalc = new PolicyIntCalc();
                                var policyModel = productDetails.ProductPSIDs.Where(x => x.PSID == item.PSID && x.PID == productDetails.PID);
                                if (policyModel.Count() > 0)
                                {
                                    policyIntCalc.PSID = item.PSID;
                                    policyIntCalc.PSName = item.PSName;
                                    policyIntCalc.IsDefault = Convert.ToBoolean(policyModel.First().IsDefault);
                                    policyIntCalc.IsChecked = true;
                                }
                                else
                                {
                                    policyIntCalc.PSID = item.PSID; ;
                                    policyIntCalc.PSName = item.PSName;
                                    policyIntCalc.IsDefault = false;
                                    policyIntCalc.IsChecked = false;
                                }
                                loanInterest.PolicyCalculationList.Add(policyIntCalc);

                            }
                            ProductDTO.LoanInterestList = loanInterest;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region WithoutProduct

                    var schemeDetails = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<SchmDetail>().GetSingle(x => x.FID == ProductDTO.FID);
                    if (schemeDetails != null)
                    {
                        SchmDetail schmDetail = new SchmDetail();
                        schmDetail = schemeDetails;
                        ProductDTO.SchmDetail = schmDetail;
                    }
                    #endregion
                }

                return View(ProductDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }



            #endregion
        }

        public bool IsFinAccGroup(int ledgerId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == ledgerId).FinSys1.IsGroup;
            return isGroup;
        }
        public JsonResult IsPrefixExists(string Apfix, int? PID)
        {
            var prefix = DepositProduct.IsPrefixExists(Apfix, PID);
            if (prefix == false)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddInterestValue()
        {
            return PartialView("_AddInterestValue");
        }

        public ActionResult GetSchemeHasDurationFromPID(int ProductId)
        {
            var hasDuration = DepositProduct.GetSchemeHasDurationFromPID(ProductId);
            return Json(hasDuration, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSchemeMultiDepFromPID(int ProductId)
        {
            var hasMultiDep = DepositProduct.GetSchemeMultiDepFromPID(ProductId);
            return Json(hasMultiDep, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInterestValueFromPID(int ProductId)
        {
            var interestVal = DepositProduct.GetInterestValueFromPID(ProductId);
            return Json(interestVal, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult CheckExistingProductName(string PName, int? PID)
        {
            bool ifProductNameExists = false;
            try
            {
                ifProductNameExists = DepositProduct.CheckProductName(PName, PID);
                return Json(!ifProductNameExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Create(Models.Models.ProductDetail ProductDetail, String Alias, HttpPostedFileBase file, List<InterestValue> ProductInterest, FixedDepositList fixedDepositList, NormalDepositList normalDepositList, LoanInterestList loanInterestList)
        {
            try
            {
                if (ProductDetail.PID == 0)
                {
                    ProductDetail.InterestValueList = new List<InterestValue>();
                    ProductDetail.FixedDepositList = new FixedDepositList();
                    ProductDetail.NormalDepositList = new NormalDepositList();
                    ProductDetail.LoanInterestList = new LoanInterestList();

                    ProductDetail.InterestValueList = ProductInterest;
                    ProductDetail.FixedDepositList = fixedDepositList;
                    ProductDetail.NormalDepositList = normalDepositList;
                    ProductDetail.LoanInterestList = loanInterestList;

                    #region Validation

                    #region RecurringValidation
                    int isallValidated = 0;
                    int isWithInterestRate = 0;
                    if (ProductInterest != null && ProductInterest.Count() > 0)
                    {
                        if (ProductInterest.FirstOrDefault().PolicyIntCalcId == 0)
                        {
                            throw new Exception("Please Select Policy Calculation");
                        }
                        foreach (var item in ProductInterest)
                        {

                            var allDurationList = item.DurationList.ToList();
                            foreach (var subitem in allDurationList)
                            {
                                var allDepositBasisList = subitem.DepositBasisList.ToList();
                                foreach (var subitem1 in allDepositBasisList)
                                {
                                    var interestRateObj = subitem1.ProductDurationInt;
                                    if (interestRateObj.ICBId != null && interestRateObj.InterestRate == null)
                                    {
                                        isallValidated++;
                                    }
                                    if (interestRateObj.ICBId == null && interestRateObj.InterestRate != null)
                                    {
                                        isWithInterestRate++;
                                    }
                                }
                            }

                        }
                        if (isallValidated > 0)
                        {
                            throw new Exception("Please Enter Interest Rate");
                        }
                        if (isWithInterestRate > 0)
                        {
                            throw new Exception("Please Select Calculation Rule");
                        }
                    }
                    #endregion

                    #region FixedDepositValidation
                    if (fixedDepositList.DurationList.Count() > 0)
                    {
                        if (fixedDepositList.PolicyIntCalcId == 0)
                        {
                            throw new Exception("Please Select Calculation Rule");
                        }
                        else
                        {
                            int hasInterestRateDefined = 0;
                            var allInterestRate = fixedDepositList.DurationList.Select(x => x.CaptList).ToList();
                            foreach (var item in allInterestRate)
                            {
                                var interestRate = item.Select(x => x.ProductDurationInt.InterestRate).FirstOrDefault();
                                if (interestRate != null)
                                {
                                    if (interestRate != 0)
                                    {
                                        hasInterestRateDefined++;
                                    }
                                }
                            }

                            if (hasInterestRateDefined == 0)
                            {
                                throw new Exception("Please Enter Interest Rate For At Least Duration");
                            }

                        }

                    }
                    else
                    {
                        ProductDetail.FixedDepositList = fixedDepositList;
                    }
                    #endregion

                    #region NormalDepositValidation

                    if (normalDepositList != null && normalDepositList.CalculationRuleList.Count() > 0 && normalDepositList.CapitalizationRuleList.Count() > 0)
                    {
                        if (normalDepositList.CalculationRuleList.Where(x => x.IsChecked == true).ToList().Count() == 0)
                        {
                            throw new Exception("Please Select Calculation Rule List");
                        }
                        if (normalDepositList.CalculationRuleList.Where(x => x.IsDefault == true).ToList().Count() == 0 || normalDepositList.CalculationRuleList.Where(x => x.IsDefault == true).ToList().Count() > 1)
                        {
                            throw new Exception("Please Select Default Value For Calculation Rule List");
                        }
                        if (normalDepositList.CapitalizationRuleList.Where(x => x.IsChecked == true).ToList().Count() == 0)
                        {
                            throw new Exception("Please Select Capitalization Rule List Rule List");
                        }
                        if (normalDepositList.CapitalizationRuleList.Where(x => x.IsDefault == true).ToList().Count() == 0 || normalDepositList.CapitalizationRuleList.Where(x => x.IsDefault == true).ToList().Count() > 1)
                        {
                            throw new Exception("Please Select Default Value For Captitalization Rule List");
                        }
                        else
                        {
                            ProductDetail.NormalDepositList = normalDepositList;
                        }
                    }
                    #endregion

                    #region LoanInterestValidation

                    if (loanInterestList != null && loanInterestList.PolicyCalculationList.Count() > 0 && loanInterestList.PolicyPenaltyList.Count() > 0 && loanInterestList.RulePayList.Count() > 0)
                    {
                        if (loanInterestList.PolicyCalculationList.Where(x => x.IsChecked == true).ToList().Count() == 0)
                        {
                            throw new Exception("Please Select Calculation Rule");
                        }
                        if (loanInterestList.PolicyPenaltyList.Where(x => x.IsChecked == true).ToList().Count() == 0)
                        {
                            throw new Exception("Please Select Penalty Rule");
                        }
                        if (loanInterestList.RulePayList.Where(x => x.IsChecked == true).ToList().Count() == 0)
                        {
                            throw new Exception("Please Select Payment Rule");
                        }
                        if (loanInterestList.PolicyCalculationList.Where(x => x.IsDefault == true).ToList().Count() == 0 || loanInterestList.PolicyCalculationList.Where(x => x.IsDefault == true).ToList().Count() > 1)
                        {
                            throw new Exception("Please Select Default Value For Calculation Rule ");
                        }
                        if (loanInterestList.PolicyPenaltyList.Where(x => x.IsDefault == true).ToList().Count() == 0 || loanInterestList.PolicyPenaltyList.Where(x => x.IsDefault == true).ToList().Count() > 1)
                        {
                            throw new Exception("Please Select Default Value For Penalty Rule ");
                        }
                        if (loanInterestList.RulePayList.Where(x => x.IsDefault == true).ToList().Count() == 0 || loanInterestList.RulePayList.Where(x => x.IsDefault == true).ToList().Count() > 1)
                        {
                            throw new Exception("Please Select Default Value For Payment Rule ");
                        }
                        else
                        {
                            ProductDetail.LoanInterestList = loanInterestList;
                        }
                    }
                    #endregion

                    #endregion

                    DepositProduct.Save(ProductDetail,Alias);
                    return RedirectToAction("Create", new { ProductDetail = 0 });
                }
                else
                {
                    DepositProduct.Save(ProductDetail, Alias);
                    return RedirectToAction("Create", new { ProductId = 0 });
                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
            //return View(ProductDetail);

        }
        public ActionResult _GetValueContribution()
        {
            return PartialView();
        }
        public ActionResult _GetValueContributionPartial(int ProductId)
        {
            var interestVal = DepositProduct.GetInterestValueFromPID(ProductId);

            return PartialView(interestVal);
        }
        public ActionResult _CreateAction()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult _UpdateFinAccTree(int selectedNode, bool allowSelectGroupNode, bool withImageIcon, bool withCheckBox, int withOutMe = 0, int rootNode = -1)
        {
            ViewData["AllowSelectGroup"] = allowSelectGroupNode;
            ViewData["WithImageIcon"] = withImageIcon;
            ViewData["WithCheckBox"] = withCheckBox;
            ViewData["param"] = new ChannakyaAccounting.Models.ViewModel.TreeViewParam(false, true, true, 0, selectedNode, "Available FinAcc Group");

            ChannakyaAccounting.Models.ViewModel.TreeView tree = new ChannakyaAccounting.Models.ViewModel.TreeView();
            if (rootNode < 0)
            {
                tree = FinAccService.GetFinAccGroupTree(withOutMe);

            }
            //else
            //{
            //    tree = FinAccService.GetFinAccGroupTree(rootNode, withOutMe);
            //}
            return PartialView("_AccountingTreeViewBody", tree);
        }
        public ActionResult _GetFixedDepositContainer()
        {

            ProductDetail productDetail = new ProductDetail();
            productDetail.FixedDepositList = new FixedDepositList();

            var durationList = new GenericUnitOfWork().Repository<Duration>().GetAll().ToList();
            //foreach (var subitem in durationList)
            //{
            //    FixedDepositList fixedList = new FixedDepositList();
            //    fixedList.DurationList = durationList;

            //}
            productDetail.FixedDepositList.DurationList = durationList;

            return PartialView(productDetail);

        }

        //[HttpPost]
        //public ActionResult _GetProductDetailTreePopup(Loader.ViewModel.TreeViewParam param)
        //{

        //    ViewData["param"] = param;

        //    var filter = param.Filter == null ? "" : param.Filter;

        //  Models.ViewModel.TreeView tree = newModels.ViewModels.TreeView();

        //    if (param.WithOutMe == 0)
        //    {
        //        if (param.Title == "Select Department")
        //        {
        //            tree = DepositProduct.GetDepartmentGroupTree(filter);
        //        }
        //        else if (param.Title == "Select Designation")
        //        {
        //            tree = DepositProduct.GetDesignationGroupTree(filter);
        //        }

        //    }
        //    else
        //    {
        //        // tree = DepositProduct.GetModels.Models.ProductDetailGroupTree(param.WithOutMe, filter);
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
            //    if (param.Title == "Select Department")
            //    {
            //        tree = DepositProduct.GetDepartmentGroupTree(filter);
            //    }
            //    else if (param.Title == "Select Designation")
            //    {
            //        tree = DepositProduct.GetDesignationGroupTree(filter);
            //    }

            //}
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Models.Models.ProductDetail Models.Models.ProductDetail = DepositProduct.GetSingle(id);
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
        //        DepositProduct.Save(Models.Models.ProductDetail);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Models.Models.ProductDetail);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Models.Models.ProductDetail ProductDetail = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductDetail>().GetSingle(x => x.PID == id);
            bool deleteConfirm = true;

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Models.Models.ProductDetail);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int ProductId)
        {
            DepositProduct.Delete(ProductId);
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