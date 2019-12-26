using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ChannakyaAccounting.Helper
{

    public enum ECustomerSearchType
    {
        [Description("Limited")]
        Limited = 1,
        [Description("Unlimited")]
        Unlimited = 2,
        [Description("Single")]
        SingleType = 3,
        [Description("AccountOpen")]
        AccountOpen = 4,
        [Description("CompanyOnly")]
        CompanyOnly = 5,
        [Description("CustomerOnly")]
        CustomerOnly = 5,
        [Description("All")]
        All = 6,
    }
    public enum EDeno
    {
        [Description("DenoIn")]
        DenoIn = 1,
        [Description("DenoOut")]
        DenoOut = 2,
    }
    public enum EAccountDetailsShow
    {
        [Description("WithAccount")]
        WithAccount = 1,
        [Description("WithOutAccount")]
        WithOutAccount = 2,
        [Description("NoDisplay")]
        NoDisplay = 3,
        [Description("WithdrawWithIntPay")]
        WithdrawWithIntPay = 4,
        [Description("ChargableAccounts")]
        ChargableAccounts = 5,
        [Description("ChequeTransHistory")]
        ChequeTransHistory = 6
    }
    public enum EAccountFilter
    {
        [Description("DepositAccept")]
        DepositAccept = 1,
        [Description("ReadyToClose")]
        ReadyToClose = 2,

        [Description("LoanAccept")]
        LoanAccept = 3,
        [Description("WithdrawAccept")]
        WithdrawAccept = 4,
        [Description("Nominee")]
        Nominee = 5,
    }
    public enum EAccountType
    {
        [Description("Loan")]
        Loan = 1,
        [Description("Normal")]
        Normal = 2,

    }
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());
            System.ComponentModel.DescriptionAttribute[] attributes =
                  (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(System.ComponentModel.DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}