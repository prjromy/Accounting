using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ChannakyaAccounting.Models.Models;

namespace ChannakyaAccounting.Service.TaskVerificationAcc
{
    public static class TaskAccUtilityService
    {
        private static TaskVerificationService taskService = null;
        static TaskAccUtilityService()
        {
            taskService = new TaskVerificationService();
        }
        public static string GetTaskNameByEventId(int EventId = 0)
        {
            var taskname = taskService.GetEventByEventId(EventId).FirstOrDefault();
            return taskname.EventName.ToString();
        }
        public static string VerifiedBy(int task1Id = 0)
        {
            string verifier = taskService.VerifiedBy(task1Id).ToString();
            return verifier;
        }
        public static SelectList GetAllEventList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var eventList = uow.Repository<Event>().GetAll().ToList();
                return new SelectList(eventList, "EventId", "EventName");
            }
        }


        public static int TaskIdByEventIdAndEventValue(Int64 eventId, Int64 eventVAlue)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var taskId = uow.Repository<ChannakyaAccounting.Models.Models.Task>().FindBy(x => x.EventId == eventId && x.EventValue == eventVAlue).Select(x => x.Task1Id).FirstOrDefault();
                //if()
                return taskId;
            }
        }

    }
}
