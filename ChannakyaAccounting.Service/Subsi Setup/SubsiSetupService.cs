using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.Subsi_Setup
{
   public class SubsiSetupService
    {
        private GenericUnitOfWork uow = null;


        public SubsiSetupService()
        {
            uow = new GenericUnitOfWork();
        }


        public List<SelectListItem> GetSubsiTable()
        {
            var dimList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiTable>().GetAll().Where(x=>x.NeedMap==true && x.IsActive==true).ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in dimList)
            {
                lst.Add(new SelectListItem { Text = item.TableName, Value = item.STBId.ToString() });

            }
            return lst;
        }
        public List<Models.Models.SubsiSetup> GetAll()
        {
            return uow.Repository<Models.Models.SubsiSetup>().GetAll().ToList();
        }

        public Models.Models.SubsiSetup GetSingle(int SubsiSetupId)
        {
            Models.Models.SubsiSetup SubsiSetup = uow.Repository<Models.Models.SubsiSetup>().GetSingle(c => c.SSId == SubsiSetupId);
            return SubsiSetup;
        }


        public void Save(Models.Models.SubsiSetup SubsiSetup)
        {
            try
            {
                GenericUnitOfWork editUOW = new GenericUnitOfWork();
                int checkExists = editUOW.Repository<Models.Models.SubsiSetup>().GetAll().Where(x => x.SSId != SubsiSetup.SSId && x.Title == SubsiSetup.Title).Count();
                if (checkExists > 0)
                {
                    throw new Exception("Duplicate SubsiSetup Found. SubsiSetup Caption Not Valid");
                }
                if (SubsiSetup.SSId == 0)
                {
                    uow.Repository<Models.Models.SubsiSetup>().Add(SubsiSetup);
                }
                else
                {

                    uow.Repository<Models.Models.SubsiSetup>().Edit(SubsiSetup);
                }
                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<Models.ViewModel.SubsiSetUpViewModal> GetSubsiSetupList(int pageNo=1,int pageSize=10,string search="")
        {
            List<Models.ViewModel.SubsiSetUpViewModal> lstSubsiSetup = new List<Models.ViewModel.SubsiSetUpViewModal>();
            string query = "select * from acc.[FGetSubsiSetupList]("+pageNo+","+pageSize+",'"+search+"')";
            lstSubsiSetup = uow.Repository<Models.ViewModel.SubsiSetUpViewModal>().SqlQuery(query).ToList();
            return lstSubsiSetup;
        }
        public bool Delete(int SubsiSetupId)
        {
            Models.Models.SubsiSetup SubsiSetup = this.GetSingle(SubsiSetupId);

            if (SubsiSetup != null)
            {
                if(SubsiSetup.SubsiVSFIds == null)
                {
                    uow.Repository<Models.Models.SubsiSetup>().Delete(SubsiSetup);
                    uow.Commit();
                    return true;
                }
                else
                {
                    return false;
                }
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
        public bool CheckExistingSubsiTitle(string Title, int SSId)
        {
            var isExists = false;
            var subsiSetup = uow.Repository<Models.Models.SubsiSetup>().FindBy(c => c.Title.ToLower().Trim() == Title.ToLower().Trim() && c.SSId != SSId).Count();
            if (subsiSetup > 0)
            {
                isExists = true;
            }
            return isExists;
        }

        public bool CheckExistingSubsiPrefix(string Prefix, int SSId)
        {
            var isExists = false;
            var schm = uow.Repository<Models.Models.SubsiSetup>().FindBy(c => c.Prefix.ToLower().Trim() == Prefix.ToLower().Trim() && c.SSId != SSId).Count();
            if (schm > 0)
            {
                isExists = true;
            }
            return isExists;
        }

    }
}
