using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Models.ViewModel
{
   public class ServerSidePagingForAll
    {
    }
    public class DimensionDefinationListViewModel {
        public int DDId { get; set; }
        public string DefName { get; set; }
        public Nullable<byte> IsManual { get; set; }
        public Nullable<int> TId { get; set; }
        public int totalCount { get; set; }
    }
    public class DimensionValueListViewModel
    {
        public int DVId { get; set; }
        public int DDId { get; set; }
        public string DimensionValue1 { get; set; }
        public string CodeNo { get; set; }
        public int totalCount { get; set; }
    }
    public class ProductDetailViewmodel {
        public byte SDID { get; set; }
        public int PID { get; set; }
        public int FID { get; set; }
        public string PName { get; set; }
        public int totalCount { get; set; }
        public byte SType { get; set; }
    }
    public class SchemeDetailViewModel
    {
        public byte SDID { get; set; }
        public int FID { get; set; }
        public string SDName { get; set; }
        public int totalCount { get; set; }
        public byte SType { get; set; }
    }
    public class SubsiSetUpViewModal {
        public int SSId { get; set; }
        public int STBId { get; set; }
        public string Title { get; set; }
        public string Prefix { get; set; }
        public Nullable<int> Length { get; set; }
        public Nullable<int> CurrentNo { get; set; }
        public int totalCount { get; set; }

    }
    public class SubsiDetailViewModel {
        public int SDID { get; set; }
        public int SSId { get; set; }
        public int CId { get; set; }
        public int FId { get; set; }
        public string AccNo { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> Enable { get; set; }
        public Nullable<decimal> DebitLimit { get; set; }
        public Nullable<decimal> CreditLimit { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        public int BranchId { get; set; }
        public int totalCount { get; set; }
    }
    public class VoucherNoViewModel {
        public int VNId { get; set; }
        public string BType { get; set; }
        public int CurrentNo { get; set; }
        public string FyName { get; set; }
        public int VTypeId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public int totalCount { get; set; }
        public string VoucherName { get; set; }
        public int BId { get; set; }
    }
    public class DurationAccViewModel {
        public int Id { get; set; }
        public string Duration1 { get; set; }
        public double Value { get; set; }
        public int totalCount { get; set; }
    }
    public class CustomerIndDetailViewModel {
        public decimal CID { get; set; }
        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Lname { get; set; }
        public byte Gender { get; set; }
        public short Occpn { get; set; }
        public string GFatherName { get; set; }
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public string Occupation { get; set; }
        public bool ISStaff { get; set; }
        public int totalCount { get; set; }
    }
    public class EmployeeViewModel {
        public int EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public int totalCount { get; set; }
    }
}
