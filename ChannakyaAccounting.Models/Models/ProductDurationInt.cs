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
    
    public partial class ProductDurationInt
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public Nullable<int> DBId { get; set; }
        public Nullable<int> ICBId { get; set; }
        public int DVId { get; set; }
        public Nullable<double> Value { get; set; }
        public Nullable<decimal> InterestRate { get; set; }
    
        public virtual ProductDetail ProductDetail { get; set; }
    }
}