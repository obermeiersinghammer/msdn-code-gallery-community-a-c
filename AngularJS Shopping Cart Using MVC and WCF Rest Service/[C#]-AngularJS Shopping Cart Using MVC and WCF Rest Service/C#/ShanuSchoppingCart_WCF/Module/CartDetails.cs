//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShanuSchoppingCart_WCF.Module
{
    using System;
    using System.Collections.Generic;
    
    public partial class CartDetails
    {
        public int Cart_ID { get; set; }
        public Nullable<int> Item_ID { get; set; }
        public int qty { get; set; }
        public System.DateTime Added_Date { get; set; }
        public string AddedBy { get; set; }
    
        public virtual ItemDetails ItemDetails { get; set; }
    }
}