using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ChannakyaAccounting.Service.Voucher1;
using MoreLinq;

namespace ChannakyaAccounting.Service.VoucherMap
{
    public class VoucherMapService
    {
        private readonly Voucher1Service voucher1Service;
        private GenericUnitOfWork uow = null;
        ReturnBaseMessageModel returnMessage = null;


        public VoucherMapService()
        {
            voucher1Service = new Service.Voucher1.Voucher1Service();
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
        }


        #region DropDownListData

        public List<SelectListItem> GetVoucherType()
        {
            var voucherList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.VoucherType>().GetAll().ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in voucherList)
            {
                lst.Add(new SelectListItem { Text = item.VoucherName, Value = item.VTypeID.ToString() });

            }
            return lst;
        }

        public List<SelectListItem> GetFiscalYear()
        {
            var fyearList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.FiscalYear>().GetAll().ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in fyearList)
            {
                lst.Add(new SelectListItem { Text = item.FyName, Value = item.FYID.ToString() });

            }
            return lst;
        }
        public List<SelectListItem> GetBatchName()
        {
            var batchList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.BatchDescription>().GetAll().ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in batchList)
            {
                lst.Add(new SelectListItem { Text = item.BatchName, Value = item.BId.ToString() });

            }
            return lst;
        }

        #endregion


        public List<Models.Models.VoucherNo> GetAll()
        {
            return uow.Repository<Models.Models.VoucherNo>().GetAll().ToList();
        }

        public Models.Models.VoucherNo GetSingle(int VoucherNoId)
        {
            Models.Models.VoucherNo VoucherNo = uow.Repository<Models.Models.VoucherNo>().GetSingle(c => c.VNId == VoucherNoId);
            return VoucherNo;
        }
        public List<Models.Models.VoucherNo> GetSingleUsertOVoucherMap(int id, int Vtype)
        {
            List<Models.Models.VoucherNo> userMappedVoucher = uow.Repository<Models.Models.VoucherNo>().GetAll().Where(x => x.BId == id).Where(x => x.VTypeId == Vtype).ToList();
            return userMappedVoucher;
        }

        public void Save(Models.Models.VoucherNo VoucherNo)
        {
            try
            {
                GenericUnitOfWork editUOW = new GenericUnitOfWork();
                int checkExists = editUOW.Repository<Models.Models.VoucherNo>().GetAll().Where(x => x.BId == VoucherNo.BId && x.VTypeId == VoucherNo.VTypeId && x.FYID == VoucherNo.FYID).Count();
                if (checkExists > 0)
                {
                    throw new Exception("Duplicate Voucher Mapping Found.");
                }
                if (VoucherNo.VNId == 0)
                {
                    VoucherNo.CompanyId = Loader.Models.Global.BranchId;
                    uow.Repository<Models.Models.VoucherNo>().Add(VoucherNo);
                }
                else
                {
                    VoucherNo.CompanyId = Loader.Models.Global.BranchId;
                    //var VoucherNoList = editUOW.Repository<Models.Models.VoucherNo>().GetSingle(x => x.VNId != VoucherNo.VNId);
                    uow.Repository<Models.Models.VoucherNo>().Edit(VoucherNo);
                }
                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<Models.ViewModel.VoucherNoViewModel> GetVoucherNoList(int pageNo = 1, int pageSize = 10, string Search = "")
        {
            List<Models.ViewModel.VoucherNoViewModel> lstVoucherNo = new List<Models.ViewModel.VoucherNoViewModel>();
            string query = "select * from [acc].[FGetVoucherNoList](" + pageNo + "," + pageSize + ",'" + Search + "')";
            lstVoucherNo = uow.Repository<Models.ViewModel.VoucherNoViewModel>().SqlQuery(query).ToList();
            return lstVoucherNo;
        }
        public bool Delete(int VoucherNoId)
        {
            Models.Models.VoucherNo VoucherNo = this.GetSingle(VoucherNoId);

            if (VoucherNo != null)
            {
                uow.Repository<Models.Models.VoucherNo>().Delete(VoucherNo);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }

        }
        #region mausham
        public List<SelectListItem> GetAllVoucher()
        {
            List<Models.Models.VoucherType> voucherList = uow.Repository<Models.Models.VoucherType>().GetAll().ToList();
            List<SelectListItem> vlist = new List<SelectListItem>();
            foreach (var item in voucherList)
            {
                vlist.Add(new SelectListItem { Text = item.VoucherName, Value = item.VTypeID.ToString() });
            }
            return vlist;
        }
        public Models.ViewModel.uservsvoucherViewModel BatchList()
        {
            List<Models.Models.BatchDescription> list = new List<Models.Models.BatchDescription>();
            list = uow.Repository<Models.Models.BatchDescription>().GetAll().ToList();
            Models.ViewModel.uservsvoucherViewModel listofvoucher = new Models.ViewModel.uservsvoucherViewModel();
            listofvoucher.BatchList = new List<Batch>();
            foreach (var item in list)
            {
                Models.ViewModel.Batch singleobject = new Batch();
                singleobject.Id = item.BId;
                singleobject.Name = item.BatchName;
                singleobject.IsSelected = false;
                listofvoucher.BatchList.Add(singleobject);
            }

            return listofvoucher;
        }

        public ReturnBaseMessageModel createUsertoVoucherMappng(uservsvoucherViewModel data)
        {
            try
            {
                List<Models.Models.UserVSVoucher> itemlist = uow.Repository<Models.Models.UserVSVoucher>().GetAll().Where(x => x.UserId == data.userid && x.VTypeId == data.Vtype).ToList();
                if (itemlist.Count > 0)
                {
                    foreach (var item in data.BatchList)
                    {
                        if (item.IsSelected == false)
                        {
                            var doesExists = uow.Repository<Models.Models.UserVSVoucher>().FindBy(x => x.UserId == data.userid && x.VTypeId == data.Vtype && x.BatchId == item.Id).FirstOrDefault();
                            if (doesExists != null)
                            {
                                uow.Repository<Models.Models.UserVSVoucher>().Delete(doesExists);
                            }
                        }
                        else
                        {
                            var doesExists = uow.Repository<Models.Models.UserVSVoucher>().FindBy(x => x.UserId == data.userid && x.VTypeId == data.Vtype && x.BatchId == item.Id).FirstOrDefault();
                            if (doesExists == null)
                            {
                                Models.Models.UserVSVoucher uvObj = new Models.Models.UserVSVoucher();
                                uvObj.VTypeId = data.Vtype;
                                uvObj.BatchId = item.Id;
                                uvObj.UserId = data.userid;
                                uow.Repository<Models.Models.UserVSVoucher>().Add(uvObj);
                            }
                        }
                    }
                    //to getbatchlist from Vtype

                }
                else
                {
                    foreach (var item in data.BatchList)
                    {
                        if (item.IsSelected == true)
                        {
                            Models.Models.UserVSVoucher singledata = new Models.Models.UserVSVoucher();
                            singledata.UserId = data.userid;
                            singledata.VTypeId = data.Vtype;
                            singledata.BatchId = item.Id;
                            uow.Repository<Models.Models.UserVSVoucher>().Add(singledata);
                        }

                    }
                }

                uow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = " User To Voucher Mapped Data is successfully Inserted";
                return returnMessage;

            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = " User To Voucher Mapped Data is not Inserted";
                return returnMessage;

            }
        }
        public usertovoucherreturnViewModel getAlltoVoucher()
        {
            usertovoucherreturnViewModel uservsvoucherlist = new usertovoucherreturnViewModel();
            uservsvoucherlist.voucherreturnViewModel = uow.Repository<Models.Models.UserVSVoucher>().GetAll().Select(x => new usertovoucherreturnViewModel
            {
                Batchtype = x.BatchId,
                userid = x.UserId,
                Vtype = x.VTypeId,
                ID = x.ID
            }).ToList();
            return uservsvoucherlist;
        }

        public uservsvoucherViewModel BatchListFromVtype(int Vtype, int UserID)
        {

            usertovoucherreturnViewModel voucherList = new usertovoucherreturnViewModel();
            voucherList.voucherreturnViewModel = uow.Repository<Models.Models.VoucherNo>().FindBy(x => x.VTypeId == Vtype).Select(x => new usertovoucherreturnViewModel
            {
                Batchtype = x.BId
            }).GroupBy(g => g.Batchtype).Select(x => x.FirstOrDefault()).ToList();
            Models.ViewModel.uservsvoucherViewModel listofvoucher = new Models.ViewModel.uservsvoucherViewModel();
            listofvoucher.BatchList = new List<Batch>();
            List<Models.Models.UserVSVoucher> prevousSelectedItem = new List<Models.Models.UserVSVoucher>();
            //  prevousSelectedItem = uow.Repository<Models.Models.UserVSVoucher>().SqlQuery(query).ToList();
            prevousSelectedItem = uow.Repository<Models.Models.UserVSVoucher>().GetAll().Where(x => x.UserId == UserID && x.VTypeId == Vtype).ToList();

            if (prevousSelectedItem.Count <= 0)
            {
                Models.Models.BatchDescription list = new Models.Models.BatchDescription();
                foreach (var item in voucherList.voucherreturnViewModel)
                {
                    string query1 = "select Distinct UV.BId,BatchName,Prefix from [acc].BatchDescription as UV inner join [acc].[VoucherNo] as VN on UV.BId=VN.BId where UV.BId=" + item.Batchtype + "";
                    //list = uow.Repository<Models.Models.BatchDescription>().FindBy(x => x.BId == item.Batchtype).SingleOrDefault();
                    list = uow.Repository<Models.Models.BatchDescription>().SqlQuery(query1).SingleOrDefault();
                    Models.ViewModel.Batch singleobject = new Batch();
                    singleobject.Id = item.Batchtype;
                    singleobject.IsSelected = false;
                    singleobject.Name = list.BatchName;
                    listofvoucher.BatchList.Add(singleobject);
                }
            }
            else
            {
                listofvoucher.userid = UserID;
                listofvoucher.Vtype = Vtype;
                foreach (var item in prevousSelectedItem)
                {
                    Batch singleItem = new Batch();
                    singleItem.Id = item.BatchId;
                    singleItem.IsSelected = true;
                    listofvoucher.BatchList.Add(singleItem);
                }

                foreach (var item in voucherList.voucherreturnViewModel)
                {
                    string query2 = "select Distinct UV.BId,BatchName,Prefix from [acc].BatchDescription as UV inner join [acc].[VoucherNo] as VN on UV.BId=VN.BId where UV.BId=" + item.Batchtype + "";
                    Models.Models.BatchDescription list = new Models.Models.BatchDescription();
                    //  list = uow.Repository<Models.Models.BatchDescription>().FindBy(x => x.BId == item.Batchtype).SingleOrDefault();
                    list = uow.Repository<Models.Models.BatchDescription>().SqlQuery(query2).SingleOrDefault();
                    Models.ViewModel.Batch singleobject = new Batch();
                    var PItem = listofvoucher.BatchList.FindAll(x => x.Id == item.Batchtype).ToList();

                    if (PItem.Count > 0)
                    {
                        listofvoucher.BatchList.Find(x => x.Id == item.Batchtype).Name = list.BatchName;
                    }
                    else
                    {
                        singleobject.Id = item.Batchtype;
                        singleobject.IsSelected = false;
                        singleobject.Name = list.BatchName;
                        listofvoucher.BatchList.Add(singleobject);
                    }
                }

            }

            return listofvoucher;

        }
        public Models.ViewModel.uservsvoucherViewModel GetEditItem(int uservsvoucherId, int vtype, int Fdate)
        {
            // List<usertovoucherreturnViewModel> GetEditItem = new List<usertovoucherreturnViewModel>();
            //GetEditItem = uow.Repository<Models.Models.UserVSVoucher>().GetAll().Where(x => x.UserId == uservsvoucherId).Where(x => x.VTypeId == vtype).Select(x => new usertovoucherreturnViewModel
            //{
            //    userid = x.UserId,
            //    Batchtype = x.BatchId,
            //    Vtype = x.VTypeId

            //}).ToList();


            //string query = "select distinct BatchId,UV.VTypeId,UserId,ID from [acc].[UserVSVoucher] as UV inner join [acc].[VoucherNo] as VN on UV.VTypeId=VN.VTypeId where" +
            //  " VN.VTypeId =" + vtype + " and VN.FYID=" + Fdate + " and UV.UserId=" + uservsvoucherId + "";

            //List<Models.Models.UserVSVoucher> prevousSelectedItem = new List<Models.Models.UserVSVoucher>();
            // prevousSelectedItem = uow.Repository<Models.Models.UserVSVoucher>().SqlQuery(query).ToList();
            List<Models.Models.UserVSVoucher> prevousSelectedItem = uow.Repository<Models.Models.UserVSVoucher>().FindBy(x => x.VTypeId == vtype && x.UserId == uservsvoucherId).ToList(); //optimized
            uservsvoucherViewModel editList = new uservsvoucherViewModel();
            editList.BatchList = new List<Batch>();
            editList.userid = uservsvoucherId;
            editList.Vtype = vtype;
            Models.Models.BatchDescription list = new Models.Models.BatchDescription();
            foreach (var item in prevousSelectedItem)
            {
                list = uow.Repository<Models.Models.BatchDescription>().FindBy(x => x.BId == item.BatchId).SingleOrDefault();
                Batch singleBatch = new Batch();
                singleBatch.IsSelected = true;
                singleBatch.Id = item.BatchId;
                singleBatch.Name = list.BatchName;
                editList.BatchList.Add(singleBatch);

            }

            usertovoucherreturnViewModel voucherList = new usertovoucherreturnViewModel();
            voucherList.voucherreturnViewModel = uow.Repository<Models.Models.VoucherNo>().FindBy(x => x.VTypeId == vtype).Select(x => new usertovoucherreturnViewModel
            {
                Batchtype = x.BId
            }).ToList();
            foreach (var item in voucherList.voucherreturnViewModel)
            {
                var IsExist = editList.BatchList.Find(x => x.Id == item.Batchtype);
                if (IsExist == null)
                {
                    list = uow.Repository<Models.Models.BatchDescription>().FindBy(x => x.BId == item.Batchtype).SingleOrDefault();
                    Batch singleitem = new Batch();
                    singleitem.Id = item.Batchtype;
                    singleitem.IsSelected = false;
                    singleitem.Name = list.BatchName;
                    editList.BatchList.Add(singleitem);
                }
            }
            return editList;

        }




        public bool DeleteUsertoVoucherMap(int userId, int Vtype)
        {


            //List<Models.Models.VoucherNo> VoucherNo = this.GetSingleUsertOVoucherMap(id, Vtype);
            //if (VoucherNo != null)
            //{
            List<Models.Models.UserVSVoucher> useritem = uow.Repository<Models.Models.UserVSVoucher>().FindBy(x => x.UserId == userId && x.VTypeId == Vtype).ToList();
            foreach (var item in useritem)
            {
                uow.Repository<Models.Models.UserVSVoucher>().Delete(item);
                uow.Commit();
            }

            return true;
            //}
            //else
            //{
            //    return false;
            //}

        }
        public bool CheckMapping(int id, int Vtype, int Btype)
        {
            //var Batchids = uow.Repository<Models.Models.UserVSVoucher>().GetAll().Where(x => x.UserId == id && x.BatchId == Btype && x.VTypeId == Vtype).ToList();
            //string query = @"select a.VNId,a.BId,a.CompanyId,a.CurrentNo,a.FYID,a.VTypeId from acc.VoucherNo a join acc.Voucher1 b on a.VNId=b.VNId where a.VTypeId=" + Vtype + "  and b.PostedBy=" + id + "";
            int count = 0;
            count = uow.Repository<Models.Models.UserVSVoucher>().FindBy(x => x.UserId == id && x.BatchId == Btype && x.VTypeId == Vtype).Count(); //optimized
            if (count > 0) //always let delete so
                return true;
            return true;
        }

        public Models.ViewModel.usertovoucherreturnViewModel GetUsersAndVoucherList()
        {

            usertovoucherreturnViewModel list = new usertovoucherreturnViewModel();
            var userVoucherList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.UserVSVoucher>().GetAll().Select(x => new usertovoucherreturnViewModel
            {
                ID = x.ID,
                userid = x.UserId,
                Vtype = x.VTypeId,
                Batchtype = x.BatchId,
                BatchList = Convert.ToString(
                   voucher1Service.GetBatchNamefordescription(x.BatchId))
            }).ToList();

            List<usertovoucherreturnViewModel> listNew = new List<usertovoucherreturnViewModel>();
            foreach (var item in userVoucherList)
            {
                var similarList = listNew.Where(x => x.userid == item.userid && x.Vtype == item.Vtype).FirstOrDefault();
                if (similarList != null)
                {
                    //listNew.Where(x => x.userid == item.userid && x.Vtype == item.Vtype).FirstOrDefault().BatchList = voucher1Service.GetBatchNamefordescription(similarList.Batchtype) +","+ voucher1Service.GetBatchNamefordescription(item.Batchtype);
                    listNew.Where(x => x.userid == item.userid && x.Vtype == item.Vtype).FirstOrDefault().BatchList = similarList.BatchList + "," + voucher1Service.GetBatchNamefordescription(item.Batchtype);

                }
                else
                {
                    item.BatchList = voucher1Service.GetBatchNamefordescription(item.Batchtype);
                    listNew.Add(item);
                }

            }

            list.voucherreturnViewModel = listNew;

            return list;
        }
        #endregion
    }
}
