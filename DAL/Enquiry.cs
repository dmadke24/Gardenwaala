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
    
    public partial class Enquiry
    {
        public int EnquiryId { get; set; }
        public System.DateTime EnqDt { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string AlternatContactNo { get; set; }
        public string Description { get; set; }
        public bool IsReplied { get; set; }
        public string Feedback { get; set; }
        public string Subject { get; set; }
    }
}