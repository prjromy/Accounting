using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using LoaderRepository = Loader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Models.Models;
using Loader.Models;
using System.Transactions;

namespace ChannakyaAccounting.Service.Voucher1
{
    public class Voucher1Service
    {
        private GenericUnitOfWork uow = null;
        ReturnBaseMessageModel returnMessage = null;
        public Voucher1Service()
        {
            returnMessage = new ReturnBaseMessageModel();

            uow = new GenericUnitOfWork();
        }
        public List<Models.Models.FiscalYear> GetAllFYear()
        {

            var allFyearList = uow.Repository<Models.Models.VoucherNo>().GetAll().OrderByDescending(x => x.FYID);

            var allFyearLists = uow.Repository<Models.Models.VoucherNo>().GetAll().Select(x => x.FiscalYear).Distinct().OrderByDescending(x => x.FYID).ToList();
            return allFyearLists;
        }
        public List<Models.Models.FiscalYear> GetAllFYearForBranch()
        {
            int BrnhID = Loader.Models.Global.BranchId;
            string query = "select * from fin.LicenseBranch where BrnhID=" + BrnhID + "";
            Models.Models.LicenseBranch GetSingle = uow.Repository<Models.Models.LicenseBranch>().SqlQuery(query).SingleOrDefault();
            int BrnhFYID = Convert.ToInt32(GetSingle.FYID);
            DateTime TDate = (DateTime)GetSingle.TDate;
            DateTime MigDate = (DateTime)GetSingle.MigDate;
            var Currentdate = uow.Repository<Models.Models.FiscalYear>().FindBy(x => x.StartDt <= TDate && x.EndDt >= TDate).SingleOrDefault();
            var MigrationDate = uow.Repository<Models.Models.FiscalYear>().FindBy(x => x.StartDt <= MigDate && x.EndDt >= MigDate).SingleOrDefault();
            var allFyearLists = uow.Repository<Models.Models.FiscalYear>().SqlQuery("select * from lg.FiscalYears where FYID between '" + MigrationDate.FYID + "' and '" + Currentdate.FYID + "' order by FYID desc").ToList();
            //     var allFyearList = uow.Repository<Models.Models.VoucherNo>().GetAll().Where(x => x.FYID >= BrnhFYID).OrderByDescending(x => x.FYID).ToList();
            // var allFyearLists = uow.Repository<Models.Models.VoucherNo>().GetAll().Select(x => x.FiscalYear).Where(x => x.FYID >= BrnhFYID).Distinct().OrderByDescending(x => x.FYID).ToList();
            return allFyearLists;
        }
        public string GetTranscationDate(int FYID)
        {
            int BrnhID = Loader.Models.Global.BranchId;
            string query = "select * from fin.LicenseBranch where BrnhID=" + BrnhID + "";
            Models.Models.LicenseBranch GetSingle = uow.Repository<Models.Models.LicenseBranch>().SqlQuery(query).SingleOrDefault();
            int BrnhFYID = Convert.ToInt32(GetSingle.FYID);
            DateTime TDate = (DateTime)GetSingle.TDate;
            DateTime MigDate = (DateTime)GetSingle.MigDate;
            var Currentdate = uow.Repository<Models.Models.FiscalYear>().FindBy(x => x.StartDt <= TDate && x.EndDt >= TDate).SingleOrDefault();
            var MigrationDate = uow.Repository<Models.Models.FiscalYear>().FindBy(x => x.StartDt <= MigDate && x.EndDt >= MigDate).SingleOrDefault();
            var allFyearLists = uow.Repository<Models.Models.FiscalYear>().SqlQuery("select * from lg.FiscalYears where FYID between '" + MigrationDate.FYID + "' and '" + Currentdate.FYID + "' order by FYID desc").ToList();
            var Tlist = "";
            if (FYID == allFyearLists.FirstOrDefault().FYID)
            {
                Tlist = Convert.ToDateTime(Loader.Models.Global.TransactionDate).ToShortDateString();
            }
            else
            {
                Tlist = allFyearLists.Find(x => x.FYID == FYID).EndDt.ToShortDateString().ToString();
            }

            return Tlist;
        }
        int BrnhID = Loader.Models.Global.BranchId;

        public List<Models.ViewModel.SubsiViewModel> GetAccountInfoFromLedgerId(int ledgerId)
        {
            List<SubsiViewModel> accInfoList = uow.Repository<SubsiViewModel>()
               .SqlQuery("select * from dbo.GetAccountInfoFromLedgerId(" + ledgerId + ")").ToList();
            return accInfoList;

        }

        public int GetSTBIDfromFid(int fid)
        {
            int STBID;
            int SSId = uow.Repository<SubsiVSFId>().GetSingle(x => x.Fid == fid).SSId.Value;
            STBID = uow.Repository<SubsiSetup>().GetSingle(x => x.SSId == SSId).STBId;
            return STBID;
        }

        public int GetCIdFromName(string[] CName, int Sid)
        {
            decimal CIdd;

            if (Sid == 1)
            {
                Models.Models.Employee Temp;
                try
                {


                    Temp = uow.Repository<Models.Models.Employee>()
                        .SqlQuery("select * from lg.Employees where EmployeeName like '%" + CName[0] + "%'").First();
                    CIdd = Convert.ToDecimal(Temp.EmployeeId);
                    //CIdd = uow.Repository<Employee>().FindBy(x=>x.EmployeeName == CName[0]).First().EmployeeId;

                    int CId = Convert.ToInt32(CIdd);
                    return CId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else if (Sid == 2)
            {
                CIdd = uow.Repository<User>()
                   .SqlQuery("select * from lg.[User] where UserName like '%" + CName[0] + "%'").First().UserId;
                int CId = Convert.ToInt32(CIdd);
                return CId;
            }
            else /*if(Sid == 3)*/
            {
                if (CName.Length == 3)
                {
                    CIdd = uow.Repository<CustIndividual>()
                       .SqlQuery("select * from cust.CustIndividual where Fname like '%" + CName[0] + "%' and Mname like '%" + CName[1] + "%' and Lname like '%" + CName[2] + "%").First().CID;
                    int CId = Convert.ToInt32(CIdd);
                    return CId;
                }
                else if (CName.Length == 2)
                {
                    string query = "select * from cust.CustIndividual where Fname like '%" + CName[0] + "%' and Lname like '%" + CName[1] + "%'";
                    CIdd = uow.Repository<CustIndividual>()
                        .SqlQuery("select * from cust.CustIndividual where Fname like '%" + CName[0] + "%' and Lname like '%" + CName[1] + "%'").First().CID;
                    int CId = Convert.ToInt32(CIdd);
                    return CId;
                }
                else if (CName.Length == 1)
                {
                    CIdd = uow.Repository<CustIndividual>()
                        .SqlQuery("select * from cust.CustIndividual where Fname like '%" + CName[0] + "%'").First().CID;
                    var temp = uow.Repository<CustIndividual>()
                        .SqlQuery("select * from cust.CustIndividual where Fname like '%" + CName[0] + "%'").First();
                    int CId = Convert.ToInt32(CIdd);
                    return CId;
                }
                else
                {
                    CIdd = uow.Repository<CustIndividual>()
                        .SqlQuery("select * from cust.CustIndividual where Fname like '%" + CName[0] + "%'").First().CID;
                    int CId = Convert.ToInt32(CIdd);
                    return CId;
                }
            }
        }
        
        #region SaveVoucher
        public int SaveTemporary(Models.Models.Voucher1T voucherMaster)
        {
            try
            {
                var voucherMasterId = 0;

                #region SaveToVoucher1MasterTable

                if (voucherMaster.V1TId == 0)
                {
                    int FYID = Loader.Models.Global.SelectedFYID;
                    int BrhID = Loader.Models.Global.BranchId;

                    var findBatchCount = uow.Repository<Models.Models.VoucherNo>().GetSingle(x => x.BId == voucherMaster.BatchNo && x.VTypeId == voucherMaster.VTypeId && x.FYID == FYID && x.CompanyId == BrhID);
                    //var voucherno=uow.Repository<Models.Models.Voucher1T>().SqlQuery("Select isnull(max(vno),0) + 1 as VNo from acc.voucher1t where VNId="+ findBatchCount.VNId + "").Select{ new Voucher1T() {VNo= } }.SingleOrDefault();
                    int VNo = uow.GetContext().Database.SqlQuery<int>("Select isnull(max(vno),0) + 1 as VNo from acc.voucher1t where VNId=" + findBatchCount.VNId + "").ToArray()[0];
                    Models.Models.Voucher1T voucher1 = new Models.Models.Voucher1T();
                    voucher1.PostedBy = Loader.Models.Global.UserId;
                    voucher1.PostedOn = DateTime.Now;
                    voucher1.VNo = VNo;
                    voucher1.VNId = findBatchCount.VNId;
                    voucher1.TDate = (DateTime)Loader.Models.Global.TransactionDate;
                    voucher1.VerifiedOn = DateTime.Now;
                    voucher1.Narration = voucherMaster.Narration;
                    voucher1.CTId = voucherMaster.CTId;

                    #region  SaveToOtherVoucherTables
                    Models.Models.Voucher2T voucher2 = new Models.Models.Voucher2T();
                    if (voucherMaster.Voucher2T != null)
                    {
                        foreach (var item in voucherMaster.Voucher2T)
                        {
                            if (item.VoucherTransType == 0)
                            {
                                item.CreditAmount = null;
                            }
                            else if (item.VoucherTransType == 1)
                            {
                                item.DebitAmount = null;
                            }
                            voucher2.CreditAmount = (item.CreditAmount);
                            voucher2.DebitAmount = item.DebitAmount;
                            voucher2.Fid = item.Fid;
                            voucher2.Particulars = item.Particulars;
                            if (voucherMaster.SubsiVSLedgerList != null)
                            {
                                foreach (var item2 in voucherMaster.SubsiVSLedgerList)
                                {
                                    Models.Models.Voucher3T voucher3 = new Models.Models.Voucher3T();
                                    voucher3.SId = item2.SId;
                                    voucher3.Description = item2.Description;
                                    voucher3.Amount = item2.Amount;
                                    voucher2.Voucher3T.Add(voucher3);
                                }
                            }
                            //if (voucherMaster.DimensionVSLedgerList != null)
                            //{
                            //    foreach (var item1 in voucherMaster.DimensionVSLedgerList)
                            //    {
                            //        Models.Models.Voucher4T voucher4 = new Models.Models.Voucher4T();
                            //        voucher4.DDID = item1.DDID;
                            //        voucher4.DVId = item1.DVId;
                            //        voucher4.Amount = item1.Amount;
                            //        voucher2.Voucher4T.Add(voucher4);
                            //    }
                            //}
                            if (voucherMaster.BankInfoList != null)
                            {
                                foreach (var item2 in voucherMaster.BankInfoList)
                                {
                                    Models.Models.Voucher5T voucher5T = new Models.Models.Voucher5T();
                                    voucher5T.ChequeAmount = item2.ChequeAmount;
                                    voucher5T.ChequeDate = item2.ChequeDate;
                                    voucher5T.ChequeNo = item2.ChequeNo;
                                    voucher5T.Payees = item2.Payees;

                                    voucher2.Voucher5T.Add(voucher5T);
                                }
                            }
                        }
                    }
                    else
                    {
                        return 9999;
                    }

                    #endregion

                    voucher1.Voucher2T.Add(voucher2);

                    //vNo.Voucher1T.Add(voucher1);
                    //voucherNo.Voucher1T.Add(voucher1);

                    if (voucher1.Voucher2T == null)
                    {
                        return 8888;
                    }

                    uow.Repository<Models.Models.Voucher1T>().Add(voucher1);
                    //uow.Repository<Models.Models.VoucherNo>().Add(vNo);
                    try
                    {
                        uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        return 7777;
                    }
                    voucherMasterId = voucher1.V1TId;

                }
                #endregion
                else
                {
                    #region AddMoreVoucherHead

                    Models.Models.Voucher1T voucher1 = uow.Repository<Models.Models.Voucher1T>().FindBy(x => x.V1TId == voucherMaster.V1TId).FirstOrDefault();
                    voucher1.Narration = voucherMaster.Narration;

                    Models.Models.Voucher2T voucher2 = new Models.Models.Voucher2T();

                    if (voucherMaster.Voucher2T != null)
                    {
                        if (voucherMaster.Voucher2T.FirstOrDefault().V2TId == 0)
                        {
                            foreach (var item in voucherMaster.Voucher2T)
                            {
                                voucher2.CreditAmount = (item.CreditAmount);
                                voucher2.DebitAmount = item.DebitAmount;
                                voucher2.Fid = item.Fid;
                                voucher2.V1Tid = voucherMaster.V1TId;
                                voucher2.Particulars = item.Particulars;
                                if (voucherMaster.SubsiVSLedgerList != null)
                                {
                                    foreach (var item2 in voucherMaster.SubsiVSLedgerList)
                                    {
                                        Models.Models.Voucher3T voucher3 = new Models.Models.Voucher3T();
                                        voucher3.SId = item2.SId;
                                        voucher3.Description = item2.Description;
                                        voucher3.Amount = item2.Amount;
                                        voucher2.Voucher3T.Add(voucher3);
                                    }
                                }

                                //if (voucherMaster.DimensionVSLedgerList != null)
                                //{
                                //    foreach (var item1 in voucherMaster.DimensionVSLedgerList)
                                //    {
                                //        Models.Models.Voucher4T voucher4 = new Models.Models.Voucher4T();
                                //        voucher4.DDID = item1.DDID;
                                //        voucher4.DVId = item1.DVId;
                                //        voucher4.Amount = item1.Amount;
                                //        voucher2.Voucher4T.Add(voucher4);
                                //    }

                                //}
                                if (voucherMaster.BankInfoList != null)
                                {
                                    foreach (var item2 in voucherMaster.BankInfoList)
                                    {
                                        Models.Models.Voucher5T voucher5T = new Models.Models.Voucher5T();
                                        voucher5T.ChequeAmount = item2.ChequeAmount;
                                        voucher5T.ChequeDate = item2.ChequeDate;
                                        voucher5T.ChequeNo = item2.ChequeNo;
                                        voucher5T.Payees = item2.Payees;

                                        voucher2.Voucher5T.Add(voucher5T);
                                    }
                                }

                            }

                            //voucher1.Voucher2T.Add(voucher2);
                            //voucherNo.Voucher1T.Add(voucher1);
                            if (voucherMaster.Narration != null)
                            {
                                uow.Repository<Models.Models.Voucher1T>().Edit(voucher1);
                            }
                            uow.Repository<Models.Models.Voucher2T>().Add(voucher2);

                            uow.Commit();
                        }
                    }
                    #endregion
                    #region EditTheVoucher
                    else
                    {
                        //This is for Editing the Voucher
                        Models.Models.Voucher1T voucher1T = new Models.Models.Voucher1T();
                        decimal totalAmount = 0;
                        //var allVoucher = uow.Repository<Models.Models.Voucher1T>().GetSingle(x => x.V1TId == voucherMaster.V1TId);
                        if (voucherMaster.Voucher2T != null)
                        {
                            foreach (var item in voucherMaster.Voucher2T)
                            {
                                if (item.Voucher5T != null)
                                {
                                    if (item.Voucher5T.Count() > 0)
                                    {
                                        foreach (var subitem in item.Voucher5T)
                                        {
                                            var voucher5T = uow.Repository<Models.Models.Voucher5T>().GetSingle(x => x.V5TId == subitem.V5TId);
                                            totalAmount = totalAmount + subitem.ChequeAmount;
                                            voucher5T.ChequeAmount = subitem.ChequeAmount;
                                            voucher5T.Payees = subitem.Payees;
                                            voucher5T.ChequeNo = subitem.ChequeNo;
                                            voucher5T.ChequeDate = subitem.ChequeDate;
                                            uow.Repository<Models.Models.Voucher5T>().Edit(voucher5T);
                                        }
                                    }
                                }
                                if (item.SubsiVSLedgerList != null)
                                {
                                    if (item.SubsiVSLedgerList.Count() > 0)
                                    {
                                        foreach (var subitem in item.SubsiVSLedgerList)
                                        {
                                            var voucher3Temp = uow.Repository<Models.Models.Voucher3T>().GetSingle(x => x.V3TId == subitem.V3TId);
                                            //voucher3Temp.SFId = subitem.SFId;
                                            voucher3Temp.SId = subitem.SId;
                                            voucher3Temp.Description = subitem.Description;
                                            voucher3Temp.Amount = subitem.Amount;
                                            totalAmount = totalAmount + subitem.Amount;

                                            uow.Repository<Models.Models.Voucher3T>().Edit(voucher3Temp);
                                        }
                                    }
                                }
                                //if (item.Voucher4T.Count() > 0)
                                //{
                                //    foreach (var subitem in item.Voucher4T)
                                //    {
                                //        Models.Models.Voucher4T voucher4T = new Models.Models.Voucher4T();
                                //        voucher4T = subitem;
                                //        uow.Repository<Models.Models.Voucher4T>().Edit(voucher4T);
                                //    }
                                //}
                                Models.Models.Voucher2T voucher2T = new Models.Models.Voucher2T();

                                var modelFromData = uow.Repository<Models.Models.Voucher2T>().GetSingle(x => x.V2TId == item.V2TId);
                                modelFromData.Particulars = item.Particulars;
                                if (voucherMaster.Voucher2T.FirstOrDefault().VoucherTransType == 0)
                                {
                                    modelFromData.DebitAmount = totalAmount;
                                }
                                else
                                {
                                    modelFromData.CreditAmount = -(totalAmount);
                                }
                                uow.Repository<Models.Models.Voucher2T>().Edit(modelFromData);
                            }
                        }
                        uow.Commit();
                    }
                    #endregion
                }
                return voucherMasterId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public bool AddCashLedger(VoucherCashLedgerModel voucherCashLedgerModel)
        {
            if(voucherCashLedgerModel == null)
            {
                return false;
            }

            var userId = Global.UserId;
            var UserWithDesignation = uow.Repository<AllDesignationViewModel>().SqlQuery("select * from fin.FGetAllUsersWithDesignation() where userid = " + userId).FirstOrDefault().DesignationId;

            if(UserWithDesignation == 4)
            {
                voucherCashLedgerModel.Fid = 6155; // Cashier 
                voucherCashLedgerModel.LedgerName = "Cashier";
                voucherCashLedgerModel.Particulars = "Cashier";
            }
            else if(UserWithDesignation == 5)
            {
                voucherCashLedgerModel.Fid = 6156; // Cash in teller
                voucherCashLedgerModel.LedgerName = "Cash In Teller";
                voucherCashLedgerModel.Particulars = "Cash In Teller";
            }
            else if(UserWithDesignation == 3)
            {
                voucherCashLedgerModel.Fid = 6157; // Cash at vault
                voucherCashLedgerModel.LedgerName = "Cash At Vault";
                voucherCashLedgerModel.Particulars = "Cash At Vault";
            }
            else
            {
                voucherCashLedgerModel.Fid = 6154; // Transaction cash
                voucherCashLedgerModel.LedgerName = "Transaction Cash";
                voucherCashLedgerModel.Particulars = "Transaction Cash";
            }
            
            var voucher1T = uow.Repository<Models.Models.Voucher1T>().GetSingle(x => x.V1TId == voucherCashLedgerModel.V1Tid);

            Voucher2T voucher2T = new Voucher2T()
            {
                V1Tid = voucherCashLedgerModel.V1Tid,
                LedgerName = voucherCashLedgerModel.LedgerName,
                Particulars = voucherCashLedgerModel.Particulars,
                VoucherTransType = voucherCashLedgerModel.VoucherTransType,
                CreditAmount = voucherCashLedgerModel.CreditAmount,
                DebitAmount = voucherCashLedgerModel.DebitAmount,
                Fid = voucherCashLedgerModel.Fid
            };

            if (voucher1T == null)
            {
                return false;
            }

            if (voucher2T == null)
            {
                return false;
            }

            var voucher2TT = uow.Repository<Models.Models.Voucher2T>().GetSingle(x => x.Fid == voucherCashLedgerModel.Fid && x.V1Tid == voucherCashLedgerModel.V1Tid);

            if (voucher2TT == null)
            {
                voucher1T.Voucher2T.Add(voucher2T);
                uow.Repository<Voucher1T>().Edit(voucher1T);

            }
            else
            {
                voucher2TT.CreditAmount = voucher2T.CreditAmount;
                voucher2TT.DebitAmount =  voucher2T.DebitAmount;

                uow.Repository<Voucher2T>().Edit(voucher2TT);
            }
            try
            {
                uow.Commit();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeleteVoucher(int V2TId)
        {
            //GenericUnitOfWork uow = new GenericUnitOfWork();
            //Loader.Repository.GenericUnitOfWork uow1 = new Loader.Repository.GenericUnitOfWork();
            bool isSavedSucessfully = false;
            var voucher2Data = uow.Repository<Models.Models.Voucher2T>().GetSingle(x => x.V2TId == V2TId);
            var voucher1Id = voucher2Data.V1Tid;
            var voucher1Data = uow.Repository<Models.Models.Voucher2T>().FindBy(x => x.V1Tid == voucher1Id).ToList();

            if (voucher1Data.Count() > 1)
            {
                uow.Repository<Models.Models.Voucher2T>().Delete(voucher2Data);

                var voucher3Data = uow.Repository<Models.Models.Voucher3T>().FindBy(x => x.V2TId == V2TId).ToList();
                foreach (var item in voucher3Data)
                {
                    uow.Repository<Models.Models.Voucher3T>().Delete(item);
                }

                var voucher4Data = uow.Repository<Models.Models.Voucher4T>().FindBy(x => x.V2TId == V2TId).ToList();
                foreach (var item in voucher4Data)
                {
                    uow.Repository<Models.Models.Voucher4T>().Delete(item);
                }
                var voucher5Data = uow.Repository<Models.Models.Voucher5T>().FindBy(x => x.V2TId == V2TId).ToList();
                foreach (var item in voucher5Data)
                {
                    uow.Repository<Models.Models.Voucher5T>().Delete(item);
                }



            }
            else
            {
                var voucher1DTO = uow.Repository<Models.Models.Voucher1T>().GetSingle(x => x.V1TId == voucher1Id);

                uow.Repository<Models.Models.Voucher1T>().Delete(voucher1DTO);
                uow.Repository<Models.Models.Voucher2T>().Delete(voucher2Data);

                var voucher3Data = uow.Repository<Models.Models.Voucher3T>().FindBy(x => x.V2TId == V2TId).ToList();
                foreach (var item in voucher3Data)
                {
                    uow.Repository<Models.Models.Voucher3T>().Delete(item);
                }

                var voucher4Data = uow.Repository<Models.Models.Voucher4T>().FindBy(x => x.V2TId == V2TId).ToList();
                foreach (var item in voucher4Data)
                {
                    uow.Repository<Models.Models.Voucher4T>().Delete(item);
                }
                var voucher5Data = uow.Repository<Models.Models.Voucher5T>().FindBy(x => x.V2TId == V2TId).ToList();
                foreach (var item in voucher5Data)
                {
                    uow.Repository<Models.Models.Voucher5T>().Delete(item);
                }
            }

            isSavedSucessfully = uow.Commit() > 0;
            return isSavedSucessfully;


        }

        public string GetUserNameforDescription(int userId)
        {
            Loader.Repository.GenericUnitOfWork uowLoader = new Loader.Repository.GenericUnitOfWork();
            var userName = uowLoader.Repository<Loader.Models.ApplicationUser>().FindBy(x => x.Id == userId).FirstOrDefault().UserName;
            return userName;
        }
        public int[] GetIdFromUserNameforUserVsVoucher(string username)
        {
            Loader.Repository.GenericUnitOfWork uowLoader = new Loader.Repository.GenericUnitOfWork();
            var userid = uowLoader.Repository<Loader.Models.ApplicationUser>().FindBy(x => x.UserName.ToLower().Contains(username.ToLower())).ToList();
            int[] id = new int[userid.Count];
            for (int i = 0; i < userid.Count; i++)
            {
                id[i] = (int)userid[i].Id;
            }
            return id;
        }

        public List<Models.Models.Voucher1> GetAll()
        {
            return uow.Repository<Models.Models.Voucher1>().GetAll().ToList();
        }

        public Models.Models.Voucher1 GetSingle(int Voucher1Id)
        {
            Models.Models.Voucher1 Voucher1 = uow.Repository<Models.Models.Voucher1>().GetSingle(c => c.V1Id == Voucher1Id);
            return Voucher1;
        }

        public List<SelectListItem> GetBatchNumber(int type)
        {

            int userID = Loader.Models.Global.UserId;
            //  string query = @"select * from [acc].[GetBatchListByVoucherNo]("+userID+","+type+")";
            string query = @"select * from acc.FGetBatchTypeOfUser(" + type + "," + Loader.Models.Global.SelectedFYID + "," + Loader.Models.Global.BranchId + "," + Loader.Models.Global.UserId + ")";
            var voucherList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().SqlQuery(query).ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in voucherList)
            {
                lst.Add(new SelectListItem { Text = item.BatchName, Value = item.BId.ToString() });
            }
            return lst;
        }
        public List<Batch> GetBatchNumberForCheckBox(int type)
        {

            int userID = Loader.Models.Global.UserId;
            //  string query = @"select * from [acc].[GetBatchListByVoucherNo]("+userID+","+type+")";
            DateTime TrDate = Convert.ToDateTime(Loader.Models.Global.TransactionDate);
            string query = @"select * from acc.FGetBatchTypeOfUser(" + type + "," + Loader.Models.Global.SelectedFYID + "," + Loader.Models.Global.BranchId + "," + Loader.Models.Global.UserId + ")";
            var voucherList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().SqlQuery(query).ToList();
            List<Batch> lst = new List<Batch>();
            foreach (var item in voucherList)
            {
                lst.Add(new Batch { Id = item.BId, Name = item.BatchName, IsSelected = true });
            }
            //if (lst.Count > 0)
            //{
            //    lst.First().IsSelected = true;
            //}
            return lst;
        }

        public List<SelectListItem> GetBatch()
        {
            var voucherList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().GetAll();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in voucherList)
            {
                lst.Add(new SelectListItem { Text = item.BatchName, Value = item.BId.ToString() });

            }
            return lst;
        }

        public string GetBatchName(int VNId)
        {
            var batchId = new GenericUnitOfWork().Repository<Models.Models.VoucherNo>().GetSingle(x => x.VNId == VNId).BId;
            return uow.Repository<Models.Models.BatchDescription>().GetSingle(c => c.BId == batchId).BatchName;

        }

        public string GetVoucherTypeName(int VNId)
        {
            var vTypeId = new GenericUnitOfWork().Repository<Models.Models.VoucherNo>().GetSingle(x => x.VNId == VNId).VTypeId;
            return uow.Repository<Models.Models.VoucherType>().GetSingle(c => c.VTypeID == vTypeId).VoucherName;
        }
        public int GetVoucherTypeId(int VNId)
        {
            var vTypeId = new GenericUnitOfWork().Repository<Models.Models.VoucherNo>().GetSingle(x => x.VNId == VNId).VTypeId;
            return vTypeId;
        }
        public List<SelectListItem> GetVoucherTypeByUserID()
        {
            //string sql = @"select distinct Vt.[VTypeID],Vt.[VoucherName],Vt.Prefix from [acc].[UserVSVoucher] as acc join 
            //  [acc].[VoucherType] as Vt on acc.VTypeId=Vt.VTypeID where acc.UserId=" + userID + "";

            //string sql = @"select distinct Vt.[VTypeID],Vt.[VoucherName],Vt.Prefix from [acc].[UserVSVoucher] as acc join 
            //  [acc].[VoucherType] as Vt on acc.VTypeId=Vt.VTypeID join acc.VoucherNo  as VN on VN.VTypeId=Vt.VTypeID where acc.UserId=" + Loader.Models.Global.UserId + "";
            //int FYID = Loader.Models.Global.SelectedFYID;
            //int BranchId = Loader.Models.Global.BranchId;
            //int UserId = Loader.Models.Global.UserId;
            var voucherList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherType>().SqlQuery("select * from acc.FGetVoucherTypeOfUser(" + Loader.Models.Global.SelectedFYID + "," + Loader.Models.Global.UserId + "," + Loader.Models.Global.BranchId + ")").ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in voucherList)
            {
                lst.Add(new SelectListItem { Text = item.VoucherName, Value = item.VTypeID.ToString() });

            }
            return lst;
        }

        public string GetBatchNamefordescription(int batchId)
        {
            return uow.Repository<Models.Models.BatchDescription>().GetSingle(c => c.BId == batchId).BatchName;
        }

        public string GetVoucherTypeNameforDescription(int VNId)
        {
            return uow.Repository<Models.Models.VoucherType>().GetSingle(c => c.VTypeID == VNId).VoucherName;
        }
        public List<SelectListItem> GetVoucherType()
        {
            var voucherList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherType>().GetAll();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in voucherList)
            {
                lst.Add(new SelectListItem { Text = item.VoucherName, Value = item.VTypeID.ToString() });

            }
            return lst;
        }

        public List<SelectListItem> GetCurrencyType()
        {
            var currencyList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.CurrencyType>().GetAll();
            var currencyId = new Loader.Repository.GenericUnitOfWork().Repository<Loader.Models.ParamValue>().FindBy(x => x.PId == 2004).FirstOrDefault();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in currencyList)
            {
                if (currencyId != null)
                {
                    if (Convert.ToInt32(currencyId.PValue) == item.CTId)
                    {
                        lst.Add(new SelectListItem { Text = item.CurrencyName, Value = currencyId.PValue.ToString(), Selected = true });
                    }
                    else
                    {
                        lst.Add(new SelectListItem { Text = item.CurrencyName, Value = item.CTId.ToString() });
                    }
                }
                else
                {
                    lst.Add(new SelectListItem { Text = item.CurrencyName, Value = item.CTId.ToString() });
                }

            }
            return lst;
        }

        public List<SelectListItem> GetFiscalYear()
        {
            var FYList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FiscalYear>().GetAll();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in FYList)
            {
                lst.Add(new SelectListItem { Text = item.FyName, Value = item.FYID.ToString() });

            }
            return lst;
        }

        public List<SelectListItem> GetVoucherDimensionList(int DDID)
        {
            var DMlist = uow.Repository<Models.Models.DimensionValue>().FindBy(x => x.DDId == DDID);
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in DMlist)
            {
                lst.Add(new SelectListItem { Text = item.DimensionValue1, Value = item.DVId.ToString() });

            }
            return lst;
        }

        public string GetDimensionName(int DDID)
        {
            var DMlist = uow.Repository<Models.Models.DimensionDefination>().GetSingle(x => x.DDId == DDID).DefName;
            return DMlist;

        }

        public string GetVoucherDimensionLabel(int Id)
        {
            var ddId = uow.Repository<Models.Models.DimensionVSLedger>().GetSingle(x => x.Id == Id).DDID;
            var Dname = uow.Repository<Models.Models.DimensionDefination>().GetSingle(x => x.DDId == ddId).DefName;

            return Dname;

        }

        public string GetVoucherVerificationRaisedBy(int v1Id)
        {
            Loader.Repository.GenericUnitOfWork uowLoader = new Loader.Repository.GenericUnitOfWork();
            var test = uowLoader.Repository<Loader.Models.ApplicationUser>().GetAll().ToList();
            var name = (from m in uow.Repository<Models.Models.Task>().FindBy(x => x.EventValue == v1Id && x.EventId == 10)
                        join n in uowLoader.Repository<Loader.Models.ApplicationUser>().GetAll() on m.RaisedBy equals n.Id
                        select n).FirstOrDefault().UserName;


            return name;

        }

        public string GetUserName(int userId)
        {
            Loader.Repository.GenericUnitOfWork uowLoader = new Loader.Repository.GenericUnitOfWork();
            var userName = uowLoader.Repository<Loader.Models.ApplicationUser>().FindBy(x => x.Id == userId).FirstOrDefault().UserName;
            return userName;
        }

        public string GetVoucherVerificationRaisedOn(int v1Id)
        {

            var raisedOn = uow.Repository<Models.Models.Task>().FindBy(x => x.EventValue == v1Id && x.EventId == 10).FirstOrDefault().RaisedOn.ToString();
            return raisedOn;

        }

        public string GetVoucherDimensionValue(int DVID)
        {
            var ddId = uow.Repository<Models.Models.DimensionValue>().GetSingle(x => x.DVId == DVID).DimensionValue1;

            return ddId;

        }

        public string GetSubsiName(int SSID)
        {
            var ddId = uow.Repository<Models.Models.SubsiSetup>().GetSingle(x => x.SSId == SSID).Title;

            return ddId;
        }

        public string GetLedgerName(int ledgerId)
        {
            if (ledgerId == 0)
            {
                return "";
            }
            var ddId = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == ledgerId).Fname;

            return ddId;
        }

        public bool DeleteTemporaryLogsValue(int V1TId)
        {
            bool isSavedSucessfully = false;

            var voucher1Id = V1TId;

            var voucher1DTO = uow.Repository<Models.Models.Voucher1T>().GetSingle(x => x.V1TId == voucher1Id);
            uow.Repository<Models.Models.Voucher1T>().Delete(voucher1DTO);

            var voucher2Data = uow.Repository<Models.Models.Voucher2T>().FindBy(x => x.V1Tid == voucher1DTO.V1TId).ToList();
            foreach (var item in voucher2Data)
            {
                uow.Repository<Models.Models.Voucher2T>().Delete(item);

                var voucher3Data = uow.Repository<Models.Models.Voucher3T>().FindBy(x => x.V2TId == item.V2TId).ToList();
                foreach (var item1 in voucher3Data)
                {
                    uow.Repository<Models.Models.Voucher3T>().Delete(item1);
                }

                var voucher4Data = uow.Repository<Models.Models.Voucher4T>().FindBy(x => x.V2TId == item.V2TId).ToList();
                foreach (var item2 in voucher4Data)
                {
                    uow.Repository<Models.Models.Voucher4T>().Delete(item2);
                }
                var voucher5Data = uow.Repository<Models.Models.Voucher5T>().FindBy(x => x.V2TId == item.V2TId).ToList();
                foreach (var item3 in voucher5Data)
                {
                    uow.Repository<Models.Models.Voucher5T>().Delete(item3);
                }

            }




            isSavedSucessfully = uow.Commit() > 0;
            return isSavedSucessfully;

        }

        //public List<SelectListItem> GetVoucherSubsiList(int? subsiId)
        //{
        //    var DMlist = uow.Repository<Models.Models.SubsiSetup>().FindBy(x => x.SSId== subsiId).ToList();
        //    List<SelectListItem> lst = new List<SelectListItem>();
        //    foreach (var item in DMlist)
        //    {
        //        lst.Add(new SelectListItem { Text = item.Title, Value = item.SSId.ToString() });

        //    }
        //    return lst;
        //}
        public List<SelectListItem> GetVoucherSubsiList(int? SFId)
        {
            LoaderRepository.GenericUnitOfWork loaderUOW = new LoaderRepository.GenericUnitOfWork();
            var tableId = from m in uow.Repository<Models.Models.SubsiVSFId>().FindBy(x => x.SFId == SFId) join n in uow.Repository<Models.Models.SubsiSetup>().GetAll() on m.SSId equals n.SSId select n.STBId;
            List<SelectListItem> lst = new List<SelectListItem>();
            int tablefinalId = Convert.ToInt32(tableId.FirstOrDefault());
            if (tablefinalId == 1)
            {
                var data = loaderUOW.Repository<Loader.Models.Employee>().GetAll().ToList();
                foreach (var item in data)
                {
                    lst.Add(new SelectListItem { Text = item.EmployeeName, Value = item.EmployeeId.ToString() });
                }

            }

            if (tablefinalId == 3 || tablefinalId == 4)
            {
                var data = uow.Repository<ChannakyaAccounting.Models.Models.CustIndividual>().GetAll().ToList();
                foreach (var item in data)
                {
                    lst.Add(new SelectListItem { Text = item.Fname, Value = item.CID.ToString() });
                }
            }


            //}
            return lst;
        }

        //public string GetVoucherTypeName(int VnId)
        //{
        //    var DMlist = uow.Repository<Models.Models.VoucherNo>().GetSingle(x => x.VNId == VnId).vo;
        //    return DMlist;
        //}
        public string GetSubsiName(int? SubsiId)
        {
            var DMlist = uow.Repository<Models.Models.SubsiSetup>().GetSingle(x => x.SSId == SubsiId).Title;
            return DMlist;

        }

        public string GetDimensionDefination(int DDID)
        {
            var DMlist = uow.Repository<Models.Models.DimensionDefination>().GetSingle(x => x.DDId == DDID).DefName;
            return DMlist;

        }

        public string GetDimensionValue(int DVId)
        {
            string dimensionVal = "";
            var DMlist = uow.Repository<Models.Models.DimensionValue>().GetSingle(x => x.DVId == DVId);
            if (DMlist != null)
            {
                dimensionVal = DMlist.DimensionValue1;
            }
            return dimensionVal;
        }
        //public string GetUserName(int userId)
        //{
        //    return  uow.Repository<Loader.Models.ApplicationUser>().FindBy(x => x.Id == userId).FirstOrDefault().UserName;
        //}
        //return Loader.Models.ApplicationUse        }

        //public string GetBatchName(int batchId)
        //{
        //    return uow.Repository<Models.Models.BatchDescription>().GetSingle(x => x.BId == batchId).BatchName;
        //}

        //public string GetVoucherType(int VtypeId)
        //{
        //    return uow.Repository<Models.Models.VoucherType>().GetSingle(x => x.VTypeID == VtypeId).VoucherName;
        //}

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

        public int DefultCurrency()
        {
            return 1;
        }

        public DenoInOutViewModel DenoList(int currencyId = 0)
        {
            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            int UserId = Loader.Models.Global.UserId;
            int UserLevel = 2;
            var deloList = uow.Repository<DenoInViewModel>().SqlQuery("select * from fin.FGetCurrentDenoList(" + UserId + "," + UserLevel + "," + currencyId + ")").ToList();
             
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
        public bool IsTransactionWithDeno()
        {

            var isWithDeno = uow.Repository<CurrencyRateViewModel>().SqlQuery("select CAST(PValue as bit) as IsTransWithDeno from LG.ParamValue where PId=32").FirstOrDefault();
            if (isWithDeno == null)
            {
                return false;
            }
            else
            {
                return isWithDeno.IsTransWithDeno;
            }

        }


        public DenoInOutViewModel DenoListByUser(int currencyId = 0, int UserId = 0)
        {
            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            //int UserId = Loader.Models.Global.UserId;
            int UserLevel = 2;
            var deloList = uow.Repository<DenoInViewModel>().SqlQuery("select * from fin.FGetCurrentDenoList(" + UserId + "," + UserLevel + "," + currencyId + ")").ToList();

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

        public DateTime GetBranchTransactionDate()
        {
            var transactionDate = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select TDate as DateValue  from lg.Company where CompanyId=" + Global.BranchId).FirstOrDefault();
            return Convert.ToDateTime(transactionDate.DateValue);
        }
        public static bool BalanceWithDenoAmount(DenoInOutViewModel denoInOutList, decimal amount)
        {
            using (GenericUnitOfWork _uow = new GenericUnitOfWork())
            {
                decimal sum = 0;
                foreach (var item in denoInOutList.DenoInList)
                {
                    // var actualDeno = _uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalId && x.UserId == Global.UserId).FirstOrDefault();

                    var OutPiece = denoInOutList.DenoOutList.Where(x => x.DenoBalIdOut == item.DenoBalId).Select(x => x.DenoNumberOut).FirstOrDefault();
                    double deno = item.Deno;
                    //if (actualDeno != null)
                    //{
                    //    if (actualDeno.Piece < OutPiece)
                    //    {
                    //        return false;
                    //    }
                    //}
                    //else
                    //{
                    //    OutPiece = 0;
                    //    item.DenoNumber = 0;
                    //}


                    decimal totalDeno = Convert.ToDecimal((item.DenoNumber * deno) - (OutPiece * deno));
                    sum = sum + totalDeno;
                }
                if (Math.Abs(sum) != amount)
                    return false;
                else
                    return true;
            }
        }

        public static ReturnBaseMessageModel AvailableDenoNumber(List<DenoOutViewModel> denoOutList)
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();


            foreach (var item in denoOutList.Where(x => x.DenoNumberOut > 0))
            {
                //if (item.DenoNumberOut != 0)
                //{
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
                //}

            }
            returnMessage.Success = true;
            return returnMessage;
        }


        public ReturnBaseMessageModel UpdateDeno(DenoInOutViewModel denoInOutModel, decimal amount, long vno)
        {
            using (TransactionScope transaction = new TransactionScope(
                    // a new transaction will always be created
                    TransactionScopeOption.RequiresNew,
                    // we will allow volatile data to be read during transaction
                    new TransactionOptions()
                    {
                        IsolationLevel = IsolationLevel.ReadUncommitted,

                    }
                ))
            {
                try
                {
                    bool IsTrnsWithDeno = IsTransactionWithDeno();
                    if (IsTrnsWithDeno)
                    {
                        if (!BalanceWithDenoAmount(denoInOutModel, amount))
                        {
                            returnMessage.Msg = "Amount is not match with deno balance.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        returnMessage = AvailableDenoNumber(denoInOutModel.DenoOutList);
                        if (!returnMessage.Success)
                        {
                            return returnMessage;
                        }
                        if (IsTrnsWithDeno)
                        {
                            DenoInOutTransaction(denoInOutModel, vno);
                        }
                    }
                    uow.Commit();
                    transaction.Complete();
                    returnMessage.Msg = "Successfully Saved";
                    returnMessage.Success = true;
                    //returnMessage.ReturnId = uow.Repository<CollSheet>().GetSingle(x => x.CollSheetId == collSheetId).SheetNo;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = " Not saved.!!" + ex.Message;
                    return returnMessage;
                }
            }
        }

        public void DenoInOutTransaction(DenoInOutViewModel denoInOutModel, Int64 transactionNumber)
        {
            try
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
                    getSingle.UserId = Global.UserId;
                    getSingle.UserLevel = 2;
                    getSingle.Piece = totalDeno;

                    denoTranx.Denoid = item.DenoID;
                    denoTranx.Trxno = transactionNumber;
                    denoTranx.vdate = GetBranchTransactionDate();
                    denoTranx.Pics = item.DenoNumber;
                    denoTranx.UserId = Global.UserId;
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
                    denoTranx.vdate = GetBranchTransactionDate();
                    denoTranx.Pics = (-item.DenoNumberOut);
                    denoTranx.UserId = Global.UserId;
                    if (item.DenoNumberOut != 0)
                    {
                        uow.Repository<DenoTrxn>().Add(denoTranx);
                    }
                }

                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
