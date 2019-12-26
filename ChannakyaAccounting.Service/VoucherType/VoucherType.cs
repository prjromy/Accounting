using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service. VoucherType
{
    public class  VoucherTypeService
    {
        private GenericUnitOfWork uow = null;


        public VoucherTypeService()
        {
            uow = new GenericUnitOfWork();
        }


        public List<Models.Models.VoucherType> GetAll()
        {
            return uow.Repository<Models.Models. VoucherType>().GetAll().ToList();
        }

        public Models.Models.VoucherType GetSingle(int  VoucherTypeId)
        {
            Models.Models.VoucherType VoucherType = uow.Repository<Models.Models.VoucherType>().GetSingle(c => c.VTypeID == VoucherTypeId);
            return VoucherType;
        }


        public void Save(Models.Models.VoucherType VoucherType)
        {
            try
            {
                if (VoucherType.VTypeID == 0)
                {
                    GenericUnitOfWork editUOW = new GenericUnitOfWork();
                    int checkExists = editUOW.Repository<Models.Models.VoucherType>().GetAll().Where(x => x.VoucherName == VoucherType.VoucherName).Count();
                    if (checkExists > 0)
                    {
                        throw new Exception("Duplicate VoucherType Found. VoucherType Caption Not Valid");
                    }
                    uow.Repository<Models.Models.VoucherType>().Add(VoucherType);
                }
                else
                {

                    uow.Repository<Models.Models.VoucherType>().Edit(VoucherType);
                }
                uow.Commit();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }



        public bool Delete(int VoucherTypeId)
        {
            Models.Models. VoucherType VoucherType = this.GetSingle( VoucherTypeId);

            if ( VoucherType != null)
            {
                uow.Repository<Models.Models. VoucherType>().Delete( VoucherType);
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
            var countExistsinVoucher1 = from m in uow.Repository<Models.Models.VoucherNo>().FindBy(x => x.VTypeId == id) join n in uow.Repository<Models.Models.Voucher1>().GetAll() on m.VNId equals n.VNId into totalCount select totalCount;
            var countExistsinVoucher1T = from m in uow.Repository<Models.Models.VoucherNo>().FindBy(x => x.VTypeId == id) join n in uow.Repository<Models.Models.Voucher1T>().GetAll() on m.VNId equals n.VNId into totalCount select totalCount;
            return (countExistsinVoucher1.Count() + countExistsinVoucher1T.Count());
        }

        public bool CheckExistingVoucherName(string VoucherName, int VTypeID)
        {
            var isExists = false;
            var voucher = uow.Repository<Models.Models.VoucherType>().FindBy(c => c.VoucherName.ToLower().Trim() == VoucherName.ToLower().Trim() && c.VTypeID!= VTypeID).Count();
            if (voucher > 0)
            {
                isExists = true;
            }
            return isExists;
        }

        public bool CheckExistingVoucherPrefix(string Prefix, int VTypeID)
        {
            var isExists = false;
            var voucher = uow.Repository<Models.Models.VoucherType>().FindBy(c => c.Prefix.ToLower().Trim() == Prefix.ToLower().Trim() && c.VTypeID != VTypeID).Count();
            if (voucher > 0)
            {
                isExists = true;
            }
            return isExists;
        }


        //public string GetAddress(int Models.Models. VoucherTypeId)
        //{
        //    string result = "";

        //    if (Models.Models. VoucherTypeId != 0)
        //    {
        //        Models.Models. VoucherType mnu = new Models.Models. VoucherType();
        //        mnu = GetSingle(Models.Models. VoucherTypeId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.Models.Models. VoucherTypeName);
        //            mnu = GetSingle(mnu.PModels.Models. VoucherTypeId);
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
