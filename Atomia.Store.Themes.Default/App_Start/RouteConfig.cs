﻿using Atomia.Store.Core;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

namespace Atomia.Store.Themes.Default
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Domains",
                url: "Domains/{action}",
                defaults: new
                {
                    controller = "Domain",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: "Hosting",
                url: "Hosting",
                defaults: new
                {
                    controller = "ProductListing",
                    action = "Index",
                    query = "Hosting",
                    viewName = "HostingPackages"
                }
            );

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}",
                defaults: new
                {
                    controller = "Account",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new 
                { 
                    controller = "Domain", 
                    action = "Index" 
                }
            );
        }
    }
}
