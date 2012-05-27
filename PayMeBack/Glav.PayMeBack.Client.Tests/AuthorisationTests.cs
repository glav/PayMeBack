using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Client.Proxies;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Client.Tests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class AuthorisationTests
	{
		public AuthorisationTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		[TestMethod]
		public void ShouldNotBeAbleToSignUp()
		{
			var proxy = new AuthorisationProxy();
			var response = proxy.PasswordCredentialsGrantRequest(Guid.NewGuid().ToString(), "P@ssw0rd1");

			Assert.IsNotNull(response);
			Assert.IsTrue(response.IsRequestSuccessfull);
			Assert.IsNotNull(response.DataObject);
			Assert.IsNotNull(response.DataObject.ErrorDetails);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.ErrorDetails.error));
		}
	}
}
