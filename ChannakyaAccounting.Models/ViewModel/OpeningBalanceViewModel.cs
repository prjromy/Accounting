using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Models.ViewModel
{
    public class OpeningBalanceViewModel
    {
        public OpeningBalanceViewModel()
        {
            this.SubsiDetailOfLedger = new List<SubsiViewModel>();
        }
        public int Id { get; set; }
        public string LedgerName { get; set; }
        public string Particulars { get; set; }
        public Nullable<int> FId { get; set; }
        public Nullable<int> FYId { get; set; }
        public Nullable<decimal> OPBal { get; set; }
        public Nullable<decimal> CLBal { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public int AmountType { get; set; }
        public List<SubsiViewModel> SubsiDetailOfLedger { get; set; }

    }
  
}
