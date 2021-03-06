﻿using Autofac.Integration.Mvc;
using Glav.PayMeBack.Web.App_Start;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Glav.PayMeBack.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DependencyInjectionConfig.Register(GlobalConfiguration.Configuration);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleManager.RegisterCssBundles(BundleTable.Bundles);
            BundleManager.RegisterJsBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            MobileConfig.Register();
        }
    }
}