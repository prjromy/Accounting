using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannakyaAccounting.Models.Models;
using System.Web.Mvc;
using System.Data;
using System.Web;
using System.Transactions;
using System.Data.Entity.Validation;

namespace ChannakyaAccounting.Service.FinAcc
{
    public class FinAccService : IDisposable
    {
        private GenericUnitOfWork uow = null;


        public FinAccService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<Models.Models.FinAcc> GetAll()
        {
            return uow.Repository<Models.Models.FinAcc>().GetAll().ToList();
        }
        public Models.Models.RuleICBDuration GetICBRule(int id)
        {
            return uow.Repository<Models.Models.RuleICBDuration>().GetSingle(x => x.ICBDurID == id);
        }
        public Models.Models.RuleDuration GetDepositBasis(int id)
        {
            return uow.Repository<Models.Models.RuleDuration>().GetSingle(x => x.DURID == id);
        }

        public List<SelectListItem> GetSubsiTitle()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Models.Models.SubsiSetup>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.Title, Value = item.SSId.ToString() });
            }
            return lst;
        }
        public List<Models.Models.FinAcc> GetAllOfParent(int parentId)
        {
            //if(parentId == 0)
            //{
            //    //it was getall() and where()
            //    return uow.Repository<Models.Models.FinAcc>().GetAll().Where(x => x.Pid == parentId).ToList();
            //}
            //else
            //{
                //changed it to findby
                return uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Pid == parentId).ToList();
            //}
        }

        public Models.Models.FinAcc GetSingle(int FinAccId)
        {
            Models.Models.FinAcc FinAcc = uow.Repository<Models.Models.FinAcc>().GetSingle(c => c.Fid == FinAccId);
            return FinAcc;
        }
        public decimal GetTotalLedgerBalance(int ledgerId)
        {
            decimal finalAmount = 0;
            var context = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().GetContext();
            string connectionString = context.Database.Connection.ConnectionString;
            string query = "select [dbo].[GetTotalBalanceOfLedger]("+ledgerId+","+Loader.Models.Global.SelectedFYID+","+Loader.Models.Global.BranchId+")";
            var returnedObj = context.Database.SqlQuery<Nullable<decimal>>(
                       query).ToList();
            if (returnedObj[0] != null)
            {
                finalAmount = Convert.ToInt32(returnedObj[0]);
            }
            return finalAmount;
            //return uow.Repository<int>().
        }

        public List<SelectListItem> GetSector()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Models.Models.SectorDef>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.CDepSectorNam, Value = item.CDepSector.ToString() });
            }
            lst = lst.OrderBy(x=>x.Text).ToList();
            return lst;
        }
        //public List<SelectListItem> GetDimensionOrder(int totalCount)
        //{
        //    List<SelectListItem> lst = new List<SelectListItem>();
        //    for (int i = 1; i <= totalCount; i++)
        //    {
        //        lst.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
        //    }

        //    return lst;
        //}

        public void EditInterestRateProduct(ChannakyaAccounting.Models.Models.ProductDurationInt productDurationObj,ProductICBDur productICBDur)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            try
            {
                if (productDurationObj != null && productDurationObj.PId != 0)
                {
                    #region RecurringAndFixed
                    if (productDurationObj.Id == 0)
                    {

                        var productDurObj = uow.Repository<Models.Models.ProductDurationInt>().GetSingle(x => x.Id == productDurationObj.Id);

                        if (productDurationObj != null)
                        {
                            productDurObj.InterestRate = productDurationObj.InterestRate;
                            if (productDurationObj.EffectiveFrom <= Loader.Models.Global.TransactionDate)
                            {
                                editUOW.Repository<Models.Models.ProductDurationInt>().Edit(productDurObj);
                            }

                            Models.Models.InterestLog interestLogs = new InterestLog();
                            if (productDurationObj.DBId != null)
                            {
                                interestLogs.DBId = productDurationObj.DBId;
                            }
                            interestLogs.DVId = productDurationObj.DVId;
                            interestLogs.ICBId = productDurationObj.ICBId;
                            interestLogs.InterestRate = (decimal)productDurationObj.InterestRate;
                            interestLogs.PId = productDurationObj.PId;
                            if (productDurationObj.Value != null)
                            {
                                interestLogs.Value = productDurationObj.Value;
                            }
                            interestLogs.EffectiveFrom = productDurationObj.EffectiveFrom;
                            interestLogs.PostedBy = Loader.Models.Global.UserId;
                            interestLogs.PostedOn = DateTime.Now;
                            editUOW.Repository<Models.Models.InterestLog>().Add(interestLogs);


                        }
                    }
                    else
                    {
                        if (productDurationObj.EffectiveFrom <= Loader.Models.Global.TransactionDate)
                        {
                            var productDurObj = uow.Repository<Models.Models.ProductDurationInt>().GetSingle(x => x.Id == productDurationObj.Id);
                            if (productDurationObj != null)
                            {
                                productDurObj.InterestRate = productDurationObj.InterestRate;
                                productDurObj.ICBId = productDurationObj.ICBId;
                                editUOW.Repository<Models.Models.ProductDurationInt>().Edit(productDurObj);
                            }
                        }

                        Models.Models.InterestLog interestLogs = new InterestLog();
                        if (productDurationObj.DBId != null)
                        {
                            interestLogs.DBId = productDurationObj.DBId;
                        }
                        interestLogs.DVId = productDurationObj.DVId;
                        interestLogs.ICBId = productDurationObj.ICBId;
                        interestLogs.InterestRate = (decimal)productDurationObj.InterestRate;
                        interestLogs.PId = productDurationObj.PId;
                        if (productDurationObj.Value != null)
                        {
                            interestLogs.Value = productDurationObj.Value;
                        }
                        interestLogs.EffectiveFrom = productDurationObj.EffectiveFrom;
                        interestLogs.PostedBy = Loader.Models.Global.UserId;
                        interestLogs.PostedOn = DateTime.Now;
                        editUOW.Repository<Models.Models.InterestLog>().Add(interestLogs);
                    }
                }
                #endregion
                else
                {
                    //For Normal Deposit of Product

                    #region NormalDeposit

                    if (productICBDur != null)
                    {
                        if (productICBDur.PICBDurID != 0)
                        {
                            var productICBDAO = editUOW.Repository<ProductICBDur>().GetSingle(x => x.PICBDurID == productICBDur.PICBDurID);
                            if (productICBDAO != null)
                            {
                                if (productICBDur.EffectiveFrom <= Loader.Models.Global.TransactionDate)
                                {
                                    productICBDAO.IRate = productICBDur.IRate;
                                    editUOW.Repository<ProductICBDur>().Edit(productICBDAO);
                                }
                                Models.Models.InterestLog interestLogs = new InterestLog();

                                interestLogs.ICBId = productICBDAO.ICBDurID;
                                interestLogs.InterestRate = (decimal)productICBDur.IRate;
                                interestLogs.PId = productICBDAO.PID;
                                interestLogs.EffectiveFrom = productICBDur.EffectiveFrom;
                                interestLogs.PostedBy = Loader.Models.Global.UserId;
                                interestLogs.PostedOn = DateTime.Now;
                                editUOW.Repository<Models.Models.InterestLog>().Add(interestLogs);

                            }
                        }
                        else
                        {

                            productICBDur.IRate = productICBDur.IRate;
                            editUOW.Repository<ProductICBDur>().Add(productICBDur);

                            Models.Models.InterestLog interestLogs = new InterestLog();
                            interestLogs.ICBId = productICBDur.ICBDurID;
                            interestLogs.InterestRate = (decimal)productICBDur.IRate;
                            interestLogs.PId = productICBDur.PID;
                            interestLogs.EffectiveFrom = productICBDur.EffectiveFrom;
                            interestLogs.PostedBy = Loader.Models.Global.UserId;
                            interestLogs.PostedOn = DateTime.Now;
                            editUOW.Repository<Models.Models.InterestLog>().Add(interestLogs);

                        }
                    }
            
                        #endregion
                }
                editUOW.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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


        public void Save(Models.Models.FinAcc FinAcc)
        {
            //var scope = new TransactionScope();

            GenericUnitOfWork edituow = new GenericUnitOfWork();
            // System.Data.Entity.DbContext context = new DbContext();

            //var scope = new TransactionScope();

            int onlyFinacc = 0;
            //using (ChannakyaAccEntities adventureWorks = new ChannakyaAccEntities())
            //using (DbContextTransaction transaction = adventureWorks.Database.BeginTransaction(
            //    IsolationLevel.ReadUncommitted))
            //using (var scope = new TransactionScope))
            //{ 
            using (var scope = new TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            {

                try
                {
                    var currencyType = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == FinAcc.Pid).Select(x => x.F2Type).Single();
                    var F2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F2id == currencyType).Select(x => x.F1id).Single();
                    Models.Models.FinAcc finacc = new Models.Models.FinAcc();

                    #region CreateNew

                    if (FinAcc.Fid == 0)
                    {
                        int checkExists = edituow.Repository<Models.Models.FinAcc>().GetAll().Where(x => x.Fid != FinAcc.Fid && x.Fname == FinAcc.Fname && x.Pid == FinAcc.Pid).Count();
                        if (checkExists > 0)
                        {
                            throw new Exception("Duplicate FinAcc Found. FinAcc Caption Not Valid");
                        }

                        #region DimensionDefination

                        if (FinAcc.DimensionDefinationList != null)
                        {
                            if (FinAcc.DimensionDefinationList.Count() > 0)
                            {
                                var finaccList = FinAcc.DimensionDefinationList;


                                DimensionVSLedger DimensionvsLedger = new DimensionVSLedger();

                                ICollection<DimensionVSLedger> dimObj = FinAcc.DimensionDefinationList.Where(x => x.isSelected == true).OrderBy(x => x.Order).Select(x => new DimensionVSLedger
                                {

                                    DDID = x.DDId
                                }).AsEnumerable().ToList();
                                int dOrder = 0;
                                foreach (var subcategory in dimObj)
                                {
                                    subcategory.DOrder = Convert.ToByte(dOrder);
                                    FinAcc.DimensionVSLedgers.Add(subcategory);
                                    dOrder = dOrder + 1;
                                }

                            }
                        }

                        #endregion

                        #region Scheme

                        if (FinAcc.SchmDetails != null)
                        {
                            if (FinAcc.SchmDetails.Count() > 0)
                            {
                                onlyFinacc++;

                                #region ForeignScheme

                                List<int> foreignschemeList = new List<int>();
                                foreignschemeList.Add(12);
                                foreignschemeList.Add(17);
                                foreignschemeList.Add(19);
                                foreignschemeList.Add(21);
                                foreignschemeList.Add(23);
                                if (FinAcc.SchmDetails.FirstOrDefault().HasOverdraw)
                                {
                                    foreignschemeList.Add(28);
                                    foreignschemeList.Add(29);
                                    foreignschemeList.Add(30);
                                    foreignschemeList.Add(34);
                                }

                                if (F2Id == 12)
                                {
                                    int count = 0;
                                    foreach (var itemType in foreignschemeList)
                                    {
                                        var finsys2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == itemType).Select(x => x.F2id).FirstOrDefault();
                                        var finaccIdcheck = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == finsys2Id);

                                        var finaccId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == finsys2Id).FirstOrDefault().Fid;


                                        count++;
                                        Models.Models.FinAcc finaccLiab = new Models.Models.FinAcc();
                                        var ledgerName = FinAcc.Fname + "(" + LedgerName(itemType) + ")";
                                        finaccLiab.Fname = ledgerName;
                                        finaccLiab.Alias = FinAcc.Alias;
                                        finaccLiab.Content = FinAcc.Content;
                                        if (itemType == 12)
                                        {
                                            finaccLiab.F2Type = FinAcc.F2Type;
                                        }
                                        else
                                        {
                                            finaccLiab.F2Type = finsys2Id;
                                        }
                                        finaccLiab.DebitRestriction = FinAcc.DebitRestriction;
                                        finaccLiab.CreditRestriction = FinAcc.CreditRestriction;
                                        finaccLiab.Code = FinAcc.Code;
                                        finaccLiab.Pid = finaccId;
                                        if (count == 1)
                                        {
                                            SchmDetail schmDetail = new SchmDetail();
                                            schmDetail = FinAcc.SchmDetails.Single();
                                            finaccLiab.SchmDetails.Add(schmDetail);
                                            schmDetail.SType = 0;
                                            schmDetail.SEnable = true;
                                            schmDetail.SDName = FinAcc.Fname;
                                        }
                                        uow.Repository<Models.Models.FinAcc>().Add(finaccLiab);
                                        uow.Commit();


                                    }
                                }

                                #endregion

                                #region LocalScheme

                                List<int> localschemeList = new List<int>();

                                localschemeList.Add(13);
                                localschemeList.Add(16);
                                localschemeList.Add(18);
                                localschemeList.Add(20);
                                localschemeList.Add(22);
                                if(FinAcc.SchmDetails.FirstOrDefault().HasOverdraw)
                                {
                                    localschemeList.Add(28);
                                    localschemeList.Add(29);
                                    localschemeList.Add(30);
                                    localschemeList.Add(34);
                                }

                                if (F2Id == 13)
                                {
                                    int count = 0;
                                    foreach (var subitemType in localschemeList)
                                    {
                                        var finsys2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == subitemType).Select(x => x.F2id).FirstOrDefault();
                                        var finaccId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == finsys2Id).FirstOrDefault().Fid;

                                        count++;
                                        Models.Models.FinAcc finaccLiab = new Models.Models.FinAcc();
                                        var ledgerName = FinAcc.Fname + "(" + LedgerName(subitemType) + ")";
                                        finaccLiab.Fname = ledgerName;
                                        finaccLiab.Content = FinAcc.Content;
                                        finaccLiab.Alias = FinAcc.Alias;
                                        if (subitemType == 13)
                                        {
                                            finaccLiab.F2Type = FinAcc.F2Type;
                                        }
                                        else
                                        {
                                            finaccLiab.F2Type = finsys2Id;
                                        }
                                        finaccLiab.DebitRestriction = FinAcc.DebitRestriction;
                                        finaccLiab.CreditRestriction = FinAcc.CreditRestriction;
                                        finaccLiab.Code = FinAcc.Code;
                                        finaccLiab.Pid = finaccId;
                                        if (count == 1)
                                        {
                                            SchmDetail schmDetail = new SchmDetail();
                                            schmDetail = FinAcc.SchmDetails.Single();
                                            schmDetail.SDName = FinAcc.Fname;
                                            schmDetail.SType = 0;
                                            schmDetail.SEnable = true;
                                            finaccLiab.SchmDetails.Add(schmDetail);
                                        }
                                        uow.Repository<Models.Models.FinAcc>().Add(finaccLiab);
                                        uow.Commit();


                                    }
                                }

                                #endregion

                                #region LoanScheme

                                if (F2Id == 124)
                                {
                                    List<int> loanScheme = new List<int>();
                                    loanScheme.Add(124);
                                    for (int i = 127; i <= 136; i++)
                                    {
                                        loanScheme.Add(i);
                                    }


                                    int count = 0;
                                    foreach (var subitemType in loanScheme)
                                    {
                                        var finsys2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == subitemType).Select(x => x.F2id).FirstOrDefault();
                                        var finaccId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == finsys2Id).FirstOrDefault().Fid;
                                        count++;
                                        Models.Models.FinAcc finaccLiab = new Models.Models.FinAcc();
                                        var ledgerName = FinAcc.Fname + "(" + LedgerName(subitemType) + ")";
                                        finaccLiab.Fname = ledgerName;
                                        finaccLiab.Content = FinAcc.Content;
                                        finaccLiab.Alias = FinAcc.Alias;
                                        if (subitemType == 124)
                                        {
                                            finaccLiab.F2Type = FinAcc.F2Type;
                                        }
                                        else
                                        {
                                            finaccLiab.F2Type = finsys2Id;
                                        }
                                        finaccLiab.DebitRestriction = FinAcc.DebitRestriction;
                                        finaccLiab.CreditRestriction = FinAcc.CreditRestriction;
                                        finaccLiab.Code = FinAcc.Code;
                                        finaccLiab.Pid = finaccId;
                                        if (count == 1)
                                        {
                                            SchmDetail schmDetail = new SchmDetail();
                                            schmDetail = FinAcc.SchmDetails.Single();
                                            schmDetail.SDName = FinAcc.Fname;
                                            schmDetail.SType = 1;
                                            schmDetail.SEnable = true;
                                            finaccLiab.SchmDetails.Add(schmDetail);
                                        }
                                        uow.Repository<Models.Models.FinAcc>().Add(finaccLiab);
                                        uow.Commit();

                                    }
                                }

                                #endregion
                            }
                        }

                        #endregion

                        #region Product

                        if (FinAcc.ProductDetails != null)
                        {
                            if (FinAcc.ProductDetails.Count() > 0)
                            {
                                int schemeId = FinAcc.ProductDetails.FirstOrDefault().SDID;
                                onlyFinacc++;

                                var schemeofProduct = uow.Repository<Models.Models.SchmDetail>().FindBy(x => x.SDID == schemeId).FirstOrDefault();

                                var F1ID = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F2id == FinAcc.F2Type).Select(x => x.F1id).Single();
                                var productParntList = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == FinAcc.Pid).Pid;
                                int val = Convert.ToInt32(FinAcc.Pid);
                                int sum = 0;
                                List<int> finalProductList = new List<int>();
                                finalProductList.Add(val);
                                int finalval = val;

                                if (F1ID == 15)
                                {
                                    if(schemeofProduct!=null && schemeofProduct.HasOverdraw)
                                    {
                                        for (int i = 0; i < 7; i++)
                                        {
                                            finalval = finalval + 1;
                                            finalProductList.Add(finalval);
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < 4; i++)
                                        {
                                            finalval = finalval + 1;
                                            finalProductList.Add(finalval);
                                        }
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
                                    var finaccPid = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == item).F2Type;
                                    var f2Type = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == finaccPid).SingleOrDefault().F2id;

                                    var f1Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == finaccPid).F1id;
                                    count++;
                                    Models.Models.FinAcc productList = new Models.Models.FinAcc();
                                    var ledgerName = FinAcc.Fname + "(" + LedgerName(f1Id) + ")";

                                    productList.Fname = ledgerName;
                                    productList.Content = FinAcc.Content;
                                    productList.Alias = FinAcc.Alias;
                                    if (count == 1)
                                    {
                                        productList.F2Type = FinAcc.F2Type;
                                    }
                                    else
                                    {
                                        productList.F2Type = f2Type;
                                    }
                                    productList.DebitRestriction = FinAcc.DebitRestriction;
                                    productList.CreditRestriction = FinAcc.CreditRestriction;
                                    productList.Code = FinAcc.Code;
                                    productList.Pid = item;
                                    if (FinAcc.DimensionDefinationList != null)
                                    {
                                        ICollection<DimensionVSLedger> dimObj = FinAcc.DimensionDefinationList.Where(x => x.isSelected == true).Select(x => new DimensionVSLedger
                                        {

                                            DDID = x.DDId
                                        }).AsEnumerable().ToList();
                                        productList.DimensionVSLedgers = dimObj;
                                    }

                                    if (count == 1)
                                    {

                                        ProductDetail productModel = new ProductDetail();
                                        productModel = FinAcc.ProductDetails.Single();
                                        productModel.Apfix = FinAcc.Code;
                                        productModel.PName = FinAcc.Fname;
                                        var sdId = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.FID == (FinAcc.Pid)).SDID;
                                        productModel.SDID = sdId;
                                        productModel.enabled = true;
                                        productList.ProductDetails.Add(productModel);


                                    }


                                    uow.Repository<Models.Models.FinAcc>().Add(productList);
                                    uow.Commit();

                                    if (count == 1)
                                    {

                                        ProductBrnch productBrnch = new ProductBrnch();

                                        var productId = productList.ProductDetails.First().PID;
                                        productBrnch.PID = productId;
                                        productBrnch.BrnchID = Loader.Models.Global.BranchId;
                                        productBrnch.LastAcNo = FinAcc.ProductDetails.FirstOrDefault().StartAcNo;
                                        productBrnch.StartAcNo = FinAcc.ProductDetails.FirstOrDefault().StartAcNo;
                                        uow.Repository<Models.Models.ProductBrnch>().Add(productBrnch);
                                        uow.Commit();

                                        if (FinAcc.ProductDetails.FirstOrDefault().InterestValueList != null)
                                        {
                                            var ProductInterest = FinAcc.ProductDetails.FirstOrDefault().InterestValueList;
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
                                                        //if (interestRate != null)
                                                        //{


                                                        ProductDurationInt ProductDurationInt = new ProductDurationInt();
                                                        if (interestRate == null)
                                                        {
                                                            interestRate = 0;
                                                        }
                                                        ProductDurationInt.InterestRate = interestRate;
                                                        ProductDurationInt.Value = Convert.ToDouble(amount);
                                                        ProductDurationInt.DBId = basisId;
                                                        ProductDurationInt.ICBId = subitem2.ProductDurationInt.ICBId;
                                                        ProductDurationInt.PId = productId;
                                                        ProductDurationInt.DVId = durationId;

                                                        uow.Repository<Models.Models.ProductDurationInt>().Add(ProductDurationInt);
                                                        uow.Commit();

                                                        //}



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
                                        if (FinAcc.ProductDetails.FirstOrDefault().FixedDepositList != null)
                                        {
                                            if (FinAcc.ProductDetails.FirstOrDefault().FixedDepositList.DurationList.Count() > 0)
                                            {
                                                var fixedproductId = productList.ProductDetails.First().PID;
                                                var fixedcollection = FinAcc.ProductDetails.FirstOrDefault().FixedDepositList;
                                                foreach (var fixeditems in fixedcollection.DurationList)
                                                {

                                                    foreach (var fixedsubitem in fixeditems.CaptList)
                                                    {

                                                        ProductDurationInt productDurationint = new ProductDurationInt();
                                                        productDurationint.DVId = fixeditems.Id;
                                                        productDurationint.InterestRate = fixedsubitem.ProductDurationInt.InterestRate;
                                                        productDurationint.ICBId = fixedsubitem.ICBDurID;
                                                        productDurationint.PId = fixedproductId;
                                                        if (productDurationint.InterestRate ==null)
                                                        {
                                                            productDurationint.InterestRate = 0;
                                                        }
                                                        uow.Repository<Models.Models.ProductDurationInt>().Add(productDurationint);
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
                                        if (FinAcc.ProductDetails.FirstOrDefault().LoanInterestList != null)
                                        {
                                            if (FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.PolicyCalculationList.Count() > 0)
                                            {
                                                var fixedproductId = productList.ProductDetails.First().PID;
                                                var policyCalc = FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.PolicyCalculationList;
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

                                            if (FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.PolicyPenaltyList != null)
                                            {
                                                if (FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.PolicyPenaltyList.Count() > 0)
                                                {
                                                    var fixedproductId = productList.ProductDetails.First().PID;
                                                    var policyCalc = FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.PolicyPenaltyList;
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
                                            if (FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.RulePayList != null)
                                            {
                                                if (FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.RulePayList.Count() > 0)
                                                {
                                                    var fixedproductId = productList.ProductDetails.First().PID;
                                                    var policyCalc = FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.RulePayList;
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


                                        if (FinAcc.ProductDetails.FirstOrDefault().NormalDepositList != null)
                                        {
                                            if (FinAcc.ProductDetails.FirstOrDefault().NormalDepositList.CalculationRuleList.Count() > 0)
                                            {

                                                var normalProductId = productList.ProductDetails.First().PID;
                                                var normalCollection = FinAcc.ProductDetails.FirstOrDefault().NormalDepositList;

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
                                                    else
                                                    {
                                                        ProductICBDur productIcbDur = new ProductICBDur();
                                                        productIcbDur.ICBDurID = Convert.ToByte(normalitems.ICBDurID);
                                                        productIcbDur.IRate = 0;
                                                        productIcbDur.PID = normalProductId;
                                                        productIcbDur.IsDefault = false;
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
                                }
                                //var fId = FinAcc.Fid;
                                //FinAcc.DimensionVSLedgers.All(x => x.Fid == fId);
                                //List<DimensionVSLedger> dim = new List<DimensionVSLedger>();
                                //dim = FinAcc.DimensionVSLedgers;



                            }
                        }

                        #endregion

                        #region SaveLedgerOnlyOnFinAccWithNoExternalParameters

                        if (onlyFinacc == 0)
                        {
                            uow.Repository<Models.Models.FinAcc>().Add(FinAcc);

                        }
                        #endregion

                    }
                    #endregion

                    #region EditLedger
                    else
                    {

                        var imageContent = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == FinAcc.Fid).Content;
                        using (var uow = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork())

                        {

                            #region Subsi
                            if (FinAcc.SubsiVSFIds != null)
                            {
                                if (FinAcc.SubsiVSFIds.Count() > 0)
                                {
                                    foreach (var item in FinAcc.SubsiVSFIds)
                                    {
                                        SubsiVSFId subsivsFId = new SubsiVSFId();
                                        subsivsFId.SSId = item.SSId;
                                        subsivsFId.SFId = item.SFId;
                                        subsivsFId.Fid = item.Fid;
                                        uow.Repository<SubsiVSFId>().Edit(subsivsFId);
                                        uow.Commit();

                                    }

                                }
                            }
                            #endregion

                            #region FinAcc
                            var FinAccdata = new Models.Models.FinAcc
                            {
                                Fid = FinAcc.Fid,
                                Fname = FinAcc.Fname,
                                F2Type = FinAcc.F2Type,
                                Pid = FinAcc.Pid,
                                Code = FinAcc.Code,
                                Content = FinAcc.Content

                            };
                            uow.Repository<Models.Models.FinAcc>().Edit(FinAccdata);

                            uow.Commit();

                            #endregion

                            #region Dimensions

                            if (FinAcc.DimensionDefinationList != null)
                            {
                                var dimsList = FinAcc.DimensionDefinationList.OrderBy(x => x.Order).ToList();

                                foreach (var item in dimsList)
                                {
                                    if (item.isSelected == true /*&& item.DLId == 0*/)
                                    {
                                        DimensionVSLedger dimVsLedger = new DimensionVSLedger();
                                        dimVsLedger.Fid = FinAcc.Fid;
                                        dimVsLedger.DDID = item.DDId;
                                        dimVsLedger.DOrder = Convert.ToByte(item.Order);
                                        DimensionVSLedger DimVsLedger = uow.Repository<Models.Models.DimensionVSLedger>().FindBy(x => x.DDID == item.DDId && x.Fid == FinAcc.Fid).FirstOrDefault();
                                        if (DimVsLedger == null)
                                        {
                                            uow.Repository<DimensionVSLedger>().Add(dimVsLedger);
                                            uow.Commit();

                                        }



                                    }
                                    else
                                    {
                                        if (item.isSelected == false/* && item.DLId != 0*/)
                                        {
                                            DimensionVSLedger DimVsLedger = uow.Repository<Models.Models.DimensionVSLedger>().FindBy(x => x.DDID == item.DDId && x.Fid == FinAcc.Fid).FirstOrDefault();
                                            if (DimVsLedger != null)
                                            {
                                                uow.Repository<DimensionVSLedger>().Delete(DimVsLedger);
                                                uow.Commit();
                                            }

                                        }
                                    }
                                }
                            }

                            #endregion


                            #region BankInfo

                            if (FinAcc.BankInfoes.Count() != 0)
                            {

                                BankInfo bankObj = new BankInfo();
                                bankObj.Fid = FinAcc.Fid;

                                foreach (var item in FinAcc.BankInfoes)
                                {
                                    bankObj.Bid = item.Bid;
                                    bankObj.Branch = item.Branch;
                                    bankObj.ContactPerson = item.ContactPerson;
                                    bankObj.InterestRate = item.InterestRate;
                                    bankObj.InterestRuleId = item.InterestRuleId;
                                    bankObj.IsFixed = item.IsFixed;
                                    bankObj.MatureDate = item.MatureDate;
                                    bankObj.MinimumBalance = item.MinimumBalance;
                                    bankObj.OpenDate = (DateTime)Loader.Models.Global.TransactionDate;
                                    bankObj.PhoneNo = item.PhoneNo;
                                }
                                //FinAccdata.BankInfoes.Add(bankObj);
                                uow.Repository<BankInfo>().Edit(bankObj);

                            }

                            #endregion

                            //table removed with discussion with sir
                            //#region SubsiTitle

                            //if (FinAcc.SubsiTitles.Count() != 0)
                            //{

                            //    foreach (var item in FinAcc.SubsiTitles)
                            //    {
                            //        SubsiTitle subsiObj = new SubsiTitle();
                            //        subsiObj.FId = FinAcc.Fid;
                            //        subsiObj.Slength = item.Slength;
                            //        subsiObj.STid = item.STid;
                            //        subsiObj.STitleName = item.STitleName;
                            //        subsiObj.STPrefix = item.STPrefix;
                            //        subsiObj.CurrentNo = item.CurrentNo;
                            //        uow.Repository<SubsiTitle>().Edit(subsiObj);

                            //    }

                            //}

                            //#endregion

                            #region Scheme
                            if (FinAcc.SchmDetails.Count() > 0)
                            {

                                var schemeData = FinAcc.SchmDetails.FirstOrDefault();
                                var finaccId = FinAcc.Fid;
                                if (schemeData.SType == 1)
                                {
                                    for (int i = finaccId; i < (finaccId + 10); i++)
                                    {

                                        Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                        finaccDT.Fname = FinAcc.Fname;
                                        finaccDT.Code = FinAcc.Code;
                                        finaccDT.DebitRestriction = FinAcc.DebitRestriction;
                                        finaccDT.CreditRestriction = FinAcc.CreditRestriction;

                                        uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);

                                    }
                                }
                                else
                                {
                                    for (int i = finaccId; i < (finaccId + 4); i++)
                                    {

                                        Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                        finaccDT.Fname = FinAcc.Fname;
                                        finaccDT.Code = FinAcc.Code;
                                        finaccDT.DebitRestriction = FinAcc.DebitRestriction;
                                        finaccDT.CreditRestriction = FinAcc.CreditRestriction;

                                        uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);

                                    }
                                }
                                //uow.Repository<Models.Models.FinAcc>().Edit(FinAccdata);


                                //var schemeId = FinAcc.SchmDetails.First().SDID;
                                //int finaccId = UOW.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeId).FID;
                                Models.Models.SchmDetail schmDetail = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeData.SDID);
                                schmDetail.SDName = FinAcc.Fname;
                                schmDetail.SDID = schemeData.SDID;
                                schmDetail.FID = finaccId;

                                schmDetail.HasCard = schemeData.HasCard;
                                schmDetail.HasCertificate = schemeData.HasCertificate;
                                schmDetail.HasCheque = schemeData.HasCheque;
                                schmDetail.HasDormancy = schemeData.HasDormancy;
                                schmDetail.HasDuration = schemeData.HasDuration;
                                schmDetail.HasIndivLimit = schemeData.HasIndivLimit;
                                schmDetail.HasIndivRate = schemeData.HasIndivRate;
                                schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                schmDetail.HASSMS = schemeData.HASSMS;
                                schmDetail.ICBID = schemeData.ICBID;
                                schmDetail.IsInsured = schemeData.IsInsured;
                                schmDetail.MID = schemeData.MID;
                                schmDetail.Nomianable = schemeData.Nomianable;
                                schmDetail.RuleMovement = schemeData.RuleMovement;
                                schmDetail.RuleICB = schemeData.RuleICB;
                                schmDetail.SType = schmDetail.SType;
                                schmDetail.SEnable = schemeData.SEnable;
                                schmDetail.Withdrawal = schemeData.Withdrawal;
                                schmDetail.Revolving = schemeData.Revolving;
                                schmDetail.IsRenewable = schemeData.IsRenewable;
                                schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                schmDetail.HasDuration = schemeData.HasDuration;
                                schmDetail.MultiDeposit = schemeData.MultiDeposit;


                                uow.Repository<Models.Models.SchmDetail>().Edit(schmDetail);
                                uow.Commit();



                            }

                            #endregion

                            #region Products
                            if (FinAcc.ProductDetails.Count() > 0)
                            {

                                var productData = FinAcc.ProductDetails.FirstOrDefault();
                                var finaccId = FinAcc.Fid;
                                var productDetails = uow.Repository<Models.Models.ProductDetail>().GetSingle(x => x.SDID == productData.SDID);
                                if (productDetails.SchmDetail.SType == 1)
                                {
                                    for (int i = finaccId; i < (finaccId + 10); i++)
                                    {

                                        Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                        if (finaccDT != null)
                                        {
                                            finaccDT.Fname = FinAcc.Fname;
                                            finaccDT.Code = FinAcc.Code;
                                            finaccDT.DebitRestriction = FinAcc.DebitRestriction;
                                            finaccDT.CreditRestriction = FinAcc.CreditRestriction;

                                            uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);
                                            uow.Commit();
                                        }

                                    }
                                }
                                else
                                {
                                    for (int i = finaccId; i < (finaccId + 4); i++)
                                    {

                                        Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                        finaccDT.Fname = FinAcc.Fname;
                                        finaccDT.Code = FinAcc.Code;
                                        finaccDT.DebitRestriction = FinAcc.DebitRestriction;
                                        finaccDT.CreditRestriction = FinAcc.CreditRestriction;

                                        uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);
                                        uow.Commit();

                                    }
                                }

                                Models.Models.ProductDetail addProduct = new ProductDetail();
                                var prObj = FinAcc.ProductDetails.FirstOrDefault();
                                if (prObj != null)
                                {
                                    //int productId = prObj.PID;
                                    var productObj = uow.Repository<Models.Models.ProductDetail>().FindBy(x => x.PID == prObj.PID).FirstOrDefault();
                                    if (productObj != null)
                                    {

                                        productObj.PName = FinAcc.Fname;
                                        productObj.Apfix = FinAcc.Code;
                                        uow.Repository<Models.Models.ProductDetail>().Edit(productObj);
                                        uow.Commit();
                                    }
                                }

                                var productId = FinAcc.ProductDetails.First().PID;
                                var allProductBrnch = uow.Repository<Models.Models.ProductBrnch>().GetSingle(x => x.PID == productId);

                                ProductBrnch productbrnch = new ProductBrnch();
                                productbrnch.PBId = allProductBrnch.PBId;
                                productbrnch.PID = productId;
                                allProductBrnch.BrnchID = 1;
                                allProductBrnch.StartAcNo = Convert.ToInt32(FinAcc.ProductDetails.FirstOrDefault().StartAcNo);
                                allProductBrnch.LastAcNo = Convert.ToInt32(FinAcc.ProductDetails.FirstOrDefault().StartAcNo);

                                uow.Repository<Models.Models.ProductBrnch>().Edit(allProductBrnch);
                                uow.Commit();

                                #region RecurringEdit
                                if (FinAcc.ProductDetails.FirstOrDefault().InterestValueList != null)
                                {
                                    if (FinAcc.ProductDetails.FirstOrDefault().InterestValueList.Count() > 0)
                                    {
                                        //foreach (var subitem in FinAcc.ProductDetails.FirstOrDefault().InterestValueList)
                                        //{
                                        //    var amount = subitem.Amount;
                                        //    foreach (var subitem1 in subitem.DurationList)
                                        //    {
                                        //        var durationId = subitem1.Id;
                                        //        foreach (var subitem2 in subitem1.DepositBasisList)
                                        //        {
                                        //            var basisId = subitem2.Id;
                                        //            if(subitem2.ProductDurationInt!=null)
                                        //            {
                                        //                var interestRate = subitem2.ProductDurationInt.InterestRate;
                                        //                if (interestRate != 0)
                                        //                {
                                        //                    ProductDurationInt ProductDurationInt = new ProductDurationInt();
                                        //                    ProductDurationInt.InterestRate = interestRate;
                                        //                    ProductDurationInt.Value = Convert.ToDouble(amount);
                                        //                    ProductDurationInt.DBId = basisId;
                                        //                    ProductDurationInt.ICBId = subitem2.ProductDurationInt.ICBId;
                                        //                    ProductDurationInt.PId = productId;
                                        //                    ProductDurationInt.DVId = durationId;


                                        //                        uow.Repository<Models.Models.ProductDurationInt>().Add(ProductDurationInt);


                                        //                    uow.Commit();
                                        //                }
                                        //            }
                                         



                                        //        }
                                        //    }

                                        //}
                                        var hasAnotherData = uow.Repository<Models.Models.ProductPSID>().GetSingle(x => x.PID == productId);
                                        if (hasAnotherData != null)
                                        {
                                            hasAnotherData.PID = productId;
                                            hasAnotherData.PSID = Convert.ToByte(FinAcc.ProductDetails.FirstOrDefault().InterestValueList.First().PolicyIntCalcId);
                                            hasAnotherData.IsDefault = true;
                                            uow.Repository<Models.Models.ProductPSID>().Edit(hasAnotherData);
                                            uow.Commit();
                                        }


                                    }
                                }

                                #endregion


                                #region FixedDepositEdit

                                if (FinAcc.ProductDetails.FirstOrDefault().FixedDepositList != null)
                                {
                                    //For Fixed Deposit Adding after Editing
                                    if (FinAcc.ProductDetails.FirstOrDefault().FixedDepositList.DurationList.Count() > 0)
                                    {
                                        var allrecuringInterest = uow.Repository<Models.Models.ProductDurationInt>().FindBy(x => x.PId == productId);
                                        if (allrecuringInterest.Count() > 0)
                                        {

                                            foreach (var subInterestitem in allrecuringInterest)
                                            {

                                                uow.Repository<Models.Models.ProductDurationInt>().Delete(subInterestitem);


                                            }
                                            uow.Commit();
                                        }
                                    }

                                    if (FinAcc.ProductDetails.FirstOrDefault().FixedDepositList.PolicyIntCalcId != 0)
                                    {
                                        var allCalcInterest = uow.Repository<Models.Models.ProductPSID>().FindBy(x => x.PID == productId);
                                        if (allCalcInterest.Count() > 0)
                                        {

                                            foreach (var subInterestitem in allCalcInterest)
                                            {

                                                uow.Repository<Models.Models.ProductPSID>().Delete(subInterestitem);


                                            }
                                            uow.Commit();
                                        }
                                    }
                                    //foreach (var item in FinAcc.ProductDetails.FirstOrDefault().FixedDepositList.DurationList)
                                    //{

                                    //    foreach (var subitem in item.CaptList)
                                    //    {
                                    //        if (subitem.ProductDurationInt.InterestRate != null || subitem.ProductDurationInt.InterestRate != 0)
                                    //        {
                                    //            ProductDurationInt productDuration = new ProductDurationInt();
                                    //            productDuration.InterestRate = subitem.ProductDurationInt.InterestRate;
                                    //            productDuration.ICBId = subitem.ICBDurID;
                                    //            productDuration.PId = productId;
                                    //            productDuration.DBId = null;
                                    //            productDuration.DVId = item.Id;
                                    //            uow.Repository<Models.Models.ProductDurationInt>().Add(productDuration);
                                    //            uow.Commit();
                                    //        }
                                    //    }


                                    //}
                                    ProductPSID productPSid = new ProductPSID();
                                    productPSid.PID = productId;
                                    productPSid.PSID = Convert.ToByte(FinAcc.ProductDetails.FirstOrDefault().FixedDepositList.PolicyIntCalcId);
                                    productPSid.IsDefault = true;
                                    uow.Repository<Models.Models.ProductPSID>().Add(productPSid);
                                    uow.Commit();

                                }

                                #endregion


                                if (FinAcc.ProductDetails.FirstOrDefault().NormalDepositList != null)
                                {
                                    //This is Normal Deposit Product Without Duration
                                    var allCaptDur = uow.Repository<Models.Models.ProductICBDur>().FindBy(x => x.PID == productId);


                                    if (allCaptDur.Count() > 0)
                                    {

                                        foreach (var subInterestitem in allCaptDur)
                                        {

                                            uow.Repository<Models.Models.ProductICBDur>().Delete(subInterestitem);


                                        }
                                        uow.Commit();
                                    }


                                    var allCalcInterest = uow.Repository<Models.Models.ProductPSID>().FindBy(x => x.PID == productId);
                                    if (allCalcInterest.Count() > 0)
                                    {

                                        foreach (var subInterestitem in allCalcInterest)
                                        {

                                            uow.Repository<Models.Models.ProductPSID>().Delete(subInterestitem);


                                        }
                                        uow.Commit();
                                    }

                                    //For Normal Deposit Adding after Editing
                                    foreach (var item in FinAcc.ProductDetails.FirstOrDefault().NormalDepositList.CapitalizationRuleList)
                                    {
                                        if (item.IsChecked == true)
                                        {
                                            ProductICBDur productIcb = new ProductICBDur();
                                            productIcb.IRate = Convert.ToInt16(item.InterestRateICB);
                                            productIcb.IsDefault = item.IsDefault;
                                            productIcb.PID = productId;
                                            productIcb.ICBDurID = item.ICBDurID;

                                            uow.Repository<Models.Models.ProductICBDur>().Add(productIcb);
                                            uow.Commit();
                                        }

                                    }

                                    foreach (var item in FinAcc.ProductDetails.FirstOrDefault().NormalDepositList.CalculationRuleList)
                                    {
                                        if (item.IsChecked == true)
                                        {
                                            ProductPSID productPSid = new ProductPSID();
                                            productPSid.PID = productId;
                                            productPSid.PSID = item.PSID;
                                            productPSid.IsDefault = item.IsDefault;
                                            uow.Repository<Models.Models.ProductPSID>().Add(productPSid);
                                            uow.Commit();
                                        }

                                    }



                                }

                                if (FinAcc.ProductDetails.FirstOrDefault().LoanInterestList != null)
                                {
                                    //This is Loan Deposit Product
                                    var allPenalty = uow.Repository<Models.Models.ProductPCID>().FindBy(x => x.PID == productId);
                                    foreach (var item in allPenalty)
                                    {
                                        uow.Repository<Models.Models.ProductPCID>().Delete(item);


                                    }
                                    var allRule = uow.Repository<Models.Models.ProductPay>().FindBy(x => x.PID == productId);
                                    foreach (var item in allRule)
                                    {
                                        uow.Repository<Models.Models.ProductPay>().Delete(item);

                                    }
                                    var calcRule = uow.Repository<Models.Models.ProductPSID>().FindBy(x => x.PID == productId);
                                    foreach (var item in calcRule)
                                    {
                                        uow.Repository<Models.Models.ProductPSID>().Delete(item);


                                    }
                                    uow.Commit();

                                    if (FinAcc.ProductDetails.FirstOrDefault().LoanInterestList != null)
                                    {
                                        //For Loan Product Pay Rule
                                        foreach (var item in FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.RulePayList)
                                        {
                                            if (item.IsChecked == true)
                                            {
                                                ProductPay prPay = new ProductPay();
                                                prPay.PID = productId;
                                                prPay.PAYID = item.PAYID;
                                                prPay.IsDefault = item.IsDefault;
                                                uow.Repository<Models.Models.ProductPay>().Add(prPay);
                                                uow.Commit();
                                            }
                                        }
                                        //For Loan Interest Calculation
                                        foreach (var item in FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.PolicyCalculationList)
                                        {
                                            if (item.IsChecked == true)
                                            {
                                                ProductPSID productPSID = new ProductPSID();
                                                productPSID.IsDefault = item.IsDefault;
                                                productPSID.PID = productId;
                                                productPSID.PSID = item.PSID;
                                                uow.Repository<Models.Models.ProductPSID>().Add(productPSID);
                                                uow.Commit();
                                            }

                                        }

                                        //For Loan Product Penalty Calculation
                                        foreach (var item in FinAcc.ProductDetails.FirstOrDefault().LoanInterestList.PolicyPenaltyList)
                                        {
                                            if (item.IsChecked == true)
                                            {
                                                ProductPCID productPCID = new ProductPCID();
                                                productPCID.PCID = item.PCID;
                                                productPCID.IsDefault = Convert.ToByte(item.IsDefault);
                                                productPCID.PID = productId;
                                                uow.Repository<Models.Models.ProductPCID>().Add(productPCID);
                                                uow.Commit();
                                            }

                                        }

                                    }


                                }
                            }

                            #endregion


                        }


                    }

                    #endregion


                    #region CommitChanges

                    uow.Commit();
                    //transaction.Commit();
                    scope.Complete();

                    #endregion
                }

                catch (DbEntityValidationException e)
                {
                    scope.Dispose();

                  
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                    //throw;
                    //rollback all transactions
                }
            }
        }





        public void RestrictCreate(int F2type)
        {
            var f2Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == F2type).F1id;


        }



        public IEnumerable<Models.ViewModel.LedgerViewModelFinAcc> GetFinAccData(int FinAccId)
        {
            var finacc = uow.Repository<Models.Models.FinAcc>().GetAll().Where(x => x.Fid == FinAccId).Select(x => new Models.ViewModel.LedgerViewModelFinAcc()
            {

                Fid = x.Fid,
                Code = x.Code,
                CreditRestriction = x.CreditRestriction,
                DebitRestriction = x.DebitRestriction,
                F2Type = x.F2Type,
                Pid = Convert.ToInt32(x.Pid),
                Content = x.Content,


            });
            return finacc;


        }
        //public void Save

        public IEnumerable<Models.ViewModel.BankLedgerViewModel> GetBankInfoData(int FinAccId)
        {
            var bankInfo = uow.Repository<Models.ViewModel.BankLedgerViewModel>().GetAll().Where(x => x.Fid == FinAccId).Select(x => new Models.ViewModel.BankLedgerViewModel()
            {

                Bid = x.Bid,
                ContactPerson = x.ContactPerson,
                IsFixed = x.IsFixed,
                Branch = x.Branch,
                Fid = x.Fid,
                InterestRate = x.InterestRate,
                InterestRuleId = x.InterestRuleId,
                MatureDate = x.MatureDate,
                MinimumBalance = x.MinimumBalance,
                OpenDate = x.OpenDate,
                PhoneNo = x.PhoneNo


            });
            return bankInfo;


        }
        public IEnumerable<Models.ViewModel.BankLedgerViewModel> GetSubsiAccountData(int FinAccId)
        {
            Models.Models.FinAcc ii = new Models.Models.FinAcc();

            var bankInfo = uow.Repository<Models.ViewModel.BankLedgerViewModel>().GetAll().Where(x => x.Fid == FinAccId).Select(x => new Models.ViewModel.BankLedgerViewModel()
            {

                Bid = x.Bid,
                ContactPerson = x.ContactPerson,
                IsFixed = x.IsFixed,
                Branch = x.Branch,
                Fid = x.Fid,
                InterestRate = x.InterestRate,
                InterestRuleId = x.InterestRuleId,
                MatureDate = x.MatureDate,
                MinimumBalance = x.MinimumBalance,
                OpenDate = x.OpenDate,
                PhoneNo = x.PhoneNo


            });
            return bankInfo;


        }

        public bool Delete(int FinAccId)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {
                try
                {
                    Models.Models.FinAcc FinAcc = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == FinAccId);


                    if (FinAcc != null)
                    {


                        if (FinAcc.BankInfoes.Count() != 0)
                        {
                            var bankList = FinAcc.BankInfoes.ToList();
                            foreach (var item in bankList)
                            {
                                BankInfo bankInfo = uow.Repository<Models.Models.BankInfo>().GetSingle(x => x.Bid == item.Bid);
                                uow.Repository<Models.Models.BankInfo>().Delete(bankInfo);
                            }

                        }
                        //SubsiTitle table is removed
                        //if (FinAcc.SubsiTitles.Count() != 0)
                        //{
                        //    var subsiList = FinAcc.SubsiTitles.ToList();
                        //    foreach (var item in subsiList)
                        //    {
                        //        SubsiTitle subsiTitle = new SubsiTitle();
                        //        subsiTitle = uow.Repository<Models.Models.SubsiTitle>().GetSingle(x => x.STid == item.STid);
                        //        uow.Repository<Models.Models.SubsiTitle>().Delete(subsiTitle);
                        //    }
                        //    var subsiVSFidList = FinAcc.SubsiVSFIds.ToList();
                        //    foreach (var item in subsiVSFidList)
                        //    {
                        //        SubsiVSFId subsivsFid = uow.Repository<Models.Models.SubsiVSFId>().GetSingle(x => x.Fid == item.Fid);
                        //        uow.Repository<Models.Models.SubsiVSFId>().Delete(subsivsFid);
                        //    }

                        //}
                        if (FinAcc.DimensionVSLedgers.Count() != 0)
                        {
                            var dimList = FinAcc.DimensionVSLedgers.ToList();
                            foreach (var item in dimList)
                            {
                                DimensionVSLedger subsivsFid = uow.Repository<Models.Models.DimensionVSLedger>().GetSingle(x => x.Id == item.Id);
                                uow.Repository<Models.Models.DimensionVSLedger>().Delete(subsivsFid);
                            }
                        }

                        if (FinAcc.Voucher2.Count() != 0)
                        {
                            var dimList = FinAcc.Voucher2.ToList();
                            foreach (var item in dimList)
                            {
                                Voucher2 voucherList = uow.Repository<Models.Models.Voucher2>().GetSingle(x => x.V2Id == item.V2Id);
                                uow.Repository<Models.Models.Voucher2>().Delete(voucherList);
                            }
                        }
                        if (FinAcc.Voucher2T.Count() != 0)
                        {
                            var dimList = FinAcc.Voucher2T.ToList();
                            foreach (var item in dimList)
                            {
                                Voucher2T voucherList = uow.Repository<Models.Models.Voucher2T>().GetSingle(x => x.V2TId == item.V2TId);
                                uow.Repository<Models.Models.Voucher2T>().Delete(voucherList);
                            }
                        }

                        #region SchemeDelete
                        if (FinAcc.SchmDetails.Count() != 0)
                        {
                            var schList = FinAcc.SchmDetails.ToList();
                            if (FinAcc.SchmDetails.First().SType == 0)
                            {

                                foreach (var item in schList)
                                {
                                    int finaccId = item.FID;
                                    SchmDetail schm = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == item.SDID);
                                    uow.Repository<Models.Models.SchmDetail>().Delete(schm);

                                    for (int i = finaccId; i < (finaccId + 5); i++)
                                    {
                                        var finaccLedger = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                        uow.Repository<Models.Models.FinAcc>().Delete(finaccLedger);
                                        uow.Commit();


                                    }
                                }
                            }
                            else
                            {
                                foreach (var item in schList)
                                {
                                    int finaccId = item.FID;
                                    SchmDetail schm = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == item.SDID);
                                    uow.Repository<Models.Models.SchmDetail>().Delete(schm);

                                    for (int i = finaccId; i < (finaccId + 11); i++)
                                    {
                                        var finaccLedger = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                        uow.Repository<Models.Models.FinAcc>().Delete(finaccLedger);
                                        uow.Commit();

                                    }
                                }
                            }
                            return false;
                        }

                        #endregion

                        #region ProductDelete
                        if (FinAcc.ProductDetails.Count() != 0)
                        {
                            var schList = FinAcc.ProductDetails.ToList();
                            if (FinAcc.ProductDetails.First().SchmDetail.SType == 0)
                            {

                                foreach (var item in schList)
                                {

                                    var productId = item.PID;
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
                                    int finaccId = item.FID;
                                    ProductDetail schm = uow.Repository<Models.Models.ProductDetail>().GetSingle(x => x.PID == item.PID);
                                    uow.Repository<Models.Models.ProductDetail>().Delete(schm);

                                    for (int i = finaccId; i < (finaccId + 5); i++)
                                    {
                                        var finaccLedger = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                        uow.Repository<Models.Models.FinAcc>().Delete(finaccLedger);
                                        uow.Commit();



                                    }
                                }
                            }
                            else
                            {
                                foreach (var item in schList)
                                {
                                    var productId = item.PID;
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
                                    int finaccId = item.FID;
                                    ProductDetail schm = uow.Repository<Models.Models.ProductDetail>().GetSingle(x => x.PID == item.PID);
                                    uow.Repository<Models.Models.ProductDetail>().Delete(schm);


                                    for (int i = finaccId; i < (finaccId + 11); i++)
                                    {
                                        var finaccLedger = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                                        uow.Repository<Models.Models.FinAcc>().Delete(finaccLedger);
                                        uow.Commit();

                                    }
                                }
                            }
                            return false;
                        }
                        #endregion

                        uow.Repository<Models.Models.FinAcc>().Delete(FinAcc);
                        uow.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;

                    //rollback all transactions
                }
                return false;
            }





        }

        public string GetAddress(int FinAccId)
        {
            string result = "";

            if (FinAccId != 0)
            {
                Models.Models.FinAcc mnu = new Models.Models.FinAcc();
                mnu = GetSingle(FinAccId);

                List<string> lst = new List<string>();


                while (mnu != null)
                {
                    lst.Add(mnu.Fname);
                    mnu = GetSingle(Convert.ToInt32(mnu.Pid));
                };

                var sorted = lst.Select((x, i) => new KeyValuePair<string, int>(x, i)).OrderByDescending(x => x.Value).ToList();

                foreach (var item in sorted)
                {
                    if (result == "")
                    {
                        result = result + item.Key;
                    }
                    else
                    {
                        result = result + "/" + item.Key;
                    }

                }
            }
            else
            {
                result = "Root";
            }
            return result;
        }
        public List<SelectListItem> GetCalculationRules()
        {
            var capList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.PolicyIntCalc>().GetAll();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in capList)
            {


                lst.Add(new SelectListItem { Text = item.PSName, Value = item.PSID.ToString() });

            }
            return lst;
        }

        #region Tree

        private List<Models.Models.FinAcc> FilterTree(List<Models.Models.FinAcc> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.Fname.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.Fid equals selList.Pid
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.Fid equals c.Fid into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.Fid).ToList();
            }
            list = filteredList;
            return list;

        }
        public Models.ViewModel.TreeView GetFinAccGroupTree(string filter = "")
        {
            //List<Models.Models.FinAcc> list = new List<Models.Models.FinAcc>();

            var treelist = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == true).ToList(); //optimize code
            treelist.Add(new Models.Models.FinAcc { F2Type = 0, Fname = "Root", Pid = -1 });
            //foreach (var item in treelist)
            //{

            //    var finsysList = uow.Repository<Models.Models.FinSys1>().GetAll().Where(x => x.IsGroup == true);
            //    foreach (var data in finsysList)
            //    {
            //        if (item.Fid == data.F1id)
            //        {
            //            list.Add(item);
            //        }

            //    }
            //}




            if (filter.Trim() != "")
            {
                treelist = FilterTree(treelist, filter);
            }
            Models.ViewModel.TreeView tree = this.GenerateTree(treelist, -1);
            return tree;
        }

        public IEnumerable<SelectListItem> GetMovement()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<RuleMovement>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.MRule, Value = item.MID.ToString() });
            }
            return lst;
        }
        public IEnumerable<SelectListItem> GetSchmDetails()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<SchmDetail>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.SDName, Value = item.SDID.ToString() });
            }
            return lst;
        }
        public IEnumerable<SelectListItem> GetRuleICB()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<RuleICB>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.ICB, Value = item.ICBID.ToString() });
            }
            return lst;
        }

        public Models.ViewModel.TreeView GetFinAccGroupTree(int FinAccIdExpect, string filter = "")
        {
            List<Models.Models.FinAcc> list = uow.Repository<Models.Models.FinAcc>().GetAll().Where(x => x.FinSys2.FinSys1.IsGroup == true).Where(x => x.Fid != FinAccIdExpect).ToList();
            list.Add(new Models.Models.FinAcc { F2Type = 0, Fname = "Root", Pid = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            Models.ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }
        public List<SelectListItem> GetFinSys1List()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Models.Models.FinSys1>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.F1Des, Value = item.F1id.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetTypeList(int FinAccId)

        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<Models.Models.FinAcc> list;
            if (FinAccId == 0 || FinAccId == 1)
            {
                list = uow.Repository<Models.Models.FinAcc>().GetAll().ToList();
            }
            else
            {
                var typeId = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == FinAccId).F2Type;
                list = uow.Repository<Models.Models.FinAcc>().GetAll().Where(x => x.Fid == typeId).ToList();
            }
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.Fname, Value = item.Fid.ToString() });
            }
            return lst;
        }

        //public ViewModel.TreeView GetFinAccGroupTree(int parentId, int FinAccIdExpect, string filter = "")
        //{
        //    List<FinAcc> list = uow.Repository<FinAcc>().GetAll().Where(x => x.IsGroup == true).Where(x => x.FinAccId != FinAccIdExpect).ToList();

        //    if (filter.Trim() != "")
        //    {
        //        list = FilterTree(list, filter);
        //    }

        //    ViewModel.TreeView tree = this.GenerateTree(list, parentId);
        //    return tree;
        //}
        public List<Models.Models.FinSys2> GenerateF2Type(int? Pid, int grpId)
        {
            if (Pid == 0)
            {
                List<Models.Models.FinSys2> finallst = new List<Models.Models.FinSys2>();

                List<Models.Models.FinSys2> finaccList = uow.Repository<Models.Models.FinSys2>().FindBy(x => (x.F1Type == grpId && x.Pid == 1) || x.F2id == 1).AsQueryable().OrderBy(x => x.F2Desc).ToList();
                foreach (var item in finaccList)
                {
                    if (item.F2id == 1 || item.Count == 0)
                    {
                        finallst.Add(item);
                    }
                    else
                    {
                        int val = CheckCount(item.F2id, grpId);
                        if (val != 0)
                        {

                            finallst.Add(item);

                        }
                    }


                }

                return finallst;

            }
            else
            {

                bool checkisFixed = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == grpId).FinSys2.IsFixed;
                List<Models.Models.FinSys2> finallst = new List<Models.Models.FinSys2>();


                if (checkisFixed == true)
                {
                    int F2type = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == grpId).F2Type;
                    List<Models.Models.FinSys2> finaccList = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == F2type).OrderBy(x => x.F2Desc).ToList();

                    foreach (var item in finaccList)
                    {
                        if (item.Count == 0)
                        {
                            finallst.Add(item);
                        }
                        else
                        {
                            int val = CheckCount(item.F2id, grpId);
                            if (val != 0)
                            {

                                finallst.Add(item);

                            }
                        }

                    }

                    return finallst;

                }
                else
                {
                    int F2type = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == grpId).F2Type;
                    List<Models.Models.FinSys2> finaccList = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == 0 || x.Pid == F2type).OrderBy(x => x.F2Desc).ToList();
                    return finaccList;


                }

            }
        }
        //public List<Models.Models.DimensionDefination> GetAllDimension(List<DimensionVSLedger> dimValue)
        //{
        //    List<Models.Models.DimensionDefination> dimDef = new List<Models.Models.DimensionDefination>();
        //    var dValue = uow.Repository<Models.Models.DimensionDefination>().GetAll().ToList();

        //    foreach (var subcatergory in dValue)
        //    {
        //        foreach (var category in dimValue)
        //        {
        //            if (category.DDID == subcatergory.DDId)
        //            {
        //                subcatergory.isSelected = true;
        //                subcatergory.DLId = category.Id;
        //                if (dimDef.Contains(subcatergory) == false)
        //                {
        //                    dimDef.Add(subcatergory);
        //                }

        //            }
        //            else
        //            {
        //                if (dimDef.Contains(subcatergory) == false)
        //                {
        //                    subcatergory.isSelected = false;
        //                    dimDef.Add(subcatergory);
        //                }
        //            }
        //        }
        //    }
        //    return dimDef;
        //}


        public int CheckCount(int groupId, int? Pid)
        {
            int countVal = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == groupId).Count;
            int checkFinacc = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == groupId && x.Pid == Pid).Count();

            int finalCount = countVal - checkFinacc;
            if (finalCount > 0)
            {
                return finalCount;
            }
            else
            {
                return 0;
            }


        }

        public int CheckLedgerCount(int groupId, int? LedgerId)

        {
            if (groupId == 1)
            {
                return 1;
            }
            var isfixed = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F2id == groupId).FirstOrDefault().IsFixed;
            if (isfixed != null)
            {
                if (isfixed == false)
                {
                    return 1;
                }
            }

            var childList = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == groupId).ToList();
            var maxCount = 0;
            var count = 0;
            foreach (var item in childList)
            {
                if (item.Count != -1)
                {
                    if (item.Count == 0)
                    {
                        return 1;
                    }
                    if (item.Count == -1)
                    {
                        break;
                    }
                    maxCount = maxCount + item.Count;
                    //var finaccChildList = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == item.F2id && x.Pid == LedgerId);
                    var finaccChildListCount = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == item.F2id && x.Pid == LedgerId).Count();
                    count = count + finaccChildListCount;
                }
            }
            int finalCount = maxCount - count;
            if (finalCount > 0)
            {
                return finalCount;
            }

            else
            {
                return -1;
            }


        }
        private Models.ViewModel.TreeView GenerateTree(List<Models.Models.FinAcc> list, int? parentFinAccId)
        {

            var parent = list.Where(x => x.Pid == parentFinAccId);
            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();
            tree.Title = "FinAcc";
            foreach (var itm in parent)
            {
                tree.AccChildren.Add(new Models.ViewModel.AccountingTreeDTO
                {
                    Id = itm.Fid,
                    PId = itm.Pid,
                    Text = itm.Fname,
                    //IsGroup = itm.FinSys1.IsGroup,
                    Image = itm.Content

                });
            }

            foreach (var itm in tree.AccChildren)
            {
                itm.AccChildren = GenerateTree(list, itm.Id).AccChildren.ToList();
            }
            return tree;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
