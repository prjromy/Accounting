using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Service.TaskVerification;
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
    public class TaskVerificationController : Controller
    {
        private TaskVerificationService taskVerification = null;

        ReturnBaseMessageModel returnMessage = null;


        public TaskVerificationController()
        {

            returnMessage = new ReturnBaseMessageModel();
            taskVerification = new TaskVerificationService();
        }
        public ActionResult VerifierList(int eventid = 0, bool ismultiVerify = false, int V1TId = 0,string narration="")
        {
            TaskVerificationList tModel = new TaskVerificationList();
            var task = taskVerification.VerifierList(eventid);
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

            var task = taskVerification.VerifierList(eventid);
            return PartialView("VerifierList", task);
        }
        public ActionResult SaveTaskDetails(int eventid = 0)
        {

            var task = taskVerification.VerifierList(eventid);
            return PartialView("VerifierList", task);
        }

        public ActionResult _MyNotifications()
        {
            TaskViewModel tasklistMod = new TaskViewModel();
            var tasklist = taskVerification.TaskList();
            tasklistMod.TaskDetailList = tasklist;
            return PartialView(tasklistMod);
        }
        public ActionResult _VerificationModal(int task1id)
        {
            TaskViewModel tsk = taskVerification.GetSingleTask(task1id);
            return PartialView(tsk);
        }

        public ActionResult _RejectModel(int task1id)
        {
            TaskViewModel tsk = taskVerification.GetSingleTask(task1id);
            return PartialView(tsk);
        }
        public ActionResult _EventRaisedDetails(int task1id)
        {
            TaskViewModel tsk = taskVerification.GetSingleTask(task1id);
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

                returnMessage = taskVerification.VerifyTaskConfirmation(eventValue, eventId, task1Id);
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

                returnMessage = taskVerification.RejectVoucher(eventValue, eventId, task1Id, remarks);
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
            var tasklist = taskVerification.ViewAllTasks("", 0, null, null, 1, 10);
            tasklistMod.TaskDetailWithIPageList = new StaticPagedList<TaskViewModel>(tasklist, 1, 10, (tasklist.Count == 0) ? 0 : tasklist.FirstOrDefault().TotalCount); ;
            return View(tasklistMod);
        }
        public ActionResult _ViewAllPendingTaskList(string employeeName, int? eventId, DateTime fromdate, DateTime todate, int pageNo = 1, int pageSize = 10)
        {
            TaskViewModel tasklistMod = new TaskViewModel();
            var tasklist = taskVerification.ViewAllTasks(employeeName, eventId, fromdate, todate, pageNo, pageSize);
            var TaskDetailWithIPageList = new StaticPagedList<TaskViewModel>(tasklist, pageNo, pageSize, (tasklist.Count == 0) ? 0 : tasklist.FirstOrDefault().TotalCount); ;
            return PartialView(TaskDetailWithIPageList);
        }


    }
}
