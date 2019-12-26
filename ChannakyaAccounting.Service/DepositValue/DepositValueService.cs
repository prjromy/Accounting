using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.Duration
{
    public class DurationService
    {
        private GenericUnitOfWork uow = null;


        public DurationService()
        {
            uow = new GenericUnitOfWork();
        }


        public List<Models.Models.Duration> GetAll()
        {
            return uow.Repository<Models.Models.Duration>().GetAll().ToList();
        }

        public Models.Models.Duration GetSingle(int DurationId)
        {
            Models.Models.Duration Duration = uow.Repository<Models.Models.Duration>().GetSingle(c => c.Id == DurationId);
            return Duration;
        }


        public void Save(Models.Models.Duration Duration)
        {
            try
            {
                if (Duration.Id == 0)
                {
                    GenericUnitOfWork editUOW = new GenericUnitOfWork();
                    //int checkExists = editUOW.Repository<Models.Models.Duration>().GetAll().Where(x => x.DimensionDefination == Duration.DimensionDefination).Count();
                    //if (checkExists > 0)
                    //{
                    //    throw new Exception("Duplicate Duration Found. Duration Caption Not Valid");
                    //}
                    uow.Repository<Models.Models.Duration>().Add(Duration);
                }
                else
                {

                    uow.Repository<Models.Models.Duration>().Edit(Duration);
                }
                uow.Commit();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }



        public List<Models.ViewModel.DurationAccViewModel> GetDurationList(int pageNo=1,int pageSize=10,string search="")
        {
            List<Models.ViewModel.DurationAccViewModel> lstDuration = new List<Models.ViewModel.DurationAccViewModel>();
            string query = "select * from acc.fgetdurationlist("+pageNo+","+pageSize+",'"+search+"')";
            lstDuration = uow.Repository<Models.ViewModel.DurationAccViewModel>().SqlQuery(query).ToList();
            return lstDuration;
        }
        public bool Delete(int DurationId)
        {
            Models.Models.Duration Duration = this.GetSingle(DurationId);

            if (Duration != null)
            {
                uow.Repository<Models.Models.Duration>().Delete(Duration);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }




    }
}
