using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace ChannakyaBase.Model.ViewModel
{

    public class CustInformationViewModel
    {
        public decimal CID { get; set; }
        public string Name { get; set; }

        public byte isind { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string SearchOption { get; set; }
        public string SearchParameter { get; set; }
        public IEnumerable<int> CIDs { get; set; }
        public int TotalCount { get; set; }

        public byte IsIndividual { get; set; }
        public bool Isselect { get; set; }

        public string Mode { get; set; }
        public List<CustInformationViewModel> SelectedCustInfoList { get; set; }
        public IPagedList<CustInformationViewModel> CustomerInfoList { get; set; }
    }
}