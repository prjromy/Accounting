using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaAccounting.Models.ViewModel
{
    public class ReturnBaseMessageModel
    {
        public ReturnBaseMessageModel()
        {
            Success = false;
            Msg = "Error";
        }
        public bool Success { get; set; }
        public string Msg { get; set; }
        public int ReturnId { get; set; }
        public decimal ValueOne { get; set; }
    }
}
