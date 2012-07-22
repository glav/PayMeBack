using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Core
{
	public class ApiHelp
	{
		public ApiHelp()
		{
			Api	= new List<ApiMethodCall>();
		}
		public List<ApiMethodCall> Api { get; set; }
	}
	
	public class ApiMethodCall
	{
		public ApiMethodCall()
		{
			Parameters = new List<ApiMethodParameter>();
		}
		public string Group { get; set; }
		public string HttpMethod { get; set; }
		public string Description { get; set; }
		public string ReturnType { get; set; }
		public string Uri { get; set; }
		public List<string> MediaTypesSupported { get; set; }
		public List<ApiMethodParameter> Parameters { get; set; }
		public string ReturnPayloadExample { get; set; }
		public string SendPayloadExample { get; set; }
	}
	public class ApiMethodParameter
	{
		public string Name { get; set; }
		public string MethodType { get; set; }
		public string Documentation { get; set; }
	}
}