using System.Web.Mvc;

namespace DAN.Areas.System
{
    public class SystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "System";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "System_default2",
                "System/{controller}/{action}/{itemId}/{act}",
                new { action = "Index", itemId = UrlParameter.Optional, act = UrlParameter.Optional }
            );
            context.MapRoute(
                "System_default",
                "System/{controller}/{action}/{itemId}",
                new { action = "Index", itemId = UrlParameter.Optional }
            );
        }
    }
}