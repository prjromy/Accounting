//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChannakyaAccounting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SMTPConfig
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public int Port { get; set; }
        public string Description { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool State { get; set; }
    }
}
