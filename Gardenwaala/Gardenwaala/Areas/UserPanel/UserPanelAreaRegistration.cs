using System.Web.Mvc;

namespace Gardenwaala.Areas.UserPanel
{
    public class UserPanelAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UserPanel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "default",
               string.Empty,
               new { action = "Index", controller = "Home" }
               );

            context.MapRoute(
                name: "Contact",
                url: "contact-us",
                defaults: new { controller = "Home", action = "ContactUs" }
                );

            context.MapRoute(
                name: "About",
                url: "about-us",
                defaults: new { controller = "Home", action = "AboutUs" }
                );

            context.MapRoute(
                "UserPanel_default",
                "UserPanel/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}