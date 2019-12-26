using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.DimensionValue
{
    public class DimensionValueService
    {
        private GenericUnitOfWork uow = null;


        public DimensionValueService()
        {
            uow = new GenericUnitOfWork();
        }


        public List<Models.Models.DimensionValue> GetAll()
        {
            return uow.Repository<Models.Models.DimensionValue>().GetAll().ToList();
        }

        public Models.Models.DimensionValue GetSingle(int DimensionValueId)
        {
            Models.Models.DimensionValue DimensionValue = uow.Repository<Models.Models.DimensionValue>().GetSingle(c => c.DVId == DimensionValueId);
            return DimensionValue;
        }


        public void Save(Models.Models.DimensionValue DimensionValue)
        {
            try
            {
                if (DimensionValue.DVId == 0)
                {
                    GenericUnitOfWork editUOW = new GenericUnitOfWork();
                    int checkExists = editUOW.Repository<Models.Models.DimensionValue>().GetAll().Where(x => x.DimensionDefination == DimensionValue.DimensionDefination).Count();
                    if (checkExists > 0)
                    {
                        throw new Exception("Duplicate DimensionValue Found. DimensionValue Caption Not Valid");
                    }
                    uow.Repository<Models.Models.DimensionValue>().Add(DimensionValue);
                }
                else
                {

                    uow.Repository<Models.Models.DimensionValue>().Edit(DimensionValue);
                }
                uow.Commit();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<Models.ViewModel.DimensionValueListViewModel> GetDimensionValueList(int pageNo=1,int pageSize=10,string search="",int DDid=0)
        {
            List<Models.ViewModel.DimensionValueListViewModel> lstDimensionValue = new List<Models.ViewModel.DimensionValueListViewModel>();
            List<Models.ViewModel.DimensionValueListViewModel> lstDimensionDefination = new List<Models.ViewModel.DimensionValueListViewModel>();
            string query = "select * from acc.[FGetDimensionValueList](" + pageNo + "," + pageSize + ",'" + search + "',"+DDid+")";
            lstDimensionValue = uow.Repository<Models.ViewModel.DimensionValueListViewModel>().SqlQuery(query).ToList();
            return lstDimensionValue;
        }
        public bool Delete(int DimensionValueId)
        {
            Models.Models.DimensionValue DimensionValue = this.GetSingle(DimensionValueId);

            if (DimensionValue != null)
            {
                uow.Repository<Models.Models.DimensionValue>().Delete(DimensionValue);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }




        public int Ismapped(int DDID)
        {
            string query = "select count(DDID) from acc.DimensionVSLedger where ddid="+DDID+"";
             int Count= uow.GetContext().Database.SqlQuery<int>(query).ToArray()[0];    
             return Count;
        }
        public List<SelectListItem> GetDimensionDefination()
        {
            var dimList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().FindBy(x=>x.IsManual==0).ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in dimList)
            {
                lst.Add(new SelectListItem { Text = item.DefName, Value = item.DDId.ToString() });

            }
            return lst;
        }
    


        //public string GetAddress(int Models.Models.DimensionValueId)
        //{
        //    string result = "";

        //    if (Models.Models.DimensionValueId != 0)
        //    {
        //        Models.Models.DimensionValue mnu = new Models.Models.DimensionValue();
        //        mnu = GetSingle(Models.Models.DimensionValueId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.Models.Models.DimensionValueName);
        //            mnu = GetSingle(mnu.PModels.Models.DimensionValueId);
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
