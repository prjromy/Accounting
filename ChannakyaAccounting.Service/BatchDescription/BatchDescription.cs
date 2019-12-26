using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.BatchDescription
{
    public class  BatchDescriptionService
    {
        private GenericUnitOfWork uow = null;


        public BatchDescriptionService()
        {
            uow = new GenericUnitOfWork();
        }


        public List<Models.Models.BatchDescription> GetAll()
        {
            return uow.Repository<Models.Models. BatchDescription>().GetAll().ToList();
        }

        public Models.Models. BatchDescription GetSingle(int  BatchDescriptionId)
        {
            Models.Models. BatchDescription BatchDescription = uow.Repository<Models.Models. BatchDescription>().GetSingle(c => c.BId == BatchDescriptionId);
            return BatchDescription;
        }


        public void Save(Models.Models. BatchDescription BatchDescription)
        {

            if ( BatchDescription.BId == 0)
            {
                GenericUnitOfWork editUOW = new GenericUnitOfWork();
                int checkExists = editUOW.Repository<Models.Models. BatchDescription>().GetAll().Where(x => x.BatchName == BatchDescription.BatchName).Count();
                if (checkExists > 0)
                {
                    throw new Exception("Duplicate BatchDescription Found. BatchDescription Caption Not Valid");
                }
                uow.Repository<Models.Models.BatchDescription>().Add( BatchDescription);
            }
            else
            {

                uow.Repository<Models.Models.BatchDescription>().Edit( BatchDescription);
            }
            uow.Commit();
        }



        public bool Delete(int BatchDescriptionId)
        {
            Models.Models. BatchDescription BatchDescription = this.GetSingle( BatchDescriptionId);

            if ( BatchDescription != null)
            {
                uow.Repository<Models.Models. BatchDescription>().Delete( BatchDescription);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<SelectListItem> GetDimensionDefination()
        {
            var dimList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.DimensionDefination>().FindBy(x => x.IsManual == 0).ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in dimList)
            {
                lst.Add(new SelectListItem { Text = item.DefName, Value = item.DDId.ToString() });

            }
            return lst;
        }

        public int IfExistsinVoucher(int id)
        {
            var countExistsinVoucher1 = from m in uow.Repository<Models.Models.VoucherNo>().FindBy(x => x.BId == id) join n in uow.Repository<Models.Models.Voucher1>().GetAll() on m.VNId equals n.VNId into totalCount select totalCount;
            var countExistsinVoucher1T = from m in uow.Repository<Models.Models.VoucherNo>().FindBy(x => x.BId == id) join n in uow.Repository<Models.Models.Voucher1T>().GetAll() on m.VNId equals n.VNId into totalCount select totalCount;
            return (countExistsinVoucher1.Count() + countExistsinVoucher1T.Count());
        }


        //public string GetAddress(int Models.Models. BatchDescriptionId)
        //{
        //    string result = "";

        //    if (Models.Models. BatchDescriptionId != 0)
        //    {
        //        Models.Models. BatchDescription mnu = new Models.Models. BatchDescription();
        //        mnu = GetSingle(Models.Models. BatchDescriptionId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.Models.Models. BatchDescriptionName);
        //            mnu = GetSingle(mnu.PModels.Models. BatchDescriptionId);
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
        public bool CheckExistingVoucherName(string BatchName, int BId)
        {
            var isExists = false;
            var batch = uow.Repository<Models.Models.BatchDescription>().FindBy(c => c.BatchName.ToLower().Trim() == BatchName.ToLower().Trim() && c.BId != BId).Count();
            if (batch > 0)
            {
                isExists = true;
            }
            return isExists;
        }
        public bool CheckExistingVoucherPrefix(string Prefix, int BId)
        {
            var isExists = false;
            var prefix = uow.Repository<Models.Models.BatchDescription>().FindBy(c => c.Prefix.ToLower().Trim() == Prefix.ToLower().Trim() && c.BId != BId).Count();
            if (prefix > 0)
            {
                isExists = true;
            }
            return isExists;
        }
    }
}
