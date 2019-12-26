using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting
{
    public static class EnumValue
    {
        public enum Parameter
        {
            IsStrictlyVerified= 2002,
            DefaultCurrency=7013
        }

        public enum Subsi
        {
            Employee=1,
            User=2,
            Customer=3,
            Debtors=4,
            Creditors=5,
            Deposit=6,
            Loan=7,
            Share=8
        }

        public enum VoucherReport
        {
            SeeMultipleVoucherReport= 6007
        }

        public enum FinSys1
        {
            Cash= 3,
            Bank = 5,
            SubsiAccount =7,
            DepositProduct=15,
            LoanProduct=126
            
        }

        public enum FinAccLedger
        {
            Liabilites=1,
            Assests=2,
            Income=3,
            Expenditure=4,
            DepositProduct= 15,
            LoanProduct= 126
        }

    }
}
