using Gardenwaala.CustomSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gardenwaala.Areas.Admin.Controllers
{
    [CustomUserAuthentication]
    public class UserAdminBaseController : Controller
    {
        
    }
}