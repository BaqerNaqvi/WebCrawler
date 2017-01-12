
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;
using WebCrawler.Scheduler;

namespace WebCrawler
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.EnableFriendlyUrls();
           
        }
    }
}
