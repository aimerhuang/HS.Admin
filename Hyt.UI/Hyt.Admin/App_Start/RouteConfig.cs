using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hyt.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
            //测试用
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Test", action = "TestHaiDai", id = UrlParameter.Optional }
            //);
        }
    }
}