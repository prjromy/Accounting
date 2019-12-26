using ChannakyaAccounting.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChannakyaAccounting.Service.FinAcc;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;

namespace ChannakyaAccounting.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {
        private readonly FinAccService finaccService;
        private static Random random = new Random(DateTime.Now.Millisecond);
        private static List<DataPoint> _dataPoints;
        private GenericUnitOfWork uow;

        // GET: Chart
        public ChartController()
        {
            uow = new GenericUnitOfWork();
            finaccService = new Service.FinAcc.FinAccService();
        }

       
        public ActionResult Index(int ledgerId=0,bool chartType=true)
        {
            int branchID = Loader.Models.Global.BranchId;
            int UserId = Loader.Models.Global.UserId;
            int pid = 0;
            var finaccObj = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == ledgerId);
            if(finaccObj!=null)
            {
                pid = (int)finaccObj.Pid;
            }
            ViewBag.IsPie = chartType;
            ViewBag.PId = pid;
            ViewBag.LedgerId = ledgerId;
            ViewBag.DataPoints = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis(ledgerId), _jsonSetting);
            return View();
        }

        public ActionResult CompareBarGraphIncomeExpenditure()
        {
          
            ViewBag.Points1 = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis((int)EnumValue.FinAccLedger.Income), _jsonSetting);
            ViewBag.Points2 = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis((int)EnumValue.FinAccLedger.Expenditure), _jsonSetting);

            return PartialView("CompareBarGraphIE");
        }
        public ActionResult CompareBarGraphLiabAssests()
        {
            
            ViewBag.Points1 = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis((int)EnumValue.FinAccLedger.Liabilites), _jsonSetting);
            ViewBag.Points2 = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis((int)EnumValue.FinAccLedger.Assests), _jsonSetting);

            
            return PartialView("CompareBarGraphLA");
        }
        public ActionResult CompareBarGraphDepLoan()
        {

            ViewBag.Points1 = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis((int)EnumValue.FinAccLedger.DepositProduct), _jsonSetting);
            ViewBag.Points2 = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis((int)EnumValue.FinAccLedger.LoanProduct), _jsonSetting);


            return PartialView("CompareBarGraphDepLoan");
        }
        //public ActionResult BarIndex()
        //{
        //    ViewBag.DataPoints = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis(10), _jsonSetting);
        //    return View();
        //}

        public List<DataPoint> GetRandomDataForCategoryAxis(int parentLedgerId=0)
        {
            _dataPoints = new List<DataPoint>();
            var allLedger = finaccService.GetAllOfParent(parentLedgerId).ToList();
            //double totalAmount = 0;
            //foreach (var item in allLedger)
            //{
            //    double individualAmnt = Convert.ToDouble(finaccService.GetTotalLedgerBalance(item.Fid));
            //    totalAmount = totalAmount + individualAmnt;
            //}

            foreach (var subitem in allLedger)
            {
                double amount = Convert.ToDouble(finaccService.GetTotalLedgerBalance(subitem.Fid));
                string label = subitem.Fname;
                //double rating = (double)Math.Floor((amount/totalAmount)*100);
                double rating = (double)amount ;
                if (Double.IsNaN(rating))
                {
                    rating = 0;
                }
                if (rating != 0)
                {
                    rating = Math.Abs(rating);
                    _dataPoints.Add(new DataPoint(rating, label, subitem.Fid));
                }
            }
           
            
            //double y = 50;
            //DateTime dateTime = new DateTime(2006, 01, 1, 0, 0, 0);
            //string label = "";




            //for (int i = 0; i < count; i++)
            //{
            //    y = y + (random.Next(0, 20) - 10);
            //    label = dateTime.ToString("dd MMM");

            //    _dataPoints.Add(new DataPoint(y, label));
            //    dateTime = dateTime.AddDays(1);
            //}

            return _dataPoints;
        }

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

    }
}