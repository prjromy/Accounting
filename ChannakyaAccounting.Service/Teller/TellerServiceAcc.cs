using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannakyaAccounting.Models.Models;
using System.ComponentModel;
using System.Web.Mvc;
using System.Data.Entity;
using Loader.Models;

namespace ChannakyaAccounting.Service.Teller
{
  
    public enum EAccountFilter
    {
        [Description("DepositAccept")]
        DepositAccept = 1,
        [Description("ReadyToClose")]
        ReadyToClose = 2,

        [Description("LoanAccept")]
        LoanAccept = 3,
        [Description("WithdrawAccept")]
        WithdrawAccept = 4,
        [Description("Nominee")]
        Nominee = 5,
    }
    public enum EAccountType
    {
        [Description("Loan")]
        Loan = 1,
        [Description("Normal")]
        Normal = 2,

    }
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());
            System.ComponentModel.DescriptionAttribute[] attributes =
                  (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(System.ComponentModel.DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
    public  class TellerServiceAcc
    {
        private GenericUnitOfWork uow = null;

        ReturnBaseMessageModel returnMessage = null;
        ApplicationDbContext _context = null;

        public TellerServiceAcc()
        {
            uow = new GenericUnitOfWork();
            _context = new ApplicationDbContext();

        }
        public  SelectList CurrentBranch(int branchId)
        {
            using (Loader.Repository.GenericUnitOfWork uow = new Loader.Repository.GenericUnitOfWork())
            {
                var branchList = uow.Repository<Loader.Models.Company>().GetAll().ToList();//.FindBy(x => x.CompanyId == branchId).ToList();
                return new SelectList(branchList, "CompanyId", "BranchName");
            }
        }
        public  SelectList GetAccountOpenCurrency()
        {
            return new SelectList(uow.Repository<Models.Models.CurrencyType>().GetAll().ToList(), "CTId", "CurrencyName");
        }
        //public List<CurrencyModel> GetAllCurrencyList()
        //{

        //    var CurrencyList = (from a in _context
        //                        join c in _context.CurrencyTypes on a.CurrID equals c.CTId
        //                        select new CurrencyModel
        //                        {
        //                            CurrencyName = c.CurrencyName,
        //                            CTId = c.CTId,
        //                            Country = c.Country
        //                        }).Distinct().ToList();
        //    var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
        //    {

        //        CurrencyName = x.CurrencyName + "(" + x.Country + ")",
        //        CTId = x.CTId
        //    }).ToList();

        //    return CodeCurrencyList;
        //}
        public  string GetProductCode(int productId)
        {
            using (GenericUnitOfWork _uow = new GenericUnitOfWork())
            {
                var productCode = _uow.Repository<ProductDetail>().GetSingle(x => x.PID == productId);
                return productCode.Apfix;
            }
        }
        public List<AccountSearchViewModelAcc> GetAccountNumberList(int branchCode, int productCode, int currencyCode, string customerName, string filterAccount, string accountType, int pageNo, int pageSize,int ledgerId = 0)
        {

            int pid = uow.Repository<ProductDetail>().GetSingle(x => x.FID == ledgerId).PID;

            AccountDetailsViewModelAcc getAccountCode = new AccountDetailsViewModelAcc();
            string query = "";

            query = @"select COUNT(*) OVER () AS TotalCount,* from fin.FGetAccountNumberList() ";// where BrchID=" + branchCode + "and CurrID=" + currencyCode;

            //if (productCode != 0)
            //{

            //    query += " where  PID=" + productCode;
            //}
            query += " where  AccState in (1,7)";
            if (customerName != "")
            {
                query += "and AccountName like '%" + customerName + "%'";
            }
            if (pid != 0)
            {
                query += "and PID ="+pid+" ";
            }
            //if (filterAccount == EAccountFilter.DepositAccept.GetDescription())
            //{
            //    query += " and  AccState in(1,5)";
            //}
            //else if (filterAccount == EAccountFilter.WithdrawAccept.GetDescription())
            //{
            //    query += " and  AccState in(1,9)";
            //}
            //else if (filterAccount == EAccountFilter.Nominee.GetDescription())
            //{
            //    query += " and  AccState in(1,9,5)";
            //}
            //if (accountType == EAccountType.Normal.GetDescription())
            //{
            //    query += " and  SType=0";
            //}
            //else if (accountType == EAccountType.Loan.GetDescription())
            //{
            //    query += " and  SType=1";
            //}



            query += @" ORDER BY  AccountName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var accountSearchList = uow.Repository<AccountSearchViewModelAcc>().SqlQuery(query).ToList();
            return accountSearchList;
        }
        
        public AccountDetailsViewModelAcc GetCodeByAccountNumber(string accountNumber)
        {

            try
            {
                var Getsingle = uow.Repository<ADetail>().FindBy(x => x.Accno.Contains(accountNumber)).Select(x => new AccountDetailsViewModelAcc()
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
    }
}
