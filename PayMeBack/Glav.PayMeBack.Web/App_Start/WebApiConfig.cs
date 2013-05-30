using Glav.PayMeBack.Core;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.PayMeBack.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Glav.PayMeBack.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var routes = config.Routes;
            routes.MapHttpRoute(
                name: "Notifications",
                routeTemplate: "api/Notification/{debtId}",
                defaults: new { controller = "Notification" }
            );

            routes.MapHttpRoute(
                name: "SignUp",
                routeTemplate: ResourceNames.Authorisation + "/Signup",
                defaults: new { controller = "OAuth", action = "Signup" }
            );

            routes.MapHttpRoute(
                name: "DebtSummary",
                routeTemplate: "api/debts/summary",
                defaults: new { Controller = "Summary" }
                );
            routes.MapHttpRoute(
                name: "OutstandingDebts",
                routeTemplate: "api/debts/Outstanding",
                defaults: new { Controller = "Debts", action = "GetOutstanding" }
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var authHandler = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(AuthenticationEngine)) as DelegatingHandler;
            var usageHandler = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ApiUsageHandler)) as DelegatingHandler;

            config.MessageHandlers.Add(usageHandler);
            config.MessageHandlers.Add(authHandler);

            config.Routes.MapHttpRoute("OAuthPing", ResourceNames.Authorisation + "/Ping", new { controller = "OAuth", action = "HeadPing" });
            config.Routes.MapHttpRoute("OAuth", ResourceNames.Authorisation, new { controller = "OAuth" });

        }
    }
}
