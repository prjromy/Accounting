using ChannakyaBase.BLL.CustomHelper;
using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using ChannakyaCustomeDatePicker.Service;
using Loader.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MoreLinq;
using System.Text;
using System.Threading.Tasks;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using ChannakyaAccounting.Models.Models;

namespace ChannakyaBase.BLL.Service
{
    public class TellerService
    {

        private GenericUnitOfWork uow = null;
        private ChannakyaBaseEntities _context = null;
        //ReturnBaseMessageModel returnMessage = null;
        //TaskVerificationService taskUow = null;
        //FinanceParameterService financeParameterService = null;

        private CommonService commonService = null;
        public TellerService()
        {
            //financeParameterService = new FinanceParameterService();
            //taskUow = new TaskVerificationService();
            uow = new GenericUnitOfWork();
            _context = new ChannakyaBaseEntities();
            //returnMessage = new ReturnBaseMessageModel();
            commonService = new CommonService();
        }


        #region Account Open Scheme Event

        public CheckProductTypeModel CheckProductStatusType(byte productId)
        {
            byte schemeId = uow.Repository<ProductDetail>().FindBy(x => x.PID == productId).Select(x => x.SDID).FirstOrDefault();
            CheckProductTypeModel chkProduct = new CheckProductTypeModel();
            chkProduct.IsDuration = TellerUtilityService.HasDuration(schemeId, productId);
            chkProduct.IsFixed = TellerUtilityService.IsFixedAccount(schemeId, productId);
            chkProduct.IsIndividualLimit = TellerUtilityService.HasIndividualLimit(productId);
            chkProduct.IsIndiviualInterestRate = TellerUtilityService.HasIndiviualInterestRate(productId);
            chkProduct.IsMovement = TellerUtilityService.IsMovementAccount(productId);
            chkProduct.IsRecurring = TellerUtilityService.IsRecurringDeposit(schemeId, productId);
            return chkProduct;

        }
        public List<ProductDurationViewModel> GetDurationList(byte pid)
        {
            var duration = (from pd in uow.Repository<ProductDurationInt>().FindBy(x => x.PId == pid).Select(x => x.DVId).Distinct()
                            join d in uow.Repository<Duration>().GetAll() on pd equals d.Id
                            select new ProductDurationViewModel
                            {
                                Id = d.Id,
                                Duration = d.Duration1
                            }).OrderBy(x => x.Id).ToList();
            return duration;
        }

        public Duration GetDurationByDurationId(int durId)
        {
            var duration = uow.Repository<Duration>().FindBy(x => x.Id == durId).FirstOrDefault();
            return duration;
        }
        public List<InterestCapitalModel> GetInterestCapitalizeByProductId(int productId)
        {

            var policyIntCalByProductId = (from prod in _context.ProductICBDurs
                                           join rule in _context.RuleICBDurations
                                           on prod.ICBDurID equals rule.ICBDurID
                                           where prod.PID == productId
                                           select new InterestCapitalModel()
                                           {
                                               InterestCapitalizeId = prod.PICBDurID,
                                               InterestCapitalizeName = rule.ICBDurNam,
                                               IsDefault = prod.IsDefault
                                           }).OrderByDescending(x => x.IsDefault).ToList();
            return policyIntCalByProductId;

        }

        public AccountDetailsViewModel GetSingleAccount(int IAccno)
        {
            var account = uow.Repository<ADetail>().GetSingle(x => x.IAccno == IAccno);
            AccountDetailsViewModel accdetail = new AccountDetailsViewModel();
            accdetail.Accno = account.Accno;
            accdetail.AccState = accdetail.AccState;
            accdetail.Aname = account.Aname;
            accdetail.PID = account.PID;
            accdetail.RDate = account.RDate;
            accdetail.CurrID = account.CurrID;
            accdetail.BrchID = account.BrchID;
            accdetail.PostedBy = account.PostedBy;
            accdetail.ApprovedBy = account.ApprovedBy;
            accdetail.Bal = account.Bal;
            accdetail.IonBal = account.IonBal;
            accdetail.InterestRate = account.IRate;
           // accdetail.Duration = Convert.ToInt32(account.ADur.Days);
            return accdetail;

        }
        public ADetail GetSingleAccountDetails(int? IAccno)
        {
            var account = uow.Repository<ADetail>().GetSingle(x => x.IAccno == IAccno);
            return account;
        }

        public ProductViewModel GetProductDetails(int productId)
        {

            var InterestRateAmount = (from x in _context.ProductDetails
                                      join s in _context.SchmDetails on x.SDID equals s.SDID
                                      where x.PID == productId
                                      select new ProductViewModel()
                                      {
                                          MinimumAmount = x.LAmt,
                                          InterestRate = x.IRate,
                                          Duration = x.Duration,
                                          Nomianable = x.Nomianable,
                                          MovementId = x.MID,
                                          DurState = x.durState == 0 ? "Day" : x.durState == 1 ? "Month" : "DayMonth",
                                          DurationType = x.durState,
                                          HasIndividualDuration = x.HasIndDuration,
                                          HasIndividualLimit = x.HasIndivLimit,
                                          HasIndividualRate = x.HasIndivRate,
                                          HasDuration = s.HasDuration
                                      }).FirstOrDefault();
            if (InterestRateAmount != null)
            {
                if (InterestRateAmount.Duration != null)
                {
                    if (InterestRateAmount.Duration % 1 != 0)
                    {
                        InterestRateAmount.Duration = InterestRateAmount.Duration.Value * 1000;
                    }
                }
            }

            if (InterestRateAmount == null)
            {
                ProductViewModel InterestRateAmount1 = new ProductViewModel();
                return InterestRateAmount1;
            }
            return InterestRateAmount;
        }

        public decimal GetSingleInterestProductDurInt(int captId, int productId)
        {
            decimal interestRate = 0;

            var schemeId = uow.Repository<ProductDetail>().FindBy(x => x.PID == productId).Select(x => x.SDID).FirstOrDefault();
            if (TellerUtilityService.IsFixedAccount(schemeId, productId))
            {
                var productDurInt = uow.Repository<ProductDurationInt>().FindBy(x => x.Id == captId && x.PId == productId).Select(x => x.InterestRate).FirstOrDefault();
                interestRate = Convert.ToDecimal(productDurInt);
            }
            else if (TellerUtilityService.IsRecurringDeposit(schemeId, productId))
            {
                var productDurInt = uow.Repository<ProductDurationInt>().FindBy(x => x.Id == captId && x.PId == productId).Select(x => x.InterestRate).FirstOrDefault();
                interestRate = Convert.ToDecimal(productDurInt);

            }
            else
            {
                var InterestCapitalize = uow.Repository<ProductICBDur>().FindBy(x => x.PICBDurID == captId).Select(x => x.IRate).FirstOrDefault();
                interestRate = Convert.ToDecimal(InterestCapitalize);
            }

            return interestRate;


        }

        public List<ProductDurationViewModel> GetRecurringBasic(double contrubAmount, int productId, int durationId)
        {
            var recDuration = (from di in uow.Repository<ProductDurationInt>().FindBy(x => x.Value == contrubAmount && x.PId == productId && x.DVId == durationId && x.DBId != null)
                               join d in uow.Repository<DepositBasi>().GetAll() on di.DBId equals d.Id
                               select new ProductDurationViewModel()
                               {
                                   Id = d.Id,
                                   Duration = d.Name
                               }).DistinctBy(x => x.Id).ToList();
            return recDuration;
        }

        public float GetCapitalizeRuleValueByProductDurationIntId(int id)
        {
            float RuleICBDurationValue = (from pi in uow.Repository<ProductDurationInt>().GetAll()
                                          join
                                         ic in uow.Repository<RuleICBDuration>().GetAll() on pi.ICBId equals ic.ICBDurID
                                          where pi.Id == id
                                          select ic.ICBDurVal
                                        ).FirstOrDefault();
            return RuleICBDurationValue;
        }
        public List<InterestCapitalModel> GetIntCapitalizeRuleForFixedAndRecurringAndOtherProduct(byte productId, int durationId)
        {
            byte schemeId = uow.Repository<ProductDetail>().FindBy(x => x.PID == productId).Select(x => x.SDID).FirstOrDefault();
            List<InterestCapitalModel> RecurringCapitalizeRule = new List<InterestCapitalModel>();
            if (TellerUtilityService.IsFixedAccount(schemeId, productId))
            {
                RecurringCapitalizeRule = uow.Repository<InterestCapitalModel>().SqlQuery("select * from fin.FGetFixedIntCapitalizeRule(" + productId + "," + durationId + ") order by InterestCapitalizeId").ToList();
            }
            else if (TellerUtilityService.IsRecurringDeposit(schemeId, productId))
            {
                RecurringCapitalizeRule = uow.Repository<ProductDurationInt>().FindBy(x => x.PId == productId && x.DBId != null && x.DVId == durationId).Select(x => new InterestCapitalModel
                {
                    contributionValue = x.Value
                }).DistinctBy(x => x.contributionValue).ToList();
                // RecurringCapitalizeRule = uow.Repository<InterestCapitalModel>().SqlQuery("select * from fin.FGetRecurringIntCapitalizeRule(" + productId + "," + durationId + ")").ToList();
            }
            else
            {
                RecurringCapitalizeRule = (from prod in _context.ProductICBDurs
                                           join rule in _context.RuleICBDurations
                                           on prod.ICBDurID equals rule.ICBDurID
                                           where prod.PID == productId
                                           select new InterestCapitalModel()
                                           {
                                               InterestCapitalizeId = prod.PICBDurID,
                                               InterestCapitalizeName = rule.ICBDurNam,
                                               IsDefault = prod.IsDefault
                                           }).OrderByDescending(x => x.IsDefault).ToList();
            }

            return RecurringCapitalizeRule;

        }
        public List<FloatingInterestModel> GetAllFloatingInterestByProductId(int productId)
        {

            var floatingInterest = (from prod in _context.ProductTIDs
                                    join rate in _context.TempIntRates
                                    on prod.TID equals rate.TID
                                    where prod.PID == productId
                                    select new FloatingInterestModel()
                                    {
                                        FloatingId = prod.PFIID,
                                        FloatingName = rate.Tname
                                    }).ToList();
            return floatingInterest;
        }

        public List<CurrencyModel> GetCurrencyByProductId(int productId)
        {

            var currencyList = uow.Repository<CurrencyModel>().SqlQuery("select * from fin.FGetCurrencyList(" + productId + ")").ToList();
            return currencyList;
        }

        public List<ProductViewModel> GetAllProductBySchemeId(int schemeId)
        {
            var productListByScheme = uow.Repository<ProductDetail>().FindBy(x => x.SDID == schemeId).Select(x => new ProductViewModel()
            {
                ProductId = x.PID,
                ProductName = x.PName
            }).ToList();
            return productListByScheme;

        }

        public List<PolicyModel> GetAllInterestCalculationRuleByProductId(int productId)
        {

            var policyIntCalByProductId = (from prod in _context.ProductPSIDs
                                           join pol in _context.PolicyIntCalcs
                                           on prod.PSID equals pol.PSID
                                           where prod.PID == productId 
                                           select new PolicyModel()
                                           {
                                               PloicyIntCalId = prod.PCalCId,
                                               PolicyIntCalName = pol.PSName,
                                               IsDefault = prod.IsDefault

                                           }).OrderByDescending(x => x.IsDefault).ToList();
            return policyIntCalByProductId;


        }

        public bool IsChargeAvailable(int productId)
        {
            var chargeByPId = uow.Repository<ChargeDetail>().FindBy(x => (x.Product == productId || x.Product == 0) && (x.ModEveID == 4 && x.Status == true)).ToList();
            if (chargeByPId.Count() > 0)
                return true;
            else
                return false;
        }
        public SchemeModel GetFixedOrRecurringDepositDuration(int schemedId, int productId)
        {
            var fixDeposit = (from s in uow.Repository<SchmDetail>().FindBy(x => x.SDID == schemedId)
                              join p in uow.Repository<ProductDetail>().FindBy(x => x.PID == productId) on s.SDID equals p.SDID

                              select new SchemeModel()
                              {
                                  HasDuration = s.HasDuration,
                                  MultiDeposit = p.MultiDep,
                                  AllowWithdraw = p.Withdrawal,
                                  Schedule = p.Schedule

                              }).FirstOrDefault();
            if (fixDeposit == null)
            {
                SchemeModel schm = new SchemeModel();
                return schm;
            }
            return fixDeposit;
        }

        public byte GetRuleICB(int schemeId)
        {
            byte? icbId = uow.Repository<SchmDetail>().FindBy(x => x.SDID == schemeId).Select(x => x.ICBID).FirstOrDefault();
            return Convert.ToByte(icbId);


        }

        public List<InterestCapitalModel> GetInterestCapitalizeForRecurringProduct(int durationId, int productId, int basicId, double? value)
        {
            var recuringBasic = (from dbi in uow.Repository<ProductDurationInt>().FindBy(x => x.PId == productId && x.Value == value && x.DBId == basicId && x.DVId == durationId)
                                 join rule in uow.Repository<RuleICBDuration>().GetAll() on dbi.ICBId equals rule.ICBDurID
                                 select new InterestCapitalModel()
                                 {
                                     InterestCapitalizeId = dbi.Id,
                                     InterestCapitalizeName = rule.ICBDurNam,

                                 }

                               ).ToList();

            return recuringBasic;
        }
        #endregion

        #region Account Open

public ReturnBaseMessageModel InsertUpdateAccountOpen(AccountDetailsViewModel aModelDetails, TaskVerificationList TaskVerifierList,List<ChargeDetail> ChargeDetailsList)
        {
            using (var Transaction = uow.BeginTransaction())
            {


                try
                {
                    float totalShare = 0;
                    var SchemeId = uow.Repository<ProductDetail>().FindBy(x => x.PID == aModelDetails.PID).FirstOrDefault();
                    foreach (var item in aModelDetails.ANominees)
                    {
                        totalShare += item.Share;
                    }
                    if (totalShare > 100)
                    {
                        returnMessage.Msg = "Nominees share is not more then 100%!!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    else if (totalShare < 100)
                    {
                        returnMessage.Msg = "Nominees share is not less then 100%!!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    if (TellerUtilityService.IsFixedAccount(SchemeId.SDID, aModelDetails.PID))
                    {
                        if (aModelDetails.AgreementAmount == 0)
                        {
                            returnMessage.Msg = "Agreement amount is required!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }
                    if (TellerUtilityService.IsNominee(aModelDetails.PID))
                    {
                        if (aModelDetails.MovementAcc == 0)
                        {
                            returnMessage.Msg = "Movement account is required!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }

                    var productDetails = this.GetProductDetails(aModelDetails.PID);
                    ADetail accountDetails = uow.Repository<ADetail>().GetSingle(x => x.IAccno == aModelDetails.IAccno);
                    if (accountDetails == null)
                    {
                        accountDetails = new ADetail();
                    }
                    accountDetails.PID = aModelDetails.PID;
                    //accountDetails.Accno = "00-00-00-00";
                    accountDetails.RDate = aModelDetails.RDate;
                    accountDetails.CurrID = aModelDetails.CurrID;
                    accountDetails.BrchID = (byte)commonService.GetBranchIdByEmployeeUserId(Global.UserId);
                    accountDetails.AccState = 6;
                    accountDetails.Aname = aModelDetails.Aname;
                    if (aModelDetails.DateType == true)
                    {
                        accountDetails.DateType = 1;
                    }
                    else
                    {
                        accountDetails.DateType = 2;
                    }

                    #region Account Nominee
                    // Account Nominee Details
                    foreach (var item in aModelDetails.ANominees)
                    {
                        ANominee accountNominee = uow.Repository<ANominee>().GetSingle(x => x.NID == item.NID);
                        if (accountNominee == null)
                        {
                            accountNominee = new ANominee();
                        }
                        accountNominee.NomNam = item.NomNam;
                        accountNominee.NomRel = item.NomRel;
                        accountNominee.CCertID = item.CCertID;
                        accountNominee.CCertno = item.CCertno;
                        accountNominee.Share = item.Share;
                        accountNominee.ContactNo = item.ContactNo;
                        accountNominee.ContactAddress = item.ContactAddress;

                        if (item.NID == 0)
                        {
                            accountDetails.ANominees.Add(accountNominee);
                        }
                        else
                        {
                            accountNominee.IAccno = aModelDetails.IAccno;
                            uow.Repository<ANominee>().Edit(accountNominee);
                        }
                    }
                    #endregion

                    #region Customer Account
                    //Account Of Customer
                    byte count = 1;
                    foreach (var item in aModelDetails.CustomerId)
                    {
                        AOfCust accountOfCustomer = uow.Repository<AOfCust>().FindBy(x => x.IAccno == aModelDetails.IAccno && x.CID == item).FirstOrDefault();
                        //  int accountCustId = 0;
                        if (accountOfCustomer == null)
                        {
                            accountOfCustomer = new AOfCust();
                        }
                        //else
                        //{
                        //    accountCustId = accountOfCustomer.AOCId;
                        //}

                        accountOfCustomer.CID = item;
                        accountOfCustomer.Sno = count;
                        count++;
                        //if (accountCustId == 0)
                        accountDetails.AOfCusts.Add(accountOfCustomer);
                        //else
                        //    uow.Repository<AOfCust>().Edit(accountOfCustomer);
                    }
                    #endregion

                    #region movement Account
                    //If movement Account given

                    if (TellerUtilityService.IsMovementAccount(aModelDetails.PID))
                    {
                        ANomAcc aNomineeAcc = uow.Repository<ANomAcc>().GetSingle(x => x.NAId == aModelDetails.MovementAccId);
                        if (aNomineeAcc == null)
                        {
                            aNomineeAcc = new ANomAcc();
                        }
                        var nomineeAccountId = uow.Repository<ADetail>().GetSingle(x => x.IAccno == aModelDetails.MovementAcc);
                        if (aModelDetails.MovementAcc != 0)
                        {
                            aNomineeAcc.NIAccno = aModelDetails.MovementAcc;
                        }

                        if (aModelDetails.MovementAccId == 0)
                        {
                            accountDetails.ANomAccs.Add(aNomineeAcc);
                        }
                        else
                        {
                            uow.Repository<ANomAcc>().Edit(aNomineeAcc);
                        }

                    }
                    #endregion

                    #region reference
                    //reference Information
                    ReferenceInfo reference = uow.Repository<ReferenceInfo>().GetSingle(x => x.RIId == aModelDetails.ReferenceId);
                    if (reference == null)
                    {
                        reference = new ReferenceInfo();
                    }
                    reference.ReferredBy = aModelDetails.ReferredBy[0];
                    reference.BroughtBy = aModelDetails.BroughtBy;
                    if (aModelDetails.ReferenceId == 0)
                    {
                        accountDetails.ReferenceInfoes.Add(reference);
                    }
                    else
                    {
                        uow.Repository<ReferenceInfo>().Edit(reference);
                    }
                    #endregion

                    #region agreement amount
                    //if agreement amount  given


                    if (TellerUtilityService.IsFixedAccount(SchemeId.SDID, aModelDetails.PID) || TellerUtilityService.IsRecurringDeposit(SchemeId.SDID, aModelDetails.PID))
                    {
                        ADrLimit agreementDrLimit = uow.Repository<ADrLimit>().GetSingle(x => x.IAccno == aModelDetails.IAccno);
                        if (agreementDrLimit == null)
                        {
                            agreementDrLimit = new ADrLimit();
                        }
                        if (TellerUtilityService.IsRecurringDeposit(SchemeId.SDID, aModelDetails.PID))
                        {
                            agreementDrLimit.AppAmt = aModelDetails.ContributionAmount;
                        }
                        else
                        {
                            agreementDrLimit.AppAmt = aModelDetails.AgreementAmount;
                        }
                      
                        if (aModelDetails.IAccno == 0)
                        {
                            accountDetails.ADrLimit = agreementDrLimit;
                        }

                    }

                    if (TellerUtilityService.HasDuration(SchemeId.SDID, aModelDetails.PID))
                    {

                        ADur accountDuration = uow.Repository<ADur>().GetSingle(x => x.IAccno == aModelDetails.IAccno);
                        if (accountDuration == null)
                        {
                            accountDuration = new ADur();
                        }

                        if (TellerUtilityService.IsFromNow(SchemeId.SDID))
                        {
                            string type = commonService.DateType();
                            if (aModelDetails.DateType == true)
                            {
                                type = "1";
                            }
                            DatePickerService datePickerService = new DatePickerService();
                            var durationById = this.GetDurationByDurationId(aModelDetails.Duration);
                            int matDuration = 0;
                            int interestCal = 0;
                            matDuration = TellerUtilityService.GetmatDurationMonth(durationById.Value);
                            float value = GetCapitalizeRuleValueByProductDurationIntId(aModelDetails.InterestCapitalize);
                            try
                            {
                                interestCal = Convert.ToInt32(matDuration / value);
                            }
                            catch
                            {
                                interestCal = 0;
                            }
                            float currentMonth = value;
                            DateTime startDate = aModelDetails.RDate;
                            for (int i = 0; i < interestCal; i++)
                            {
                                ADSch adSch = new ADSch();
                                if (type == "1")
                                {
                                    DateTime scheduleDate = commonService.GetMatDate(startDate, Convert.ToDecimal(currentMonth), type);
                                    adSch.MDate = scheduleDate;
                                    accountDetails.ADSches.Add(adSch);
                                    currentMonth += value;
                                }
                                else
                                {
                                    DateTime scheduleDate = commonService.GetMatDate(startDate, Convert.ToDecimal(value), type);
                                    adSch.MDate = scheduleDate;
                                    accountDetails.ADSches.Add(adSch);
                                    startDate = scheduleDate;
                                }

                            }

                        }
                        accountDuration.DurationId = aModelDetails.Duration;
                        accountDuration.ValDat = aModelDetails.RDate;
                        accountDuration.MatDat = aModelDetails.MaturationDate;
                        if (aModelDetails.IAccno == 0)
                        {
                            accountDetails.ADur = accountDuration;
                        }
                        else
                        {
                            uow.Repository<ADur>().Edit(accountDuration);
                        }
                    }
                    #endregion

                    #region Interest Rate
                    //Account Interest Rate
                    ARateMaster accountRateMaster = uow.Repository<ARateMaster>().GetSingle(x => x.ARMID == aModelDetails.IRateMasterId);
                    if (accountRateMaster == null)
                    {
                        accountRateMaster = new ARateMaster();
                    }

                    accountRateMaster.PostedBy = Global.UserId;
                    accountRateMaster.PostedDate = commonService.GetBranchTransactionDate();
                    accountRateMaster.EffectiveDate = commonService.GetBranchTransactionDate();
                    if (aModelDetails.IRateMasterId == 0)
                    {
                        uow.Repository<ARateMaster>().Add(accountRateMaster);
                    }
                    else
                    {
                        uow.Repository<ARateMaster>().Edit(accountRateMaster);
                    }
                    ARate accountInterestRate = uow.Repository<ARate>().GetSingle(x => x.ARMID == aModelDetails.IRateMasterId);
                    if (accountInterestRate == null)
                    {
                        accountInterestRate = new ARate();
                    }

                    if (TellerUtilityService.HasIndiviualInterestRate(aModelDetails.PID))
                    {
                        accountInterestRate.IRate = (float)aModelDetails.InterestRate;
                        accountDetails.IRate = aModelDetails.InterestRate;
                    }
                    else
                    {
                        accountInterestRate.IRate = (float)GetSingleInterestProductDurInt(aModelDetails.InterestCapitalize, aModelDetails.PID);
                        accountDetails.IRate = GetSingleInterestProductDurInt(aModelDetails.InterestCapitalize, aModelDetails.PID);
                    }
                    if (aModelDetails.IRateMasterId == 0)
                    {
                        accountDetails.ARates.Add(accountInterestRate);
                        accountRateMaster.ARates.Add(accountInterestRate);
                    }
                    else
                    {
                        uow.Repository<ARate>().Edit(accountInterestRate);
                    }
                    #endregion

                    #region Account Minimun balance Change
                    AMinBal accountMinBal = uow.Repository<AMinBal>().GetSingle(x => x.IAccno == accountDetails.IAccno);
                    if (accountMinBal == null)
                    {
                        accountMinBal = new AMinBal();
                    }
                    if (aModelDetails.MinimumBalance != 0)
                    {
                        if (TellerUtilityService.HasIndividualLimit(aModelDetails.PID))
                        {
                            accountMinBal.Minbal = aModelDetails.MinimumBalance;
                        }
                        else
                        {
                            accountMinBal.Minbal = productDetails.MinimumAmount;
                        }

                        if (aModelDetails.IAccno == 0)
                        {
                            if (aModelDetails.MinimumBalance != 0)
                            {
                                accountDetails.AMinBal = accountMinBal;
                            }

                        }
                        else
                        {
                            uow.Repository<AMinBal>().Edit(accountMinBal);
                        }
                    }


                    #endregion

                    #region PolicyInterest
                    var policyInterest = uow.Repository<APolicyInterest>().GetSingle(x => x.IAccno == aModelDetails.IAccno);
                    if (policyInterest == null)
                    {
                        policyInterest = new APolicyInterest();
                    }
                    policyInterest.PSID = aModelDetails.InterestCalRule;
                    var ruleIcdDuration = uow.Repository<AICBDur>().GetSingle(x => x.IAccno == aModelDetails.IAccno);
                    if (ruleIcdDuration == null)
                    {
                        ruleIcdDuration = new AICBDur();
                    }
                    ruleIcdDuration.ICBDurID = aModelDetails.InterestCapitalize;
                    if (aModelDetails.IAccno == 0)
                    {
                        accountDetails.APolicyInterest = policyInterest;
                        accountDetails.AICBDur = ruleIcdDuration;
                    }
                    else
                    {
                        uow.Repository<APolicyInterest>().Edit(policyInterest);
                        uow.Repository<AICBDur>().Edit(ruleIcdDuration);
                    }


                    #endregion

                    if (aModelDetails.IAccno == 0)
                    {
                        accountDetails.PostedBy = Global.UserId.ToString();
                        uow.Repository<ADetail>().Add(accountDetails);
                    }
                    else
                    {
                        //accountDetails.ApprovedBy=Global.UserId.ToString();
                        //accountDetails.AccState = 1;
                        uow.Repository<ADetail>().Edit(accountDetails);
                    }

                    uow.Commit();

                    int iaccno = accountDetails.IAccno;
                    taskUow.SaveTaskNotification(TaskVerifierList, iaccno, 1);
                   
                        financeParameterService.ChargeUnverifiedTransaction(ChargeDetailsList, iaccno, TaskVerifierList);
                    
                    
                    Transaction.Commit();
                    returnMessage.Msg = "Account create Successfully";
                    returnMessage.Success = true;
                }

                catch (Exception ex)
                {
                    Transaction.Rollback();
                    returnMessage.Msg = "Account  not save. Please contact your administrator!!";
                    returnMessage.Success = false;
                }
            }
            return returnMessage;
        }



        public CurrencyRateViewModel GetCurrencyRateAndExchangeRate(int currencyId)
        {
            var currencyRate = uow.Repository<FrxCurrencyRate>().FindBy(x => x.CurrID == currencyId).Select(x => new CurrencyRateViewModel()
            {
                BuyingRate = x.Brate,
                RatePerUnit = x.BSPerUnit
            }).FirstOrDefault();
            return currencyRate;
        }

        public IPagedList<AccountDetailsViewModel> GetAllAccountList(int accState, int pageNumber, int pageSize)
        {
            if (accState != 0)
            {
                var accountOpenList = _context.ADetails.Where(x => x.AccState == accState).Select(x => new AccountDetailsViewModel
                {
                    Accno = x.Accno,
                    Aname = x.Aname,
                    RDate = x.RDate,
                    IAccno = x.IAccno,
                    AccState = x.AccState


                }).OrderByDescending(x => x.IAccno).ToPagedList(pageNumber, pageSize);

                return accountOpenList;
            }
            else
            {
                var accountOpenList = _context.ADetails.Select(x => new AccountDetailsViewModel
                {
                    Accno = x.Accno,
                    Aname = x.Aname,
                    RDate = x.RDate,
                    IAccno = x.IAccno,
                    AccState = x.AccState
                }).OrderByDescending(x => x.IAccno).ToPagedList(pageNumber, pageSize);

                return accountOpenList;
            }

        }

        public List<CurrencyModel> GetAllCurrencyList()
        {

            var CurrencyList = (from a in _context.ADetails
                                join c in _context.CurrencyTypes on a.CurrID equals c.CTId
                                select new CurrencyModel
                                {
                                    CurrencyName = c.CurrencyName,
                                    CTId = c.CTId,
                                    Country = c.Country
                                }).Distinct().ToList();
            var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
            {

                CurrencyName = x.CurrencyName + "(" + x.Country + ")",
                CTId = x.CTId
            }).ToList();

            return CodeCurrencyList;
        }

        //public List<CurrencyModel> GetAllCurrencyList(string currencyCode, string currencyName)
        //{
        //    var CurrencyList = GetAllCurrencyList();
        //    if (currencyCode == "" && currencyName == "")
        //    {
        //        var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
        //        {
        //            CurrencyCode = TellerUtilityService.GetCurrencyCode(x.CTId),
        //            CurrencyName = x.CurrencyName + "(" + x.Country + ")",
        //            CTId = x.CTId
        //        }).ToList();
        //        return CodeCurrencyList;
        //    }
        //    else if (currencyCode != "" && currencyName == "")
        //    {
        //        var CodeCurrencyList = GetAllCurrencyList(currencyCode);
        //        return CodeCurrencyList;
        //    }
        //    else
        //    {
        //        var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
        //        {
        //            CurrencyCode = TellerUtilityService.GetCurrencyCode(x.CTId),
        //            CurrencyName = x.CurrencyName + "(" + x.Country + ")",
        //            CTId = x.CTId
        //        }).Where(x => x.CurrencyName.Contains(currencyCode)).ToList();
        //        return CodeCurrencyList;
        //    }

        //}

        //public List<CurrencyModel> GetAllCurrencyList(string currencyCode)
        //{
        //    var CurrencyList = GetAllCurrencyList();
        //    var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
        //    {
        //        CurrencyCode = TellerUtilityService.GetCurrencyCode(x.CTId),
        //        CurrencyName = x.CurrencyName + "(" + x.Country + ")",
        //        CTId = x.CTId
        //    }).ToList();

        //    var a = CodeCurrencyList.Where(x => x.CurrencyCode.Contains(currencyCode)).ToList();
        //    return a;
        //}

        public IPagedList<ProductViewModel> GetAllProductList(string productCode, int pageNumber, int pageSize)
        {
            if (productCode == "")
            {
                var ProductList = _context.ProductDetails.Select(x => new ProductViewModel
                {
                    ProductCode = x.Apfix,
                    ProductId = x.PID,
                    ProductName = x.PName
                }).OrderBy(x => x.ProductCode).ToPagedList(pageNumber, pageSize);
                return ProductList;
            }
            else
            {
                var ProductList = _context.ProductDetails.Where(x => x.Apfix.Contains(productCode)).Select(x => new ProductViewModel
                {
                    ProductCode = x.Apfix,
                    ProductId = x.PID,
                    ProductName = x.PName
                }).OrderBy(x => x.ProductCode).ToPagedList(pageNumber, pageSize);
                return ProductList;
            }

        }

        public IPagedList<ProductViewModel> GetAllProductListByName(string productName, int pageNumber, int pageSize)
        {
            var ProductList = _context.ProductDetails.Where(x => x.PName.Contains(productName)).Select(x => new ProductViewModel
            {
                ProductCode = x.Apfix,
                ProductId = x.PID,
                ProductName = x.PName
            }).OrderBy(x => x.ProductCode).ToPagedList(pageNumber, pageSize); ;
            return ProductList;
        }

        public List<ProductViewModel> GetAllProductList(string productCode)
        {
            var ProductList = _context.ProductDetails.Where(x => x.Apfix.Contains(productCode)).Select(x => new ProductViewModel
            {
                ProductCode = x.Apfix,
                ProductId = x.PID,
                ProductName = x.PName
            }).ToList();
            return ProductList;
        }

        public ProductViewModel GetSingleProduct(int productId)
        {
            var singleProduct = uow.Repository<ProductDetail>().FindBy(x => x.PID == productId).Select(x => new ProductViewModel
            {
                ProductCode = x.Apfix,
                ProductId = x.PID,
                IntraBrnhTrnx = x.IntraBrnhTrnx
            }).FirstOrDefault();
            return singleProduct;
        }

        //public CurrencyModel GetSingleCurrency(int currencyId)
        //{
        //    var CurrencyList = GetAllCurrencyList();
        //    var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
        //    {
        //        CurrencyCode = TellerUtilityService.GetCurrencyCode(x.CTId),
        //        CurrencyName = x.CurrencyName + "(" + x.Country + ")",
        //        CTId = x.CTId
        //    }).Where(x => x.CTId == currencyId).FirstOrDefault();
        //    return CodeCurrencyList;
        //}

        public AccountDetailsViewModel GetCodeByAccountNumber(string accountNumber)
        {
            try
            {
                var Getsingle = uow.Repository<ADetail>().FindBy(x => x.Accno.Contains(accountNumber)).Select(x => new AccountDetailsViewModel()
                {
                    PID = x.PID,
                    CurrID = x.CurrID,
                    BrchID = x.BrchID
                }).FirstOrDefault();


                return Getsingle;
            }
            catch (Exception ex)
            {

                throw new Exception("");
            }
        }

        public List<AccountSearchViewModel> GetAccountNumberList(string accountNumber, int pageNo, int pageSize)
        {
            AccountDetailsViewModel getAccountCode = new AccountDetailsViewModel();
            string query = "";

            query = @"select COUNT(*) OVER () AS TotalCount,* from fin.FGetAccountNumberList()";
            if (accountNumber != "")
            {

                query += " where AccountNumber like '%" + accountNumber + "%'";
            }
            else
            {
                query += " where BrchID=1";
            }
            query += @" ORDER BY  AccountName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var accountSearchList = uow.Repository<AccountSearchViewModel>().SqlQuery(query).ToList();
            return accountSearchList;
        }

        public List<AccountSearchViewModel> GetAccountNumberList(int branchCode, int productCode, int currencyCode, string customerName, string filterAccount, string accountType, int pageNo, int pageSize)
        {
            AccountDetailsViewModel getAccountCode = new AccountDetailsViewModel();
            string query = "";

            query = @"select COUNT(*) OVER () AS TotalCount,* from fin.FGetAccountNumberList() where BrchID=" + branchCode + "and CurrID=" + currencyCode;

            if (productCode != 0)
            {

                query += " and  PID=" + productCode;
            }

            if (customerName != "")
            {
                query += "and AccountName like '%" + customerName + "%'";
            }
            if (filterAccount == EAccountFilter.DepositAccept.GetDescription())
            {
                query += " and  AccState in(1,5)";
            }
            else if (filterAccount == EAccountFilter.WithdrawAccept.GetDescription())
            {
                query += " and  AccState in(1,9)";
            }
            else if (filterAccount == EAccountFilter.Nominee.GetDescription())
            {
                query += " and  AccState in(1,9,5)";
            }
            if (accountType == EAccountType.Normal.GetDescription())
            {
                query += " and  SType=0";
            }
            else if(accountType == EAccountType.Loan.GetDescription())
            {
                query += " and  SType=1";
            }
            query += @" ORDER BY  AccountName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var accountSearchList = uow.Repository<AccountSearchViewModel>().SqlQuery(query).ToList();
            return accountSearchList;
        }

        public AccountSearchViewModel GetSelectAccountNumber(Int64 accountNumber)
        {
            var getSingelAccountNumber = uow.Repository<ADetail>().FindBy(x => x.IAccno == accountNumber).Select(x => new AccountSearchViewModel
            {
                AccountId = x.IAccno,
                AccountNumber = x.Accno,
            }).FirstOrDefault();

            return getSingelAccountNumber;
        }
        public List<AccountDetailsViewModel> GetAccountNumber(string accountNumber)
        {
            try
            {
                var getManyAccountNumber = uow.Repository<ADetail>().FindBy(x => x.Accno.Contains(accountNumber)).Select(x => new AccountDetailsViewModel()
                {
                    Accno = x.Accno,
                    IAccno = x.IAccno
                }).ToList();


                return getManyAccountNumber;
            }
            catch (Exception ex)
            {

                throw new Exception("");
            }
        }
        #endregion
        #region Account Client Details
        public List<CustomerAccountsViewModel> GetAccountsWiseCustomer(Int64 IAccno = 0)
        {
            var customerAccounts = uow.Repository<CustomerAccountsViewModel>().SqlQuery(@"select IAccno,cl.CID,Name,Contact,location from fin.AOfCust ac 
                                                                                           inner join cust.FGetCustList() cl on ac.CID=cl.cid where IAccno = (" + IAccno + ")").ToList();

            return customerAccounts;
        }
        #endregion
        #region Account Verify
        public ReturnBaseMessageModel VerifyAccount(int IAccno)
        {
            using (var Transaction = uow.BeginTransaction())
            {
            try
            {
                ADetail ad = uow.Repository<ADetail>().GetSingle(x => x.IAccno == IAccno);
                ad.AccState = 1;
                ad.ApprovedBy = Loader.Models.Global.UserId.ToString();
                uow.Repository<ADetail>().Edit(ad);
                    var cst = financeParameterService.ChargeTranxNoByEventIdAndEventValue(4, IAccno);
                   
                    foreach (var item in cst)
                    {
                        financeParameterService.VerifyChargeTransaction(item.TrnxNo);
                    }
                uow.Commit();
                    Transaction.Commit();
                returnMessage.Msg = "Account Verified Successfully";
                returnMessage.Success = true;
            }
            catch (Exception ex)
            {
                    Transaction.Rollback();
                returnMessage.Msg = "Account Cannot Verified Successfully";
                returnMessage.Success = false;

            }
            return returnMessage;
        }

        }
        #endregion

        #region DenoList
        public DenoInOutViewModel DenoList(int currencyId)
        {
            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            int userId = Loader.Models.Global.UserId;
            int UserLevel = 2;
            var deloList = uow.Repository<DenoInViewModel>().SqlQuery("select * from fin.FGetCurrentDenoList(" + userId + "," + UserLevel + "," + currencyId + ")").ToList();

            var denoOutList = deloList.Select(x => new DenoOutViewModel()
            {
                CurrIDOut = x.CurrID,
                DenoBalIdOut = x.DenoBalId,
                DenoIDOut = x.DenoID,
                DenoOut = x.Deno,
                PieceOut = x.Piece,
                UserIdOut = x.UserId,
                UserLevelOut = x.UserLevel
            }).ToList();
            denoInOutModel.DenoInList = deloList;
            denoInOutModel.DenoOutList = denoOutList;
            return denoInOutModel;
        }


        #endregion

        #region Depost transaction
        public AccountDetailsShowViewModel GetAccountDetailsViewShow(Int64 accountId)
        {
            var accountDetailsShow = uow.Repository<AccountDetailsShowViewModel>().SqlQuery("select * from fin.FGetAccountDetails(" + accountId + ")").FirstOrDefault();
            return accountDetailsShow;
        }
        public byte GetSchemeIdByAccountId(Int64 accounId)
        {
            byte schemeId = _context.Database.SqlQuery<byte>(@"select s.SDID from fin.ADetail ad join fin.ProductDetail pd on ad.PID=pd.PID
                                                               join fin.SchmDetails s on s.SDID = pd.SDID where ad.IAccno =" + accounId).FirstOrDefault();
            return schemeId;
        }
        public ReturnBaseMessageModel InsertDepositTransaction(DepositViewModel depositModel, DenoInOutViewModel denoInOutModel, TaskVerificationList taskVerifierList)
        {
            using (var transaction = uow.BeginTransaction())
            {
                try
                {
                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                    int productId = uow.Repository<ADetail>().FindBy(x => x.IAccno == depositModel.AccountId).Select(x => x.PID).FirstOrDefault();
                    if (TellerUtilityService.IsFixedAccount(this.GetSchemeIdByAccountId(depositModel.AccountId), productId))
                    {
                        var adrLimit = uow.Repository<ADrLimit>().FindBy(x => x.IAccno == depositModel.AccountId).Select(x => x.AppAmt).FirstOrDefault();
                        var crAmount = uow.Repository<ABal>().FindBy(x => x.IAccno == depositModel.AccountId && x.Flag == 2).Select(x => x.Bal).FirstOrDefault();
                        var goodAmount = uow.Repository<ABal>().FindBy(x => x.IAccno == depositModel.AccountId && x.Flag == 3).Select(x => x.Bal).FirstOrDefault();
                        decimal currentAmount = crAmount + goodAmount;
                        decimal addedAmount = currentAmount + depositModel.Amount;
                        if (addedAmount > adrLimit)
                        {
                            returnMessage.Msg = "Deposit amount is greater than aggrement amount.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }
                    if (IsTrnsWithDeno)
                    {
                        if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, depositModel.Amount))
                        {
                            returnMessage.Msg = "Amount is not match with deno balance.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        returnMessage = TellerUtilityService.AvailableDenoNumber(denoInOutModel.DenoOutList);
                        if (!returnMessage.Success)
                        {
                            return returnMessage;
                        }

                    }

                    ASTrn asTransaction = new ASTrn();

                    asTransaction.IAccno = depositModel.AccountId;
                    asTransaction.tdate = commonService.GetBranchTransactionDate();
                    asTransaction.notes = depositModel.DepositBy;
                    asTransaction.slpno = depositModel.ReceiptNo;
                    asTransaction.cramt = depositModel.Amount;
                    asTransaction.ttype = 1;
                    asTransaction.postedby = Loader.Models.Global.UserId;
                    asTransaction.IsSlp = true;
                    asTransaction.brnhno = commonService.GetBranchIdByEmployeeUserId(Loader.Models.Global.UserId);
                    Int64 transactionNumber = commonService.GetUtno();
                    asTransaction.tno = transactionNumber;

                    if (IsTrnsWithDeno)
                    {
                        foreach (var item in denoInOutModel.DenoInList)
                        {
                            DenoTrxn denoTranx = new DenoTrxn();
                            var getSingle = uow.Repository<DenoBal>().GetSingle(x => x.DenoBalId == item.DenoBalId);
                            if (getSingle == null)
                            {
                                getSingle = new DenoBal();
                            }


                            var currentNumber = uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalId).Select(x => x.Piece).FirstOrDefault();
                            var OutPiece = denoInOutModel.DenoOutList.Where(x => x.DenoBalIdOut == item.DenoBalId).Select(x => x.DenoNumberOut).FirstOrDefault();
                            int totalDeno = currentNumber + item.DenoNumber - OutPiece;
                            getSingle.DenoId = item.DenoID;
                            getSingle.UserId = Loader.Models.Global.UserId;
                            getSingle.UserLevel = 2;
                            getSingle.Piece = totalDeno;

                            denoTranx.Denoid = item.DenoID;
                            denoTranx.Trxno = transactionNumber;
                            denoTranx.vdate = commonService.GetBranchTransactionDate();
                            denoTranx.Pics = item.DenoNumber;
                            if (item.DenoNumber != 0)
                            {
                                uow.Repository<DenoTrxn>().Add(denoTranx);
                            }
                            if (item.DenoBalId == null)
                            {
                                if (getSingle.Piece != 0)
                                {
                                    uow.Repository<DenoBal>().Add(getSingle);
                                }
                            }
                            else
                            {
                                uow.Repository<DenoBal>().Edit(getSingle);
                            }
                        }
                        foreach (var item in denoInOutModel.DenoOutList)
                        {
                            DenoTrxn denoTranx = new DenoTrxn();
                            denoTranx.Denoid = item.DenoIDOut;
                            denoTranx.Trxno = transactionNumber;
                            denoTranx.vdate = commonService.GetBranchTransactionDate();
                            denoTranx.Pics = (-item.DenoNumberOut);
                            if (item.DenoNumberOut != 0)
                            {
                                uow.Repository<DenoTrxn>().Add(denoTranx);
                            }
                        }

                    }
                    decimal shadowCr = 0;
                    ABal availableForInsert = null;
                    ABal availableBal = uow.Repository<ABal>().GetSingle(x => x.IAccno == depositModel.AccountId && x.Flag == 2);
                    if (availableBal == null)
                    {
                        availableBal = new ABal();
                        //  availableForInsert = new ABal();
                    }
                    else
                    {
                        shadowCr = availableBal.Bal;
                        availableForInsert = availableBal;
                    }
                    availableBal.IAccno = depositModel.AccountId;
                    availableBal.Flag = 2;
                    availableBal.FId = commonService.GetFidByIAccno(depositModel.AccountId);
                    availableBal.Bal = depositModel.Amount + shadowCr;
                    if (availableForInsert == null)
                    {
                        uow.Repository<ABal>().Add(availableBal);
                    }
                    else
                    {
                        uow.Repository<ABal>().Edit(availableBal);
                    }
                    uow.Repository<ASTrn>().Add(asTransaction);
                    uow.Commit();

                    taskUow.SaveTaskNotification(taskVerifierList, transactionNumber, 2);
                    transaction.Commit();
                    returnMessage.Success = true;
                    returnMessage.Msg = "Deposit save successfully with Transaction number #" + transactionNumber;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Deposit not save.!!";
                    return returnMessage;
                }
            }


        }

        public List<AccountBalanceViewModel> GetAccountBalanceByFlag(Int64 iAccno)
        {
            var accountBalance = (from ab in uow.Repository<ABal>().FindBy(x => x.IAccno == iAccno)
                                  join f in uow.Repository<Flag>().GetAll() on ab.Flag equals f.FId
                                  select new AccountBalanceViewModel()
                                  {
                                      Amount = ab.Bal,
                                      FlagName = f.FlagName,
                                      FlagId = f.FId

                                  }).ToList();
            return accountBalance;
        }

        public IPagedList<ASTrnViewModel> GetUnverifiedTransactionList(string accountNumber, int pageNo, int PageSize)
        {

            var unverifiedList = (from a in uow.Repository<ASTrn>().GetAll()
                                  join ac in uow.Repository<ADetail>().GetAll() on a.IAccno equals ac.IAccno
                                  join t in uow.Repository<TType>().GetAll() on a.ttype equals t.Id
                                  where a.brnhno == commonService.GetBranchIdByEmployeeUserId(Loader.Models.Global.UserId) && string.IsNullOrEmpty(accountNumber) || ac.Accno.Contains(accountNumber)
                                  select new ASTrnViewModel()
                                  {
                                      accountnumber = ac.Accno,
                                      tdate = a.tdate,
                                      tno = a.tno,
                                      notes = a.notes,
                                      slpno = a.slpno,
                                      dramt = a.dramt,
                                      cramt = a.cramt,
                                      ttype = t.TType1
                                  }
                                ).ToPagedList(pageNo, PageSize);
            return unverifiedList;
        }

        public ASTrnViewModel GetSingleUnverifiedTransaction(Int64 utno)
        {
            ASTrnViewModel unverifiedList = new ASTrnViewModel();

            unverifiedList = (from a in uow.Repository<ASTrn>().GetAll()
                              join ac in uow.Repository<ADetail>().GetAll() on a.IAccno equals ac.IAccno
                              join c in uow.Repository<CurrencyType>().GetAll() on ac.CurrID equals c.CTId
                              where a.tno == utno
                              select new ASTrnViewModel()
                              {
                                  IAccno = a.IAccno,
                                  tdate = a.tdate,
                                  tno = a.tno,
                                  notes = a.notes,
                                  slpno = a.slpno,
                                  dramt = a.dramt,
                                  cramt = a.cramt,
                                  currency = c.CurrencyName + "(" + c.Country + ")",
                                  IsSlp = a.IsSlp

                              }
                               ).FirstOrDefault();
            if (unverifiedList == null)
            {
                unverifiedList = (from a in uow.Repository<AMTrn>().GetAll()
                                  join ac in uow.Repository<ADetail>().GetAll() on a.IAccno equals ac.IAccno
                                  join c in uow.Repository<CurrencyType>().GetAll() on ac.CurrID equals c.CTId
                                  where a.tno == utno
                                  select new ASTrnViewModel()
                                  {
                                      IAccno = a.IAccno,
                                      tdate = a.tdate,
                                      tno = a.tno,
                                      notes = a.notes,
                                      slpno = a.slpno,
                                      dramt = a.dramt,
                                      cramt = a.cramt,
                                      currency = c.CurrencyName + "(" + c.Country + ")",
                                      IsSlp = a.Isslp
                                  }
                                             ).FirstOrDefault();
            }
            return unverifiedList;
        }

        public List<DenoInViewModel> GetDenoByTransactionNumber(Int64 utno)
        {
            var denoNumberList = (from dt in uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == utno)
                                  join d in uow.Repository<Denosetup>().GetAll() on dt.Denoid equals d.DenoID
                                  join c in uow.Repository<CurrencyType>().GetAll() on d.CurrID equals c.CTId
                                  where dt.Trxno == utno
                                  select new DenoInViewModel()
                                  {
                                      Deno = d.Deno,
                                      Piece = dt.Pics,
                                      Currency = c.CurrencyName + "(" + c.Country + ")",
                                      DenoID = d.DenoID

                                  }).ToList();


            return denoNumberList;
        }
        public int VerifyDepositWithdrawTransaction(Int64 tNo)
        {
            string UserId = Loader.Models.Global.UserId.ToString();
            int isSuccess = _context.Database.ExecuteSqlCommand("exec [Trans].[PSetDepositWithdrawVerify] @TNO,@APPROVEDBY", new SqlParameter("@TNO", tNo), new SqlParameter("@APPROVEDBY", UserId));
            return isSuccess;
        }

        #endregion

        #region Withdraw transaction
        public ReturnBaseMessageModel WithdrawTransaction(WithdrawViewModel withdrawModel, DenoInOutViewModel denoInOutModel, TaskVerificationList taskVerifier, InterestPayableViewModel intPayModel)
        {
            using (var transaction = uow.BeginTransaction())
            {
                try
                {
                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();

                    int productId = uow.Repository<ADetail>().FindBy(x => x.IAccno == withdrawModel.AccountId).Select(x => x.PID).FirstOrDefault();
                    if (withdrawModel.Amount != 0 && intPayModel.InterestAmount != 0 && withdrawModel.Payee != "" && intPayModel.InterestPayee != "")
                    {
                        returnMessage.Msg = "Please fill either withdraw or interest payable.!! Both is NOT Allowed. ";
                        returnMessage.Success = false;
                        return returnMessage;
                    }

                    if (withdrawModel.Amount != 0 && withdrawModel.Payee != null)
                    {
                        var goodAmount = uow.Repository<ADetail>().FindBy(x => x.IAccno == withdrawModel.AccountId).Select(x => x.Bal).FirstOrDefault();
                        decimal TransshadowDr = uow.Repository<ABal>().FindBy(x => x.IAccno == withdrawModel.AccountId && x.Flag == 1).Select(x => x.Bal).FirstOrDefault();

                        decimal currentAmount = goodAmount - withdrawModel.Amount - TransshadowDr;

                        if (currentAmount < 0)
                        {
                            returnMessage.Msg = "Withdraw NOT Allowed!!Amount is greater then available amount.";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        if (withdrawModel.withdraw == "Cheque")
                        {
                            bool IsExiestscheque = TellerUtilityService.CheckChequeNumber(withdrawModel.ChequeNumber, withdrawModel.AccountId);
                            if (IsExiestscheque == false)
                            {
                                returnMessage.Msg = "This cheque number doesn't belongs to account holder.";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            if (TellerUtilityService.IsDuplicateChequeNo(withdrawModel.AccountId, withdrawModel.ChequeNumber))
                            {
                                returnMessage.Msg = "Cheque  number is already used.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            returnMessage = TellerUtilityService.CheckChequeState(withdrawModel.AccountId, withdrawModel.ChequeNumber);
                            if (!returnMessage.Success)
                            {
                                return returnMessage;
                            }

                            if (TellerUtilityService.IsDeactiveChequeBook(withdrawModel.AccountId, withdrawModel.ChequeNumber))
                            {
                                returnMessage.Msg = "Cheque book is finished.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                        }
                        if (IsTrnsWithDeno)
                        {
                            if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, withdrawModel.Amount))
                            {
                                returnMessage.Msg = "Amount is not match with deno balance.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            returnMessage = TellerUtilityService.AvailableDenoNumber(denoInOutModel.DenoOutList);
                            if (!returnMessage.Success)
                            {
                                return returnMessage;
                            }

                        }
                    }
                    else if (intPayModel.InterestAmount != 0 && intPayModel.InterestPayee != null)
                    {
                        var interestAvailable = InterestPayable(withdrawModel.AccountId);
                        if (interestAvailable.IsNomee == true)
                        {
                            decimal sumInterest = interestAvailable.Amount - intPayModel.InterestAmount;
                            if (sumInterest <= 0)
                            {
                                returnMessage.Msg = "Withdraw NOT Allowed!!Amount is greater then available amount.";
                                returnMessage.Success = false;
                                return returnMessage;
                            }

                        }



                        if (IsTrnsWithDeno)
                        {
                            if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, intPayModel.InterestAmount))
                            {
                                returnMessage.Msg = "Amount is not match with deno balance.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            returnMessage = TellerUtilityService.AvailableDenoNumber(denoInOutModel.DenoOutList);
                            if (!returnMessage.Success)
                            {
                                return returnMessage;
                            }

                        }
                    }

                    if (TellerUtilityService.IsFixedAccount(this.GetSchemeIdByAccountId(withdrawModel.AccountId), productId))
                    {
                        var durationComplete = uow.Repository<ADetail>().FindBy(x => x.IAccno == withdrawModel.AccountId && x.AccState == 7).FirstOrDefault();
                        if (durationComplete == null)
                        {
                            returnMessage.Msg = "Withdraw NOT Allowed!!duration not complete.";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }

                    Int64 transactionNumber = commonService.GetUtno();

                    if (IsTrnsWithDeno)
                    {
                        foreach (var item in denoInOutModel.DenoInList)
                        {
                            DenoTrxn denoTranx = new DenoTrxn();
                            var getSingle = uow.Repository<DenoBal>().GetSingle(x => x.DenoBalId == item.DenoBalId);
                            if (getSingle == null)
                            {
                                getSingle = new DenoBal();
                            }
                            var currentNumber = uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalId).Select(x => x.Piece).FirstOrDefault();
                            var OutPiece = denoInOutModel.DenoOutList.Where(x => x.DenoBalIdOut == item.DenoBalId).Select(x => x.DenoNumberOut).FirstOrDefault();
                            int totalDeno = currentNumber + item.DenoNumber - OutPiece;
                            getSingle.DenoId = item.DenoID;
                            getSingle.UserId = Loader.Models.Global.UserId;
                            getSingle.UserLevel = 2;
                            getSingle.Piece = totalDeno;

                            denoTranx.Denoid = item.DenoID;
                            denoTranx.Trxno = transactionNumber;
                            denoTranx.vdate = commonService.GetBranchTransactionDate();
                            denoTranx.Pics = item.DenoNumber;
                            if (item.DenoNumber != 0)
                            {
                                uow.Repository<DenoTrxn>().Add(denoTranx);
                            }
                            if (item.DenoBalId == null)
                            {
                                if (getSingle.Piece != 0)
                                {
                                    uow.Repository<DenoBal>().Add(getSingle);
                                }
                            }
                            else
                            {
                                uow.Repository<DenoBal>().Edit(getSingle);
                            }
                        }
                        foreach (var item in denoInOutModel.DenoOutList)
                        {
                            DenoTrxn denoTranx = new DenoTrxn();
                            denoTranx.Denoid = item.DenoIDOut;
                            denoTranx.Trxno = transactionNumber;
                            denoTranx.vdate = commonService.GetBranchTransactionDate();
                            denoTranx.Pics = (-item.DenoNumberOut);
                            if (item.DenoNumberOut != 0)
                            {
                                uow.Repository<DenoTrxn>().Add(denoTranx);
                            }
                        }

                    }
                    if (withdrawModel.Amount != 0 && withdrawModel.Payee != null)
                    {
                        ASTrn asTransaction = new ASTrn();
                        asTransaction.IAccno = withdrawModel.AccountId;
                        asTransaction.tdate = commonService.GetBranchTransactionDate();
                        asTransaction.notes = withdrawModel.Payee;
                        if (withdrawModel.withdraw == "Cheque")
                        {
                            asTransaction.slpno = withdrawModel.ChequeNumber;
                            asTransaction.IsSlp = false;
                        }
                        else
                        {
                            asTransaction.slpno = withdrawModel.SlipNo;
                            asTransaction.IsSlp = true;
                        }
                        asTransaction.dramt = withdrawModel.Amount;
                        asTransaction.ttype = 1;
                        asTransaction.postedby = Loader.Models.Global.UserId;
                        asTransaction.brnhno = commonService.GetBranchIdByEmployeeUserId(Loader.Models.Global.UserId);
                        asTransaction.tno = transactionNumber;

                        decimal shadowDr = 0;
                        ABal availableForInsert = null;
                        ABal availableBal = uow.Repository<ABal>().GetSingle(x => x.IAccno == withdrawModel.AccountId && x.Flag == 1);
                        if (availableBal == null)
                        {
                            availableBal = new ABal();
                            //  availableForInsert = new ABal();
                        }
                        else
                        {
                            shadowDr = availableBal.Bal;
                            availableForInsert = availableBal;
                        }
                        availableBal.IAccno = withdrawModel.AccountId;
                        availableBal.Flag = 1;
                        availableBal.FId = commonService.GetFidByIAccno(withdrawModel.AccountId);
                        availableBal.Bal = withdrawModel.Amount + shadowDr;
                        if (availableForInsert == null)
                        {
                            uow.Repository<ABal>().Add(availableBal);
                        }
                        else
                        {
                            uow.Repository<ABal>().Edit(availableBal);
                        }
                        uow.Repository<ASTrn>().Add(asTransaction);
                        uow.Commit();
                        taskUow.SaveTaskNotification(taskVerifier, transactionNumber, 3);

                    }
                    else if (intPayModel.InterestAmount != 0 && intPayModel.InterestPayee != null)
                    {
                        string remarks = string.Empty;
                        remarks = "Paid";
                        if (intPayModel.InterestPayee != "")
                        {
                            remarks = " to " + intPayModel.InterestPayee;
                        }
                        if (intPayModel.InterestSlipNo != 0)
                        {
                            remarks = remarks + " for slip no. " + intPayModel.InterestSlipNo;
                        }
                        if (remarks == "Paid")
                        {
                            remarks = "";
                        }
                        AintPayable aintPayableRow = new AintPayable();
                        aintPayableRow.Iaccno = withdrawModel.AccountId;
                        aintPayableRow.Tdate = commonService.GetBranchTransactionDate();
                        aintPayableRow.IntAmt = 0;
                        aintPayableRow.TaxRt = 0;
                        aintPayableRow.DrAmt = intPayModel.InterestAmount;
                        aintPayableRow.PostedBy = Loader.Models.Global.UserId;
                        aintPayableRow.Valued = 0;
                        aintPayableRow.Tno = transactionNumber;
                        aintPayableRow.ByTeller = true;
                        aintPayableRow.Remarks = remarks;

                        uow.Repository<AintPayable>().Add(aintPayableRow);
                        uow.Commit();
                        taskUow.SaveTaskNotification(taskVerifier, transactionNumber, 6);
                    }


                    transaction.Commit();
                    returnMessage.Success = true;
                    returnMessage.Msg = "withdraw save successfully with Transaction number #" + transactionNumber;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    returnMessage.Success = false;
                    returnMessage.Msg = "withdraw not save.!!";
                    return returnMessage;
                }
            }
        }
        public InterestPayableModel InterestPayable(int accountId)
        {
            var productId = uow.Repository<ADetail>().FindBy(x => x.IAccno == accountId).Select(x => x.PID).FirstOrDefault();
            bool isNominee = TellerUtilityService.IsNominee(productId);
            decimal balance = 0;
            if (isNominee == true)
            {
                balance = _context.Database.SqlQuery<decimal>("SELECT 'bal'=isnull(SUM(CRAmt),0)-isnull(SUM(DRAmt),0) FROM fin.AIntPayable WHERE iaccno=" + accountId + " GROUP BY iaccno").FirstOrDefault();
            }
            InterestPayableModel interestPayable = new InterestPayableModel();
            interestPayable.Amount = balance;
            interestPayable.IsNomee = isNominee;
            return interestPayable;
        }

        #endregion
        public byte GetSchemeIdByProductId(int productId)
        {
            return  uow.Repository<ProductDetail>().FindBy(x => x.PID == productId).Select(x => x.SDID).FirstOrDefault();
        }
    }
}

