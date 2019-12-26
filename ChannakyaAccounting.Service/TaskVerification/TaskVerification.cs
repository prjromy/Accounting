using ChannakyaAccounting.Models.Models;
using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Service.TaskVerificationAcc
{

    public class TaskVerificationService
    {
        GenericUnitOfWork uow = null;

        ReturnBaseMessageModel returnMessage = null;


        public TaskVerificationService()
        {

            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();
            //financeParameterService = new FinanceParameterService();
        }
        public List<TaskVerificationList> VerifierList(int EventId = 1)
        {
            var verificationList = uow.Repository<TaskVerificationList>().SqlQuery(@"select * from lg.fgetverifierlist(" + EventId + ")").ToList();

            return verificationList;
        }
        public int CountOfNofication()
        {
            int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select count(*) as CountOfTask from mast.FGetTaskDetails(" + UserId + ") where SeenOn is null and IsVerified=0").FirstOrDefault();
            return taskList.CountOfTask;
        }
        public List<TaskViewModel> TaskList()
        {
            int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.FGetTaskDetails(" + UserId + ")where SeenOn is null and IsVerified=0 order by RaisedOn Desc").ToList();
            return taskList;
        }
        public List<TaskViewModel> ViewAllTasks(string employeeName, int? eventId, DateTime? fromdate, DateTime? todate, int pageNumber, int pageSize)
        {
            //List<string> date = dateRange.Split('|').ToList<string>();
            int UserId = Loader.Models.Global.UserId;
            string query = "select COUNT(*) OVER () AS TotalCount, * from mast.FGetTaskDetails(" + UserId + ") where IsVerified=0";

            if (employeeName != "")
            {
                query += " and EmployeeName like'" + employeeName + "%'";
            }
            if (eventId != 0 && eventId != null)
            {
                query += " and EventId=" + eventId;
            }
            if ((fromdate != null) && (todate != null))
            {
                query += " and RaisedOn between" + "'" + fromdate + "'" + "and" + "'" + todate + "'";
            }
            query += @" ORDER BY RaisedOn Desc
                       OFFSET ((" + pageNumber + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(query).ToList();
            return taskList;


            //var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.FGetTaskDetails(" + UserId + ")where IsVerified=0 order by RaisedOn Desc").ToList();
            //return taskList;
        }
        public TaskViewModel GetSingleTask(int task1Id)
        {
            int UserId = Loader.Models.Global.UserId;

            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.FGetTaskDetails(" + UserId + ") where Task1id=" + task1Id).FirstOrDefault();

            NotificationSeenOn(task1Id);
            return taskList;
        }
        public string VerifiedBy(int task1Id)
        {
            //int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.[FGetTaskVerifiedBy](" + task1Id + ")").FirstOrDefault();
            if (taskList == null)
                return "";
            else
                return taskList.Verifier;
        }

        public void SaveTaskNotification(TaskVerificationList TaskVerifierList, Int64 eventValue, int eventId)
        {
            Loader.Repository.GenericUnitOfWork loaderUow = new Loader.Repository.GenericUnitOfWork();
            ChannakyaAccounting.Models.Models.Task task = new ChannakyaAccounting.Models.Models.Task();
            task.EventValue = eventValue;
            task.EventId = eventId;
            task.RaisedBy = Loader.Models.Global.UserId;
            // task.RaisedOn = (DateTime)Loader.Models.Global.TransactionDate;//transcation date
            task.RaisedOn = DateTime.Now;
            task.Message = TaskVerifierList.Message;
            task.IsVerified = false;
            bool isNew;
            bool isStrictlyVerifiable = Convert.ToBoolean(loaderUow.Repository<Loader.Models.ParamValue>().GetSingle(x => x.PId ==(int) ChannakyaAccounting.EnumValue.Parameter.IsStrictlyVerified).PValue);
            if (isStrictlyVerifiable == false)
            {
                TaskVerifierList.VerifierList = VerifierList(eventId).ToList();
            }
            if (task.Task1Id == 0)
            {


                foreach (var item in TaskVerifierList.VerifierList)
                {
                    if (item.IsSelected == true || isStrictlyVerifiable == false)
                    {
                        TaskDetail taskDetails = new TaskDetail();
                        taskDetails.TaskTo = item.UserId;
                        
                        task.TaskDetails.Add(taskDetails);
                    }
                }
                uow.Repository<ChannakyaAccounting.Models.Models.Task>().Add(task);
                isNew = true;
            }
            else
            {
                uow.Repository<ChannakyaAccounting.Models.Models.Task>().Edit(task);
                isNew = false;
            }

            uow.Commit();
            if (isNew==true)
            {
                var recentTask=uow.Repository<ChannakyaAccounting.Models.Models.Task>().GetAll().OrderByDescending(x => x.Task1Id).FirstOrDefault();
                if (recentTask != null)
                {
                    Loader.Service.ParameterService Ob = new Loader.Service.ParameterService();
                    Ob.SendNotification(recentTask.Task1Id);
                }
            }
        }

        //public void SaveTaskNotificationStrictlyUnverified( Int64 eventValue, int eventId)
        //{
        //    ChannakyaAccounting.Models.Models.Task task = new ChannakyaAccounting.Models.Models.Task();
        //    task.EventValue = eventValue;
        //    task.EventId = eventId;
        //    task.RaisedBy = Loader.Models.Global.UserId;
        //    task.RaisedOn = DateTime.Now;
        //    task.Message = "";
        //    task.IsVerified = false;
        //    bool isStrictlyVerifiable = Convert.ToBoolean(uow.Repository<Models.Models.ParamValue>().GetSingle(x => x.PId == 1032).PValue);
        //    if (isStrictlyVerifiable == false)
        //    {
        //        var verifierList = VerifierList(eventId).ToList();
        //        foreach (var item in verifierList)
        //        {
                    
        //                TaskDetail taskDetails = new TaskDetail();
        //                taskDetails.TaskTo = item.UserId;
        //                task.TaskDetails.Add(taskDetails);
                    
                   
        //        }
        //    }
        //    uow.Repository<ChannakyaAccounting.Models.Models.Task>().Add(task);
        //    uow.Commit();
        //}
        public void NotificationSeenOn(int task1id)
        {
            int userid = Loader.Models.Global.UserId;
            TaskDetail tskm = new TaskDetail();
            var tsk = uow.Repository<TaskDetail>().FindBy(x => x.Task1Id == task1id && x.TaskTo == userid).FirstOrDefault();
            tskm = tsk;
            tskm.SeenOn = DateTime.Now;
            uow.Repository<TaskDetail>().Edit(tskm);
            uow.Commit();
        }

        public List<Event> GetEventByEventId(int EventId = 0)
        {
            var EvenList = (from e in uow.Repository<Event>().FindBy(x => x.EventId == EventId)
                            join
t in uow.Repository<ChannakyaAccounting.Models.Models.Task>().FindBy(x => x.EventId == EventId) on e.EventId equals t.EventId into ps
                            select new Event()
                            {
                                EventId = e.EventId,
                                EventName = e.EventName
                            }).ToList();
            return EvenList;
        }
        public ReturnBaseMessageModel VerifyTaskConfirmation(int eventValue = 0, int eventId = 0, int task1Id = 0)
        {

            switch (eventId)
            {


                case 30:
                    if (VerifiedBy(task1Id) == "")
                    {
                        //returnMessage = tellerService.VerifyAccount(eventValue);
                        VerifiedOn(task1Id, eventValue);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;




            }
            return returnMessage;


        }
        public ReturnBaseMessageModel RejectVoucher(int eventValue = 0, int eventId = 0, int task1Id = 0,string remarks="")
        {

            switch (eventId)
            {


                case 10:
                   
                        //returnMessage = tellerService.VerifyAccount(eventValue);
                        RejectedOn(task1Id, eventValue,remarks);
                    break;
                  




            }
            return returnMessage;


        }
        public void VerifiedOn(int task1id, int eventValue)
        {
            int userid = Loader.Models.Global.UserId;

            var task = uow.Repository<Models.Models.Task>().FindBy(x => x.Task1Id == task1id).FirstOrDefault();
            task.IsVerified = true;
            uow.Repository<Models.Models.Task>().Edit(task);

            TaskDetail tskm = new TaskDetail();
            var tsk = uow.Repository<TaskDetail>().FindBy(x => x.Task1Id == task1id && x.TaskTo == userid).FirstOrDefault();
            tskm = tsk;
            tskm.VerifiedOn = (DateTime)Loader.Models.Global.TransactionDate;
            uow.Repository<TaskDetail>().Edit(tskm);

            var voucherObj = uow.Repository<Models.Models.Voucher1>().GetSingle(x => x.V1Id == eventValue);

            var currentVNId = voucherObj.VNId;
            var voucherNumberObj = uow.Repository<Models.Models.VoucherNo>().GetSingle(x => x.VNId == currentVNId);
            voucherNumberObj.CurrentNo = voucherNumberObj.CurrentNo + 1;

            
            voucherObj.ApprovedBy = userid;
            //  voucherObj.ApprovedOn = (DateTime)Loader.Models.Global.TransactionDate;
            voucherObj.ApprovedOn = DateTime.Now;

            voucherObj.VNo = voucherNumberObj.CurrentNo;
            uow.Repository<Models.Models.Voucher1>().Edit(voucherObj);

          

            uow.Repository<Models.Models.VoucherNo>().Edit(voucherNumberObj);
            string query = " [acc].[PSetOpeningBal] "+eventValue+"";

            uow.executeProcedure(query);
            uow.Commit();

        }
        public void RejectedOn(int task1Id,int eventValue,string remarks)
        {

            int userid = Loader.Models.Global.UserId;
            var task = uow.Repository<Models.Models.Task>().FindBy(x => x.Task1Id == task1Id).FirstOrDefault();
            task.IsVerified = true;
            uow.Repository<Models.Models.Task>().Edit(task);

            TaskDetail tskm = new TaskDetail();
            var tsk = uow.Repository<TaskDetail>().FindBy(x => x.Task1Id == task1Id && x.TaskTo == userid).FirstOrDefault();
            tskm = tsk;
            tskm.VerifiedOn = (DateTime)Loader.Models.Global.TransactionDate;
            
            uow.Repository<TaskDetail>().Edit(tskm);

            var voucherObj = uow.Repository<Models.Models.Voucher1>().GetSingle(x => x.V1Id == eventValue);
            voucherObj.ApprovedBy = userid;
            voucherObj.ApprovedOn = (DateTime)Loader.Models.Global.TransactionDate;
            Models.Models.VoucherRejected voucherRejected = new VoucherRejected();
            voucherRejected.V1id = eventValue;
            voucherRejected.Remarks = remarks;

            voucherObj.VoucherRejecteds.Add(voucherRejected);

            uow.Repository<Models.Models.Voucher1>().Edit(voucherObj);
            uow.Commit();
        }

    }
}
