using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Service.TaskVerificationAcc;
using DataRepository = ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using LoaderRepository = Loader.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loader;

namespace ChannakyaAccounting.Controllers
{


    [MyAuthorize]
    public class TaskVerificationAccController : Controller
    {
        private TaskVerificationService TaskVerificationAcc = null;

        ReturnBaseMessageModel returnMessage = null;


        public TaskVerificationAccController()
        {

            returnMessage = new ReturnBaseMessageModel();
            TaskVerificationAcc = new TaskVerificationService();
        }
        public ActionResult VerifierList(int eventid = 0, bool ismultiVerify = false, int V1TId = 0,string narration="")
        {
            TaskVerificationList tModel = new TaskVerificationList();
            var task = TaskVerificationAcc.VerifierList(eventid);
            tModel.VerifierList = task;
            tModel.VoucherTempId = V1TId;
            tModel.EventId = eventid;
            tModel.Narration = narration;
            ViewBag.IsStrictlyVerified = Convert.ToBoolean(new LoaderRepository.GenericUnitOfWork().Repository<Loader.Models.ParamValue>().GetSingle(x => x.PId == (int)ChannakyaAccounting.EnumValue.Parameter.IsStrictlyVerified).PValue);
            if (ismultiVerify == true)
            {
                tModel.IsMultiVerifier = ismultiVerify;
            }
            return PartialView(tModel);
        }
        public ActionResult _VerifierDetails(int eventid = 0)
        {

            var task = TaskVerificationAcc.VerifierList(eventid);
            return PartialView("VerifierList", task);
        }
        public ActionResult SaveTaskDetails(int eventid = 0)
        {

            var task = TaskVerificationAcc.VerifierList(eventid);
            return PartialView("VerifierList", task);
        }

        public ActionResult _MyNotifications()
        {
            TaskViewModel tasklistMod = new TaskViewModel();
            var tasklist = TaskVerificationAcc.TaskList();
            tasklistMod.TaskDetailList = tasklist;
            return PartialView(tasklistMod);
        }
        public ActionResult _VerificationModal(int task1id)
        {
            TaskViewModel tsk = TaskVerificationAcc.GetSingleTask(task1id);
            return PartialView(tsk);
        }

        public ActionResult _RejectModel(int task1id)
        {
            TaskViewModel tsk = TaskVerificationAcc.GetSingleTask(task1id);
            return PartialView(tsk);
        }
        public ActionResult _EventRaisedDetails(int task1id)
        {
            TaskViewModel tsk = TaskVerificationAcc.GetSingleTask(task1id);
            return PartialView(tsk);
        }

        public JsonResult IsStrictlyVerified()
        {
            bool isStrictlyVerified = Convert.ToBoolean(new LoaderRepository.GenericUnitOfWork().Repository<Loader.Models.ParamValue>().GetSingle(x => x.PId ==(int)ChannakyaAccounting.EnumValue.Parameter.IsStrictlyVerified).PValue);
            return Json(isStrictlyVerified, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult VerificationConfirm(int eventValue = 0, int eventId = 0, int task1Id = 0)
        {
            try
            {

                returnMessage = TaskVerificationAcc.VerifyTaskConfirmation(eventValue, eventId, task1Id);
                returnMessage.Msg = "Success";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public ActionResult RejectConfirm(int eventValue = 0, int eventId = 0, int task1Id = 0, string remarks = "")
        {
            try
            {

                returnMessage = TaskVerificationAcc.RejectVoucher(eventValue, eventId, task1Id, remarks);
                returnMessage.Msg = "Success";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult _ViewAllPendingTaskIndex()
        {
            TaskViewModel tasklistMod = new TaskViewModel();
            var tasklist = TaskVerificationAcc.ViewAllTasks("", 0, null, null, 1, 10);
            tasklistMod.TaskDetailWithIPageList = new StaticPagedList<TaskViewModel>(tasklist, 1, 10, (tasklist.Count == 0) ? 0 : tasklist.FirstOrDefault().TotalCount); ;
            return View(tasklistMod);
        }
        public ActionResult _ViewAllPendingTaskList(string employeeName, int? eventId, DateTime ?fromdate=null, DateTime ?todate=null, int pageNo = 1, int pageSize = 10)
        {
            TaskViewModel tasklistMod = new TaskViewModel();
            var tasklist = TaskVerificationAcc.ViewAllTasks(employeeName, eventId, fromdate, todate, pageNo, pageSize);
            var TaskDetailWithIPageList = new StaticPagedList<TaskViewModel>(tasklist, pageNo, pageSize, (tasklist.Count == 0) ? 0 : tasklist.FirstOrDefault().TotalCount); ;
            return PartialView(TaskDetailWithIPageList);
        }


    }
}
