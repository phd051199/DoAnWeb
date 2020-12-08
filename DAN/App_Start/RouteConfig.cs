using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DAN
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "VanChuyen",
                url: "Home/Chinh-Sach-Van-Chuyen",
                defaults: new { controller = "Home", action = "VanChuyen" },
                namespaces: new string[] { "DAN.Controllers" }
            ); 
            routes.MapRoute(
               name: "Default3",
               url: "Order/Invoice/{OId}",
               defaults: new { controller = "Order", action = "Invoice", OId = UrlParameter.Optional},
               namespaces: new string[] { "DAN.Controllers" }
           );
            routes.MapRoute(
                name: "Default2",
                url: "{controller}/{action}/{itemId}/{page}",
                defaults: new { controller = "Home", action = "Index", itemId = UrlParameter.Optional, page = UrlParameter.Optional },
                namespaces: new string[] { "DAN.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional, page = UrlParameter.Optional},
                namespaces : new string[] { "DAN.Controllers" }
            );
            
        }
    }
	
}
