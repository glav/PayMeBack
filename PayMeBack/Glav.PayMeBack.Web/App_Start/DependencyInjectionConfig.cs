using Autofac.Integration.Mvc;
using Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Glav.PayMeBack.Web.App_Start
{
    public class DependencyInjectionConfig
    {
        public static void Register(System.Web.Http.HttpConfiguration config)
        {
            var builder = new WebDependencyBuilder();
            var container = builder.BuildDependencies();

            var resolver = new ApiDependencyResolver(container);
            config.DependencyResolver = resolver;


            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}