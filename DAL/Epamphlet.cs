//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Epamphlet
    {
        public int EpamphletId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
