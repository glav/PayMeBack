using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Autofac;

namespace Glav.PayMeBack.Web.Domain
{
	public class ApiDependencyResolver : ApiScopeContainer, IDependencyResolver
	{
		private IContainer _container;
		public ApiDependencyResolver(IContainer container)
			: base(container)
		{
			_container= container;
		}
		public IDependencyScope BeginScope()
		{
			return new ApiScopeContainer(_container.BeginLifetimeScope());
		}
	}

	public class ApiScopeContainer : IDependencyScope
	{
		private ILifetimeScope _scope;

		public ApiScopeContainer(ILifetimeScope scope)
		{
			_scope = scope;
		}
		public object GetService(Type serviceType)
		{
			object instance;
			if (_scope.TryResolve(serviceType, out instance))
			{
				return instance;
			}
			return null;
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			if (_scope.IsRegistered(serviceType))
			{
				return (IEnumerable<object>)_scope.Resolve(serviceType);
			}
			return null;
		}
		public void Dispose()
		{
			_scope.Dispose();
		}
	}

}