using ChannakyaAccounting.Models.Models;
using System;
using System.Collections.Generic;
using DataEntities = ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using ChannakyaAccounting.Models.ViewModel;
using MoreLinq;
using Loader;
using System.Data.Entity.Validation;

namespace ChannakyaAccounting.Controllers
{
    [MyAuthorize]
    public class FinAccController : Controller
    {
        private ChannakyaAccounting.Service.FinAcc.FinAccService FinAccService = null;

        public FinAccController()
        {
            FinAccService = new Service.FinAcc.FinAccService();
        }


        public ActionResult Index()
        {
            //Loader.Service.LayoutMenuMainService lmSrvice = new Loader.Service.LayoutMenuMainService();
            //lmSrvice.GetLayoutMenuGroupTree();
            if (Request.IsAjaxRequest() == true)
            {
                Models.ViewModel.TreeView tree = FinAccService.GetFinAccGroupTree();
                ViewBag.treeview = tree;
                ViewBag.Address = FinAccService.GetAddress(0);
                ViewBag.ViewType = 1;
                ViewBag.ParentFinAccId = 0;
                ViewData["param"] = new ChannakyaAccounting.Models.ViewModel.TreeViewParam(false, true, true, 0, 0, "Available Ledger Group");

                return View(FinAccService.GetAllOfParent(0).ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditProductInterestRatePopUp(int productICBId)
        {
            try
            {
                var productIcbObject = new DataEntities.GenericUnitOfWork().Repository<ProductDurationInt>().GetSingle(x => x.Id == productICBId);
                var productICBObj = new DataEntities.GenericUnitOfWork().Repository<ProductICBDur>().GetSingle(x => x.PICBDurID == productICBId);
                if (productIcbObject == null && productICBObj == null)
                {
                    throw new Exception("No data found in the table for the provided input");

                }
                var tupleModel = new Tuple<ProductDurationInt, ProductICBDur>(productIcbObject, productICBObj);
                return PartialView(tupleModel);
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }

        }
        [HttpPost]
        public ActionResult EditProductInterestRate(ChannakyaAccounting.Models.Models.ProductDurationInt Item1, ChannakyaAccounting.Models.Models.ProductICBDur Item2)
        {
            #region Check
            try
            {
                FinAccService.EditInterestRateProduct(Item1, Item2);
               
                #region FixedAndRecurring
                if (Item1.PId != 0)
                {
                    var productObj = new DataEntities.GenericUnitOfWork().Repository<ProductDetail>().GetSingle(x => x.PID == Item1.PId);
                    if (Item1.DBId != null)
                    {

                        var totalDuration = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetAll().ToList();
                        var totalBasis = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DepositBasi>().GetAll().ToList();

                        productObj.InterestValueList = new List<InterestValue>();
                        var valueList = productObj.ProductDurationInts.DistinctBy(x => x.Value).ToList();
                        if (productObj.ProductDurationInts.First().DBId != null)
                        {
                            #region Recurring
                            foreach (var item in valueList)
                            {
                                InterestValue interestValue = new InterestValue();
                                interestValue.Amount = Convert.ToString(item.Value);
                                interestValue.PolicyIntCalcId = productObj.ProductPSIDs.First().PSID;
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
                                            depositBasis.ProductDurationInt.Id = interestRate.FirstOrDefault().Id;

                                        }
                                        else
                                        {
                                            depositBasis.ProductDurationInt.InterestRate = 0;
                                        }
                                        duration.DepositBasisList.Add(depositBasis);
                                    }
                                    interestValue.DurationList.Add(duration);
                                }


                                productObj.InterestValueList.Add(interestValue);
                            }
                            if (productObj.ProductPSIDs != null && productObj.ProductPSIDs.Count() > 0)
                            {
                                if (productObj.InterestValueList == null)
                                {
                                    productObj.InterestValueList = new List<InterestValue>();
                                }
                                productObj.InterestValueList.ToList()[0].PolicyIntCalcId = productObj.ProductPSIDs.ToList()[0].PSID;
                            }
                            return PartialView("_RecurringProduct", productObj);
                        }

                        #endregion
                    }
                    else
                    {
                        #region FixedProduct
                        //This is Fixed Deposit Product

                        FixedDepositList fdList = new FixedDepositList();
                        //fdList.PolicyIntCalcId = productDetails.ProductPSIDs.First().PSID;

                        var durationList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetAll().ToList();
                        foreach (var subitem in durationList)
                        {
                            var captList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.RuleICBDuration>().GetAll();

                            foreach (var subitem1 in captList)
                            {
                                subitem1.ProductDurationInt = new ProductDurationInt();

                                var doesExists = productObj.ProductDurationInts.Where(x => x.ICBId == Convert.ToInt32(subitem1.ICBDurID) && x.DVId == subitem.Id && x.PId == productObj.PID).Count();
                                if (doesExists > 0)
                                {
                                    var elementinLoan = productObj.ProductDurationInts.Where(x => x.ICBId == Convert.ToInt32(subitem1.ICBDurID) && x.DVId == subitem.Id && x.PId == productObj.PID).FirstOrDefault();
                                    ProductDurationInt product = new ProductDurationInt();
                                    product.DVId = subitem.Id;
                                    product.Id = elementinLoan.Id;
                                    product.InterestRate = elementinLoan.InterestRate;
                                    product.ICBId = subitem1.ICBDurID;
                                    product.PId = productObj.PID;

                                    subitem1.ProductDurationInt = product;
                                }

                                subitem.CaptList.Add(subitem1);



                            }
                            fdList.DurationList.Add(subitem);
                        }

                        productObj.FixedDepositList = fdList;
                        if (productObj.ProductPSIDs != null && productObj.ProductPSIDs.Count() > 0)
                        {
                            productObj.FixedDepositList.PolicyIntCalcId = productObj.ProductPSIDs.ToList()[0].PSID;
                        }

                        return PartialView("_GetFixedDepositContainer", productObj);
                        #endregion

                    }
                }

                #endregion

                #region Normal
                if (Item2.PID != 0)
                {
                    var productObj = new DataEntities.GenericUnitOfWork().Repository<ProductDetail>().GetSingle(x => x.PID == Item2.PID);

                    var totalRuleIcb = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.RuleICBDuration>().GetAll();
                    var totalRulePsid = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.PolicyIntCalc>().GetAll();
                    //This is Normal Deposit Product Without Duration
                    NormalDepositList normalDeposit = new NormalDepositList();
                    foreach (var item in totalRuleIcb)
                    {
                        RuleICBDuration ruleIcb = new RuleICBDuration();
                        var ruleList = productObj.ProductICBDurs.Where(x => x.ICBDurID == item.ICBDurID && x.PID == productObj.PID);
                        if (ruleList.Count() > 0)
                        {
                            ruleIcb.ICBDurID = item.ICBDurID;
                            ruleIcb.ICBDurNam = item.ICBDurNam;
                            ruleIcb.IsChecked = true;
                            ruleIcb.PICBId = ruleList.First().PICBDurID;
                            ruleIcb.IsDefault = ruleList.First().IsDefault;
                            ruleIcb.InterestRateICB = Convert.ToDouble(item.ProductICBDurs.FirstOrDefault().IRate);
                        }
                        else
                        {
                            ruleIcb.ICBDurID = item.ICBDurID;
                            ruleIcb.PICBId = 0;
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
                        var psRule = productObj.ProductPSIDs.Where(x => x.PSID == item.PSID && x.PID == productObj.PID);
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
                    productObj.NormalDepositList = normalDeposit;
                    return PartialView("_GetNormalDepositContainer", productObj);
                    //
                }

                #endregion


            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
            #endregion
            return RedirectToAction("Index", "Home");
        }


        public ActionResult _Details(int parentId, bool allowSelectGroup, int ViewType = 1)
        {
            var finaccDTO = new GenericUnitOfWork().Repository<FinAcc>().GetSingle(x => x.Fid == parentId);
            if (finaccDTO != null)
            {
                ViewBag.CheckCount = FinAccService.CheckLedgerCount(finaccDTO.F2Type, parentId);
            }

            ViewBag.Address = FinAccService.GetAddress(parentId);
            ViewBag.ParentFinAccId = parentId;
            ViewBag.ViewType = ViewType;
            return PartialView(FinAccService.GetAllOfParent(parentId).ToList());
        }

        public ActionResult Details(int id = 0)
        {
            FinAcc FinAcc = FinAccService.GetSingle(id);
            if (FinAcc == null)
            {
                return HttpNotFound();
            }
            return View(FinAcc);
        }
        public JsonResult GetFinSysType(int id)
        {
            int finId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == id).F1id;

            return Json(finId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InterestValueList(string allValue, int productId = 0)
        {
            var valueList = allValue.Split(',').ToList();
            ProductDetail productDetail = new ProductDetail();
            productDetail.InterestValueList = new List<InterestValue>();
            if (productId != 0)
            {
                foreach (var item in valueList)
                {
                    if (item != "")
                    {
                        double amountValue = Convert.ToDouble(item);
                        Models.Models.InterestValue interestValue = new InterestValue();
                        interestValue.Amount = item;
                        interestValue.DurationList = new List<Duration>();
                        var hasAmount = new DataEntities.GenericUnitOfWork().Repository<Models.Models.ProductDurationInt>().FindBy(x => x.PId == productId && x.Value == amountValue);
                        var durationList = new GenericUnitOfWork().Repository<Duration>().GetAll().ToList();
                        var depositBasisList = new GenericUnitOfWork().Repository<DepositBasi>().GetAll().ToList();
                        var value = Convert.ToDouble(item);
                        foreach (var subitem in durationList)
                        {
                            subitem.DepositBasisList = new GenericUnitOfWork().Repository<DepositBasi>().GetAll().ToList();
                            foreach (var childItem in subitem.DepositBasisList)
                            {
                                if (hasAmount.Select(x => x.DBId).Contains(childItem.Id) && hasAmount.Select(x => x.DVId).Contains(subitem.Id) && hasAmount.Select(x => x.Value).Contains(value))
                                {
                                    var productDurationObj = hasAmount.Where(x => x.DBId == childItem.Id && x.DVId == subitem.Id && x.Value == value).FirstOrDefault();

                                    if (productDurationObj != null)
                                    {
                                        childItem.ProductDurationInt = productDurationObj;
                                    }

                                }
                                else
                                {
                                    childItem.ProductDurationInt = new ProductDurationInt();
                                }
                            }
                            interestValue.DurationList.Add(subitem);
                        }
                        productDetail.InterestValueList.Add(interestValue);
                    }

                }
            }
            else
            {
                foreach (var item in valueList)
                {
                    if (item != "")
                    {
                        Models.Models.InterestValue interestValue = new InterestValue();
                        interestValue.Amount = item;
                        interestValue.DurationList = new List<Duration>();
                        var durationList = new GenericUnitOfWork().Repository<Duration>().GetAll().ToList();
                        var depositBasisList = new GenericUnitOfWork().Repository<DepositBasi>().GetAll().ToList();
                        foreach (var subitem in durationList)
                        {
                            subitem.DepositBasisList = new GenericUnitOfWork().Repository<DepositBasi>().GetAll().ToList();
                            //subitem.DepositBasisList.ForEach(x=>x.ProductDurationInt.Equals)
                            subitem.DepositBasisList.ForEach(x => x.ProductDurationInt=new ProductDurationInt());
                            interestValue.DurationList.Add(subitem);
                        }
                        productDetail.InterestValueList.Add(interestValue);
                    }

                }
            }

            return PartialView(productDetail);




        }
        [HttpGet]
        public ActionResult Create(string activeText, int activeId, int FinAccId = 0)
        {


            if (Request.IsAjaxRequest())
            {
                FinAcc FinAccDTO = new FinAcc();


                //FinAccDTO.DimensionDefinationList = new List<DimensionDefination>();
                int? parentLedgerId;
                if (activeId == 0)
                {
                    parentLedgerId = 0;
                }
                else
                {
                    parentLedgerId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinAcc>().GetSingle(x => x.Fid == activeId).Pid;
                }


                if (FinAccId != 0)
                {
                    var productDetails = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<ProductDetail>().GetSingle(x => x.FID == FinAccId);
                    SubsiVSFId subsiVsFid = new SubsiVSFId();
                    //SubsiTitle table is removed
                    //SubsiTitle subsiTitle = new SubsiTitle();
                    //FinAccDTO.SubsiTitles.Add(subsiTitle);
                    FinAccDTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinAcc>().GetSingle(x => x.Fid == (FinAccId));
                    if (productDetails != null)
                    {
                        Models.Models.SchmDetail schemeDetail = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().FindBy(x => x.FID == activeId).FirstOrDefault();
                        FinAccDTO.ProductDetails.First().SchemeofProduct = schemeDetail;
                        var totalDuration = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetAll().ToList();
                        var totalBasis = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DepositBasi>().GetAll().ToList();
                        var ProductDurationIntList = new GenericUnitOfWork().Repository<ProductDurationInt>().FindBy(x => x.PId == productDetails.PID).DistinctBy(x => x.Value).ToList();

                        var startACNo = new GenericUnitOfWork().Repository<Models.Models.ProductBrnch>().GetSingle(x => x.PID == productDetails.PID).StartAcNo;

                        FinAccDTO.ProductDetails.FirstOrDefault().StartAcNo = Convert.ToInt32(startACNo);


                        if (productDetails.ProductDurationInts.Count() > 0 && productDetails.ProductPSIDs.Count() == 1)
                        {

                            FinAccDTO.ProductDetails.FirstOrDefault().InterestValueList = new List<InterestValue>();
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
                                                depositBasis.ProductDurationInt.Id = interestRate.FirstOrDefault().Id;

                                            }
                                            else
                                            {
                                                depositBasis.ProductDurationInt.InterestRate = 0;
                                            }
                                            duration.DepositBasisList.Add(depositBasis);
                                        }
                                        interestValue.DurationList.Add(duration);
                                    }


                                    FinAccDTO.ProductDetails.First().InterestValueList.Add(interestValue);

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
                                            product.Id = elementinLoan.Id;
                                            product.InterestRate = elementinLoan.InterestRate;
                                            product.ICBId = subitem1.ICBDurID;
                                            product.PId = productDetails.PID;

                                            subitem1.ProductDurationInt = product;
                                        }

                                        subitem.CaptList.Add(subitem1);



                                    }
                                    fdList.DurationList.Add(subitem);
                                }

                                FinAccDTO.ProductDetails.FirstOrDefault().FixedDepositList = fdList;

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
                                    ruleIcb.PICBId = ruleList.First().PICBDurID;
                                    ruleIcb.IsDefault = ruleList.First().IsDefault;
                                    ruleIcb.InterestRateICB = Convert.ToDouble(item.ProductICBDurs.FirstOrDefault().IRate);
                                }
                                else
                                {
                                    ruleIcb.ICBDurID = item.ICBDurID;
                                    ruleIcb.PICBId = 0;
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
                            FinAccDTO.ProductDetails.FirstOrDefault().NormalDepositList = normalDeposit;
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
                            FinAccDTO.ProductDetails.FirstOrDefault().LoanInterestList = loanInterest;
                        }
                    }
                    //if (productDetails.ProductDurationInts.Count() > 0 && productDetails.ProductPSIDs.Count() == 1)
                    //{
                    //    FixedDepositList fdList = new FixedDepositList();
                    //    foreach (var item in productDetails.ProductDurationInts)
                    //    {
                    //        //This is Fixed Deposit Product
                    //        var durationObj = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetSingle(x => x.Id == item.DVId);
                    //        durationObj.ProductDurationInt = item;
                    //        fdList.DurationList.Add(durationObj);


                    //    }

                    //    productDetails.FixedDepositList = fdList;

                    //}



                    //Models.Models.InterestValue interestValue = new InterestValue();
                    //interestValue.Amount = item;
                    //var durationList = new GenericUnitOfWork().Repository<Duration>().GetAll().ToList();
                    //var depositBasisList = new GenericUnitOfWork().Repository<DepositBasi>().GetAll().ToList();
                    //foreach (var subitem in durationList)
                    //{
                    //    subitem.DepositBasisList = new GenericUnitOfWork().Repository<DepositBasi>().GetAll().ToList();
                    //    interestValue.DurationList.Add(subitem);
                    //}
                    //productDetail.InterestValueList.Add(interestValue);



                    var F2List = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinSys2>().FindBy(x => x.F2id == FinAccDTO.F2Type).ToList();
                    ViewBag.F2Type = new SelectList(F2List, "F2id", "F2Desc");
                    ViewBag.ActiveId = activeId;
                    var f2Id = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinAcc>().GetSingle(x => x.Fid == FinAccId).F2Type;
                    ViewBag.IsGroup = IsFinAccGroup(f2Id);
                    var schemeDetails = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<SchmDetail>().GetSingle(x => x.FID == (FinAccId + 4));
                    if (schemeDetails != null)
                    {
                        SchmDetail schmDetail = new SchmDetail();
                        schmDetail = schemeDetails;
                        FinAccDTO.SchmDetails.Add(schmDetail);
                    }


                    //var dimValue = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<DimensionVSLedger>().GetAll().Where(x => x.Fid == FinAccId).ToList();
                    //FinAccDTO.DimensionDefinationList = FinAccService.GetAllDimension(dimValue);




                    if (FinAccDTO.Content != null)
                    {
                        ViewBag.Image = Convert.ToBase64String(FinAccDTO.Content, Base64FormattingOptions.None);
                    }
                }
                else
                {

                    var F2list = FinAccService.GenerateF2Type(parentLedgerId, activeId);
                    ViewBag.F2Type = new SelectList(F2list, "F2id", "F2Desc");
                    ViewBag.IsGroup = true;
                    if (F2list.Count() > 0)
                    {
                        ViewBag.IsGroup = IsFinAccGroup(F2list.FirstOrDefault().F2id);
                    }

                    //var interestRateList = new GenericUnitOfWork().Repository<Models.Models.ProductDurationInt>().FindBy(x=>x.PId)
                    FinAccDTO.Pid = activeId;
                }

                ViewBag.ActiveText = activeText;
                ViewData["param"] = new ChannakyaAccounting.Models.ViewModel.TreeViewParam(false, true, false, 0, 0, "Available FinAcc Group");
                return PartialView(FinAccDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult _GetFixedDepositContainer()
        {

            ProductDetail productDetail = new ProductDetail();
            productDetail.FixedDepositList = new FixedDepositList();

            var durationList = new GenericUnitOfWork().Repository<Duration>().GetAll().ToList();
            foreach (var subitem in durationList)
            {
                List<RuleICBDuration> fixedList = new List<RuleICBDuration>();
                fixedList = new GenericUnitOfWork().Repository<RuleICBDuration>().GetAll().ToList();
                subitem.CaptList = fixedList;
                foreach (var item in subitem.CaptList)
                {
                    item.ProductDurationInt = new ProductDurationInt();
                }

            }
            productDetail.FixedDepositList.DurationList = durationList;

            return PartialView(productDetail);

        }

        public ActionResult _GetNormalDepositContainer()
        {
            ProductDetail productDetail = new ProductDetail();
            productDetail.NormalDepositList = new NormalDepositList();

            Service.DepositProduct.DepositProductService productService = new Service.DepositProduct.DepositProductService();
            NormalDepositList normalDeposit = new NormalDepositList();
            normalDeposit.CapitalizationRuleList = new List<RuleICBDuration>();
            normalDeposit.CalculationRuleList = new List<PolicyIntCalc>();
            normalDeposit.CalculationRuleList = new GenericUnitOfWork().Repository<Models.Models.PolicyIntCalc>().GetAll().ToList();
            normalDeposit.CapitalizationRuleList = new GenericUnitOfWork().Repository<Models.Models.RuleICBDuration>().GetAll().ToList(); ;

            productDetail.NormalDepositList = normalDeposit;

            return PartialView(productDetail);


        }

        public ActionResult AddBankInfoDetails()
        {
            FinAcc modelBank = new FinAcc();
            modelBank.BankInfoes.Add(new BankInfo());
            return PartialView(modelBank);
        }
        public ActionResult AddLoanAccount()
        {
            FinAcc modelProduct = new FinAcc();
            modelProduct.ProductDetails = new List<ProductDetail>();
            modelProduct.ProductDetails.Add(new ProductDetail());
            return PartialView(modelProduct);
        }
        public ActionResult AddDepositProduct(int schemeId)
        {
            FinAcc modelProduct = new FinAcc();
            Models.Models.SchmDetail schemeDetails = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().FindBy(x => x.FID == schemeId).FirstOrDefault();
            modelProduct.ProductDetails = new List<ProductDetail>();
            ProductDetail productDetail = new ProductDetail();
            productDetail.DurationList = new List<Duration>();
            productDetail.InterestValueList = new List<InterestValue>();
            productDetail.LoanInterestList = new LoanInterestList();
            LoanInterestList loanInterest = new LoanInterestList();
            loanInterest.PolicyPenaltyList = new GenericUnitOfWork().Repository<Models.Models.PolicyPenCalc>().GetAll().ToList();
            productDetail.LoanInterestList = loanInterest;

            var depositBasisList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DepositBasi>().GetAll().ToList();
            var depositDuration = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetAll().ToList();
            foreach (var item in depositDuration)
            {

                item.DepositBasisList = depositBasisList;

                productDetail.DurationList.Add(item);
            }


            productDetail.SchemeofProduct = schemeDetails;
            modelProduct.ProductDetails.Add(productDetail);
            return PartialView(modelProduct);
        }

        public ActionResult _GetPolicyPenaltyList()
        {
            ProductDetail productDetail = new ProductDetail();
            productDetail.LoanInterestList = new LoanInterestList();
            LoanInterestList loanInterest = new LoanInterestList();
            loanInterest.PolicyPenaltyList = new GenericUnitOfWork().Repository<Models.Models.PolicyPenCalc>().GetAll().ToList();
            productDetail.LoanInterestList = loanInterest;
            return PartialView(productDetail);

        }
        public ActionResult _GetPaymentRuleList()
        {
            ProductDetail productDetail = new ProductDetail();
            productDetail.LoanInterestList = new LoanInterestList();
            LoanInterestList loanInterest = new LoanInterestList();
            loanInterest.RulePayList = new GenericUnitOfWork().Repository<Models.Models.RulePay>().GetAll().ToList();
            productDetail.LoanInterestList = loanInterest;
            return PartialView(productDetail);

        }
        public ActionResult AddDepositScheme()
        {
            SchmDetail schm = new SchmDetail();
            ViewBag.SchemeType = 0;
            FinAcc modelScheme = new FinAcc();
            modelScheme.SchmDetails = new List<SchmDetail>();
            schm.SType = 0;
            modelScheme.SchmDetails.Add(schm);
            return PartialView(modelScheme);
        }
        public ActionResult AddLoanScheme()
        {
            SchmDetail schm = new SchmDetail();
            ViewBag.SchemeType = 1;
            FinAcc modelScheme = new FinAcc();
            modelScheme.SchmDetails = new List<SchmDetail>();

            schm.SType = 1;
            modelScheme.SchmDetails.Add(schm);

            return PartialView("AddDepositScheme", modelScheme);
        }
        public ActionResult AddSubsiDetails()
        {
            FinAcc modelSubsi = new FinAcc();
            modelSubsi.SubsiVSFIds.Add(new SubsiVSFId());
            return PartialView(modelSubsi);
        }
        public ActionResult AddDimension()
        {
            var Dimvalue = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<DimensionDefination>().GetAll();

            var dimensionList = Dimvalue.AsEnumerable().ToList();
            FinAcc modelFinAcc = new FinAcc();
            modelFinAcc.DimensionDefinationList = new List<DimensionDefination>();

            modelFinAcc.DimensionDefinationList = dimensionList;
            ViewBag.DimensionCount = dimensionList.Count();

            return PartialView(modelFinAcc);
        }
        public ActionResult _GetValueContribution()
        {
            return PartialView();
        }

        public JsonResult CheckIfCodeDuplicates(string Code, int Fid)
        {
            bool isanyCode = true;
            if (Fid == 0)
            {
                isanyCode = !new DataEntities.GenericUnitOfWork().Repository<FinAcc>().GetAll().Any(x => x.Code == Code.Trim());
            }
            else
            {
                isanyCode = new DataEntities.GenericUnitOfWork().Repository<FinAcc>().GetAll().Any(x => x.Code == Code.Trim() && x.Fid == Fid);
            }

            return Json(isanyCode, JsonRequestBehavior.AllowGet);

        }
        public ActionResult IsGroup(int nodeId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinSys2>().GetSingle(x => x.F2id == nodeId).FinSys1.IsGroup;
            return Json(isGroup, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IsGroupFinSys1(int finsys1Id)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinSys1>().GetSingle(x => x.F1id == finsys1Id).IsGroup;
            return Json(isGroup, JsonRequestBehavior.AllowGet);
        }

        public bool IsFinAccGroup(int ledgerId)
        {
            var isGroup = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinSys2>().GetSingle(x => x.F2id == ledgerId).FinSys1.IsGroup;
            return isGroup;
        }
        public ActionResult _GetType(int Pid)
        {
            int? parentLedgerId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinAcc>().GetSingle(x => x.Fid == Pid).Pid;
            var F2list = FinAccService.GenerateF2Type(parentLedgerId, Pid);
            ViewBag.F2Type = new SelectList(F2list, "F2id", "F2Desc");


            return PartialView("_GetType");
        }
        public ActionResult _GetLoanInterestContainer()
        {
            ProductDetail productDetail = new ProductDetail();
            productDetail.LoanInterestList = new LoanInterestList();
            LoanInterestList loanInterest = new LoanInterestList();
            loanInterest.PolicyCalculationList = new List<PolicyIntCalc>();
            loanInterest.PolicyCalculationList = new GenericUnitOfWork().Repository<Models.Models.PolicyIntCalc>().GetAll().ToList();
            productDetail.LoanInterestList = loanInterest;
            return PartialView(productDetail);
        }


        [HttpPost]
        public ActionResult Create(FinAcc FinAcc, HttpPostedFileBase file, List<InterestValue> ProductInterest, FixedDepositList fixedDepositList, NormalDepositList normalDepositList, LoanInterestList loanInterestList)
        {

            if (file != null)
            {
                using (var reader = new System.IO.BinaryReader(file.InputStream))
                {
                    FinAcc.Content = reader.ReadBytes(file.ContentLength);
                }
            }
            try
            {
                if (FinAcc.Fid == 0)
                {
                    //foreach (var item in FinAcc.ProductDetails)
                    //{
                    //    foreach (var subitem in ProductInterest)
                    //    {
                    //        item.InterestValueList = subitem.InterestValueList;
                    //    }
                    //}
                    if (FinAcc.ProductDetails.Count() > 0)
                    {
                        FinAcc.ProductDetails.FirstOrDefault().InterestValueList = new List<InterestValue>();
                        FinAcc.ProductDetails.FirstOrDefault().FixedDepositList = new FixedDepositList();
                        FinAcc.ProductDetails.FirstOrDefault().NormalDepositList = new NormalDepositList();
                        FinAcc.ProductDetails.FirstOrDefault().LoanInterestList = new LoanInterestList();

                        FinAcc.ProductDetails.FirstOrDefault().InterestValueList = ProductInterest;

                        #region Validation

                        //#region RecurringValidation
                        //int isallValidated = 0;
                        //int isWithInterestRate = 0;
                        //if (ProductInterest != null && ProductInterest.Count() > 0)
                        //{
                        //    if (ProductInterest.FirstOrDefault().PolicyIntCalcId == 0)
                        //    {
                        //        throw new Exception("Please Select Policy Calculation");
                        //    }
                        //    foreach (var item in ProductInterest)
                        //    {

                        //        var allDurationList = item.DurationList.ToList();
                        //        foreach (var subitem in allDurationList)
                        //        {
                        //            var allDepositBasisList = subitem.DepositBasisList.ToList();
                        //            foreach (var subitem1 in allDepositBasisList)
                        //            {
                        //                var interestRateObj = subitem1.ProductDurationInt;
                        //                if (interestRateObj.ICBId != null && interestRateObj.InterestRate == null)
                        //                {
                        //                    isallValidated++;
                        //                }
                        //                if (interestRateObj.ICBId == null && interestRateObj.InterestRate != null)
                        //                {
                        //                    isWithInterestRate++;
                        //                }
                        //            }
                        //        }

                        //    }
                        //    if (isallValidated > 0)
                        //    {
                        //        throw new Exception("Please Enter Interest Rate");
                        //    }
                        //    if (isWithInterestRate > 0)
                        //    {
                        //        throw new Exception("Please Select Calculation Rule");
                        //    }
                        //}
                        //#endregion

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
                            FinAcc.ProductDetails.FirstOrDefault().FixedDepositList = fixedDepositList;
                        }
                        else
                        {
                            FinAcc.ProductDetails.FirstOrDefault().FixedDepositList = fixedDepositList;
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
                                FinAcc.ProductDetails.FirstOrDefault().NormalDepositList = normalDepositList;
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
                                FinAcc.ProductDetails.FirstOrDefault().LoanInterestList = loanInterestList;
                            }
                        }
                        #endregion

                        #endregion


                    }

                    FinAccService.Save(FinAcc);
                    return RedirectToAction("Create", new { activeText = "Root", activeId = 0 });
                }
                else
                {
                    if (FinAcc.ProductDetails.FirstOrDefault() != null)
                    {
                        FinAcc.ProductDetails.FirstOrDefault().InterestValueList = new List<InterestValue>();
                        FinAcc.ProductDetails.FirstOrDefault().InterestValueList = ProductInterest;
                    }

                    FinAccService.Save(FinAcc);
                    var parentNode = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinAcc>().GetSingle(x => x.Fid == FinAcc.Pid);

                    return RedirectToAction("Create", new { activeText = parentNode.Fname, activeId = parentNode.Pid });
                }


            }
            catch (DbEntityValidationException ex)
            {
                //return JavaScript(ex.Message);
                return JavaScript(ex.Message);
            }



            return View(FinAcc);

        }
        public ActionResult AddInterestValue()
        {
            return PartialView("_AddInterestValue");
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




        [HttpPost]
        public ActionResult _GetFinAccTreePopup(ChannakyaAccounting.Models.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ChannakyaAccounting.Models.ViewModel.TreeView tree = new ChannakyaAccounting.Models.ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = FinAccService.GetFinAccGroupTree(filter);
            }
            else
            {
                tree = FinAccService.GetFinAccGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_AccountingTreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetFinAccTree(ChannakyaAccounting.Models.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ChannakyaAccounting.Models.ViewModel.TreeView tree = new ChannakyaAccounting.Models.ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = FinAccService.GetFinAccGroupTree(filter);
            }
            else
            {
                tree = FinAccService.GetFinAccGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_AccountingTreeViewBody", tree);
        }

        public ActionResult Edit(int id = 0)
        {
            FinAcc FinAcc = FinAccService.GetSingle(id);
            if (FinAcc == null)
            {
                return HttpNotFound();
            }
            return View(FinAcc);
        }




        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            FinAcc FinAcc = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinAcc>().GetSingle(x => x.Pid == id);
            var ifMapped = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Voucher2>().GetSingle(x => x.Fid == id);
            int deleteConfirm = 0;// false
            if (FinAcc == null && ifMapped == null)
            {
                deleteConfirm = 1;// true
            }
            if (ifMapped != null)
            {
                deleteConfirm = 2;// already mapped
            }

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(FinAcc);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int FinAccId)
        {
            FinAccService.Delete(FinAccId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DisplayImage(HttpPostedFileBase imagefile)
        {
            using (var reader = new System.IO.BinaryReader(imagefile.InputStream))
            {
                byte[] ContentImage = reader.ReadBytes(imagefile.ContentLength);
                var ImageContent = Convert.ToBase64String(ContentImage, Base64FormattingOptions.None);
                return Json(ImageContent, JsonRequestBehavior.AllowGet);
            }


        }



    }
}