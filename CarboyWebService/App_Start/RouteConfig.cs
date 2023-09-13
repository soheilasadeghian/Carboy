using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarBoyWebservice
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                        name: "tracker",
                        url: "tracker/{hash}",
                        defaults: new { controller = "Home", action = "Tracker" }
                    );
            routes.MapRoute(
                        name: "geo",
                        url: "geo",
                        defaults: new { controller = "Home", action = "Geo" }
                    );
        }
    }
}
