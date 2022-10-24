using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;

namespace OpenData.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // To handle routes like /Mobile
            config.Routes.MapHttpRoute(
                name: "ControllerOnly",
                routeTemplate: "{controller}"
            );

            // Controller with ID
            // To handle routes like /api/Mobile/1
            config.Routes.MapHttpRoute(
                name: "ControllerAndId",
                routeTemplate: "{controller}/{id}",
                defaults: null,
                constraints: new { id = @"^\d+$" } // Only integers 
            );

            // Controllers with Actions
            // To handle routes like /api/Mobile/actionname
            config.Routes.MapHttpRoute(
                name: "ControllerAndAction",
                routeTemplate: "{controller}/{action}"
            );

            config.EnableQuerySupport();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi1",
            //    routeTemplate: "{controller}/{name}/{id}",
            //    defaults: new { id = RouteParameter.Optional, action="Passport" }
            //);

            // Раскомментируйте следующую строку кода, чтобы включить поддержку запросов для действий с типом возвращаемого значения IQueryable или IQueryable<T>.
            // Чтобы избежать обработки неожиданных или вредоносных запросов, используйте параметры проверки в QueryableAttribute, чтобы проверять входящие запросы.
            // Дополнительные сведения см. по адресу http://go.microsoft.com/fwlink/?LinkId=279712.
            config.EnableQuerySupport();

            // Чтобы отключить трассировку в приложении, закомментируйте или удалите следующую строку кода
            // Дополнительные сведения см. по адресу: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
    new System.Net.Http.Formatting.QueryStringMapping("format", "json", "application/json")
);
        }
    }
}
