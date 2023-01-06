using System.Web.Mvc;

namespace Gardenwaala.Areas.Client
{
    public class ClientAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Client";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //   "default",
            //   string.Empty,
            //   new { action = "Index", controller = "Home" }
            //   );

            //context.MapRoute(
            //    name: "Contact",
            //    url: "contact-us",
            //    defaults: new { controller = "Home", action = "ContactUs" }
            //    );

            //context.MapRoute(
            //    name: "About",
            //    url: "about-us",
            //    defaults: new { controller = "Home", action = "AboutUs" }
            //    );

            context.MapRoute(
                "Client_default",
                "Client/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}