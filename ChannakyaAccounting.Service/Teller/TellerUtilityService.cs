using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public static class TellerUtilityService
    {
        private static TellerService tellerService = null;

        static TellerUtilityService()
        {
            tellerService = new TellerService();

        }
        public static SelectList GetAllDepositScheme()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var schemeList = uow.Repository<SchmDetail>().FindBy(x => x.SType == 0).Select(x => new SchemeModel()
                {
                    SchemeID = x.SDID,
                    SchemeName = x.SDName
                }).OrderBy(x => x.SchemeName).ToList();
                return new SelectList(schemeList, "SchemeID", "SchemeName");
            }

        }
        public static SelectList GetCertificateForNominee()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var certlist = uow.Repository<CertificateDef>().GetAll().Take(5).ToList();

                return new SelectList(certlist, "CCCertID", "CCCert");
            }



        }
        public static SelectList GetAllProductBySchemeId(int schemeId)
        {
            return new SelectList(tellerService.GetAllProductBySchemeId(schemeId), "ProductId", "ProductName");
        }
        public static SelectList GetAllInterestCalculationRuleByProductId(int productId)
        {
            return new SelectList(tellerService.GetAllInterestCalculationRuleByProductId(productId), "PloicyIntCalId", "PolicyIntCalName");
        }
        public static SelectList GetCurrencyByProductId(int productId)
        {
            return new SelectList(tellerService.GetCurrencyByProductId(productId), "CTId", "CurrencyName");
        }
        public static SelectList GetFloatingInterest(int productId)
        {
            return new SelectList(tellerService.GetAllFloatingInterestByProductId(productId), "FloatingId", "FloatingName");
        }
        public static SelectList GetInterestCapitalizeByProductId(int productId)
        {
            return new SelectList(tellerService.GetInterestCapitalizeByProductId(productId), "PloicyIntCalId", "PolicyIntCalName");
        }

        #region Condition
        public static bool IsMovementAccount(int productId)
        {
            var IsMovementAccount = tellerService.GetProductDetails(productId);
            if ((IsMovementAccount.Nomianable == true) && (IsMovementAccount.MovementId == 3 || IsMovementAccount.MovementId == 5))
                return true;
            else
                return false;

        }
        public static bool IsNominee(int productId)
        {
            var IsMovementAccount = tellerService.GetProductDetails(productId);
            if ((IsMovementAccount.Nomianable == true) && (IsMovementAccount.MovementId == 3))
                return true;
            else
                return false;

        }

        public static bool IsFixedAccount(int schemeId, int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(schemeId, productId);
            if (SchemeDetails.HasDuration == true && SchemeDetails.MultiDeposit == false && SchemeDetails.AllowWithdraw == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsRecurringDeposit(int schemeId, int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(schemeId, productId);
            if (SchemeDetails.HasDuration == true && SchemeDetails.MultiDeposit == true && SchemeDetails.AllowWithdraw == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool HasDuration(byte schemeId, int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(schemeId, productId);
            if (SchemeDetails.HasDuration == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsSchedule(byte schemeId, int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(schemeId, productId);
            if (SchemeDetails.Schedule == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsFromNow(byte schemeId)
        {
            byte icbId = tellerService.GetRuleICB(schemeId);
            if (icbId == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool HasIndividualDuration(byte productId)
        {
            var productDetails = tellerService.GetProductDetails(productId);
            if (productDetails.HasIndividualDuration == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HasIndiviualInterestRate(byte productId)
        {
            var productDetails = tellerService.GetProductDetails(productId);
            if (productDetails.HasIndividualRate == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HasIndividualLimit(byte productId)
        {
            var productDetails = tellerService.GetProductDetails(productId);
            if (productDetails.HasIndividualLimit == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static AccountDetailsViewModel GetDateDiffInMonthDays(DateTime startdate, DateTime enddate)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {

                var monthsdays = uow.Repository<AccountDetailsViewModel>().SqlQuery("select* from FGetDateInMonthDays(@startdate,@enddate)",
                    new SqlParameter("@startdate", startdate), new SqlParameter("@enddate", enddate)).FirstOrDefault();

                return monthsdays;
            }
        }

        public static SelectList GetAccountOpenCurrency()
        {
            return new SelectList(tellerService.GetAllCurrencyList(), "CTId", "CurrencyName");
        }

        public static SelectList GetBranchList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var branchList = uow.Repository<LicenseBranch>().GetAll().ToList();
                return new SelectList(branchList, "BrnhID", "BrnhNam");
            }
        }

        public static SelectList CurrentBranch(int branchId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var branchList = uow.Repository<LicenseBranch>().FindBy(x => x.BrnhID == branchId).ToList();
                return new SelectList(branchList, "BrnhID", "BrnhNam");
            }
        }
        public static string GetAccountStateNameByAccStateId(int accState)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string accountState = uow.Repository<string>().SqlQuery("Select AccountState from fin.AccountStatus where AsId=" + accState).FirstOrDefault();
                return accountState;
            }
        }

        public static SelectList DurationList(byte pid)
        {
            return new SelectList(tellerService.GetDurationList(pid), "Id", "Duration");
        }
        //public static string GetBranchCode(int branchId)
        //{
        //    string branchCode = "";
        //    if (branchId < 10)
        //    {
        //        branchCode = "00" + branchId;
        //    }
        //    else if (branchId < 100)
        //    {
        //        branchCode = "0" + branchId;
        //    }
        //    else
        //    {
        //        branchCode = branchId.ToString();
        //    }
        //    return branchCode;
        //}
        //public static string GetCurrencyCode(int CurrencyId)
        //{
        //    string currencyCode = "";
        //    if (CurrencyId < 10)
        //    {
        //        currencyCode = "00" + CurrencyId;
        //    }
        //    else if (CurrencyId < 100)
        //    {
        //        currencyCode = "0" + CurrencyId;
        //    }
        //    else
        //    {
        //        currencyCode = CurrencyId.ToString();
        //    }
        //    return currencyCode;
        //}
        public static string GetProductCode(int productId)
        {
            using (GenericUnitOfWork _uow = new GenericUnitOfWork())
            {
                var productCode = _uow.Repository<ProductDetail>().GetSingle(x => x.PID == productId);
                return productCode.Apfix;
            }
        }

        public static ReturnBaseMessageModel AvailableDenoNumber(List<DenoOutViewModel> denoOutList)
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();
            foreach (var item in denoOutList)
            {
                using (GenericUnitOfWork _uow = new GenericUnitOfWork())
                {
                    var availablePiece = _uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalIdOut).Select(x => x.Piece).FirstOrDefault();
                    decimal totaldenoPiece = availablePiece - item.DenoNumberOut;
                    if (totaldenoPiece < 0)
                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "You don't have sufficient balance for+" + item.DenoOut + "!!";
                        return returnMessage;
                    }
                }
            }
            returnMessage.Success = true;
            return returnMessage;
        }

        public static bool BalanceWithDenoAmount(DenoInOutViewModel denoInOutList, decimal amount)
        {
            decimal sum = 0;
            foreach (var item in denoInOutList.DenoInList)
            {

                var OutPiece = denoInOutList.DenoOutList.Where(x => x.DenoBalIdOut == item.DenoBalId).Select(x => x.DenoNumberOut).FirstOrDefault();
                double deno = item.Deno;

                decimal totalDeno = Convert.ToDecimal((item.DenoNumber * deno) - (OutPiece * deno));
                sum = sum + totalDeno;
            }
            if (Math.Abs(sum) != amount)
                return false;
            else
                return true;
        }

        public static int GetmatDurationMonth(double value)
        {
            int matDuration = Convert.ToInt32(Math.Floor(value / 30));
            return matDuration;
        }

        public static decimal GetmatDurationDays(double value)
        {
            decimal matDuration = Convert.ToDecimal(value / 1000);
            return matDuration;
        }

        public static bool IsCheckByDays(double value)
        {
            if (value < 180)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckChequeNumber(int chequeNumber, int accId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var cheque = uow.Repository<AChq>().FindBy(x => x.IAccno == accId && x.cto >= chequeNumber && x.cfrom <= chequeNumber).FirstOrDefault();
                if (cheque != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsDuplicateChequeNo(int iAccno, int chequeNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var tempTrans = uow.Repository<CheckChequeNo>().SqlQuery("SELECT * FROM [fin].[FGetUsedChk]() where IAccno=" + iAccno + " and ChqNo=" + chequeNo).FirstOrDefault();
                if (tempTrans != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static ReturnBaseMessageModel CheckChequeState(int iAccno, int chequeNo)
        {
            ReturnBaseMessageModel rModel = new ReturnBaseMessageModel();
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var chequeState = uow.Repository<AchqH>().FindBy(x => x.IAccno == iAccno && x.chkNo == chequeNo).FirstOrDefault();
                if (chequeState.cstate == 3)
                {
                    rModel.Success = false;
                    rModel.Msg = "Cheque book is blocked.!!";
                }
                else if (chequeState.cstate == 4)
                {
                    rModel.Success = false;
                    rModel.Msg = "This Cheque piece is blocked.!!";
                }
                else
                {
                    rModel.Success = true;
                }
                return rModel;
            }

        }

        public static bool IsDeactiveChequeBook(int iAccno, int chequeNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var deactiveChequeBook = uow.Repository<AChq>().FindBy(x => x.IAccno == iAccno && x.cto >= chequeNo && x.cfrom <= chequeNo).FirstOrDefault();
                if(deactiveChequeBook.cstate==5)
                {
                    return true;
                }else
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
