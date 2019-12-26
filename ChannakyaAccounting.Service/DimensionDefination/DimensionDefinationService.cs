using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.DimensionDefination
{
    public class DimensionDefinationService
    {
        private GenericUnitOfWork uow = null;


        public DimensionDefinationService()
        {
            uow = new GenericUnitOfWork();
        }


        public List<SelectListItem> GetDimensionTable()
        {
            var dimList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionTable>().GetAll().ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in dimList)
            {
                lst.Add(new SelectListItem { Text = item.TableName, Value = item.Tid.ToString() });

            }
            return lst;
        }
        public List<Models.Models.DimensionDefination> GetAll()
        {
            return uow.Repository<Models.Models.DimensionDefination>().GetAll().ToList();
        }

        public Models.Models.DimensionDefination GetSingle(int DimensionDefinationId)
        {
            Models.Models.DimensionDefination DimensionDefination = uow.Repository<Models.Models.DimensionDefination>().GetSingle(c => c.DDId == DimensionDefinationId);
            return DimensionDefination;
        }


        public void Save(Models.Models.DimensionDefination DimensionDefination)
        {
            try
            {
                GenericUnitOfWork editUOW = new GenericUnitOfWork();
                int checkExists = editUOW.Repository<Models.Models.DimensionDefination>().GetAll().Where(x => x.DDId != DimensionDefination.DDId && x.DefName == DimensionDefination.DefName).Count();
                System.Diagnostics.Debugger.NotifyOfCrossThreadDependency();
                var dimensionDefinationAll = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().GetAll().OrderBy(x => x.DDId).ToList();
                string dimensionNameToLower = DimensionDefination.DefName.ToLower();
                var caseSensitiveCheck = /*dimensionDefinationAll.Where(x => x.DefName.ToLower()==dimensionNameToLower);*/dimensionDefinationAll.Where(x => string.Compare(x.DefName.ToLower(), dimensionNameToLower, true) == 0).Count();
                if (checkExists > 0 || caseSensitiveCheck>0)
                {
                    throw new Exception("Duplicate DimensionDefination Found. DimensionDefination Caption Not Valid");
                }
                if (DimensionDefination.DDId == 0)
                {
                    uow.Repository<Models.Models.DimensionDefination>().Add(DimensionDefination);
                }
                else
                {

                    uow.Repository<Models.Models.DimensionDefination>().Edit(DimensionDefination);
                }
                uow.Commit();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }




        public List<Models.Models.DimensionDefination> GetDimensionDefinationList()
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            List<Models.Models.DimensionDefination> lstDimensionDefination = new List<Models.Models.DimensionDefination>();
            lstDimensionDefination = editUOW.Repository<Models.Models.DimensionDefination>().GetAll().ToList();
            //ViewBag.FirstIndexDimensionValue=lstDimensionDefination[0].DefName;
            return lstDimensionDefination;
        }
        public List<Models.ViewModel.DimensionDefinationListViewModel> GetDimensionDefinationList(int pageNo, int pageSize, string search = "")
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            List<Models.ViewModel.DimensionDefinationListViewModel> lstDimensionDefination = new List<Models.ViewModel.DimensionDefinationListViewModel>();
            string query = "select * from acc.FGetDimensionDefinationList(" + pageNo + "," + pageSize + ",'" + search + "')";
            lstDimensionDefination = editUOW.Repository<Models.ViewModel.DimensionDefinationListViewModel>().SqlQuery(query).ToList();
            //ViewBag.FirstIndexDimensionValue=lstDimensionDefination[0].DefName;
            return lstDimensionDefination;
        }

        public bool Delete(int DimensionDefinationId)
        {
            Models.Models.DimensionDefination DimensionDefination = this.GetSingle(DimensionDefinationId);

            if (DimensionDefination != null)
            {
                uow.Repository<Models.Models.DimensionDefination>().Delete(DimensionDefination);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
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

        //public string GetAddress(int Models.Models.DimensionDefinationId)
        //{
        //    string result = "";

        //    if (Models.Models.DimensionDefinationId != 0)
        //    {
        //        Models.Models.DimensionDefination mnu = new Models.Models.DimensionDefination();
        //        mnu = GetSingle(Models.Models.DimensionDefinationId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.Models.Models.DimensionDefinationName);
        //            mnu = GetSingle(mnu.PModels.Models.DimensionDefinationId);
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
