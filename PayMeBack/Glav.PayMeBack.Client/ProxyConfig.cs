using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Glav.PayMeBack.Client
{
	public static class ProxyConfig
	{
		public static string BaseUri = ConfigurationSettings.AppSettings["BaseUri"];

	}
}
