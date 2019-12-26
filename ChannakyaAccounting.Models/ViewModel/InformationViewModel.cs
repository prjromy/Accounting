using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class ChqRqstModel
    {
        [Required]
        [DisplayName("Account No.")]
        public int? Iaccno { get; set; }
        [Required]
        [DisplayName("No. Of Pieces")]
        [Range(1, byte.MaxValue, ErrorMessage = "Number of cheque cann't be greater than 255 at one time.!!")]
        public byte Pics { get; set; }
        public System.DateTime Tdate { get; set; }
        public int Rno { get; set; }
        public int PostedBy { get; set; }
        public string AccountNo { get; set; }
        public string PostedUser { get; set; }

        public IPagedList<ChqRqstModel> ChequeRequestList { get; set;}
    }

    public  class AChqHModel
    {
        public int AchqHId { get; set; }
        [Required]
        [DisplayName("Account No.")]
        public int IAccno { get; set; }
        public int chkNo { get; set; }
        [Required]
        [DisplayName("Action")]
        public byte cstate { get; set; }
        public System.DateTime tdate { get; set; }
        public int postedby { get; set; }
        public Nullable<int> modifiedBy { get; set; }
        public string remarks { get; set; }


    }
    public  class AchqHHModel
    {
        public int AchqHHId { get; set; }
        public int IAccno { get; set; }
        public int cbkno { get; set; }
        public byte cstate { get; set; }
        public System.DateTime tdate { get; set; }
        public short postedby { get; set; }
        public Nullable<short> approvedby { get; set; }
        public System.DateTime Udate { get; set; }
        public short Ublkby { get; set; }
        public string Remarks { get; set; }
    }
    public  class AChqModel
    {
        public int rno { get; set; }
        public int IAccno { get; set; }
        public int cfrom { get; set; }
        public int cto { get; set; }
        public byte cstate { get; set; }
        public int postedby { get; set; }
        public Nullable<int> approvedby { get; set; }
        public System.DateTime tdate { get; set; }
        public Nullable<bool> IsPrinted { get; set; }

        public string AccountNumber { get; set; }
        public IPagedList<AChqModel> AchqPageList { get; set; }
        public List<AChqModel> AchqList { get; set; }

        public AChqHModel AchqHistory { get; set; }
        
    }
}
