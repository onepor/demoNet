using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.App_Start;
using WebApi.Security;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Test Routing
            config.Routes.MapHttpRoute(
                name: "Test",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { ordertype = "tests", id = RouteParameter.Optional }
                );

            //Config Routing
            config.Filters.Add(new WebApiExceptionFilterAttribute());
            config.Filters.Add(new IdentityBasicAuthentication());
        }
    }
}
