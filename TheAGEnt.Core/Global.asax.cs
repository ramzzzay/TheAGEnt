using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using TheAGEnt.Core.Util;

namespace TheAGEnt.Core
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutofacConfig.Configure();
        }
    }
}