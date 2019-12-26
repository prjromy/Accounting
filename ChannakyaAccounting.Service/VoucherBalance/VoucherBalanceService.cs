using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using MoreLinq;
using System.Data.SqlClient;
using ChannakyaAccounting.Models.Models;

namespace ChannakyaAccounting.Service.VoucherBalance
{
    public class VoucherBalanceService
    {
        private GenericUnitOfWork uow = null;
        ChannakyaAccounting.Service.Subsi_Setup.SubsiDetailService subsiDetail = null;

        public VoucherBalanceService()
        {
            uow = new GenericUnitOfWork();
            subsiDetail = new Subsi_Setup.SubsiDetailService();
        }


        public List<Models.Models.VoucherBal> GetAll()
        {
            return uow.Repository<Models.Models.VoucherBal>().GetAll().ToList();
        }

        public Models.Models.VoucherBal GetSingle(int VoucherBalanceId)
        {
            Models.Models.VoucherBal VoucherBalance = uow.Repository<Models.Models.VoucherBal>().GetSingle(c => c.Id == VoucherBalanceId);
            return VoucherBalance;
        }

        public int GetFirstTransactionFYear()
        {
            int fyName = 0;
            var fyObj = uow.Repository<Models.Models.VoucherNo>().GetAll().OrderByDescending(x => x.FYID).LastOrDefault();
            if (fyObj != null)
            {
                var fnameObj = uow.Repository<Models.Models.FiscalYear>().FindBy(x => x.FYID == fyObj.FYID).FirstOrDefault();
                if (fnameObj != null)
                {
                    fyName = fnameObj.FYID;
                }
            }
            return fyName;
        }

        public int GetBranchEstFiscalYear()
        {
            int branchId=Loader.Models.Global.BranchId;
            string query = "select * from fin.LicenseBranch where BrnhID=" + branchId + "";
            var Branch = uow.Repository<LicenseBranch>().SqlQuery(query).SingleOrDefault();
            return Convert.ToInt32(Branch.FYID);
        }
        public int GetHeadBranchEstFiscalYear()
        {
            int branchId = Loader.Models.Global.BranchId;
            string query = "select * from fin.LicenseBranch where BrnhID= 1";
            var Branch = uow.Repository<LicenseBranch>().SqlQuery(query).SingleOrDefault();
            return Convert.ToInt32(Branch.FYID);
        }

        public void SaveSubsiOpeningBalance(List<ChannakyaAccounting.Models.ViewModel.SubsiBalanceViewModel> subsiBalanceList)
        {
            try
            {
                decimal totalOPBal = 0;
                foreach (var item in subsiBalanceList)
                {
                    //if (Convert.ToDecimal(item.OpeningBalance) == 0)
                    //{
                    int ledgerId = Convert.ToInt32(item.FId);
                    int subsiId = Convert.ToInt32(item.SubsiId);
                    if (item.AmountType == "2")
                    {
                        totalOPBal = totalOPBal + Convert.ToDecimal(-1 * Convert.ToDecimal(item.OpeningBalance));
                    }
                    else
                    {
                        totalOPBal = totalOPBal + Convert.ToDecimal(item.OpeningBalance);
                    }


                    int doesExists = uow.Repository<Models.Models.SubsiVSOpeningBalance>().FindBy(x => x.FId == ledgerId && x.FYID == Loader.Models.Global.SelectedFYID && x.SubsiId == subsiId).Count();
                    if (doesExists > 0)
                    {
                        var openingBalObject = uow.Repository<Models.Models.SubsiVSOpeningBalance>().FindBy(x => x.FId == ledgerId && x.FYID == Loader.Models.Global.SelectedFYID && x.SubsiId == subsiId).FirstOrDefault();
                        if (openingBalObject != null)
                        {
                            // ChannakyaAccounting.Models.Models.SubsiVSOpeningBalance subsiVsOpeningBalDTO = new Models.Models.SubsiVSOpeningBalance();

                            if (item.AmountType == "2")
                            {

                                if (openingBalObject.OpeningBal == openingBalObject.ClosingBal)
                                {
                                    totalOPBal = totalOPBal - (-1) * Convert.ToDecimal(item.OpeningBalance) + ((-1) * Convert.ToDecimal(item.OpeningBalance) - openingBalObject.OpeningBal);
                                    openingBalObject.OpeningBal = Convert.ToDecimal(-1 * Convert.ToDecimal(item.OpeningBalance));
                                    openingBalObject.ClosingBal = openingBalObject.OpeningBal;

                                }
                                else
                                {
                                    totalOPBal = totalOPBal - (-1) * Convert.ToDecimal(item.OpeningBalance) + ((-1) * Convert.ToDecimal(item.OpeningBalance) - openingBalObject.OpeningBal);
                                    openingBalObject.ClosingBal = openingBalObject.ClosingBal + Convert.ToDecimal(Convert.ToDecimal(item.OpeningBalance) - openingBalObject.OpeningBal);
                                    openingBalObject.OpeningBal = Convert.ToDecimal(-1 * Convert.ToDecimal(item.OpeningBalance));
                                }
                                //openingBalObject.OpeningBal = Convert.ToDecimal(-1 * Convert.ToDecimal(item.OpeningBalance));

                            }
                            else
                            {
                                //openingBalObject.OpeningBal = Convert.ToDecimal(item.OpeningBalance);
                                if (openingBalObject.OpeningBal == openingBalObject.ClosingBal)
                                {
                                    totalOPBal = totalOPBal - Convert.ToDecimal(item.OpeningBalance) + (Convert.ToDecimal(item.OpeningBalance) - openingBalObject.OpeningBal);
                                    openingBalObject.OpeningBal = Convert.ToDecimal(item.OpeningBalance);
                                    openingBalObject.ClosingBal = openingBalObject.OpeningBal;

                                }
                                else
                                {
                                    totalOPBal = totalOPBal - Convert.ToDecimal(item.OpeningBalance) + (Convert.ToDecimal(item.OpeningBalance) - openingBalObject.OpeningBal);
                                    openingBalObject.ClosingBal = openingBalObject.ClosingBal + Convert.ToDecimal(Convert.ToDecimal(item.OpeningBalance) - openingBalObject.OpeningBal);
                                    openingBalObject.OpeningBal = Convert.ToDecimal(item.OpeningBalance);

                                }

                                //change to update closing balance
                                //openingBalObject.ClosingBal = Convert.ToDecimal(totalOPBal);
                                uow.Repository<Models.Models.SubsiVSOpeningBalance>().Edit(openingBalObject);
                            }
                        }
                    }
                    else
                    {
                        ChannakyaAccounting.Models.Models.SubsiVSOpeningBalance subsiVsOpeningBalDTO = new Models.Models.SubsiVSOpeningBalance();
                        subsiVsOpeningBalDTO.FId = Convert.ToInt32(item.FId);
                        subsiVsOpeningBalDTO.FYID = GetFirstTransactionFYear();
                        //change to store negative value
                        if (item.AmountType == "2")
                        {

                            decimal openingBal = Convert.ToDecimal(-1 * Convert.ToDecimal(item.OpeningBalance));
                            subsiVsOpeningBalDTO.OpeningBal = Convert.ToDecimal(openingBal);
                            subsiVsOpeningBalDTO.ClosingBal = Convert.ToDecimal(openingBal);
                        }
                        else
                        {

                            subsiVsOpeningBalDTO.OpeningBal = Convert.ToDecimal(item.OpeningBalance);
                            //change for closing balance
                            subsiVsOpeningBalDTO.ClosingBal = Convert.ToDecimal(item.OpeningBalance);
                        }
                        subsiVsOpeningBalDTO.SubsiId = Convert.ToInt32(item.SubsiId);
                        uow.Repository<Models.Models.SubsiVSOpeningBalance>().Add(subsiVsOpeningBalDTO);
                    }
                    // }
                }
                var ledgerObj = subsiBalanceList.FirstOrDefault();

                if (ledgerObj != null)
                {
                    var ledgerId = Convert.ToDecimal(ledgerObj.FId);
                    var voucherBalanceObj = uow.Repository<Models.Models.VoucherBal>().FindBy(x => x.FId == ledgerId && x.BranchId == Loader.Models.Global.BranchId && x.FYId == Loader.Models.Global.SelectedFYID).FirstOrDefault();
                    if (voucherBalanceObj != null)
                    {
                        if (voucherBalanceObj.OPBal == voucherBalanceObj.CLBal)
                        {
                            voucherBalanceObj.OPBal = voucherBalanceObj.OPBal + totalOPBal;
                            voucherBalanceObj.CLBal = voucherBalanceObj.CLBal + totalOPBal;
                        }
                        else
                        {
                            voucherBalanceObj.CLBal = voucherBalanceObj.CLBal + (totalOPBal - voucherBalanceObj.OPBal);
                            voucherBalanceObj.OPBal = voucherBalanceObj.OPBal + totalOPBal;
                            //change for closing balance
                        }
                        uow.Repository<Models.Models.VoucherBal>().Edit(voucherBalanceObj);
                    }
                    else
                    {
                        Models.Models.VoucherBal voucherBal = new Models.Models.VoucherBal();
                        voucherBal.BranchId = Loader.Models.Global.BranchId;
                        voucherBal.FId = Convert.ToInt32(ledgerObj.FId);
                        voucherBal.FYId = GetFirstTransactionFYear();
                        //right not wrong
                        voucherBal.OPBal = totalOPBal;
                        //changen for closing balance
                        voucherBal.CLBal = totalOPBal;
                        uow.Repository<Models.Models.VoucherBal>().Add(voucherBal);
                    }
                }
                uow.Commit();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Save(List<ChannakyaAccounting.Models.ViewModel.OpeningBalanceViewModel> openingBalanceList)
        {
            try
            {
                GenericUnitOfWork UOW = new GenericUnitOfWork();
                foreach (var item in openingBalanceList.Distinct().ToList())
                {
                    if (item.FId == 15594)
                    {

                    }
                    var openingBalObject = uow.Repository<Models.Models.SubsiVSOpeningBalance>().FindBy(x => x.FId == item.FId /*&& x.FYID == item.FYId*/).ToList();

                    decimal totalOpeningBal = 0;
                    if (openingBalObject.Count() > 0)
                    {
                        foreach (var subitem in openingBalObject)
                        {
                            totalOpeningBal = totalOpeningBal + subitem.OpeningBal;
                        }

                    }
                    else
                    {
                        totalOpeningBal = (decimal)item.OPBal;
                    }

                    Models.Models.VoucherBal voucherBalance = new Models.Models.VoucherBal();
                    voucherBalance.FId = item.FId;
                    voucherBalance.FYId = GetFirstTransactionFYear();
                    if (item.AmountType == 1)
                    {
                        voucherBalance.OPBal = totalOpeningBal;
                    }
                    else
                    {
                        voucherBalance.OPBal = Convert.ToDecimal(totalOpeningBal * (-1));
                    }
                    voucherBalance.CLBal = voucherBalance.OPBal;
                    voucherBalance.BranchId = Loader.Models.Global.BranchId;
                    voucherBalance.Id = item.Id;

                    if (voucherBalance.Id == 0)
                    {

                        UOW.Repository<Models.Models.VoucherBal>().Add(voucherBalance);
                    }
                    else
                    {
                        decimal? currentOpBal = null;
                        decimal? newAddedOpBal = null;
                        int tYear = GetFirstTransactionFYear();
                        var currentOpBalObj = uow.Repository<Models.Models.VoucherBal>().FindBy(x => x.FId == item.FId && x.FYId == tYear).FirstOrDefault();
                        if (currentOpBalObj != null)
                        {
                            currentOpBal = currentOpBalObj.OPBal;
                            if (item.AmountType == 2)
                            {
                                totalOpeningBal = totalOpeningBal * -1;
                            }
                            newAddedOpBal = totalOpeningBal - currentOpBal;

                            var allnextFyearObj = uow.Repository<Models.Models.VoucherBal>().FindBy(x => x.FId == item.FId && x.FYId > tYear).ToList();
                            foreach (var subitem in allnextFyearObj)
                            {

                                Models.Models.VoucherBal voucherbalObj = uow.Repository<Models.Models.VoucherBal>().GetSingle(x => x.Id == subitem.Id);
                                voucherbalObj.OPBal = voucherbalObj.OPBal + newAddedOpBal;
                                voucherbalObj.CLBal = voucherbalObj.CLBal + newAddedOpBal;
                                uow.GetContext().Database.ExecuteSqlCommand("exec [dbo].[PSetCLosingBalanceBalance] @OPBal,@CLBal,@FId,@FYId", new SqlParameter("@OPBal", voucherbalObj.OPBal), new SqlParameter("@CLBal", voucherbalObj.CLBal), new SqlParameter("@FId", voucherbalObj.FId), new SqlParameter("@FYId", tYear));
                            }
                            //if(voucherBalance.FYId>=tYear)
                            //{
                            //    voucherBalance.OPBal = voucherBalance.OPBal + newAddedOpBal;
                            //    voucherBalance.CLBal = voucherBalance.CLBal + newAddedOpBal;
                            //}
                        }
                        UOW.Repository<Models.Models.VoucherBal>().Edit(voucherBalance);
                    }
                }
                bool isSaved = UOW.Commit() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
                //throw;
                //rollback all transactions
            }
            //}
        }
       
        public List<Models.ViewModel.OpeningBalanceViewModel> GetMappedDataOfOpeningBalance(int ledgerId = 0, int withsubsi = 0,int page=1,int pageSize=5,string AccNo="",string AccName="")
         {
            //var ledgerList = uow.Repository<Models.Models.FinAcc>().GetAll().Where(x => x.FinSys2.FinSys1.IsGroup == false && HasDepositOrLoanProduct(x.Fid) == false);
            //if (ledgerId != 0)
            //{
            //    ledgerList = uow.Repository<Models.Models.FinAcc>().GetAll().Where(x => x.FinSys2.FinSys1.IsGroup == false && x.Fid == ledgerId);
            //}

            //var finalList = (from p in ledgerList
            //                 from m in ledgerList.DefaultIfEmpty()
            //                 join n in uow.Repository<Models.Models.VoucherBal>().GetAll() on m.Fid equals n.FId into dataList
            //                 from y in dataList.DefaultIfEmpty()
            //                 join subsiDetail in uow.Repository<Models.Models.SubsiVSOpeningBalance>().GetAll() on m.Fid equals subsiDetail.FId into
            //                  subsiFinal
            //                 from z in subsiFinal.DefaultIfEmpty()
            //                     //join o in uow.Repository<Models.Models.ProductDetail>().GetAll() on p.Fid equals o.FID into prdctDetail
            //                     //from op in prdctDetail.DefaultIfEmpty()

            //                 select new OpeningBalanceViewModel
            //                 {
            //                     Id = y == null ? 0 : y.Id,
            //                     FId = m.Fid,
            //                     FYId = y == null ? 0 : y.FYId,
            //                     OPBal = y == null ? 0 : y.OPBal,
            //                     CLBal = y == null ? 0 : y.CLBal,
            //                     LedgerName = m.Fname,
            //                     //SubsiDetailOfLedger = MapToSubsiViewModel(subsiFinal.ToList(), m.Fid)
            //                 }).ToList();
            //return finalList;
            int BranchID = Loader.Models.Global.BranchId;
            List<OpeningBalanceViewModel> dataList = uow.Repository<Models.ViewModel.OpeningBalanceViewModel>().SqlQuery("select * from acc.FGetOpeningBalance(" + ledgerId + "," + withsubsi + ","+ Loader.Models.Global.SelectedFYID +")").ToList();
            string query = "select * from acc.FGetSubsiChildByFid(" + ledgerId + "," + Loader.Models.Global.SelectedFYID + "," + BranchID + "," + page + "," + pageSize + ",'" + AccNo + "','" + AccName + "') ";
            if (ledgerId == 0)
            {
                var finalList = dataList.Select(x => new OpeningBalanceViewModel
                {
                    Id = x.Id, 
                    FId = x.FId,
                    FYId = x.FYId,
                    OPBal = x.OPBal,
                    CLBal = x.CLBal,
                    LedgerName = x.LedgerName,
                    //SubsiDetailOfLedger = ledgerId == 0 ? new List<SubsiViewModel>() : MapToSubsiViewModel(uow.Repository<Models.Models.SubsiVSOpeningBalance>().FindBy(y => y.FId == x.FId).ToList(), (int)x.FId);
                    //SubsiDetailOfLedger=MapToSubsiViewModel(uow.Repository<Models.Models.SubsiVSOpeningBalance>().FindBy(y=>y.FId==x.FId).ToList(),(int)x.FId),


                }).ToList();
                return finalList;
            }
            else
            {
                var finalList = dataList.Select(x => new OpeningBalanceViewModel
                {
                    Id = x.Id,
                    FId = x.FId,
                    FYId = x.FYId,
                    OPBal = x.OPBal,
                    CLBal = x.CLBal,
                    LedgerName = x.LedgerName,

                   // SubsiDetailOfLedger = MapToSubsiViewModel(uow.Repository<Models.Models.SubsiVSOpeningBalance>().SqlQuery(@"select * from acc.SubsiVSOpeningBalance where FId="+x.FId+" ORDER BY SubsiId  OFFSET("+page+" - 1) * "+pageSize+" ROWS FETCH NEXT  "+ pageSize + " ROWS ONLY").ToList(), (int)x.FId),
                    SubsiDetailOfLedger =uow.Repository<Models.ViewModel.SubsiViewModel>().SqlQuery(@"select * from acc.FGetSubsiChildByFid(" + ledgerId+"," + Loader.Models.Global.SelectedFYID + ","+BranchID+","+page+","+pageSize+",'"+AccNo+"','"+AccName+"')").ToList(),


                }).ToList();
                return finalList;
            }




        }

        public IEnumerable<OpeningBalanceViewModel> OtherLedgerWithSubsiDetail(int ledgerId, OpeningBalanceViewModel lsObj, int parentId = 0)
        {

            List<OpeningBalanceViewModel> ls = new List<OpeningBalanceViewModel>();
            var finaccType = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == ledgerId).FinSys2.FinSys1.F1id;
            if (finaccType == 15)
            {
                for (int i = (ledgerId + 1); i < (ledgerId + 4); i++)
                {
                    //if (lsObj.SubsiDetailOfLedger.FirstOrDefault() != null)
                    //{
                    //    lsObj.SubsiDetailOfLedger.FirstOrDefault().AccNo = subsiDetail.GenerateAccountNumberForDepositAndLoan(i);
                    //}
                    var finalList = GetMappedDataOfOpeningBalance(i).FirstOrDefault();
                    if (finalList.SubsiDetailOfLedger == null || finalList.SubsiDetailOfLedger.FirstOrDefault() == null)
                    {
                        lsObj.SubsiDetailOfLedger.ForEach(x => x.ParentId = ledgerId);
                        finalList.SubsiDetailOfLedger = lsObj.SubsiDetailOfLedger;
                    }
                    else
                    {
                        finalList.SubsiDetailOfLedger.ForEach(x => x.ParentId = ledgerId);
                    }



                    ls.Add(finalList);
                }
            }
            if (finaccType == 126)
            {
                for (int i = (ledgerId + 1); i < (ledgerId + 11); i++)
                {
                    //if (lsObj.SubsiDetailOfLedger.FirstOrDefault() != null)
                    //{
                    //    lsObj.SubsiDetailOfLedger.FirstOrDefault().AccNo = subsiDetail.GenerateAccountNumberForDepositAndLoan(i);
                    //}
                    var finalList = GetMappedDataOfOpeningBalance(i).FirstOrDefault();

                    lsObj.SubsiDetailOfLedger.ForEach(x => x.ParentId = ledgerId);
                    finalList.SubsiDetailOfLedger = lsObj.SubsiDetailOfLedger;

                    ls.Add(finalList);
                }
            }
            if (parentId != 0 && HasDepositOrLoanProduct(ledgerId))
            {
                List<SubsiViewModel> subsiViewModel = subsiDetail.GetAllAccountDetailsDepositLoanProduct((int)parentId);
                var finalList = GetMappedDataOfOpeningBalance(ledgerId).FirstOrDefault();
                finalList.SubsiDetailOfLedger = subsiViewModel;
                if (finalList.SubsiDetailOfLedger.FirstOrDefault() != null)
                {
                    finalList.SubsiDetailOfLedger.FirstOrDefault().Accno = subsiDetail.GenerateAccountNumberForDepositAndLoan(parentId);
                }
                finalList.SubsiDetailOfLedger.ForEach(x => x.ParentId = ledgerId);
                ls.Add(finalList);

            }

            return ls;
        }

        public bool HasDepositOrLoanProduct(int ledgerId)
        {
            bool isProduct = false;
            var finaccType = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == ledgerId).FinSys2.FinSys1.F1id;
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
            if (depositProduct.Contains(finaccType) || loanProduct.Contains(finaccType))
            {
                isProduct = true;
            }
            return isProduct;
        }

        public List<SubsiViewModel> MapToSubsiViewModel(List<Models.Models.SubsiVSOpeningBalance> listSubsi, int ledgerId)
        {
            //if (ledgerId == 15555)
            //{

            //}
            List<SubsiViewModel> subsiViewModel = new List<SubsiViewModel>();
            if (IsParentDepositOrLoanProduct(ledgerId))
            {
                Service.Subsi_Setup.SubsiDetailService subsiDetailService = new Service.Subsi_Setup.SubsiDetailService();
                List<SubsiViewModel> subsiViewMdl = subsiDetailService.GetAllAccountDetailsDepositLoanProduct((int)ledgerId);
                subsiViewModel = subsiViewMdl;
            }
            else
            {
                if (listSubsi.Count() > 0)
                {
                    #region SubsiArea-NotInUse
                    //foreach (var item in listSubsi)

                    //{
                    //    SubsiViewModel subsiVM = new SubsiViewModel();
                    //    subsiVM.Id = (int)item.SubsiId;
                    //    subsiVM.OpeningBalance = (decimal)item.OpeningBal;
                    //    int? subsitableId = ChannakyaAccounting.Service.Helper.GenerateSubsiTypeList.GenerateSubsiTableFromLedgerId((int)ledgerId) as Nullable<int>;

                    //    //if (IsParentDepositOrLoanProduct(ledgerId))
                    //    //{
                    //    //    subsiVM.Name = ChannakyaAccounting.Service.Helper.GenerateSubsiTypeList.GenerateSubsiName((int)ledgerId, (int)subsitableId);
                    //    //    subsiVM.Accno = subsiDetail.GenerateAccountNumberForDepositAndLoan((int)item.FId);
                    //    //    //Service.Subsi_Setup.SubsiDetailService subsiDetailService = new Service.Subsi_Setup.SubsiDetailService();
                    //    //    //List<SubsiViewModel> subsiViewMdl = subsiDetailService.GetAllAccountDetailsDepositLoanProduct((int)item.FId);
                    //    //    //subsiViewModel = subsiViewMdl;
                    //    //}
                    //    //else
                    //    //{
                    //        subsiVM.Name = ChannakyaAccounting.Service.Helper.GenerateSubsiTypeList.GenerateSubsiName((int)item.SubsiId, (int)subsitableId);
                    //        subsiVM.Accno = subsiDetail.GenerateAccountNumber((int)item.FId);
                    //    //}
                    //    subsiViewModel.Add(subsiVM);


                    //    //if (VoucherBalance.CheckIfIsSubsi((int)item.FId) == true)
                    //    //{


                    //        //ViewData["SubsiTableId"] = subsiTableId;
                    //        //ViewData["LedgerId"] = item.FId;



                    //    //    item.SubsiDetailOfLedger = allSubsiList.ToList();

                    //    //}




                    //}
                    #endregion

                    int subsiTableId = Helper.GenerateSubsiTypeList.GenerateSubsiTableFromLedgerId((int)ledgerId);
                    var subsiList = Helper.GenerateSubsiTypeList.GenerateSubsiList(subsiTableId);
                    var mappedSubsi = new GenericUnitOfWork().Repository<Models.Models.SubsiDetail>().FindBy(x => x.FId == ledgerId).Distinct();

                    var allSubsiList = (from m in mappedSubsi
                                        join n in subsiList on m.CId equals n.Id
                                        join o in new GenericUnitOfWork().Repository<Models.Models.SubsiVSOpeningBalance>().GetAll()
                                        on new { f1 = m.FId, f2 = n.Id } equals new { f1 = o.FId, f2 = o.SubsiId } into temp
                                        from p in temp.DefaultIfEmpty()
                                        select new SubsiViewModel
                                        {
                                            Id = n.Id,
                                            Accno = m.AccNo,
                                            AName = Helper.GenerateSubsiTypeList.GenerateSubsiName(m.CId, Helper.GenerateSubsiTypeList.GenerateSubsiTableFromLedgerId(m.FId)),
                                            OpeningBalance = p == null ? 0 : p.OpeningBal

                                        }).ToList();
                    subsiViewModel = allSubsiList;
                }
            }

            return subsiViewModel;
        }
        //public decimal GetOpeningBalanceForDepositProduct(int ledgerId)
        //{

        //}

        public bool IsParentDepositOrLoanProduct(int ledgerId)
        {
            bool isParentPr = false;
            var finaccType = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == ledgerId).FinSys2.FinSys1.F1id;
            if (finaccType == 15 || finaccType == 126)
            {
                isParentPr = true;
            }
            return isParentPr;
        }
        public bool CheckIfIsSubsi(int fId)
        {
            bool hasSubsi = false;
            var voucher2Data = uow.Repository<Models.Models.Voucher2>().FindBy(x => x.Fid == fId).FirstOrDefault();
            if (voucher2Data != null)
            {
                hasSubsi = uow.Repository<Models.Models.Voucher3>().GetAll().Any(x => x.V2Id == voucher2Data.V2Id);
            }
            return hasSubsi;
        }



        public List<Models.Models.VoucherBal> GetVoucherBalanceList()
        {
            List<Models.Models.VoucherBal> lstVoucherBalance = new List<Models.Models.VoucherBal>();
            return lstVoucherBalance;
        }
        public bool Delete(int VoucherBalanceId)
        {
            Models.Models.VoucherBal VoucherBalance = this.GetSingle(VoucherBalanceId);

            if (VoucherBalance != null)
            {
                uow.Repository<Models.Models.VoucherBal>().Delete(VoucherBalance);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<SelectListItem> GetDimensionDefination()
        {
            var dimList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().FindBy(x => x.IsManual == 0).ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in dimList)
            {
                lst.Add(new SelectListItem { Text = item.DefName, Value = item.DDId.ToString() });

            }
            return lst;
        }

        public static List<SelectListItem> GEtSubsiName(List<ChannakyaAccounting.Models.ViewModel.OpeningBalanceViewModel> Model)
        {

            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var item in Model)
            {
                options.Add(new SelectListItem { Text = item.LedgerName, Value = item.FId.ToString() });
            }

            return options;
        }

        //public string GetAddress(int Models.Models.VoucherBalId)
        //{
        //    string result = "";

        //    if (Models.Models.VoucherBalId != 0)
        //    {
        //        Models.Models.VoucherBal mnu = new Models.Models.VoucherBal();
        //        mnu = GetSingle(Models.Models.VoucherBalId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.Models.Models.VoucherBalName);
        //            mnu = GetSingle(mnu.PModels.Models.VoucherBalId);
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

    }
}
