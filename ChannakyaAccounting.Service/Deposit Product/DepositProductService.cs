using ChannakyaAccounting.Models.Models;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.DepositProduct
{
    public class DepositProductService
    {
        private GenericUnitOfWork uow = null;

        private Loader.Service.BranchSetupService branchServices = new Loader.Service.BranchSetupService();

        public DepositProductService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<Models.Models.ProductDetail> GetAll()
        {
            return uow.Repository<Models.Models.ProductDetail>().GetAll().ToList();
        }

        public Models.Models.ProductDetail GetSingle(int ProductDetailId)
        {
            Models.Models.ProductDetail ProductDetail = uow.Repository<Models.Models.ProductDetail>().GetSingle(c => c.PID == ProductDetailId);
            return ProductDetail;
        }

        public List<SelectListItem> GetAllScheme()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Models.Models.SchmDetail>().FindBy(x => x.SEnable == true).ToList();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.SDName, Value = item.SDID.ToString() });
            }
            lst = lst.OrderBy(x => x.Text).ToList();
            return lst;
        }

        public List<Models.Models.DepositBasi> AllDepositBasis()
        {
            List<Models.Models.DepositBasi> depositBasisList = new List<Models.Models.DepositBasi>();
            var depositBasis = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DepositBasi>().GetAll();
            foreach (var item in depositBasis)
            {
                depositBasisList.Add(item);
            }
            return depositBasisList;
        }

        public List<Models.Models.Duration> AllDuration()
        {
            List<Models.Models.Duration> DurationList = new List<Models.Models.Duration>();
            var Duration = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.Duration>().GetAll();
            foreach (var item in Duration)
            {
                DurationList.Add(item);
            }
            return DurationList;
        }

        public List<Models.Models.RuleRenew> AllRuleRenew()
        {
            List<Models.Models.RuleRenew> DurationList = new List<Models.Models.RuleRenew>();
            var Duration = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.RuleRenew>().GetAll();
            foreach (var item in Duration)
            {
                DurationList.Add(item);
            }
            return DurationList.OrderBy(x => x.RenewRule).ToList();
        }

        public string LedgerName(int F1Id)
        {
            string ledgerName = "";
            if (F1Id == 12 || F1Id == 13)
            {
                ledgerName = "Deposit Scheme";
            }

            if (F1Id == 124)
            {
                ledgerName = "Loan Scheme";
            }
            if (F1Id == 16 || F1Id == 17 || F1Id == 25)
            {
                ledgerName = "Int. Exp";
            }
            if (F1Id == 14)
            {
                ledgerName = "Deposit Product";
            }
            if (F1Id == 125)
            {
                ledgerName = "Loan Product";
            }
            if (F1Id == 18 || F1Id == 19 || F1Id == 24)
            {
                ledgerName = "Int. Payable";
            }
            if (F1Id == 10 || F1Id == 21 || F1Id == 26)
            {
                ledgerName = "Int. Acc";
            }
            if (F1Id == 22 || F1Id == 23 || F1Id == 27)
            {
                ledgerName = "Tax On Int.";
            }
            if (F1Id == 127 || F1Id == 240)
            {
                ledgerName = "Int. Rec";
            }
            if (F1Id == 128 || F1Id == 241)
            {
                ledgerName = "Pen. Rec";
            }
            if (F1Id == 129 || F1Id == 242)
            {
                ledgerName = "Int. Suspense";
            }
            if (F1Id == 130 || F1Id == 243)
            {
                ledgerName = "Pen. Suspense";
            }
            if (F1Id == 131 || F1Id == 244)
            {
                ledgerName = "Int. Income";
            }
            if (F1Id == 132 || F1Id == 245)
            {
                ledgerName = "Pen. Income";
            }

            if (F1Id == 133 || F1Id == 246)
            {
                ledgerName = "Int. Rebate";
            }
            if (F1Id == 134 || F1Id == 247)
            {
                ledgerName = "Pen. Rebate";
            }
            if (F1Id == 135 || F1Id == 248)
            {
                ledgerName = "Other Loan";
            }
            if (F1Id == 136 || F1Id == 249)
            {
                ledgerName = "Other Int. Income";
            }
            return ledgerName;
        }

        public List<Models.Models.LicenseBranch> AllLicenseBranch()
        {
            List<Models.Models.LicenseBranch> DurationList = new List<Models.Models.LicenseBranch>();
            var Duration = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.LicenseBranch>().GetAll();
            foreach (var item in Duration)
            {
                DurationList.Add(item);
            }
            return DurationList;
        }

        public bool CheckProductName(string PName, int? PID)
        {
            var isExists = false;
            var productCount = uow.Repository<Models.Models.ProductDetail>().FindBy(c => c.PName.ToLower().Trim() == PName.ToLower().Trim() && c.PID != PID).Count();
            if (productCount > 0)
            {
                isExists = true;
            }
            return isExists;
        }

        public bool IsPrefixExists(string Apfix, int? PID)
        {
            int count = uow.Repository<ProductDetail>().FindBy(x => x.Apfix.ToLower().Trim() == Apfix.ToLower().Trim() && x.PID != PID).Count();
            //int count = uow.Repository<DAL.DatabaseModel.LicenseBranch>().FindBy(x => x.BrnhNam.Trim().ToLower() == BrnhNam.Trim() && x.BrnhID != BrnhID).Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Save(Models.Models.ProductDetail productDetail, string Alias)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();

            try
            {
                #region Create
                if (productDetail.PID == 0)
                {
                    var schemeId = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == productDetail.SDID);
                    var sId = schemeId.SDID;

                    var F2Type = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == schemeId.FID).F2Type;
                    var F2Id = 0;
                    if (schemeId.SType == 0)
                    {
                        F2Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.Pid == F2Type && x.F1id == 15).F2id;
                    }
                    else
                    {
                        F2Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.Pid == F2Type && x.F1id == 126).F2id;
                    }

                    List<int> finalProductList = new List<int>();
                    finalProductList.Add(schemeId.FID);
                    int finalval = schemeId.FID;
                    //for deposit and loan types from scheme setup
                    if (schemeId.SType == 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            finalval = finalval + 1;
                            finalProductList.Add(finalval);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            finalval = finalval + 1;
                            finalProductList.Add(finalval);
                        }
                    }
                    //var finalProductList = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Pid == productParntList).Select(x => x.Fid).ToList();
                    int count = 0;
                    foreach (var item in finalProductList)
                    {
                        count++;
                        var F2TypeFA = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == item).F2Type;
                        F2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == F2TypeFA).SingleOrDefault().F2id;

                        //var alias = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == item).Alias;
                        var alias = Alias;
                        
                        var f1Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == F2TypeFA).F1id;
                        Models.Models.FinAcc productList = new Models.Models.FinAcc();

                        productList.Fname = productDetail.PName + "(" + LedgerName(f1Id) + ")";
                        productList.F2Type = F2Id;
                        productList.Pid = item;
                        productList.Alias = alias;

                        if (count == 1)
                        {
                            Models.Models.ProductDetail productModel = new Models.Models.ProductDetail();
                            productModel = productDetail;
                            productModel.enabled = true;
                            productModel.HasIndivLimit = productDetail.HasIndivLimit;
                            productList.ProductDetails.Add(productModel);
                        }
                        uow.Repository<Models.Models.FinAcc>().Add(productList);
                        uow.Commit();

                        #region Prdct
                        if (count == 1)
                        {
                            var allBranch = branchServices.GetAll();
                            var productId = productList.ProductDetails.First().PID;

                            foreach (var branch in allBranch) //loop for each and every branch for created product (modification by Dorje through Romy)
                            {
                                ProductBrnch productBrnch = new ProductBrnch();
                                productBrnch.PID = productId;
                                productBrnch.BrnchID = branch.CompanyId;
                                productBrnch.LastAcNo = productDetail.StartAcNo;
                                productBrnch.StartAcNo = productDetail.StartAcNo;
                                uow.Repository<Models.Models.ProductBrnch>().Add(productBrnch);
                                uow.Commit();
                            }

                            if (productDetail.InterestValueList != null)
                            {
                                if (productDetail.InterestValueList.Count != 0)
                                {
                                    var ProductInterest = productDetail.InterestValueList;
                                    foreach (var subitem in ProductInterest)
                                    {
                                        var amount = subitem.Amount;
                                        foreach (var subitem1 in subitem.DurationList)
                                        {
                                            var durationId = subitem1.Id;
                                            foreach (var subitem2 in subitem1.DepositBasisList)
                                            {
                                                var basisId = subitem2.Id;
                                                var interestRate = subitem2.ProductDurationInt.InterestRate;
                                                if (interestRate != null)
                                                {
                                                    ProductDurationInt ProductDurationInt = new ProductDurationInt();
                                                    ProductDurationInt.InterestRate = interestRate;
                                                    ProductDurationInt.Value = Convert.ToDouble(amount);
                                                    ProductDurationInt.DBId = basisId;
                                                    ProductDurationInt.ICBId = subitem2.ProductDurationInt.ICBId;
                                                    ProductDurationInt.PId = productId;
                                                    ProductDurationInt.DVId = durationId;

                                                    uow.Repository<Models.Models.ProductDurationInt>().Add(ProductDurationInt);
                                                    uow.Commit();
                                                }
                                            }
                                        }
                                    }
                                    ProductPSID productCalc = new ProductPSID();

                                    productCalc.PSID = Convert.ToByte(ProductInterest.FirstOrDefault().PolicyIntCalcId);
                                    productCalc.PID = productId;
                                    productCalc.IsDefault = true;
                                    uow.Repository<Models.Models.ProductPSID>().Add(productCalc);

                                    uow.Commit();
                                }
                            }
                            if (productDetail.FixedDepositList != null)
                            {
                                if (productDetail.FixedDepositList.DurationList.Count() > 0)
                                {
                                    var fixedproductId = productList.ProductDetails.First().PID;
                                    var fixedcollection = productDetail.FixedDepositList;
                                    foreach (var fixeditems in fixedcollection.DurationList)
                                    {

                                        foreach (var fixedsubitem in fixeditems.CaptList)
                                        {

                                            ProductDurationInt productDurationint = new ProductDurationInt();
                                            productDurationint.DVId = fixeditems.Id;
                                            productDurationint.InterestRate = fixedsubitem.ProductDurationInt.InterestRate;
                                            productDurationint.ICBId = fixedsubitem.ICBDurID;
                                            productDurationint.PId = fixedproductId;
                                            if (productDurationint.InterestRate != null)
                                            {
                                                uow.Repository<Models.Models.ProductDurationInt>().Add(productDurationint);
                                            }
                                        }
                                    }
                                    ProductPSID productCalc = new ProductPSID();

                                    productCalc.PSID = Convert.ToByte(fixedcollection.PolicyIntCalcId);
                                    productCalc.PID = fixedproductId;
                                    productCalc.IsDefault = false;
                                    uow.Repository<Models.Models.ProductPSID>().Add(productCalc);

                                    uow.Commit();
                                }
                            }
                            if (productDetail.LoanInterestList != null)
                            {
                                if (productDetail.LoanInterestList.PolicyCalculationList.Count() > 0)
                                {
                                    var fixedproductId = productList.ProductDetails.First().PID;
                                    var policyCalc = productDetail.LoanInterestList.PolicyCalculationList;
                                    foreach (var fixeditems in policyCalc)
                                    {
                                        if (fixeditems.IsChecked == true)
                                        {
                                            ProductPSID productCalc = new ProductPSID();

                                            productCalc.PSID = Convert.ToByte(fixeditems.PSID);
                                            productCalc.PID = fixedproductId;
                                            productCalc.IsDefault = fixeditems.IsDefault;
                                            uow.Repository<Models.Models.ProductPSID>().Add(productCalc);


                                        }

                                    }
                                    uow.Commit();

                                }

                                if (productDetail.LoanInterestList.PolicyPenaltyList != null)
                                {
                                    if (productDetail.LoanInterestList.PolicyPenaltyList.Count() > 0)
                                    {
                                        var fixedproductId = productList.ProductDetails.First().PID;
                                        var policyCalc = productDetail.LoanInterestList.PolicyPenaltyList;
                                        foreach (var fixeditems in policyCalc)
                                        {
                                            if (fixeditems.IsChecked == true)
                                            {
                                                ProductPCID productCalc = new ProductPCID();

                                                productCalc.PCID = Convert.ToByte(fixeditems.PCID);
                                                productCalc.PID = fixedproductId;
                                                productCalc.IsDefault = Convert.ToByte(fixeditems.IsDefault);
                                                uow.Repository<Models.Models.ProductPCID>().Add(productCalc);


                                            }
                                        }
                                        uow.Commit();

                                    }
                                }
                                if (productDetail.LoanInterestList.RulePayList != null)
                                {
                                    if (productDetail.LoanInterestList.RulePayList.Count() > 0)
                                    {
                                        var fixedproductId = productList.ProductDetails.First().PID;
                                        var policyCalc = productDetail.LoanInterestList.RulePayList;
                                        foreach (var fixeditems in policyCalc)
                                        {
                                            if (fixeditems.IsChecked == true)
                                            {
                                                ProductPay productCalc = new ProductPay();

                                                productCalc.PAYID = Convert.ToByte(fixeditems.PAYID);
                                                productCalc.PID = fixedproductId;
                                                productCalc.IsDefault = fixeditems.IsDefault;
                                                uow.Repository<Models.Models.ProductPay>().Add(productCalc);
                                            }
                                        }
                                        uow.Commit();
                                    }
                                }
                            }
                            if (productDetail.NormalDepositList != null)
                            {
                                if (productDetail.NormalDepositList.CalculationRuleList.Count() > 0)
                                {

                                    var normalProductId = productList.ProductDetails.First().PID;
                                    var normalCollection = productDetail.NormalDepositList;

                                    foreach (var normalitems in normalCollection.CapitalizationRuleList)
                                    {
                                        if (normalitems.IsChecked == true)
                                        {
                                            ProductICBDur productIcbDur = new ProductICBDur();
                                            productIcbDur.ICBDurID = Convert.ToByte(normalitems.ICBDurID);
                                            productIcbDur.IRate = Convert.ToInt32(normalitems.InterestRateICB);
                                            productIcbDur.PID = normalProductId;
                                            productIcbDur.IsDefault = normalitems.IsDefault;
                                            uow.Repository<Models.Models.ProductICBDur>().Add(productIcbDur);

                                        }

                                    }
                                    uow.Commit();
                                    foreach (var normalCalcItems in normalCollection.CalculationRuleList)
                                    {
                                        if (normalCalcItems.IsChecked == true)
                                        {
                                            ProductPSID productCalc = new ProductPSID();
                                            productCalc.PSID = Convert.ToByte(normalCalcItems.PSID);
                                            productCalc.IsDefault = normalCalcItems.IsDefault;
                                            productCalc.PID = normalProductId;
                                            uow.Repository<Models.Models.ProductPSID>().Add(productCalc);
                                        }
                                    }
                                    uow.Commit();
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region Edit
                else
                {
                    //var imageContent = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == FinAcc.Fid).Content;
                    using (var uow = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork())
                    {
                        var finaccId = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == productDetail.SDID).FID;
                        var schemeData = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == productDetail.SDID);
                        var mainFinacc = productDetail.FID;

                        if (schemeData.SType == 0)
                        {
                            for (int i = mainFinacc; i < (mainFinacc + 4); i++)
                            {
                                Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                finaccDT.Fname = productDetail.PName;
                                finaccDT.Alias = Alias;
                                uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);
                                uow.Commit();
                            }
                        }
                        else
                        {
                            for (int i = mainFinacc; i < (mainFinacc + 10); i++)
                            {
                                Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                finaccDT.Fname = productDetail.PName;
                                finaccDT.Alias = Alias;
                                uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);
                                uow.Commit();
                            }
                        }
                        GenericUnitOfWork UOW = new GenericUnitOfWork();
                        Models.Models.ProductDetail productDT = new Models.Models.ProductDetail();
                        productDT = productDetail;
                        UOW.Repository<Models.Models.ProductDetail>().Edit(productDT);
                        UOW.Commit();

                        var productId = productDetail.PID;

                        #region Products

                        var productData = productDetail;
                        var productDetails = uow.Repository<Models.Models.ProductDetail>().GetSingle(x => x.SDID == productData.SDID);
                        var allProductBrnch = uow.Repository<Models.Models.ProductBrnch>().GetSingle(x => x.PID == productId);

                        var allBranch = branchServices.GetAll();

                        foreach (var branch in allBranch) //loop for each and every branch for created product (modification by Dorje through Romy)
                        {
                            ProductBrnch productBrnch = uow.Repository<ProductBrnch>().GetSingle(x => x.PID == productId && x.BrnchID == branch.CompanyId);
                            //productBrnch.PID = productId;
                            //productBrnch.BrnchID = branch.CompanyId;
                            //productBrnch.LastAcNo = productDetail.StartAcNo; //because last account number might be changed to start account number in edit
                            productBrnch.StartAcNo = productDetail.StartAcNo;
                            uow.Repository<Models.Models.ProductBrnch>().Edit(productBrnch);
                            uow.Commit();
                        }
                        #region discarded edit
                        //this are edited in change product interest
                        //if (productDetail.InterestValueList != null)
                        //{
                        //    if (productDetail.InterestValueList.Count() > 0)
                        //    {
                        //        foreach (var subitem in productDetail.InterestValueList)
                        //        {
                        //            var amount = subitem.Amount;
                        //            foreach (var subitem1 in subitem.DurationList)
                        //            {
                        //                var durationId = subitem1.Id;
                        //                foreach (var subitem2 in subitem1.DepositBasisList)
                        //                {
                        //                    if(subitem2.ProductDurationInt != null)
                        //                    {
                        //                        var basisId = subitem2.Id;
                        //                        var interestRate = subitem2.ProductDurationInt.InterestRate;
                        //                        if (interestRate != 0)
                        //                        {
                        //                            ProductDurationInt ProductDurationInt = new ProductDurationInt();
                        //                            ProductDurationInt.InterestRate = interestRate;
                        //                            ProductDurationInt.Value = Convert.ToDouble(amount);
                        //                            ProductDurationInt.DBId = basisId;
                        //                            ProductDurationInt.ICBId = subitem2.ProductDurationInt.ICBId;
                        //                            ProductDurationInt.PId = productId;
                        //                            ProductDurationInt.DVId = durationId;

                        //                            uow.Repository<Models.Models.ProductDurationInt>().Add(ProductDurationInt);
                        //                            uow.Commit();
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }

                        //        ProductPSID productPSid = uow.Repository<Models.Models.ProductPSID>().FindBy(x => x.PID == productId).FirstOrDefault();

                        //        productPSid.PSID = Convert.ToByte(productDetail.InterestValueList.First().PolicyIntCalcId);
                        //        productPSid.IsDefault = true;
                        //        uow.Repository<Models.Models.ProductPSID>().Edit(productPSid);
                        //        uow.Commit();
                        //    }
                        //}

                        //if (productDetail.FixedDepositList != null)
                        //{
                        //    //For Fixed Deposit Adding after Editing
                        //    if (productDetail.FixedDepositList.DurationList.Count() > 0)
                        //    {
                        //        var allrecuringInterest = uow.Repository<Models.Models.ProductDurationInt>().FindBy(x => x.PId == productId);
                        //        if (allrecuringInterest.Count() > 0)
                        //        {
                        //            foreach (var subInterestitem in allrecuringInterest)
                        //            {
                        //                uow.Repository<Models.Models.ProductDurationInt>().Delete(subInterestitem);
                        //            }
                        //            uow.Commit();
                        //        }
                        //    }

                        //    if (productDetail.FixedDepositList.PolicyIntCalcId != 0)
                        //    {
                        //        var allCalcInterest = uow.Repository<Models.Models.ProductPSID>().FindBy(x => x.PID == productId);
                        //        if (allCalcInterest.Count() > 0)
                        //        {
                        //            foreach (var subInterestitem in allCalcInterest)
                        //            {
                        //                uow.Repository<Models.Models.ProductPSID>().Delete(subInterestitem);
                        //            }
                        //            uow.Commit();
                        //        }
                        //    }
                        //    foreach (var item in productDetail.FixedDepositList.DurationList)
                        //    {
                        //        foreach (var subitem in item.CaptList)
                        //        {
                        //            if (subitem.ProductDurationInt.InterestRate != null || subitem.ProductDurationInt.InterestRate != 0)
                        //            {
                        //                ProductDurationInt productDuration = new ProductDurationInt();
                        //                productDuration.InterestRate = subitem.ProductDurationInt.InterestRate;
                        //                productDuration.ICBId = subitem.ICBDurID;
                        //                productDuration.PId = productId;
                        //                productDuration.DBId = null;
                        //                productDuration.DVId = item.Id;
                        //                uow.Repository<Models.Models.ProductDurationInt>().Add(productDuration);
                        //                uow.Commit();
                        //            }
                        //        }
                        //    }
                        //    ProductPSID productPSid = new ProductPSID();
                        //    productPSid.PID = productId;
                        //    productPSid.PSID = Convert.ToByte(productDetail.FixedDepositList.PolicyIntCalcId);
                        //    productPSid.IsDefault = true;
                        //    uow.Repository<Models.Models.ProductPSID>().Add(productPSid);
                        //    uow.Commit();

                        //}

                        //if (productDetail.NormalDepositList != null)
                        //{
                        //    //This is Normal Deposit Product Without Duration
                        //    var allCaptDur = uow.Repository<Models.Models.ProductICBDur>().FindBy(x => x.PID == productId);

                        //    if (allCaptDur.Count() > 0)
                        //    {
                        //        foreach (var subInterestitem in allCaptDur)
                        //        {
                        //            uow.Repository<Models.Models.ProductICBDur>().Delete(subInterestitem);
                        //        }
                        //        uow.Commit();
                        //    }

                        //    var allCalcInterest = uow.Repository<Models.Models.ProductPSID>().FindBy(x => x.PID == productId);
                        //    if (allCalcInterest.Count() > 0)
                        //    {
                        //        foreach (var subInterestitem in allCalcInterest)
                        //        {
                        //            uow.Repository<Models.Models.ProductPSID>().Delete(subInterestitem);
                        //        }
                        //        uow.Commit();
                        //    }

                        //    //For Normal Deposit Adding after Editing
                        //    foreach (var item in productDetail.NormalDepositList.CapitalizationRuleList)
                        //    {
                        //        if (item.IsChecked == true)
                        //        {
                        //            ProductICBDur productIcb = new ProductICBDur();
                        //            productIcb.IRate = Convert.ToInt16(item.InterestRateICB);
                        //            productIcb.IsDefault = item.IsDefault;
                        //            productIcb.PID = productId;
                        //            productIcb.ICBDurID = item.ICBDurID;

                        //            uow.Repository<Models.Models.ProductICBDur>().Add(productIcb);
                        //            uow.Commit();
                        //        }
                        //    }

                        //    foreach (var item in productDetail.NormalDepositList.CalculationRuleList)
                        //    {
                        //        if (item.IsChecked == true)
                        //        {
                        //            ProductPSID productPSid = new ProductPSID();
                        //            productPSid.PID = productId;
                        //            productPSid.PSID = item.PSID;
                        //            productPSid.IsDefault = item.IsDefault;
                        //            uow.Repository<Models.Models.ProductPSID>().Add(productPSid);
                        //            uow.Commit();
                        //        }
                        //    }
                        //}

                        //if (productDetail.LoanInterestList != null)
                        //{
                        //    //This is Loan Deposit Product
                        //    var allPenalty = uow.Repository<Models.Models.ProductPCID>().FindBy(x => x.PID == productId);
                        //    foreach (var item in allPenalty)
                        //    {
                        //        uow.Repository<Models.Models.ProductPCID>().Delete(item);
                        //    }
                        //    var allRule = uow.Repository<Models.Models.ProductPay>().FindBy(x => x.PID == productId);
                        //    foreach (var item in allRule)
                        //    {
                        //        uow.Repository<Models.Models.ProductPay>().Delete(item);
                        //    }
                        //    var calcRule = uow.Repository<Models.Models.ProductPSID>().FindBy(x => x.PID == productId);
                        //    foreach (var item in calcRule)
                        //    {
                        //        uow.Repository<Models.Models.ProductPSID>().Delete(item);
                        //    }
                        //    uow.Commit();

                        //    if (productDetail.LoanInterestList != null)
                        //    {
                        //        //For Loan Product Pay Rule
                        //        foreach (var item in productDetail.LoanInterestList.RulePayList)
                        //        {
                        //            if (item.IsChecked == true)
                        //            {
                        //                ProductPay prPay = new ProductPay();
                        //                prPay.PID = productId;
                        //                prPay.PAYID = item.PAYID;
                        //                prPay.IsDefault = item.IsDefault;
                        //                uow.Repository<Models.Models.ProductPay>().Add(prPay);
                        //                uow.Commit();
                        //            }
                        //        }
                        //        //For Loan Interest Calculation
                        //        foreach (var item in productDetail.LoanInterestList.PolicyCalculationList)
                        //        {
                        //            if (item.IsChecked == true)
                        //            {
                        //                ProductPSID productPSID = new ProductPSID();
                        //                productPSID.IsDefault = item.IsDefault;
                        //                productPSID.PID = productId;
                        //                productPSID.PSID = item.PSID;
                        //                uow.Repository<Models.Models.ProductPSID>().Add(productPSID);
                        //                uow.Commit();
                        //            }
                        //        }
                        //        //For Loan Product Penalty Calculation
                        //        foreach (var item in productDetail.LoanInterestList.PolicyPenaltyList)
                        //        {
                        //            if (item.IsChecked == true)
                        //            {
                        //                ProductPCID productPCID = new ProductPCID();
                        //                productPCID.PCID = item.PCID;
                        //                productPCID.IsDefault = Convert.ToByte(item.IsDefault);
                        //                productPCID.PID = productId;
                        //                uow.Repository<Models.Models.ProductPCID>().Add(productPCID);
                        //                uow.Commit();
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                    #endregion
                }
                #endregion

                uow.Commit();
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Models.ViewModel.ProductDetailViewmodel> GetProductDetailList(int pageNo = 1, int pageSize = 10, string search = "")
        {
            List<Models.ViewModel.ProductDetailViewmodel> lstProductDetail = new List<Models.ViewModel.ProductDetailViewmodel>();
            string query = "select * from [acc].[FGetProductDetailList](" + pageNo + "," + pageSize + ",'" + search + "')";
            lstProductDetail = uow.Repository<Models.ViewModel.ProductDetailViewmodel>().SqlQuery(query).ToList();
            return lstProductDetail;
        }

        public bool Delete(int ProductDetailId)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {
                try
                {
                    var productObj = this.GetSingle(ProductDetailId);
                    if (productObj != null)
                    {
                        var schList = productObj;
                        if (productObj.SchmDetail.SType == 0)
                        {
                            var productId = productObj.PID;
                            var allrecuringInterest = uow.Repository<Models.Models.ProductDurationInt>().FindBy(x => x.PId == productId);
                            if (allrecuringInterest.Count() > 0)
                            {
                                foreach (var subInterestitem in allrecuringInterest)
                                {
                                    uow.Repository<Models.Models.ProductDurationInt>().Delete(subInterestitem);
                                }
                                uow.Commit();
                            }
                            var allProductBrnch = uow.Repository<Models.Models.ProductBrnch>().FindBy(x => x.PID == productId).ToList();

                            foreach (var subitem in allProductBrnch)
                            {
                                uow.Repository<Models.Models.ProductBrnch>().Delete(subitem);
                                uow.Commit();
                            }

                            var allcalcRecord = uow.Repository<Models.Models.ProductPSID>().FindBy(x => x.PID == productId);
                            if (allcalcRecord.Count() > 0)
                            {
                                foreach (var subInterestCalcitem in allcalcRecord)
                                {
                                    uow.Repository<Models.Models.ProductPSID>().Delete(subInterestCalcitem);
                                }
                                uow.Commit();
                            }
                            var allPCIdRecord = uow.Repository<Models.Models.ProductPCID>().FindBy(x => x.PID == productId);
                            if (allPCIdRecord.Count() > 0)
                            {
                                foreach (var subInterestCalcitem in allPCIdRecord)
                                {
                                    uow.Repository<Models.Models.ProductPCID>().Delete(subInterestCalcitem);
                                }
                                uow.Commit();
                            }

                            var allRulePayRecord = uow.Repository<Models.Models.ProductPay>().FindBy(x => x.PID == productId);
                            if (allRulePayRecord.Count() > 0)
                            {

                                foreach (var subInterestCalcitem in allRulePayRecord)
                                {

                                    uow.Repository<Models.Models.ProductPay>().Delete(subInterestCalcitem);


                                }
                                uow.Commit();

                            }
                            var allRulepayRecord = uow.Repository<Models.Models.ProductPCID>().FindBy(x => x.PID == productId);
                            if (allPCIdRecord.Count() > 0)
                            {

                                foreach (var subInterestCalcitem in allPCIdRecord)
                                {

                                    uow.Repository<Models.Models.ProductPCID>().Delete(subInterestCalcitem);


                                }
                                uow.Commit();

                            }


                            var allNormalProductRecord = uow.Repository<Models.Models.ProductICBDur>().FindBy(x => x.PID == productId);
                            if (allNormalProductRecord.Count() > 0)
                            {

                                foreach (var subInterestCalcitem in allNormalProductRecord)
                                {

                                    uow.Repository<Models.Models.ProductICBDur>().Delete(subInterestCalcitem);


                                }
                                uow.Commit();

                            }
                            int finaccId = productObj.FID;
                            ProductDetail schm = uow.Repository<Models.Models.ProductDetail>().GetSingle(x => x.PID == productObj.PID);
                            uow.Repository<Models.Models.ProductDetail>().Delete(schm);

                            for (int i = finaccId; i < (finaccId + 5); i++)
                            {
                                var finaccLedger = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                uow.Repository<Models.Models.FinAcc>().Delete(finaccLedger);
                                uow.Commit();



                            }

                        }
                        else
                        {

                            var productId = productObj.PID;
                            var allrecuringInterest = uow.Repository<Models.Models.ProductDurationInt>().FindBy(x => x.PId == productId);
                            if (allrecuringInterest.Count() > 0)
                            {

                                foreach (var subInterestitem in allrecuringInterest)
                                {

                                    uow.Repository<Models.Models.ProductDurationInt>().Delete(subInterestitem);


                                }
                                uow.Commit();

                            }

                            var allcalcRecord = uow.Repository<Models.Models.ProductPSID>().FindBy(x => x.PID == productId);
                            if (allcalcRecord.Count() > 0)
                            {

                                foreach (var subInterestCalcitem in allcalcRecord)
                                {

                                    uow.Repository<Models.Models.ProductPSID>().Delete(subInterestCalcitem);


                                }
                                uow.Commit();

                            }


                            var allPCIdRecord = uow.Repository<Models.Models.ProductPCID>().FindBy(x => x.PID == productId);
                            if (allPCIdRecord.Count() > 0)
                            {

                                foreach (var subInterestCalcitem in allPCIdRecord)
                                {

                                    uow.Repository<Models.Models.ProductPCID>().Delete(subInterestCalcitem);


                                }
                                uow.Commit();

                            }

                            var allRulePayRecord = uow.Repository<Models.Models.ProductPay>().FindBy(x => x.PID == productId);
                            if (allRulePayRecord.Count() > 0)
                            {

                                foreach (var subInterestCalcitem in allRulePayRecord)
                                {

                                    uow.Repository<Models.Models.ProductPay>().Delete(subInterestCalcitem);


                                }
                                uow.Commit();

                            }
                            var allRulepayRecord = uow.Repository<Models.Models.ProductPCID>().FindBy(x => x.PID == productId);
                            if (allPCIdRecord.Count() > 0)
                            {

                                foreach (var subInterestCalcitem in allPCIdRecord)
                                {

                                    uow.Repository<Models.Models.ProductPCID>().Delete(subInterestCalcitem);


                                }
                                uow.Commit();

                            }


                            var allNormalProductRecord = uow.Repository<Models.Models.ProductICBDur>().FindBy(x => x.PID == productId);
                            if (allNormalProductRecord.Count() > 0)
                            {

                                foreach (var subInterestCalcitem in allNormalProductRecord)
                                {

                                    uow.Repository<Models.Models.ProductICBDur>().Delete(subInterestCalcitem);


                                }
                                uow.Commit();

                            }
                            var allProductBrnch = uow.Repository<Models.Models.ProductBrnch>().FindBy(x => x.PID == productId).ToList();

                            foreach (var subitem in allProductBrnch)
                            {
                                uow.Repository<Models.Models.ProductBrnch>().Delete(subitem);
                                uow.Commit();
                            }
                            int finaccId = productObj.FID;
                            ProductDetail schm = uow.Repository<Models.Models.ProductDetail>().GetSingle(x => x.PID == productObj.PID);
                            uow.Repository<Models.Models.ProductDetail>().Delete(schm);


                            for (int i = finaccId; i < (finaccId + 11); i++)
                            {
                                var finaccLedger = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                uow.Repository<Models.Models.FinAcc>().Delete(finaccLedger);
                                uow.Commit();

                            }


                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Rollback all Transactions
                }
                return false;
            }
        }

        public List<SelectListItem> GetSchemeTYpe()
        {
            var F2List = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == 12 || x.F1id == 13);
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in F2List)
            {
                var finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FinAcc>().GetAll().Where(x => x.F2Type == item.F2id).FirstOrDefault();
                lst.Add(new SelectListItem { Text = finaccList.Fname, Value = finaccList.Fid.ToString() });

            }
            return lst;
        }

        public List<SelectListItem> GetCapitalizationRule(int? selectedId = 0)
        {
            var capList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.RuleICBDuration>().GetAll();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in capList)
            {
                if (item.ICBDurID == selectedId)
                {
                    lst.Add(new SelectListItem { Text = item.ICBDurNam, Value = item.ICBDurID.ToString(), Selected = true });
                }
                else
                {

                    lst.Add(new SelectListItem { Text = item.ICBDurNam, Value = item.ICBDurID.ToString() });
                }

            }
            return lst;
        }

        //public List<RuleICBDuration> GetCaptModel()
        //{
        //    var caplist = uow.Repository<Models.Models.RuleICBDuration>().GetAll().ToList();
        //    return caplist;
        //}

        public List<SelectListItem> GetCalculationRule(int? selectedId = 0)
        {
            var calList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.PolicyIntCalc>().GetAll();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in calList)
            {
                if (item.PSID == selectedId)
                {
                    lst.Add(new SelectListItem { Text = item.PSName, Value = item.PSID.ToString(), Selected = true });
                }
                else
                {
                    lst.Add(new SelectListItem { Text = item.PSName, Value = item.PSID.ToString() });
                }

            }
            return lst;
        }

        public List<SelectListItem> GetPenaltyCalculationRule()
        {
            var calList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.PolicyPenCalc>().GetAll();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in calList)
            {

                lst.Add(new SelectListItem { Text = item.PCNAME, Value = item.PCID.ToString() });

            }
            return lst;
        }

        //public string GetAddress(int Models.Models.ProductDetailId)
        //{
        //    string result = "";

        //    if (Models.Models.ProductDetailId != 0)
        //    {
        //        Models.Models.ProductDetail mnu = new Models.Models.ProductDetail();
        //        mnu = GetSingle(Models.Models.ProductDetailId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.Models.Models.ProductDetailName);
        //            mnu = GetSingle(mnu.PModels.Models.ProductDetailId);
        //        };

        //        var sorted = lst.Select((x, i) => new KeyValuePair<string, int>(x, i)).OrderByDescending(x => x.Value).ToList();

        //        foreach (var item in sorted)
        //        {
        //            if (result == "")
        //            {
        //                result = result + item.Key;
        //            }
        //            else
        //            {
        //                result = result + "/" + item.Key;
        //            }

        //        }
        //    }
        //    else
        //    {
        //        result = "Root";
        //    }
        //    return result;
        //}

        public static string GetSchemeNameFromSDID(int SDID)
        {
            var SchemeName = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == SDID).SDName;
            return SchemeName;
        }

        public bool GetSchemeHasDurationFromPID(int ProductId)
        {
            var schemeId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductDetail>().GetSingle(x => x.PID == ProductId).SDID;
            var schemeDetail = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeId).HasDuration;
            return schemeDetail;
        }

        public bool? GetSchemeMultiDepFromPID(int ProductId)
        {
            var schemeId = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductDetail>().GetSingle(x => x.PID == ProductId).SDID;
            var multiDep = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeId).MultiDeposit;
            return multiDep;
        }

        public IEnumerable<ProductDurationInt> GetInterestValueFromPID(int ProductId)
        {
            var interestVal = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductDurationInt>().GetAll().Where(x => x.PId == ProductId);
            return interestVal;
        }

        public double getInterestFromICBDurID(byte ICBDurID, int Pid)
        {
            var IRate = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.ProductICBDur>().GetSingle(x => x.ICBDurID == ICBDurID && x.PID == Pid).IRate;
            return Convert.ToDouble(IRate);
        }

    }
}