using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Web.Models;

namespace Glav.PayMeBack.Web.Domain.Engines
{
	public class HelpEngine : IHelpEngine
	{
		public ApiHelp GetApiHelpInformation()
		{
			var helpDoco = new ApiHelp();
			var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();

			var sortedItems = apiExplorer.ApiDescriptions.OrderBy(a => a.ActionDescriptor.ControllerDescriptor.ControllerName);
			foreach (var apiMethod in sortedItems)
			{
				// Strip out any OAuth related methods as they will be listed again
				// in the authorisation route
				if (!apiMethod.RelativePath.Contains("OAuth"))
				{
					var apiCall = GetApiMethodCall(apiMethod);
					helpDoco.Api.Add(apiCall);
				}
			}

			return helpDoco;
		}

		private ApiMethodCall GetApiMethodCall(ApiDescription apiMethod)
		{
			var methodCall = new ApiMethodCall();
			methodCall.HttpMethod = apiMethod.HttpMethod.ToString();

			methodCall.Group = apiMethod.ActionDescriptor.ControllerDescriptor.ControllerName;
			if (methodCall.Group.ToLowerInvariant() == "oauth")
			{
				methodCall.Group = "Authorisation";
			}
			methodCall.Parameters = GetApimethodCallParameters(apiMethod.ParameterDescriptions);
			methodCall.Description = apiMethod.Documentation;
			methodCall.ReturnType = apiMethod.ActionDescriptor.ReturnType.ToString();
			methodCall.Uri = apiMethod.RelativePath;

			PopulateReturnPayloadExample(apiMethod, methodCall);

			return methodCall;
		}

		private void PopulateReturnPayloadExample(ApiDescription apiMethod, ApiMethodCall methodCall)
		{
			if (methodCall.Group.ToLowerInvariant() != "authorisation")
			{
				//NOTE: Blows up for interfaces
				//var returnDto = Activator.CreateInstance(apiMethod.ActionDescriptor.ReturnType);
				//methodCall.ReturnPayloadExample = Newtonsoft.Json.JsonConvert.SerializeObject(returnDto);
			}
			else
			{
				if (methodCall.Uri.ToLowerInvariant().Contains("ping"))
				{
					methodCall.ReturnPayloadExample = "true/false";
				}
				else
				{
					var returnDto = new OAuthAuthorisationGrantResponse();
					methodCall.ReturnPayloadExample = Newtonsoft.Json.JsonConvert.SerializeObject(returnDto);
				}
			}
		}

		private List<ApiMethodParameter> GetApimethodCallParameters(Collection<ApiParameterDescription> parameterDescriptions)
		{
			var methodParameterList = new List<ApiMethodParameter>();
			if (parameterDescriptions == null)
			{
				return methodParameterList;
			}

			foreach (var parm in parameterDescriptions)
			{
				
			}

			return methodParameterList;
		}
	}
}