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
    
    public partial class ALCollVehicle
    {
        public int ColVechicleId { get; set; }
        public int Sno { get; set; }
        public string LicNo { get; set; }
        public string LicRight { get; set; }
        public string IssueOff { get; set; }
        public string ModNo { get; set; }
        public string ChassisNo { get; set; }
        public string EngNo { get; set; }
        public string Color { get; set; }
        public string CC { get; set; }
        public string MC { get; set; }
        public string MD { get; set; }
        public byte type { get; set; }
        public string description { get; set; }
        public string VehicleNo { get; set; }
    
        public virtual ALColl ALColl { get; set; }
    }
}
