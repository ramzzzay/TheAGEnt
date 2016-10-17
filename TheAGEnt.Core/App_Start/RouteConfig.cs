using System.Web.Mvc;
using System.Web.Routing;

namespace TheAGEnt.Core
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Registration",
                "Registration/{action}",
                new { controller = "Registration", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "EditedInfo",
                "EditingInfo/{action}/{id}",
                new { controller = "EditingInfo", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}