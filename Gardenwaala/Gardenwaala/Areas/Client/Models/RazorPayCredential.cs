using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gardenwaala.Areas.Client.Models
{
    public class RazorPayCredential
    {
        //Test Key and secret
        private static string key = "rzp_test_wFpHCEYmDSVyZ7";
        private static string secret = "oiXgfpejASUZpyfusTCNPmTk";

        public static void GenerateParams(string name, string email, string phone, float amt)
        {

        }
        RazorpayClient client = new RazorpayClient(key, secret);
    }
}