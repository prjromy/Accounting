using ChannakyaAccounting.Models.Models;
using ChannakyaAccounting.Models.ViewModel;
using ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ChannakyaAccounting.Service.DepositScheme
{
    public class DepositSchemeService
    {
        private GenericUnitOfWork uow = null;
        ReturnBaseMessageModel returnMessage = null;


        public DepositSchemeService()
        {
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();

        }


        public List<Models.Models.SchmDetail> GetAll()
        {
            return uow.Repository<Models.Models.SchmDetail>().GetAll().ToList();
        }

        public Models.Models.SchmDetail GetSingle(int SchmDetailId)
        {
            Models.Models.SchmDetail SchmDetail = uow.Repository<Models.Models.SchmDetail>().FindBy(c => c.SDID == SchmDetailId).SingleOrDefault();
            return SchmDetail;
        }

        public bool GetAlias(string Alias,int? schemeId)
        {
            bool isExists = false;

            if(schemeId != 0)
            {
                var schm2 = uow.Repository<Models.Models.SchmDetail>().FindBy(c => c.SDID == schemeId).SingleOrDefault();
                if(Alias.ToLower().Trim() == schm2.FinAcc.Alias.ToLower().Trim())
                {
                    isExists = false;
                }
                else
                {
                    var schm = uow.Repository<Models.Models.SchmDetail>().FindBy(c => c.FinAcc.Alias.ToLower().Trim() == Alias.ToLower().Trim()).SingleOrDefault();
                    if (schm != null)
                    {
                        isExists = true;
                    }
                }
            }
            else
            {
                var schm = uow.Repository<Models.Models.SchmDetail>().FindBy(c => c.FinAcc.Alias.ToLower().Trim() == Alias.ToLower().Trim()).SingleOrDefault();
                if (schm != null)
                {
                    isExists = true;
                }
            }
            return isExists;
        }

        public bool CheckSchemenName(string SDName,int SDID)
        {
            var isExists = false;
           var schm = uow.Repository<Models.Models.SchmDetail>().FindBy(c => c.SDName.ToLower().Trim() == SDName.ToLower().Trim() && c.SDID != SDID).Count();
            if(schm > 0)
            {
                isExists = true;
            }
            return isExists;
        }

        public string LedgerName(int F1Id)
        {
            string ledgerName = "";
            if (F1Id == 12 || F1Id == 13)
            {
                ledgerName = "Deposit Scheme";
            }

            if (F1Id == 124)
            {
                ledgerName = "Loan Scheme";
            }
            if (F1Id == 16 || F1Id == 17 || F1Id == 25)
            {
                ledgerName = "Int. Exp";
            }
            if (F1Id == 14)
            {
                ledgerName = "Deposit Product";
            }
            if (F1Id == 125)
            {
                ledgerName = "Loan Product";
            }
            if (F1Id == 18 || F1Id == 19 || F1Id == 24)
            {
                ledgerName = "Int. Payable";
            }
            if (F1Id == 20 || F1Id == 21 || F1Id == 26)
            {
                ledgerName = "Int. Acc";
            }
            if (F1Id == 22 || F1Id == 23 || F1Id == 27)
            {
                ledgerName = "Tax On Int.";
            }
            if (F1Id == 127 || F1Id == 240)
            {
                ledgerName = "Int. Rec";
            }
            if (F1Id == 128 || F1Id == 241)
            {
                ledgerName = "Pen. Rec";
            }
            if (F1Id == 129 || F1Id == 242)
            {
                ledgerName = "Int. Suspense";
            }
            if (F1Id == 130 || F1Id == 243)
            {
                ledgerName = "Pen. Suspense";
            }
            if (F1Id == 131 || F1Id == 244)
            {
                ledgerName = "Int. Income";
            }
            if (F1Id == 132 || F1Id == 245)
            {
                ledgerName = "Pen. Income";
            }

            if (F1Id == 133 || F1Id == 246)
            {
                ledgerName = "Int. Rebate";
            }
            if (F1Id == 134 || F1Id == 247)
            {
                ledgerName = "Pen. Rebate";
            }
            if (F1Id == 135 || F1Id == 248)
            {
                ledgerName = "Other Loan";
            }
            if (F1Id == 136 || F1Id == 249)
            {
                ledgerName = "Other Int. Income";
            }
            return ledgerName;
        }
        public void Save(Models.Models.SchmDetail schemeData, string alias)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();

            try
            {
                #region Create
                if (schemeData.SDID == 0)
                {
                    int checkExists = editUOW.Repository<Models.Models.SchmDetail>().GetAll().Where(x => x.SDID == schemeData.SDID || x.SDID == schemeData.SDID).Count();
                    if (checkExists > 0)
                    {
                        throw new Exception("Duplicate Models.Models.SchmDetail Found. Models.Models.SchmDetail Caption Not Valid");
                    }

                    #region Deposit Create
                    if (schemeData.SType == 0)
                    {
                        var parentId1 = schemeData.F2Type; // F2TYpe = FId ( ex 255) 
                        var F2Id = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == parentId1).F2Type; // ex : 24
                        var F1Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == F2Id).F1id; // Finsys1 f1id ex 13/12

                        if (F1Id == 13)
                        {
                            List<int> localschemeList = new List<int>();
                            localschemeList.Add(13);
                            localschemeList.Add(16);
                            localschemeList.Add(18);
                            localschemeList.Add(20);
                            localschemeList.Add(22);

                            int count = 0;

                            //itemtype = f1id
                            foreach (var itemType in localschemeList)
                            {
                                var pidForF2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == itemType).Select(x => x.F2id).FirstOrDefault(); //yesmai aauxa sir le vannu vako
                                var parentId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == pidForF2Id).Select(x => x.Fid).FirstOrDefault();

                                var f2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == pidForF2Id).Select(x => x.F2id).FirstOrDefault();

                                count++;
                                Models.Models.FinAcc finaccLiab = new Models.Models.FinAcc();

                                finaccLiab.Fname = schemeData.SDName + "(" + LedgerName(itemType) + ")";
                                finaccLiab.Pid = parentId; //ok
                                finaccLiab.Alias = alias;  //ok
                                if (count == 1)
                                {
                                    finaccLiab.F2Type = f2Id;  // Group Node 1 vako ids tannu

                                    Models.Models.SchmDetail schmDetail = new Models.Models.SchmDetail();

                                    schmDetail.SDName = schemeData.SDName;
                                    schmDetail.HasCard = schemeData.HasCard;
                                    schmDetail.HasCertificate = schemeData.HasCertificate;
                                    schmDetail.HasCheque = schemeData.HasCheque;
                                    schmDetail.HasDormancy = schemeData.HasDormancy;
                                    schmDetail.HasDuration = schemeData.HasDuration;
                                    schmDetail.HasIndivLimit = schemeData.HasIndivLimit;
                                    schmDetail.HasIndivRate = schemeData.HasIndivRate;
                                    schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                    schmDetail.HASSMS = schemeData.HASSMS;
                                    schmDetail.ICBID = schemeData.ICBID;
                                    schmDetail.IsInsured = schemeData.IsInsured;
                                    schmDetail.MID = schemeData.MID;
                                    schmDetail.Nomianable = schemeData.Nomianable;
                                    schmDetail.RuleMovement = schemeData.RuleMovement;
                                    schmDetail.RuleICB = schemeData.RuleICB;
                                    schmDetail.SEnable = true;
                                    schmDetail.SType = 0;
                                    schmDetail.Withdrawal = schemeData.Withdrawal;
                                    schmDetail.Revolving = schemeData.Revolving;
                                    schmDetail.IsRenewable = schemeData.IsRenewable;
                                    schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                    schmDetail.HasDuration = schemeData.HasDuration;
                                    schmDetail.MultiDeposit = schemeData.MultiDeposit;
                                    schmDetail.IsFDLoan = schemeData.IsFDLoan;
                                    finaccLiab.SchmDetails.Add(schmDetail);
                                }
                                else
                                {
                                    finaccLiab.F2Type = pidForF2Id;  // Group Node 1 vako ids tannu

                                }
                                uow.Repository<Models.Models.FinAcc>().Add(finaccLiab);
                                uow.Commit();
                            }
                        }
                        else
                        {
                            List<int> foreignschemeList = new List<int>();
                            foreignschemeList.Add(12);
                            foreignschemeList.Add(17);
                            foreignschemeList.Add(19);
                            foreignschemeList.Add(21);
                            foreignschemeList.Add(23);

                            int count = 0;
                            foreach (var itemType in foreignschemeList)
                            {

                                var pidForF2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == itemType).Select(x => x.F2id).FirstOrDefault();
                                var parentId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == pidForF2Id).Select(x => x.Fid).FirstOrDefault();

                                var f2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == pidForF2Id).Select(x => x.F2id).FirstOrDefault();


                                //var f2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == itemType).Select(x => x.F2id).FirstOrDefault();
                                //var parentId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == f2Id).Select(x => x.Fid).FirstOrDefault();

                                count++;
                                Models.Models.FinAcc finaccLiab = new Models.Models.FinAcc();
                                var f2Type = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == schemeData.F2Type).F2Type;

                                finaccLiab.Fname = schemeData.SDName + "(" + LedgerName(itemType) + ")";
                                finaccLiab.Pid = parentId;
                                finaccLiab.Alias = alias;

                                if (count == 1)
                                {
                                    Models.Models.SchmDetail schmDetail = new Models.Models.SchmDetail();
                                    finaccLiab.F2Type = f2Id;
                                    schmDetail.SDName = schemeData.SDName;
                                    schmDetail.HasCard = schemeData.HasCard;
                                    schmDetail.HasCertificate = schemeData.HasCertificate;
                                    schmDetail.HasCheque = schemeData.HasCheque;
                                    schmDetail.HasDormancy = schemeData.HasDormancy;
                                    schmDetail.HasDuration = schemeData.HasDuration;
                                    schmDetail.HasIndivLimit = schemeData.HasIndivLimit;
                                    schmDetail.HasIndivRate = schemeData.HasIndivRate;
                                    schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                    schmDetail.HASSMS = schemeData.HASSMS;
                                    schmDetail.ICBID = schemeData.ICBID;
                                    schmDetail.IsInsured = schemeData.IsInsured;
                                    schmDetail.MID = schemeData.MID;
                                    schmDetail.Nomianable = schemeData.Nomianable;
                                    schmDetail.RuleMovement = schemeData.RuleMovement;
                                    schmDetail.RuleICB = schemeData.RuleICB;
                                    schmDetail.SEnable = true;
                                    schmDetail.SType = 0;
                                    schmDetail.Withdrawal = schemeData.Withdrawal;
                                    schmDetail.Revolving = schemeData.Revolving;
                                    schmDetail.IsRenewable = schemeData.IsRenewable;
                                    schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                    schmDetail.HasDuration = schemeData.HasDuration;
                                    schmDetail.MultiDeposit = schemeData.MultiDeposit;
                                    schmDetail.IsFDLoan = schemeData.IsFDLoan;
                                    finaccLiab.SchmDetails.Add(schmDetail);

                                }
                                else
                                {
                                    finaccLiab.F2Type = pidForF2Id;

                                }
                                uow.Repository<Models.Models.FinAcc>().Add(finaccLiab);
                                uow.Commit();
                            }
                        }

                    }
                    #endregion

                    #region Loan Create
                    if (schemeData.SType == 1)
                    {
                        List<int> loanScheme = new List<int>();
                        loanScheme.Add(124);
                        for (int i = 127; i <= 136; i++)
                        {
                            loanScheme.Add(i);
                        }

                        var f2Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F1id == 125).F2id; // f2id = 29
                        var f2Type = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.F2Type == f2Id).Fid;   // 472,483,6170,6181
                        int count = 0;
                        foreach (var subitemType in loanScheme)
                        {
                            var finsys2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == subitemType).Select(x => x.F2id).FirstOrDefault(); //f2id = 28
                            var finaccId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == finsys2Id).FirstOrDefault().Fid; //fid = 17

                            var F2Type = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F1id == 125).F2id; //f2id = 29

                            count++;
                            Models.Models.FinAcc finaccLiab = new Models.Models.FinAcc();

                            finaccLiab.Fname = schemeData.SDName + "(" + LedgerName(subitemType) + ")";
                            finaccLiab.Pid = finaccId;
                            if (count == 1)
                            {
                                finaccLiab.F2Type = F2Type; // suru ma ya janxa
                            }
                            else
                            {
                                finaccLiab.F2Type = finsys2Id; // paxi ya
                            }
                            finaccLiab.Alias = alias;

                            if (count == 1)
                            {
                                Models.Models.SchmDetail schmDetail = new Models.Models.SchmDetail();

                                schmDetail.SDName = schemeData.SDName;
                                schmDetail.HasCard = schemeData.HasCard;
                                schmDetail.HasCertificate = schemeData.HasCertificate;
                                schmDetail.HasCheque = schemeData.HasCheque;
                                schmDetail.HasDormancy = schemeData.HasDormancy;
                                schmDetail.HasDuration = true; //sir le true nai pathaidinu vannu vako.. loan registration ma error le garda
                                schmDetail.HasIndivLimit = schemeData.HasIndivLimit;
                                schmDetail.HasIndivRate = schemeData.HasIndivRate;
                                schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                schmDetail.HASSMS = schemeData.HASSMS;
                                schmDetail.ICBID = schemeData.ICBID;
                                schmDetail.IsInsured = schemeData.IsInsured;
                                schmDetail.MID = schemeData.MID;
                                schmDetail.Nomianable = schemeData.Nomianable;
                                schmDetail.RuleMovement = schemeData.RuleMovement;
                                schmDetail.RuleICB = schemeData.RuleICB;
                                schmDetail.SEnable = true;
                                schmDetail.SType = 1;
                                schmDetail.Withdrawal = schemeData.Withdrawal;
                                schmDetail.Revolving = schemeData.Revolving;
                                schmDetail.IsRenewable = schemeData.IsRenewable;
                                schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                //schmDetail.HasDuration = schemeData.HasDuration;
                                schmDetail.MultiDeposit = schemeData.MultiDeposit;
                                schmDetail.IsFDLoan = schemeData.IsFDLoan;
                                //uow.Repository<Models.Models.SchmDetail>().Add(schmDetail);
                                finaccLiab.SchmDetails.Add(schmDetail);

                            }
                            uow.Repository<Models.Models.FinAcc>().Add(finaccLiab);
                            uow.Commit();
                        }
                    }
                    //else
                    //{
                    //    using (var uow = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork())
                    //    {
                    //        var schemeDataLatest = uow.Repository<Models.Models.SchmDetail>().GetAll().LastOrDefault();
                    //        var finaccId = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeDataLatest.SDID).FID;

                    //        if (schemeData.SType == 0)
                    //        {
                    //            for (int i = finaccId; i < (finaccId + 4); i++)
                    //            {
                    //                Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                    //                finaccDT.Fname = schemeData.SDName;
                    //                uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);
                    //                uow.Commit();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            for (int i = finaccId; i < (finaccId + 10); i++)
                    //            {
                    //                Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                    //                finaccDT.Fname = schemeData.SDName;
                    //                uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);
                    //                uow.Commit();
                    //            }
                    //        }

                    //        GenericUnitOfWork UOW = new GenericUnitOfWork();
                    //        Models.Models.SchmDetail schmDetail = new Models.Models.SchmDetail();

                    //        schmDetail.SDName = schemeData.SDName;
                    //        if (schemeData.SDID != 0)
                    //        {
                    //            schmDetail.SDID = schemeData.SDID;
                    //        }
                    //        else
                    //        {
                    //            schmDetail.SDID = schemeDataLatest.SDID;
                    //        }
                    //        schmDetail.FID = finaccId;
                    //        schmDetail.HasCard = schemeData.HasCard;
                    //        schmDetail.HasCertificate = schemeData.HasCertificate;
                    //        schmDetail.HasCheque = schemeData.HasCheque;
                    //        schmDetail.HasDormancy = schemeData.HasDormancy;
                    //        schmDetail.HasDuration = schemeData.HasDuration;
                    //        schmDetail.HasIndivLimit = schemeData.HasIndivLimit;
                    //        schmDetail.HasIndivRate = schemeData.HasIndivRate;
                    //        schmDetail.HasOverdraw = schemeData.HasOverdraw;
                    //        schmDetail.HASSMS = schemeData.HASSMS;
                    //        schmDetail.ICBID = schemeData.ICBID;
                    //        schmDetail.IsInsured = schemeData.IsInsured;
                    //        schmDetail.MID = schemeData.MID;
                    //        schmDetail.Nomianable = schemeData.Nomianable;
                    //        schmDetail.RuleMovement = schemeData.RuleMovement;
                    //        schmDetail.RuleICB = schemeData.RuleICB;
                    //        schmDetail.SType = schemeData.SType;
                    //        schmDetail.Withdrawal = schemeData.Withdrawal;
                    //        schmDetail.Revolving = schemeData.Revolving;
                    //        schmDetail.SEnable = schemeData.SEnable;
                    //        schmDetail.IsRenewable = schemeData.IsRenewable;
                    //        schmDetail.HasOverdraw = schemeData.HasOverdraw;
                    //        schmDetail.HasDuration = schemeData.HasDuration;
                    //        schmDetail.MultiDeposit = schemeData.MultiDeposit;

                    //        UOW.Repository<Models.Models.SchmDetail>().Edit(schmDetail);
                    //        UOW.Commit();
                    //    }
                    //}

                    #endregion
                }
                #endregion

                #region Edit
                else //edit code here
                {
                    #region Deposit Edit
                    if (schemeData.SType == 0)
                    {
                        var fid = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeData.SDID).FID;

                        var parentId1 = schemeData.F2Type; // F2TYpe = FId ( ex 255) 
                        var F2Id = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == parentId1).F2Type; // ex : 24
                        var F1Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == F2Id).F1id; // Finsys1 f1id ex 13/12

                        if (F1Id == 13)
                        {
                            List<int> localschemeList = new List<int>();
                            localschemeList.Add(13);
                            localschemeList.Add(16);
                            localschemeList.Add(18);
                            localschemeList.Add(20);
                            localschemeList.Add(22);

                            int count = 0;

                            //itemtype = f1id
                            foreach (var itemType in localschemeList)
                            {
                                //var f2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == itemType).Select(x => x.F2id).FirstOrDefault();
                                //var parentId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == f2Id).Select(x => x.Fid).FirstOrDefault();

                                var pidForF2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == itemType).Select(x => x.F2id).FirstOrDefault(); //yesmai aauxa sir le vannu vako
                                var parentId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == pidForF2Id).Select(x => x.Fid).FirstOrDefault();

                                var f2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == pidForF2Id).Select(x => x.F2id).FirstOrDefault();


                                count++;
                                if (count > 1)
                                {
                                    fid++;
                                }
                                Models.Models.FinAcc finaccLiab = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fid).FirstOrDefault();
                                // var f2Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F1id == 14).F2id;

                                finaccLiab.Fname = schemeData.SDName + "(" + LedgerName(itemType) + ")";
                                finaccLiab.Pid = parentId;
                                finaccLiab.Alias = alias;

                                if (count == 1)
                                {
                                    finaccLiab.F2Type = f2Id;

                                    Models.Models.SchmDetail schmDetail = uow.Repository<Models.Models.SchmDetail>().FindBy(x => x.SDID == schemeData.SDID).FirstOrDefault();

                                    schmDetail.SDName = schemeData.SDName;
                                    schmDetail.HasCard = schemeData.HasCard;
                                    schmDetail.HasCertificate = schemeData.HasCertificate;
                                    schmDetail.HasCheque = schemeData.HasCheque;
                                    schmDetail.HasDormancy = schemeData.HasDormancy;
                                    schmDetail.HasDuration = schemeData.HasDuration;
                                    schmDetail.HasIndivLimit = schemeData.HasIndivLimit;
                                    schmDetail.HasIndivRate = schemeData.HasIndivRate;
                                    schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                    schmDetail.HASSMS = schemeData.HASSMS;
                                    schmDetail.ICBID = schemeData.ICBID;
                                    schmDetail.IsInsured = schemeData.IsInsured;
                                    schmDetail.MID = schemeData.MID;
                                    schmDetail.Nomianable = schemeData.Nomianable;
                                    schmDetail.RuleMovement = schemeData.RuleMovement;
                                    schmDetail.RuleICB = schemeData.RuleICB;
                                    schmDetail.SEnable = schemeData.SEnable;
                                    schmDetail.SType = 0;
                                    schmDetail.Withdrawal = schemeData.Withdrawal;
                                    schmDetail.Revolving = schemeData.Revolving;
                                    schmDetail.IsRenewable = schemeData.IsRenewable;
                                    schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                    schmDetail.HasDuration = schemeData.HasDuration;
                                    schmDetail.MultiDeposit = schemeData.MultiDeposit;
                                    schmDetail.IsFDLoan = schemeData.IsFDLoan;
                                    uow.Repository<Models.Models.SchmDetail>().Edit(schmDetail);
                                }
                                else
                                {
                                    finaccLiab.F2Type = pidForF2Id;
                                }
                                uow.Repository<Models.Models.FinAcc>().Edit(finaccLiab);
                                uow.Commit();
                            }
                        }
                        else
                        {
                            List<int> foreignschemeList = new List<int>();
                            foreignschemeList.Add(12);
                            foreignschemeList.Add(17);
                            foreignschemeList.Add(19);
                            foreignschemeList.Add(21);
                            foreignschemeList.Add(23);

                            int count = 0;
                            foreach (var itemType in foreignschemeList)
                            {
                                var pidForF2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == itemType).Select(x => x.F2id).FirstOrDefault();
                                var parentId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == pidForF2Id).Select(x => x.Fid).FirstOrDefault();

                                var f2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.Pid == pidForF2Id).Select(x => x.F2id).FirstOrDefault();

                                count++;
                                if (count > 1)
                                {
                                    fid++;
                                }
                                Models.Models.FinAcc finaccLiab = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fid).FirstOrDefault();

                                var finaccId = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == schemeData.F2Type).F2Type;
                                var F2Type = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.Pid == finaccId && x.F1id == 14).F2id;
                                finaccLiab.Fname = schemeData.SDName + "(" + LedgerName(itemType) + ")";
                                finaccLiab.F2Type = f2Id;
                                finaccLiab.Pid = parentId;
                                finaccLiab.Alias = alias;

                                if (count == 1)
                                {
                                    finaccLiab.F2Type = f2Id;

                                    Models.Models.SchmDetail schmDetail = uow.Repository<Models.Models.SchmDetail>().FindBy(x => x.SDID == schemeData.SDID).FirstOrDefault();

                                    schmDetail.SDName = schemeData.SDName;
                                    schmDetail.HasCard = schemeData.HasCard;
                                    schmDetail.HasCertificate = schemeData.HasCertificate;
                                    schmDetail.HasCheque = schemeData.HasCheque;
                                    schmDetail.HasDormancy = schemeData.HasDormancy;
                                    schmDetail.HasDuration = schemeData.HasDuration;
                                    schmDetail.HasIndivLimit = schemeData.HasIndivLimit;
                                    schmDetail.HasIndivRate = schemeData.HasIndivRate;
                                    schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                    schmDetail.HASSMS = schemeData.HASSMS;
                                    schmDetail.ICBID = schemeData.ICBID;
                                    schmDetail.IsInsured = schemeData.IsInsured;
                                    schmDetail.MID = schemeData.MID;
                                    schmDetail.Nomianable = schemeData.Nomianable;
                                    schmDetail.RuleMovement = schemeData.RuleMovement;
                                    schmDetail.RuleICB = schemeData.RuleICB;
                                    schmDetail.SEnable = schemeData.SEnable;
                                    schmDetail.SType = 0;
                                    schmDetail.Withdrawal = schemeData.Withdrawal;
                                    schmDetail.Revolving = schemeData.Revolving;
                                    schmDetail.IsRenewable = schemeData.IsRenewable;
                                    schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                    schmDetail.HasDuration = schemeData.HasDuration;
                                    schmDetail.MultiDeposit = schemeData.MultiDeposit;
                                    schmDetail.IsFDLoan = schemeData.IsFDLoan;
                                    uow.Repository<Models.Models.SchmDetail>().Edit(schmDetail);

                                }
                                else
                                {
                                    finaccLiab.F2Type = pidForF2Id;

                                }
                                uow.Repository<Models.Models.FinAcc>().Edit(finaccLiab);
                                uow.Commit();
                            }
                        }

                    }
                    #endregion

                    #region Loan Edit
                    if (schemeData.SType == 1)
                    {
                        List<int> loanScheme = new List<int>();
                        loanScheme.Add(124);
                        for (int i = 127; i <= 136; i++)
                        {
                            loanScheme.Add(i);
                        }

                        var f2Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F1id == 125).F2id; // f2id = 29
                        var f2Type = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.F2Type == f2Id).Fid;   // 472,483,6170,6181
                        int count = 0;

                        var fid = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeData.SDID).FID;

                        foreach (var subitemType in loanScheme)
                        {
                            var finsys2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == subitemType).Select(x => x.F2id).FirstOrDefault(); //f2id = 28
                            var finaccId = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == finsys2Id).FirstOrDefault().Fid; //fid = 17

                            var F2Type = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F1id == 125).F2id; //f2id = 29

                            count++;
                            if (count > 1)
                            {
                                fid = fid + 1;
                            }
                            Models.Models.FinAcc finaccLiab = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fid).FirstOrDefault();

                            finaccLiab.Fname = schemeData.SDName + "(" + LedgerName(subitemType) + ")";
                            if (count == 1)
                            {
                                finaccLiab.F2Type = F2Type; // suru ma ya janxa
                            }
                            else
                            {
                                finaccLiab.F2Type = finsys2Id; // paxi ya
                            }
                            finaccLiab.Pid = finaccId;
                            finaccLiab.Alias = alias;

                            if (count == 1)
                            {
                                Models.Models.SchmDetail schmDetail = uow.Repository<Models.Models.SchmDetail>().FindBy(x => x.SDID == schemeData.SDID).FirstOrDefault();

                                schmDetail.SDName = schemeData.SDName;
                                schmDetail.HasCard = schemeData.HasCard;
                                schmDetail.HasCertificate = schemeData.HasCertificate;
                                schmDetail.HasCheque = schemeData.HasCheque;
                                schmDetail.HasDormancy = schemeData.HasDormancy;
                                schmDetail.HasDuration = true; //sir le true nai pathaidinu vannu vako.. loan registration ma error le garda
                                schmDetail.HasIndivLimit = schemeData.HasIndivLimit;
                                schmDetail.HasIndivRate = schemeData.HasIndivRate;
                                schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                schmDetail.HASSMS = schemeData.HASSMS;
                                schmDetail.ICBID = schemeData.ICBID;
                                schmDetail.IsInsured = schemeData.IsInsured;
                                schmDetail.MID = schemeData.MID;
                                schmDetail.Nomianable = schemeData.Nomianable;
                                schmDetail.RuleMovement = schemeData.RuleMovement;
                                schmDetail.RuleICB = schemeData.RuleICB;
                                schmDetail.SEnable = schemeData.SEnable;
                                schmDetail.SType = 1;
                                schmDetail.Withdrawal = schemeData.Withdrawal;
                                schmDetail.Revolving = schemeData.Revolving;
                                schmDetail.IsRenewable = schemeData.IsRenewable;
                                schmDetail.HasOverdraw = schemeData.HasOverdraw;
                                //schmDetail.HasDuration = schemeData.HasDuration;
                                schmDetail.MultiDeposit = schemeData.MultiDeposit;
                                schmDetail.IsFDLoan = schemeData.IsFDLoan;
                                //finaccLiab.SchmDetails.Add(schmDetail);
                                uow.Repository<Models.Models.SchmDetail>().Edit(schmDetail);
                            }
                            finaccLiab.Alias = alias;
                            uow.Repository<Models.Models.FinAcc>().Edit(finaccLiab);
                            uow.Commit();
                        }
                    }
                    //else
                    //{
                    //    using (var uow = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork())
                    //    {
                    //        var schemeDataLatest = uow.Repository<Models.Models.SchmDetail>().GetAll().LastOrDefault();
                    //        var finaccId = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeDataLatest.SDID).FID;

                    //        if (schemeData.SType == 0)
                    //        {
                    //            for (int i = finaccId; i < (finaccId + 4); i++)
                    //            {
                    //                Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                    //                finaccDT.Fname = schemeData.SDName;
                    //                uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);
                    //                uow.Commit();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            for (int i = finaccId; i < (finaccId + 10); i++)
                    //            {
                    //                Models.Models.FinAcc finaccDT = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == i);
                    //                finaccDT.Fname = schemeData.SDName;
                    //                uow.Repository<Models.Models.FinAcc>().Edit(finaccDT);
                    //                uow.Commit();
                    //            }
                    //        }

                    //        //uow.Repository<Models.Models.FinAcc>().Edit(FinAccdata);

                    //        GenericUnitOfWork UOW = new GenericUnitOfWork();
                    //        //var schemeId = FinAcc.SchmDetails.First().SDID;
                    //        //int finaccId = UOW.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeId).FID;

                    //        Models.Models.SchmDetail schmDetail = UOW.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == schemeData.SDID);

                    //        schmDetail.SDName = schemeData.SDName;
                    //        if (schemeData.SDID != 0)
                    //        {
                    //            schmDetail.SDID = schemeData.SDID;
                    //        }
                    //        else
                    //        {
                    //            schmDetail.SDID = schemeDataLatest.SDID;
                    //        }
                    //        schmDetail.FID = finaccId;
                    //        schmDetail.HasCard = schemeData.HasCard;
                    //        schmDetail.HasCertificate = schemeData.HasCertificate;
                    //        schmDetail.HasCheque = schemeData.HasCheque;
                    //        schmDetail.HasDormancy = schemeData.HasDormancy;
                    //        schmDetail.HasDuration = schemeData.HasDuration;
                    //        schmDetail.HasIndivLimit = schemeData.HasIndivLimit;
                    //        schmDetail.HasIndivRate = schemeData.HasIndivRate;
                    //        schmDetail.HasOverdraw = schemeData.HasOverdraw;
                    //        schmDetail.HASSMS = schemeData.HASSMS;
                    //        schmDetail.ICBID = schemeData.ICBID;
                    //        schmDetail.IsInsured = schemeData.IsInsured;
                    //        schmDetail.MID = schemeData.MID;
                    //        schmDetail.Nomianable = schemeData.Nomianable;
                    //        schmDetail.RuleMovement = schemeData.RuleMovement;
                    //        schmDetail.RuleICB = schemeData.RuleICB;
                    //        schmDetail.SType = schemeData.SType;
                    //        schmDetail.Withdrawal = schemeData.Withdrawal;
                    //        schmDetail.Revolving = schemeData.Revolving;
                    //        schmDetail.SEnable = schemeData.SEnable;
                    //        schmDetail.IsRenewable = schemeData.IsRenewable;
                    //        schmDetail.HasOverdraw = schemeData.HasOverdraw;
                    //        schmDetail.HasDuration = schemeData.HasDuration;
                    //        schmDetail.MultiDeposit = schemeData.MultiDeposit;

                    //        UOW.Repository<Models.Models.SchmDetail>().Edit(schmDetail);
                    //        UOW.Commit();
                    //    }
                    //}

                }
                #endregion

                #endregion

                uow.Commit();
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        //List<int> localschemeList = new List<int>();
        //localschemeList.Add(13);
        //localschemeList.Add(16);
        //localschemeList.Add(18);
        //localschemeList.Add(20);
        //localschemeList.Add(22);

        //if (F2Id == 13)
        //{
        //    int count = 0;
        //    foreach (var subitemType in localschemeList)
        //    {
        //        var finsys2Id = uow.Repository<Models.Models.FinSys2>().FindBy(x => x.F1id == subitemType).Select(x => x.F2id).FirstOrDefault();
        //        var finaccList = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.F2Type == finsys2Id).ToList();
        //        foreach (var item in finaccList)
        //        {
        //            count++;
        //            Models.Models.FinAcc finaccLiab = new Models.Models.FinAcc();
        //            finaccLiab.Fname = FinAcc.Fname;
        //            finaccLiab.Content = FinAcc.Content;
        //            finaccLiab.F2Type = FinAcc.F2Type;
        //            finaccLiab.DebitRestriction = FinAcc.DebitRestriction;
        //            finaccLiab.CreditRestriction = FinAcc.CreditRestriction;
        //            finaccLiab.Code = FinAcc.Code;
        //            finaccLiab.Pid = item.Fid;
        //            if (count == 1)
        //            {
        //                SchmDetail schmDetail = new SchmDetail();
        //                schmDetail = FinAcc.SchmDetails.Single();
        //                schmDetail.SDName = FinAcc.Fname; ;
        //                finaccLiab.SchmDetails.Add(schmDetail);
        //            }
        //            uow.Repository<Models.Models.FinAcc>().Add(finaccLiab);
        //            uow.Commit();

        //        }
        //    }


        //    //SchmDetail schmDetail = FinAcc.SchmDetails.FirstOrDefault();
        //    //uow.Repository<SchmDetail>().Add(schmDetail);
        //    //uow.Commit();



        //}

        public List<Models.ViewModel.SchemeDetailViewModel> GetSchmDetailList(int pageNo, int PageSize, string Search = "")
        {
            List<Models.ViewModel.SchemeDetailViewModel> lstSchmDetail = new List<Models.ViewModel.SchemeDetailViewModel>();
            string query = "select * from [acc].[FGetSchemeDetailList](" + pageNo + "," + PageSize + ",'" + Search + "')";
            lstSchmDetail = uow.Repository<Models.ViewModel.SchemeDetailViewModel>().SqlQuery(query).ToList();
            return lstSchmDetail;
        }
        public ReturnBaseMessageModel Delete(int SchmDetailId)
        {
            Models.Models.SchmDetail SchmDetail = this.GetSingle(SchmDetailId);

            Models.Models.FinAcc FinAccLedger = uow.Repository<Models.Models.FinAcc>().GetSingle(x => x.Fid == SchmDetail.FID);

            Models.Models.ProductDetail productdetail = uow.Repository<Models.Models.ProductDetail>().GetSingle(x => x.SDID == SchmDetail.SDID);

            if(productdetail != null)
            {
                returnMessage.Msg = "Scheme is mapped to product";
                returnMessage.Success = false;
                return returnMessage;
            }
            else
            {
                int count = 0;
                if (SchmDetail.SType == 0)
                {
                    var parentId1 = FinAccLedger.F2Type; // F2TYpe = FId ( ex 255) 
                    var F2Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == parentId1).F2id; // ex : 24
                    var F1Id = uow.Repository<Models.Models.FinSys2>().GetSingle(x => x.F2id == F2Id).F1id; // Finsys1 f1id ex 13/12
                    var fid = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == SchmDetail.SDID).FID;

                    if (F1Id == 13)
                    {
                        List<int> localschemeList = new List<int>();
                        localschemeList.Add(13);
                        localschemeList.Add(16);
                        localschemeList.Add(18);
                        localschemeList.Add(20);
                        localschemeList.Add(22);

                        //itemtype = f1id
                        bool isDeleted = DeleteScheme(SchmDetail);
                        if (isDeleted == false)
                        {
                            returnMessage.Msg = "Scheme counldnt be delete";
                            returnMessage.Success = false;
                            return returnMessage;
                        }

                        foreach (var itemType in localschemeList)
                        {
                            count++;
                            if (count > 1)
                            {
                                fid = fid + 1;
                            }

                            Models.Models.FinAcc finaccLiab = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fid).FirstOrDefault();

                            uow.Repository<Models.Models.FinAcc>().Delete(finaccLiab);
                            uow.Commit();
                        }
                    }
                    else
                    {
                        List<int> foreignschemeList = new List<int>();
                        foreignschemeList.Add(12);
                        foreignschemeList.Add(17);
                        foreignschemeList.Add(19);
                        foreignschemeList.Add(21);
                        foreignschemeList.Add(23);

                        bool isDeleted = DeleteScheme(SchmDetail);
                        if (isDeleted == false)
                        {
                            returnMessage.Msg = "Scheme counldnt be delete";
                            returnMessage.Success = false;
                            return returnMessage;
                        }

                        foreach (var itemType in foreignschemeList)
                        {
                            count++;
                            if (count > 1)
                            {
                                fid = fid + 1;
                            }

                            Models.Models.FinAcc finaccLiab = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fid).FirstOrDefault();

                            uow.Repository<Models.Models.FinAcc>().Delete(finaccLiab);
                            uow.Commit();
                        }
                    }
                }
                else
                {
                    List<int> loanScheme = new List<int>();
                    loanScheme.Add(124);
                    for (int i = 127; i <= 136; i++)
                    {
                        loanScheme.Add(i);
                    }
                    var fid = uow.Repository<Models.Models.SchmDetail>().GetSingle(x => x.SDID == SchmDetail.SDID).FID;
                    bool isDeleted = DeleteScheme(SchmDetail);
                    if (isDeleted == false)
                    {
                        returnMessage.Msg = "Scheme counldnt be delete";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    foreach (var subitemType in loanScheme)
                    {
                        count++;
                        if (count > 1)
                        {
                            fid = fid + 1;
                        }
                        Models.Models.FinAcc finaccLiab = uow.Repository<Models.Models.FinAcc>().FindBy(x => x.Fid == fid).FirstOrDefault();

                        uow.Repository<Models.Models.FinAcc>().Delete(finaccLiab);
                        uow.Commit();
                    }
                }
                returnMessage.Msg = "Scheme Deleted Sucessfully";
                returnMessage.Success = true;
                return returnMessage;

            }
        }

        public bool DeleteScheme(SchmDetail schemeDetail)
        {
            if (schemeDetail != null)
            {
                uow.Repository<Models.Models.SchmDetail>().Delete(schemeDetail);
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
                if (finaccList != null)
                {
                    lst.Add(new SelectListItem { Text = finaccList.Fname, Value = finaccList.Fid.ToString() });
                }

            }
            return lst;
        }

        //public string GetAddress(int Models.Models.SchmDetailId)
        //{
        //    string result = "";

        //    if (Models.Models.SchmDetailId != 0)
        //    {
        //        Models.Models.SchmDetail mnu = new Models.Models.SchmDetail();
        //        mnu = GetSingle(Models.Models.SchmDetailId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.Models.Models.SchmDetailName);
        //            mnu = GetSingle(mnu.PModels.Models.SchmDetailId);
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
        public int GetStypeFromSchemeId(int SchmDetailId)
        {
            Models.Models.SchmDetail SchmDetail = uow.Repository<Models.Models.SchmDetail>().FindBy(c => c.SDID == SchmDetailId).SingleOrDefault();

            return SchmDetail.SType;
        }
        public int GetDepositTypeFromSchemeId(int SchmDetailId)
        {
            Models.Models.SchmDetail SchmDetail = uow.Repository<Models.Models.SchmDetail>().FindBy(c => c.SDID == SchmDetailId).SingleOrDefault();
            Models.Models.FinAcc finAcc = uow.Repository<Models.Models.FinAcc>().FindBy(c => c.Fid == SchmDetail.FID).SingleOrDefault();

            return finAcc.Pid.Value;
        }


    }
}