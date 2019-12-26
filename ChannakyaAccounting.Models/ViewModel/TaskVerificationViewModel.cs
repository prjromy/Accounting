using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class TaskVerificationList
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool IsMultiVerifier { get; set; }
        public bool IsSelected { get; set; }
        public string Message { get; set; }

        public int EventId { get; set; }
        public List<TaskVerificationList> VerifierList { get; set; }
       // public List<Task> TaskList { get; set; }
    }
    public class TaskViewModel
    {
        
        public int Task1Id { get; set; }
        public System.DateTime RaisedOn { get; set; }
        public Nullable<int> RaisedBy { get; set; }
        public int EventId { get; set; }
        public Nullable<Int64> EventValue { get; set; }
        public string Message { get; set; }
        public int TaskTo { get; set; }
        public Nullable<System.DateTime> VerifiedOn { get; set; }
        public Nullable<System.DateTime> SeenOn { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
      
        public int CountOfTask { get; set; }
        public bool IsVerified { get; set; }
        public IPagedList<TaskViewModel> TaskDetailWithIPageList { get; set; }
        public List<TaskViewModel> TaskDetailList { get; set; }
        public int TotalCount { get; set; }
        public string Verifier { get; set; }
    }
    public class TaskDetailsMOdel
    {
        public int Task2Id { get; set; }
        public int Task1Id { get; set; }
        public int TaskTo { get; set; }
        public DateTime VerifiedOn { get; set; }
        public virtual Task Task { get; set; }
    }
    
}
