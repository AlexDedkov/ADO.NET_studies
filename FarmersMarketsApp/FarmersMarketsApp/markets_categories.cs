//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FarmersMarketsApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class markets_categories
    {
        public int market_category_id { get; set; }
        public int market_id { get; set; }
        public int category_id { get; set; }
    
        public virtual category category { get; set; }
        public virtual market market { get; set; }
    }
}