//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChannakyaAccounting.Models.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StatusChangeLog
    {
        public int LogStatusID { get; set; }
        public long IAccNo { get; set; }
        public System.DateTime TDate { get; set; }
        public short UserID { get; set; }
        public short AccState { get; set; }
        public System.DateTime ChangeOn { get; set; }
    }
}
