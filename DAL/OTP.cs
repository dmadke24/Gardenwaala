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
    
    public partial class OTP
    {
        public int OTPId { get; set; }
        public int Code { get; set; }
        public string MobileNo { get; set; }
        public System.DateTime SentTime { get; set; }
        public bool IsVerified { get; set; }
    }
}
