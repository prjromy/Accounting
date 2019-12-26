using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ChannakyaBase.Model.ViewModel;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Core.Objects;
using ChannakyaCustomeDatePicker;
namespace ChannakyaBase.BLL
{

    public class CommonService
    {
        private GenericUnitOfWork uow = null;
        private Loader.Service.DesignationService designationService = null;
        ChannakyaBaseEntities _context = null;

        public CommonService()
        {
            uow = new GenericUnitOfWork();
            designationService = new Loader.Service.DesignationService();
            _context = new ChannakyaBaseEntities();
        }
        public List<Designation> GetRoleList()
        {
            var designation = designationService.GetAll();
            return designation;
        }
        public string GetEmployeeName(int empId)
        {
            Loader.Service.EmployeeService empService = new Loader.Service.EmployeeService();
            return empService.GetSingle(empId).EmployeeName;
        }
        public SelectList GetDesignationRoleList()
        {
            return new SelectList(GetRoleList(), "DGId", "DGName");
        }

        public int GetBranchIdByEmployeeUserId(int userId)
        {
            var getBranchByuserId = uow.Repository<EmployeeBranchMapModel>().SqlQuery(@"select BranchId from fin.EmployeeVsBranch
                                                                                        where MapId=(select  max(eb.MapId) from fin.EmployeeVsBranch eb
                                                                                        join[lg].[User] u on eb.EmpId = u.EmployeeId
                                                                                        where u.UserId =" + userId + @"
                                                                                        and StartFrom <= CONVERT(VARCHAR(10), GETDATE(), 110))").FirstOrDefault();
            return getBranchByuserId.BranchId;
        }
        public string GetProductNameByPid(int? PID)
        {
            string productName = uow.Repository<String>().SqlQuery(@"select Pname from fin.ProductDetail where Pid=" + PID).FirstOrDefault();
            return productName;
        }
        public DateTime GetBranchTransactionDate()
        {
            DateTime transactionDate = uow.Repository<LicenseBranch>().GetAll().Select(x => x.TDate).FirstOrDefault();
            return transactionDate;
        }
        public bool IsTransactionWithDeno()
        {
            var isWithDeno = uow.Repository<CurrencyRateViewModel>().SqlQuery("select CAST(PValue as bit) as IsTransWithDeno from LG.ParamValue where PId=2003").FirstOrDefault();
            if (isWithDeno == null)
            {
                return false;
            }
            else
            {
                return isWithDeno.IsTransWithDeno;
            }

        }

        public Int64 GetUtno()
        {

            var uttno = new SqlParameter();
            uttno.ParameterName = "@TNO";
            uttno.Direction = ParameterDirection.Output;
            uttno.SqlDbType = SqlDbType.BigInt;
            var getUttno = uow.Repository<UttnoModel>().SqlQuery("exec [Mast].[GetTransno] @TNO out", uttno).FirstOrDefault();
            return getUttno.uttno;
        }

        public void InsertAvailableBalance(int flag, DepositViewModel depositModel)
        {
            try
            {
                decimal shadowCr = 0;
                ABal availableForInsert = null;
                ABal availableBal = uow.Repository<ABal>().GetSingle(x => x.IAccno == depositModel.AccountId && x.Flag == flag);
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
                availableBal.FId = 0;
                availableBal.Bal = depositModel.Amount + shadowCr;
                if (availableForInsert == null)
                {
                    uow.Repository<ABal>().Add(availableBal);
                }
                else
                {
                    uow.Repository<ABal>().Edit(availableBal);
                }

            }
            catch (Exception ex)
            {

                throw;
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
                    getSingle.UserId = Loader.Models.Global.UserId;
                    getSingle.UserLevel = 2;
                    getSingle.Piece = totalDeno;

                    denoTranx.Denoid = item.DenoID;
                    denoTranx.Trxno = transactionNumber;
                    denoTranx.vdate = GetBranchTransactionDate();
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
                    denoTranx.vdate = GetBranchTransactionDate();
                    denoTranx.Pics = (-item.DenoNumberOut);
                    if (item.DenoNumberOut != 0)
                    {
                        uow.Repository<DenoTrxn>().Add(denoTranx);
                    }
                }

                uow.Commit();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public bool isStrictlyVerifiable()
        {
            Loader.Service.ParameterService parameterService = new Loader.Service.ParameterService();
            var isVerifiable = Convert.ToBoolean(parameterService.GetSingle(2002).paramValue.PValue);
            return Convert.ToBoolean(isVerifiable);

        }

        public int GetFidByIAccno(Int64 iAccno)
        {
            return _context.Database.SqlQuery<int>("select * from fin.FGetFidByIAccno(" + iAccno + ")").FirstOrDefault();
        }
        public string DateType()
        {
            string dateType = DateCommonService.DateType();
            return dateType;
        }
        public string GetNepaliDate(DateTime date)
        {
            if (DateType() == "1")
            {
                return date.ToShortDateString();
            }
            else
            {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string nepaliDate = uow.Repository<string>().SqlQuery("Select [dbo].[FGetDateBS](@EngDate)",
                    new SqlParameter("@EngDate", date)).FirstOrDefault();
                return nepaliDate;
            }
            }

        }

        public DateTime GetMatDate(DateTime date, decimal month, string type)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                bool isBs = false;
                if (type == "1")
                {
                    isBs = false;
                }
                else
                {
                    isBs = true;
                }

                DateTime nepaliDate = _context.Database.SqlQuery<DateTime>("Select [dbo].[FGetMatDat](@DateAD,@AddMonthDay,@IsDateBS)",
                new SqlParameter("@DateAD", date.ToShortDateString()),
                new SqlParameter("@AddMonthDay", month),
                new SqlParameter("@IsDateBS", isBs)
                ).FirstOrDefault();
                return nepaliDate;
            }
        }

        public int GetAccStatusByIaccno(Int64 TrnxNo = 0)
        {
            int? iaccno = 0;
            iaccno= uow.Repository<ChgSTrnx>().FindBy(x => x.TrnxNo == TrnxNo).Select(x => x.Iaccno).FirstOrDefault();
            if(iaccno!=null||iaccno==0)
            {
                int accstate = uow.Repository<ADetail>().FindBy(x => x.IAccno == iaccno).Select(x => x.AccState).FirstOrDefault();
                return accstate;
            }
            return 0;
        }
    }
}
