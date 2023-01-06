using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Gardenwaala.CustomSecurity
{
    public class CustomPrincipal : IPrincipal
    {
        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
        public IIdentity Identity
        {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        //extra properties

        public int UserID { get; set; }
        public string Email { get; set; }
        public DateTime LastLogin { get; set; }
    }
}