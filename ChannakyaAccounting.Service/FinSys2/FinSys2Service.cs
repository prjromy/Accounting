using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannakyaAccounting.Models.Models;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.FinSys2
{
    public class FinSys2Service
    {

        private GenericUnitOfWork uow = null;


        public FinSys2Service()
        {
            uow = new GenericUnitOfWork();
        }

        public List<Models.Models.FinSys2> GetAll()
        {
            return uow.Repository<Models.Models.FinSys2>().GetAll().ToList();
        }

        public List<Models.Models.FinSys2> GetAllOfParent(int parentId)
        {
            return uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == parentId).ToList();
        }

        public Models.Models.FinSys2 GetSingle(int FinSys2Id)
        {
            Models.Models.FinSys2 FinSys2 = uow.Repository<Models.Models.FinSys2>().GetSingle(c => c.F2id == FinSys2Id);
            return FinSys2;
        }

        public void Save(Models.Models.FinSys2 FinSys2)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            int checkExists = editUOW.Repository<Models.Models.FinSys2>().GetAll().Where(x => x.F2id != FinSys2.F2id && x.F2Desc == FinSys2.F2Desc && x.Pid == FinSys2.Pid).Count();
            if (checkExists > 0)
            {
                throw new Exception("Duplicate FinSys2 Found. FinSys2 Caption Not Valid");
            }
            if (FinSys2.F2id == 0)
            {
                uow.Repository<Models.Models.FinSys2>().Add(FinSys2);
            }
            else
            {
             
                uow.Repository<Models.Models.FinSys2>().Edit(FinSys2);
            }
            uow.Commit();
        }

        public bool Delete(int FinSys2Id)
        {
           
            Models.Models.FinSys2 FinSys2 = this.GetSingle(FinSys2Id);

            if (FinSys2 != null)
            {
                var checkFinacc = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == FinSys2Id).Count();
                if(checkFinacc>0)
                {
                    throw new Exception("Data in Ledger is Mapped.Please Delete all the Ledger First");
                   
                }
                else
                {
                    uow.Repository<Models.Models.FinSys2>().Delete(FinSys2);
                    uow.Commit();
                }
                //foreach (var item in checkFinacc)
                //{
                //    uow.Repository<Models.Models.FinAcc>().Delete(item);
                //}
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetAddress(int FinSys2Id)
        {
            string result = "";

            if (FinSys2Id != 0)
            {
                Models.Models.FinSys2 mnu = new Models.Models.FinSys2();
                mnu = GetSingle(FinSys2Id);

                List<string> lst = new List<string>();


                while (mnu != null)
                {
                    lst.Add(mnu.F2Desc);
                    mnu = GetSingle(Convert.ToInt32(mnu.Pid));
                };

                var sorted = lst.Select((x, i) => new KeyValuePair<string, int>(x, i)).OrderByDescending(x => x.Value).ToList();

                foreach (var item in sorted)
                {
                    if (result == "")
                    {
                        result = result + item.Key;
                    }
                    else
                    {
                        result = result + "/" + item.Key;
                    }

                }
            }
            else
            {
                result = "Root";
            }
            return result;
        }

        #region Tree

        private List<Models.Models.FinSys2> FilterTree(List<Models.Models.FinSys2> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.F2Desc.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.F2id equals selList.Pid
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.F2id equals c.F2id into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.F2id).ToList();
            }
            list = filteredList;
            return list;

        }
        public Models.ViewModel.TreeView GetFinSys2GroupTree(string filter = "")
        {
            List<Models.Models.FinSys2> list = new List<Models.Models.FinSys2>();
            list.Add(new Models.Models.FinSys2 { F1Type = 0, IsFixed = true, F2Desc = "Root", Pid = -1 });
            var treelist = uow.Repository<Models.Models.FinSys2>().GetAll();
            foreach (var item in treelist)
            {

                var finsysList = uow.Repository<Models.Models.FinSys1>().FindBy(x => x.IsGroup == true);
                foreach (var data in finsysList)
                {
                    if (item.F1id == data.F1id)
                    {
                        list.Add(item);
                    }

                }
            }
            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            Models.ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }

        public Models.ViewModel.TreeView GetFinSys2GroupTree(int FinSys2IdExpect, string filter = "")
        {
            List<Models.Models.FinSys2> list = uow.Repository<Models.Models.FinSys2>().GetAll().Where(x => x.FinSys1.IsGroup == true).Where(x => x.F2id != FinSys2IdExpect).ToList();
            list.Add(new Models.Models.FinSys2 { F1Type = 0, IsFixed = true, F2Desc = "Root", Pid = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            Models.ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }
        public List<SelectListItem> GetFinSys1List()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Models.Models.FinSys1>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.F1Des, Value = item.F1id.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetTypeList(int ledgerId)

        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<Models.Models.FinAcc> list;
            if (ledgerId == 0 || ledgerId == 1)
            {
                list = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Pid == 0).ToList();
            }
            else
            {
                var typeId = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == ledgerId).F1Type;
                list = uow.Repository<Models.Models.FinAcc>().GetAll().Where(x => x.Fid == typeId).ToList();
            }
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.Fname, Value = item.Fid.ToString() });
            }
            return lst;
        }

        //public ViewModel.TreeView GetFinSys2GroupTree(int parentId, int FinSys2IdExpect, string filter = "")
        //{
        //    List<FinSys2> list = uow.Repository<FinSys2>().GetAll().Where(x => x.IsGroup == true).Where(x => x.FinSys2Id != FinSys2IdExpect).ToList();

        //    if (filter.Trim() != "")
        //    {
        //        list = FilterTree(list, filter);
        //    }

        //    ViewModel.TreeView tree = this.GenerateTree(list, parentId);
        //    return tree;
        //}

        private Models.ViewModel.TreeView GenerateTree(List<Models.Models.FinSys2> list, int? parentFinSys2Id)
        {

            var parent = list.Where(x => x.Pid == parentFinSys2Id);
            Models.ViewModel.TreeView tree = new Models.ViewModel.TreeView();
            tree.Title = "FinSys2";
            foreach (var itm in parent)
            {
                tree.AccChildren.Add(new Models.ViewModel.AccountingTreeDTO
                {
                    Id = itm.F2id,
                    PId = itm.Pid,
                    Text = itm.F2Desc,
                    //IsGroup = itm.FinSys1.IsGroup,
                    Image = itm.Content

                });
            }

            foreach (var itm in tree.AccChildren)
            {
                itm.AccChildren = GenerateTree(list, itm.Id).AccChildren.ToList();
            }
            return tree;
        }
        #endregion

    }
}
