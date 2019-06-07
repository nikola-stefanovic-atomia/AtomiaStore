using System.Web.Mvc;
using System.Web.Routing;
using Atomia.Store.AspNetMvc.Infrastructure;

namespace Atomia.Store.Themes.Default
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapOrderFlowRoute(
                name: "Domains",
                url: "Domains/{action}",
                defaults: new
                {
                    controller = "Domains",
                    action = "Index"
                }
            );

            routes.MapOrderFlowRoute(
                name: "HostingPackage",
                url: "Hosting",
                defaults: new
                {
                    controller = "ProductListing",
                    action = "Index",
                    query = "HostingPackage",
                    viewName = "HostingPackage"
                }
            );

            routes.MapOrderFlowRoute(
                name: "Account",
                url: "Account/{action}",
                defaults: new
                {
                    controller = "Account",
                    action = "Index"
                }
            );

            routes.MapOrderFlowRoute(
                name: "ExistingCustomer",
                url: "ExistingCustomer/{action}",
                defaults: new
                {
                    controller = "ExistingCustomer",
                    action = "Index"
                }
            );

            routes.MapOrderFlowRoute(
                name: "Checkout",
                url: "Checkout/{action}",
                defaults: new
                {
                    controller = "Checkout",
                    action = "Index"
                }
            );

            routes.MapOrderFlowRoute(
                name: "OrderFlowStart",
                url: "",
                defaults: new
                {
                    controller = "Domains",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: "Campaign",
                url: "Campaign/{campaign}",
                defaults: new
                {
                    controller = "Select",
                    action = "Campaign"
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new 
                {
                    action = "Index"
                }
            );
        }
    }
}
