using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Autofac;
using System.Collections;

namespace Glav.PayMeBack.Web.Domain
{
	public class ApiDependencyResolver : ApiScopeContainer, IDependencyResolver
	{
		private IContainer _container;
		public ApiDependencyResolver(IContainer container)
			: base(container)
		{
			_container = container;
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
			Type enumerableType = typeof(IEnumerable<>).MakeGenericType(new Type[] { serviceType });
			var customTypes = ((IEnumerable)_scope.Resolve(enumerableType)).Cast<object>();
			var types = new List<object>();
			types.AddRange(customTypes);

			return types;

		}
		public void Dispose()
		{
			_scope.Dispose();
		}
	}

}