using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Client.Proxies;
using Glav.PayMeBack.Core;
using System.Net;

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
		public void ShouldBeAbleToPingAfterSignUp()
		{
			var proxy = new AuthorisationProxy();
			var response = proxy.Signup(Guid.NewGuid().ToString()+"@blah.com","test","dude", "P@ssw0rd1");

			Assert.IsNotNull(response);
			Assert.IsTrue(response.IsRequestSuccessfull);
			Assert.IsNotNull(response.DataObject);
			Assert.IsNotNull(response.DataObject.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));

			proxy.BearerToken = response.DataObject.AccessGrant.access_token;
			var pingResponse = proxy.AuthorisationPing();
			Assert.IsNotNull(pingResponse);
			Assert.IsTrue(pingResponse.IsRequestSuccessfull);
		}


		[TestMethod]
		public void ShouldBeAbleToSignUp()
		{
			var proxy = new AuthorisationProxy();
			var email = string.Format("{0}@integrationtest.com",Guid.NewGuid());
			var response = proxy.Signup(email, "test","dude","P@ssw0rd1");

			Assert.IsNotNull(response);
			Assert.IsTrue(response.IsRequestSuccessfull);
			Assert.IsNotNull(response.DataObject);
			Assert.IsNotNull(response.DataObject.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));
		}

		[TestMethod]
		public void ShouldNotBeAbleToAccessServiceWithBogusToken()
		{
			var proxy = new AuthorisationProxy();

			//Now as for a refresh of the access token
			proxy.BearerToken = "JJSJ99SMMMnbxuiwslsodi==";
			var response = proxy.AuthorisationPing();
			Assert.IsNotNull(response);
			Assert.IsFalse(response.IsRequestSuccessfull);
			Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Unauthorized,  response.StatusCode);
		}

		[TestMethod]
		public void ShouldBeAbleToRefreshTokenAfterSignup()
		{
			var proxy = new AuthorisationProxy();
			var response = proxy.Signup(Guid.NewGuid().ToString()+"@blah.com", "test", "refresh-dude", "P@ssw0rd1");
			// Check that we signed up
			Assert.IsNotNull(response);
			Assert.IsTrue(response.IsRequestSuccessfull);
			Assert.IsNotNull(response.DataObject);
			Assert.IsNotNull(response.DataObject.AccessGrant);
			Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));

			//Now as for a refresh of the access token
			proxy.BearerToken = response.DataObject.AccessGrant.access_token;
			var refreshResponse = proxy.RefreshAccessToken(response.DataObject.AccessGrant.refresh_token, AuthorisationScopeValue.Modify);
			Assert.IsNotNull(refreshResponse);
			Assert.IsTrue(refreshResponse.IsRequestSuccessfull);
			Assert.AreNotEqual<string>(response.DataObject.AccessGrant.access_token, refreshResponse.DataObject.AccessGrant.access_token);

			// Finally, make sur we can ping with the new refresh token
			proxy.BearerToken = refreshResponse.DataObject.AccessGrant.access_token;
			var pingRespose = proxy.AuthorisationPing();
			Assert.IsNotNull(pingRespose);
			Assert.IsTrue(pingRespose.IsRequestSuccessfull);
		}

	}
}
