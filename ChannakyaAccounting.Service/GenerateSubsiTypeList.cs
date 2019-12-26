using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChannakyaAccounting.Models.ViewModel;

namespace ChannakyaAccounting.Service.Helper
{
    #region SubsiGeneratorStaticHelper

    public static class GenerateSubsiTypeList
    {
        public static IEnumerable<ChannakyaAccounting.Models.ViewModel.SubsiViewModel> GenerateSubsiList(int subsiTblId, string query = "")
        {
            GenericUnitOfWork UOW = new GenericUnitOfWork();
            Loader.Repository.GenericUnitOfWork loaderUOW = new Loader.Repository.GenericUnitOfWork();

            IEnumerable<SubsiViewModel> finalLst = new List<SubsiViewModel>();

            if (subsiTblId == 1)
            {

                var susbiList = loaderUOW.Repository<Loader.Models.Employee>().GetAll();
                if (!string.IsNullOrEmpty(query))
                {
                    susbiList = susbiList.Where(m => m.EmployeeName.ToUpper().Contains(query.ToUpper()) || m.EmployeeNo.ToUpper().Contains(query.ToUpper())).OrderBy(x => x.EmployeeName);
                }
                finalLst = susbiList.Select(x => new SubsiViewModel
                {
                    Id = x.EmployeeId,
                    Name = x.EmployeeName,


                }).ToList();

            }

            if (subsiTblId == 2)
            {
                var susbiList = loaderUOW.Repository<Loader.Models.ApplicationUser>().GetAll();
                if (!string.IsNullOrEmpty(query))
                {
                    susbiList = susbiList.Where(m => m.UserName.ToUpper().Contains(query.ToUpper()) || m.PhoneNumber.ToUpper().Contains(query.ToUpper())).ToList().OrderBy(x => x.UserName);
                }
                finalLst = susbiList.Select(x => new SubsiViewModel
                {
                    Id = x.Id,
                    Name = x.UserName,


                }).ToList();

            }
            if (subsiTblId == 3)
            {
                var susbiList = UOW.Repository<Models.Models.CustIndividual>().GetAll();
                if (!string.IsNullOrEmpty(query))
                {
                    susbiList = susbiList.Where(m => m.Fname.ToUpper().Contains(query.ToUpper()) || m.Mname.ToUpper().Contains(query.ToUpper()) || m.Lname.ToUpper().Contains(query.ToUpper())).ToList().OrderBy(x => x.Fname);
                }
                finalLst = susbiList.Select(x => new SubsiViewModel
                {
                    Id = Convert.ToInt32(x.CID),
                    Name = x.Fname,


                }).ToList();

            }
            if (subsiTblId == 4 || subsiTblId == 5)
            {
                var subsiList = UOW.Repository<Models.ViewModel.FGetSubsiNameListModel>().SqlQuery("select * from FGetSubsiList(" + subsiTblId + ")").ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    subsiList = subsiList.Where(m => m.FirstName.ToUpper().Contains(query.ToUpper())).OrderBy(x => x.FirstName).ToList();
                }
                finalLst = subsiList.Select(x => new SubsiViewModel
                {
                    Id = Convert.ToInt32(x.CustId),
                    Name = x.FirstName,
                    //AccNo = x.
                }).ToList();
            }

            if (subsiTblId == 6)
            {
                //var susbiList = UOW.Repository<ChannakyaAccounting.Models.Models.ProductDetail>().GetAll();
                //if (!string.IsNullOrEmpty(query))
                //{
                //    susbiList = susbiList.Where(m => m..ToUpper().Contains(query.ToUpper()) || m.PhoneNumber.ToUpper().Contains(query.ToUpper())).ToList().OrderBy(x => x.UserName);
                //}
                //finalLst = susbiList.Select(x => new SubsiViewModel
                //{
                //    Id = x.Id,
                //    Name = x.UserName,


                //}).ToList();
            }
            return finalLst;

        }

        public static string GenerateSubsiName(int id, int subsiTblId)
        {
            Loader.Repository.GenericUnitOfWork loaderUOW = new Loader.Repository.GenericUnitOfWork();
            GenericUnitOfWork UOW = new GenericUnitOfWork();
            string subsiName = "";
            if (subsiTblId == 1)
            {
                var subsiObj = loaderUOW.Repository<Loader.Models.Employee>().GetSingle(x => x.EmployeeId == id);
                if (subsiObj != null)
                {
                    subsiName = subsiObj.EmployeeName;
                }

            }

            if (subsiTblId == 2)
            {
                var subsiObj = loaderUOW.Repository<Loader.Models.ApplicationUser>().GetSingle(x => x.Id == id);
                if (subsiObj != null)
                {
                    subsiName = subsiObj.UserName;
                }

            }
            if (subsiTblId == 3)
            {
                var subsiObj = UOW.Repository<Models.Models.CustIndividual>().GetSingle(x => x.CID == id);
                if (subsiObj != null)
                {
                    subsiName = subsiObj.Fname;
                }
                

            }
            if (subsiTblId == 4 || subsiTblId == 5)
            {
                var subsiList = UOW.Repository<Models.ViewModel.FGetSubsiNameListModel>().SqlQuery("select * from FGetSubsiList(" + subsiTblId + ")").ToList();
                 if(subsiList.Where(x => x.CustId == id).FirstOrDefault()!=null)
                    subsiName = subsiList.Where(x => x.CustId == id).FirstOrDefault().FirstName;

            }
            if(subsiTblId==6 || subsiTblId==7)
            {
                //var subsiObj = UOW.Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == id).FirstOrDefault();
                //if(subsiObj!=null)
                //{
                //    subsiName = subsiObj.Fname;
                //}
                ChannakyaAccounting.Service.Subsi_Setup.SubsiDetailService subsiDetailService = new Service.Subsi_Setup.SubsiDetailService();
                var subsiObj = subsiDetailService.GetAllAccountDetailsDepositLoanProduct(id);
                if(subsiObj!=null && subsiObj.FirstOrDefault()!=null)
                {
                    subsiName = subsiObj.FirstOrDefault().Name;
                }
            }
            return subsiName;
        }

        public static int GenerateSubsiTableFromLedgerId(int ledgerId)
        {
            int subsiTbl = 0;
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var subsitblId = from m in uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == ledgerId)
                                 join n in uow.Repository<Models.Models.SubsiVSFId>().GetAll() on m.Fid equals n.Fid
                                 join o in uow.Repository<Models.Models.SubsiSetup>().GetAll() on n.SSId equals o.SSId
                                 join p in uow.Repository<Models.Models.SubsiTable>().GetAll() on o.STBId equals p.STBId
                                 into finalsubsiData
                                 select finalsubsiData.FirstOrDefault();
                if (subsitblId != null)
                {
                    if (subsitblId.FirstOrDefault() != null)
                    {
                        subsiTbl = subsitblId.FirstOrDefault().STBId;
                    }
                    else
                    {
                        var finaccObj = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == ledgerId);
                        if (finaccObj != null)
                        { 
                        int finaccType = finaccObj.FinSys2.FinSys1.F1id;
                        ChannakyaAccounting.Service.VoucherBalance.VoucherBalanceService voucherService = new Service.VoucherBalance.VoucherBalanceService();
                        bool isProduct = voucherService.HasDepositOrLoanProduct(ledgerId);

                        List<int> depositProduct = new List<int>();
                        List<int> loanProduct = new List<int>();


                        for (int i = 24; i <= 27; i++)
                        {
                            depositProduct.Add(i);
                        }

                        for (int i = 237; i <= 249; i++)
                        {
                            loanProduct.Add(i);
                        }
                        if (depositProduct.Contains(finaccType) || finaccType==15)
                        {
                            subsiTbl = (int)ChannakyaAccounting.EnumValue.Subsi.Deposit;
                        }
                        if (loanProduct.Contains(finaccType) || finaccType == 15)
                        {
                            subsiTbl = (int)ChannakyaAccounting.EnumValue.Subsi.Loan;
                        }
                    }
                    }
                    
                }
            }
            return subsiTbl;

        }
        #endregion
    }
}