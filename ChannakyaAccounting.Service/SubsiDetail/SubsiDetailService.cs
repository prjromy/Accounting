using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.Subsi_Setup
{
    public class SubsiDetailService
    {
        private GenericUnitOfWork uow = null;


        public SubsiDetailService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<SelectListItem> GetSubsiLedger(int custId)
        {
            var dimList = from m in uow.Repository<Models.Models.FinAcc>().GetAll()
                          join n in uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == 7)
                          on m.F2Type equals n.F2id
                          select new Models.Models.FinAcc
                          {
                              Fid = m.Fid,
                              Fname = m.Fname
                          };

            //var dimList = uow.Repository<Models.Models.FinAcc>().SqlQuery("select m.fid,m.fname from acc.finacc m inner join acc.finsys2 n on m.F2Type = n.F2id where f1id = 7 ").AsQueryable();

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in dimList)
            {
                //if (custId != 0)
                //{
                //var checkifExists = uow.Repository<Models.Models.SubsiDetail>().FindBy(x => x.FId == item.Fid && x.CId == custId).Count();
                //if (checkifExists == 0)
                //{
                lst.Add(new SelectListItem { Text = item.Fname, Value = item.Fid.ToString() });
                //}
                //}

            }
            return lst;
        }

        public SelectListItem GetSubsiLedgerItem(int custId)
        {
            var dimList = from m in uow.Repository<Models.Models.FinAcc>().FindBy(x=>x.Fid==custId)
                          join n in uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == 7)
                          on m.F2Type equals n.F2id
                          select new Models.Models.FinAcc

                          {
                              Fid = m.Fid,
                              Fname = m.Fname

                          };
            SelectListItem lst = new SelectListItem {Text=dimList.SingleOrDefault().Fname,Value= dimList.SingleOrDefault().Fid.ToString()};
            return lst;
        }
        public List<int> GetSubsiLedgerIdByName(string query)
        {
          var  dimList = from m in uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Fname.ToLower().Contains(query.ToLower()))
                          join n in uow.Repository<Models.Models.SubsiDetail>().GetAll()
                          on m.Fid equals n.FId
                          select new Models.Models.FinAcc

                          {
                              Fid = m.Fid,
                              Fname = m.Fname

                          };
            List<int> FId = dimList.Select(x => x.Fid).ToList<int>();
            return FId;
        }


        public string GenerateAccountNumber(int ledgerNo)
        {
            var accDetails = from m in uow.Repository<Models.Models.SubsiVSFId>().FindBy(x => x.Fid == ledgerNo)
                             join n in uow.Repository<Models.Models.SubsiSetup>().GetAll()
                             on m.SSId equals n.SSId
                             select new Models.Models.SubsiSetup
                             {
                                 Length = n.Length,
                                 Prefix = n.Prefix,
                                 CurrentNo=n.CurrentNo

                             };

            //var detail = /*select * from acc.SubsiVSFId a inner join acc.SubsiSetup b on a.SSId=b.SSId  inner join acc.SubsiDetail c on a.Fid=c.FId where a.Fid=1134 order by c.SDID desc*/;

            //var accDetails = uow.Repository<Models.Models.SubsiSetup>().SqlQuery("select * from acc.FGetAccountNoByFID(" + ledgerNo + ") ");
            //// to get initial numbers
            //var Initialdata = accDetails.FirstOrDefault().Title;
            //var initialNumber = 1;
            //if (accDetails.FirstOrDefault().Title != null || accDetails.FirstOrDefault().Title=="")
            //{
            //    int count = Convert.ToInt32(accDetails.FirstOrDefault().Length) + accDetails.FirstOrDefault().Prefix.Length;
            //    var number = Initialdata.Remove(0, count);
            //    initialNumber = Convert.ToInt32(number.Remove(number.Length - 1, 1)) + 1;
            //}    
           
            string accNumber ="0";
            if (accDetails.Count() > 0)
            {
                 accNumber = accDetails.FirstOrDefault().Prefix + accDetails.SingleOrDefault().CurrentNo.ToString().PadLeft(Convert.ToInt32(accDetails.SingleOrDefault().Length), '0');
            }
            return accNumber;
            //var accNumber = accDetails.Select(c => new { c.Length, c.Prefix = c.Length + " " + c.Prefix });

        }

        public List<Models.ViewModel.SubsiViewModel> GetAllAccountDetailsDepositLoanProduct(int ledgerId)
        {
            GenericUnitOfWork UOW = new GenericUnitOfWork();
            var subsiList = UOW.Repository<Models.ViewModel.DepositAndLoanAccountDetail>().SqlQuery("select * from dbo.GenerateAccountDetailOfDepositAndLoan(" + ledgerId + ")").ToList().Select(x=> new Models.ViewModel.SubsiViewModel
            {Id=x.IAccno,
            Accno=x.Accno,
            OpeningBalance=x.OpeningBal,
            Name =x.AName}).ToList();
            return subsiList;

        }

        public string GenerateAccountNumberForDepositAndLoan(int ledgerNo)
        {
            string accNumber = "";
            GenericUnitOfWork UOW = new GenericUnitOfWork();
            var subsiList = UOW.Repository<Models.ViewModel.DepositAndLoanAccountDetail>().SqlQuery("select * from dbo.GenerateAccountDetailOfDepositAndLoan(" + ledgerNo + ")").ToList();
            if(subsiList!=null && subsiList.FirstOrDefault()!=null)
            {
                accNumber = subsiList.FirstOrDefault().Accno;
            }
            return accNumber;
        }

        public int GenerateIAccNoForDepositAndLoan(int ledgerNo)
        {
            int accNumber =0;
            GenericUnitOfWork UOW = new GenericUnitOfWork();
            var subsiList = UOW.Repository<Models.ViewModel.DepositAndLoanAccountDetail>().SqlQuery("select * from dbo.GenerateAccountDetailOfDepositAndLoan(" + ledgerNo + ")").ToList();
            if (subsiList != null && subsiList.FirstOrDefault() != null)
            {
                accNumber = subsiList.FirstOrDefault().IAccno;
            }
            return accNumber;
        }


        public List<SelectListItem> GetSubsiTable()
        {
            var dimList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiTable>().GetAll().ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in dimList)
            {
                lst.Add(new SelectListItem { Text = item.TableName, Value = item.STBId.ToString() });

            }
            return lst;
        }
        public List<Models.Models.SubsiDetail> GetAll()
        {
            return uow.Repository<Models.Models.SubsiDetail>().GetAll().ToList();
        }

        public Models.Models.SubsiDetail GetSingle(int SubsiDetailId)
        {
            Models.Models.SubsiDetail SubsiDetail = uow.Repository<Models.Models.SubsiDetail>().GetSingle(c => c.SDID == SubsiDetailId);
            return SubsiDetail;
        }


        public void Save(Models.Models.SubsiDetail SubsiDetail)
        {
            try
            {
                GenericUnitOfWork editUOW = new GenericUnitOfWork();
                //int checkExists = editUOW.Repository<Models.Models.SubsiDetail>().GetAll().Where(x => x.SDID != SubsiDetail.SDID).Count();
                //if (checkExists > 0)
                //{
                //    throw new Exception("Duplicate SubsiDetail Found. SubsiDetail Caption Not Valid");
                //}
                if (SubsiDetail.SDID == 0)
                {
                    SubsiDetail.PostedBy = Loader.Models.Global.UserId;
                    SubsiDetail.PostedOn = DateTime.Now;
                    SubsiDetail.BranchId = Loader.Models.Global.BranchId;
                    uow.Repository<Models.Models.SubsiDetail>().Add(SubsiDetail);
                }
                else
                {
                    var subsiDetailList = editUOW.Repository<Models.Models.SubsiDetail>().GetSingle(x => x.SDID == SubsiDetail.SDID);
                    subsiDetailList.DebitLimit = SubsiDetail.DebitLimit;
                    subsiDetailList.CreditLimit = SubsiDetail.CreditLimit;
                    subsiDetailList.FId = SubsiDetail.FId;
                    subsiDetailList.Status = SubsiDetail.Status;
                    subsiDetailList.Enable = SubsiDetail.Enable;
                    uow.Repository<Models.Models.SubsiDetail>().Edit(subsiDetailList);
                }
                var SubsiSetUpitem = uow.Repository<Models.Models.SubsiSetup>().SqlQuery("select a.* from acc.SubsiSetup a inner join acc.SubsiVSFId b on a.SSId=b.ssid where b.Fid=" + SubsiDetail.FId + "").SingleOrDefault();
                if (SubsiSetUpitem != null)
                {
                    SubsiSetUpitem.CurrentNo += 1;
                    uow.Repository<Models.Models.SubsiSetup>().Edit(SubsiSetUpitem);
                }
                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<Models.ViewModel.SubsiDetailViewModel> GetSubsiDetailList(int pageNo=1,int PageSize=10,string search="")
        {
            List<Models.ViewModel.SubsiDetailViewModel> lstSubsiDetail = new List<Models.ViewModel.SubsiDetailViewModel>();
            string query = "select * from  [acc].[FGetMapSubsiList]("+pageNo+","+PageSize+",'"+search+"',"+Loader.Models.Global.BranchId+")";
            lstSubsiDetail = uow.Repository<Models.ViewModel.SubsiDetailViewModel>().SqlQuery(query).ToList(); ;
            return lstSubsiDetail;
        }

        public bool Delete(int SubsiDetailId)
        {
            Models.Models.SubsiDetail SubsiDetail = this.GetSingle(SubsiDetailId);

            if (SubsiDetail != null)
            {
                uow.Repository<Models.Models.SubsiDetail>().Delete(SubsiDetail);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
        public int MappedCount(int Fid)
        {
            string query = "select count(V2Id) from acc.voucher2  where fid=" + Fid + "";
            return uow.GetContext().Database.SqlQuery<int>(query).ToArray()[0];
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
        public List<Models.ViewModel.CustomerIndDetailViewModel> getCustList(int pageNo, int pageSize, string search = "")
        {
            List<Models.ViewModel.CustomerIndDetailViewModel> lstcutind = new List<Models.ViewModel.CustomerIndDetailViewModel>();
            string query = "select * from acc.[FGetCustomerDetailList]("+pageNo+","+pageSize+",'"+search+"')";
            lstcutind = uow.Repository<Models.ViewModel.CustomerIndDetailViewModel>().SqlQuery(query).ToList();
            return lstcutind;
        }
        public List<Models.ViewModel.EmployeeViewModel> getempList(int pageNo, int pageSize, string search = "")
        {
            List<Models.ViewModel.EmployeeViewModel> lstempp = new List<Models.ViewModel.EmployeeViewModel>();
            string query = "select * from acc.[FGetEmployeeDetailList](" + pageNo + "," + pageSize + ",'" + search + "')";
            lstempp = uow.Repository<Models.ViewModel.EmployeeViewModel>().SqlQuery(query).ToList();
            return lstempp;
        }

    }
}
