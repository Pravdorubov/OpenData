using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OpenData.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "",
                new { 
                    controller="OpenDataSet",
                    action="List2",
                    categoryId=0,
                    authorityId=0,
                    page=1
                }
            );

            routes.MapRoute(null, "onclick",
                new
                {
                    controller = "Nav",
                    action = "Ajax",
                    Length=11
                });

            routes.MapRoute(null, "DataSets",
                new
                {
                    controller = "OpenDataSet",
                    action = "List2",
                    categoryId = 0,
                    authorityId = 0,
                    page = 1
                }
            );

            //routes.MapRoute(null, "Maps",
            //    new
            //    {
            //        controller = "Maps",
            //        action = "Index",
            //        categoryId = 0,
            //        authorityINN = 0,
            //        page = 1
            //    }
            //);


            routes.MapRoute(null, "datasets/{Odid}", new { controller = "Table", action = "Index", Odid = UrlParameter.Optional });

            routes.MapRoute(null, "Page{page}",
                new {controller="OpenDataSet",action="List", categoryId=0, authorityId = 0},
                new {page=@"\d+"}
            );

            routes.MapRoute(null, "Category{categoryId}",
                new { controller = "OpenDataSet", action = "List", page=1, authorityId=0 }
            );

            routes.MapRoute(null, "Authority{authorityId}",
                new { controller = "OpenDataSet", action = "List", page = 1, categoryId = 0 }
            );

            routes.MapRoute(null, "Category{categoryId}/Authority{authorityId}/Page{page}",
                new {controller="OpenDataSet",action="List"},
                new {page=@"\d+"}
            );

            //routes.MapRoute("Table", "{ODID}", new { controller = "Table", action = "Index", page = 1 });
            //routes.MapRoute(null, "{ODID}/Page{page}", new { controller = "Table", action = "Index" }, new { page = @"\d+" });
            routes.MapRoute(null, "{controller}", new { action = "Index" });

            routes.MapRoute(null, "Search", new { action = "Index", controller = "Search" });

            routes.MapRoute(null,"{controller}/{action}");

            

            //routes.MapRoute(
            //    name: null,
            //    url: "Page{page}",
            //    defaults: new { controller = "OpenDataSet", action = "List" }
            //);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "OpenDataSet", action = "List", id = UrlParameter.Optional }
            //);
        }
    }
}