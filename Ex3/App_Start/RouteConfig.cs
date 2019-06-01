using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("display", "display/{ip}/{port}",
            defaults: new { controller = "FlightLocation", action = "display" });

            routes.MapRoute("displayAndUpdate", "display/{ip}/{port}/{time}",
            defaults: new { controller = "FlightLocation", action = "displayAndUpdate" });

            routes.MapRoute("saveFlightInfo", "save/{ip}/{port}/{time}/{duration}/{file}",
            defaults: new { controller = "FlightLocation", action = "saveFlightInfo" });

            routes.MapRoute("displayFromFile", "display/{file}/{time}",
            defaults: new { controller = "FlightLocation", action = "displayFromFile" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "FlightLocation", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
