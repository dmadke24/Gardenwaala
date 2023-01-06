using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace GardenWaalaAPI.Security
{
    public class CustomPrincipal :IPrincipal
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
            if (role.Contains(this.Role))
                return true;
            else
                return false;
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Contact { get; set; }
        public string Features { get; set; }
        public string EmailId { get; set; }
    }
}