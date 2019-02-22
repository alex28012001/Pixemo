using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorChat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{imageId}",
                defaults: new { controller = "media", action = "main", imageId = 0 }
            );

            routes.MapRoute(
                name: "Info",
                url: "{controller}/{action}/{id}/{ownerId_Url}",
                defaults: new { controller = "media", action = "info", id = 0, ownerId_Url = "" }
            );
        }
    }
}
