
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
   public class CollectionSheetViewModel
    {
        public int Id { get; set; }
        public int SheetNo { get; set; }
        public int CollectorId { get; set; }
        public System.DateTime TDate { get; set; }
        public System.DateTime VDate { get; set; }
        public int PostedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public int BrchId { get; set; }
        public decimal TotalAmount { get; set; }
        public string note { get; set; }
         
        public int IAccNo { get; set; }
        public int TNo { get; set; }
        public int SType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }


    }
}
