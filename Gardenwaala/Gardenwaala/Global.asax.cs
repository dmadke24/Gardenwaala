using Gardenwaala.CustomSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Gardenwaala
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string data = ticket.UserData;
                string[] userData = data.Split('|');
                CustomPrincipal newUser = new CustomPrincipal(userData[1]);
                newUser.UserID = Convert.ToInt32(userData[0]);
                newUser.Email = userData[2];
                newUser.LastLogin = Convert.ToDateTime("2022-12-05");
                HttpContext.Current.User = newUser;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            HttpException httpException = exception as HttpException;
            Response.TrySkipIisCustomErrors = true;

            switch (httpException.GetHttpCode())
            {
                case 404:
                    Response.StatusCode = 404;
                    Response.RedirectToRoute("NotFound");
                    break;

            }
            Server.ClearError();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["id"] = HttpContext.Current.Session.SessionID;
        }
    }
}
