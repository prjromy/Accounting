using ChannakyaAccounting.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using Loader;

namespace ChannakyaAccounting.Controllers
{
    [MyAuthorize]
    public class FinSys2Controller : Controller
    {

        private Service.FinSys2.FinSys2Service FinSys2Service = null;

        public FinSys2Controller()
        {
            FinSys2Service = new Service.FinSys2.FinSys2Service();
        }


        public ActionResult Index()
        {
            if (Request.IsAjaxRequest()==true)
            {
                Models.ViewModel.TreeView tree = FinSys2Service.GetFinSys2GroupTree();
                ViewBag.treeview = tree;
                ViewBag.Address = FinSys2Service.GetAddress(0);
                ViewBag.ViewType = 1;
                ViewBag.ParentFinSys2Id = 0;
                ViewData["param"] = new ChannakyaAccounting.Models.ViewModel.TreeViewParam(false, true, true, 0, 0, "Available Node Group");

                return View(FinSys2Service.GetAllOfParent(0).ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(int parentId, bool allowSelectGroup, int ViewType = 1)
        {
            ViewBag.Address = FinSys2Service.GetAddress(parentId);
            ViewBag.ParentFinSys2Id = parentId;
            ViewBag.ViewType = ViewType;
            return PartialView(FinSys2Service.GetAllOfParent(parentId).ToList());
        }
        public ActionResult _GetType(int Pid)
        {
                ViewBag.TypeList = FinSys2Service.GetTypeList(Pid);
            
          
            return PartialView("_GetType");
        }
        public ActionResult Details(int id = 0)
        {
            FinSys2 finsys2 = FinSys2Service.GetSingle(id);
            if (finsys2 == null)
            {
                return HttpNotFound();
            }
            return View(finsys2);
        }

        [HttpGet]
        public ActionResult Create(string activeText, int activeId, int FinSys2Id = 0)
        {
            if (Request.IsAjaxRequest())
            {
                FinSys2 FinSys2DTO = new FinSys2();
                ViewBag.FinSys1List = FinSys2Service.GetFinSys1List();
               
                ViewBag.TypeList = FinSys2Service.GetTypeList(activeId);
                if (FinSys2Id != 0)
                {

                    FinSys2DTO = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinSys2>().GetSingle(x => x.F2id == FinSys2Id);
                    if (FinSys2DTO.Content != null)
                    {
                        ViewBag.Image = Convert.ToBase64String(FinSys2DTO.Content, Base64FormattingOptions.None);
                    }
                }
                else
                {
                    FinSys2DTO.Pid = activeId;
                }

                ViewBag.ActiveText = activeText;
                ViewData["param"] = new ChannakyaAccounting.Models.ViewModel.TreeViewParam(false, true, false, 0, 0, "Available FinSys2 Group");
                return PartialView(FinSys2DTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(FinSys2 FinSys2, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        FinSys2.Content = reader.ReadBytes(file.ContentLength);
                    }
                }
                try
                {
                    if (FinSys2.F2id == 0)
                    {
                        FinSys2Service.Save(FinSys2);
                        return RedirectToAction("Create", new { activeText = "Root", activeId = 0 });
                    }
                    else
                    {
                        FinSys2Service.Save(FinSys2);
                        var parentNode = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinSys2>().GetSingle(x => x.F2id == FinSys2.Pid);

                        return RedirectToAction("Create", new { activeText = parentNode.F2Desc, activeId = parentNode.Pid });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(FinSys2);

        }

        [HttpGet]
        public ActionResult _UpdateFinSys2Tree(int selectedNode, bool allowSelectGroupNode, bool withImageIcon, bool withCheckBox, int withOutMe = 0, int rootNode = -1)
        {
            ViewData["AllowSelectGroup"] = allowSelectGroupNode;
            ViewData["WithImageIcon"] = withImageIcon;
            ViewData["WithCheckBox"] = withCheckBox;
            ViewData["param"] = new ChannakyaAccounting.Models.ViewModel.TreeViewParam(false, true, true, 0, selectedNode, "Available FinSys2 Group");

             ChannakyaAccounting.Models.ViewModel.TreeView tree = new ChannakyaAccounting.Models.ViewModel.TreeView();
            if (rootNode < 0)
            {
                tree = FinSys2Service.GetFinSys2GroupTree(withOutMe);

            }
            //else
            //{
            //    tree = FinSys2Service.GetFinSys2GroupTree(rootNode, withOutMe);
            //}
            return PartialView("_AccountingTreeViewBody", tree);
        }




        [HttpPost]
        public ActionResult _GetFinSys2TreePopup(ChannakyaAccounting.Models.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ChannakyaAccounting.Models.ViewModel.TreeView tree = new ChannakyaAccounting.Models.ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = FinSys2Service.GetFinSys2GroupTree(filter);
            }
            else
            {
                tree = FinSys2Service.GetFinSys2GroupTree(param.WithOutMe, filter);
            }
            return PartialView("_AccountingTreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetFinSys2Tree(ChannakyaAccounting.Models.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ChannakyaAccounting.Models.ViewModel.TreeView tree = new ChannakyaAccounting.Models.ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = FinSys2Service.GetFinSys2GroupTree(filter);
            }
            else
            {
                tree = FinSys2Service.GetFinSys2GroupTree(param.WithOutMe, filter);
            }
            return PartialView("_AccountingTreeViewBody", tree);
        }

        public ActionResult Edit(int id = 0)
        {
            FinSys2 FinSys2 = FinSys2Service.GetSingle(id);
            if (FinSys2 == null)
            {
                return HttpNotFound();
            }
            return View(FinSys2);
        }


        [HttpPost]
        public ActionResult Edit(FinSys2 FinSys2)
        {
            if (ModelState.IsValid)
            {
                FinSys2Service.Save(FinSys2);

                return RedirectToAction("Index");
            }
            return View(FinSys2);
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            FinSys2 FinSys2 = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinSys2>().GetSingle(x => x.Pid == id);
            bool deleteConfirm = false;
            if (FinSys2 == null)
            {
                deleteConfirm = true;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(FinSys2);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int FinSys2Id)
        {
            try
            {
                FinSys2Service.Delete(FinSys2Id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return JavaScript(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult DisplayImage(HttpPostedFileBase imagefile)
        {
            using (var reader = new System.IO.BinaryReader(imagefile.InputStream))
            {
                byte[] ContentImage = reader.ReadBytes(imagefile.ContentLength);
                var ImageContent = Convert.ToBase64String(ContentImage, Base64FormattingOptions.None);
                return Json(ImageContent, JsonRequestBehavior.AllowGet);
            }


        }
     

    
}
}