using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glav.PayMeBack.Client.Proxies;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Client.Tests
{
	[TestClass]
	public class NotificationTests
	{
		[TestMethod]
		public void ShouldBeAbleToRetrieveNotificationOptions()
		{
            var bearerToken = SetupValidUserAndAuthenticate();
			var notifyProxy = new NotificationsProxy(bearerToken);

            var notifyResponse = notifyProxy.GetNotificationOptions(Guid.Empty);
            Assert.IsNotNull(notifyResponse);
            Assert.IsTrue(notifyResponse.IsRequestSuccessfull);
            Assert.IsNotNull(notifyResponse.DataObject);
		}

		[TestMethod]
		public void ShouldBeAbleToUpdateNotificationOptions()
		{
            var bearerToken = SetupValidUserAndAuthenticate();
            var notifyProxy = new NotificationsProxy(bearerToken);

            //TODO: Need to create a debt first to associate the notification with
            //      before this test will pass
            var notifyResponse = notifyProxy.GetNotificationOptions(Guid.Empty);
            Assert.IsNotNull(notifyResponse);
            Assert.IsTrue(notifyResponse.IsRequestSuccessfull);
            Assert.IsNotNull(notifyResponse.DataObject);

            var options = notifyResponse.DataObject;
            options.NotificationMethod = NotificationType.Sms;
            var dummySms = DateTime.Now.Millisecond.ToString();
            options.NotificationSmsNumber = dummySms;
            notifyProxy.UpdateNotificationOptions(options);

            var updatedOptions = notifyProxy.GetNotificationOptions(Guid.Empty);
            Assert.AreEqual<NotificationType>(NotificationType.Sms, updatedOptions.DataObject.NotificationMethod);
            Assert.AreEqual<string>(dummySms, updatedOptions.DataObject.NotificationSmsNumber);
        }

        [TestMethod]
        public void ShouldNotBeAbleToUpdateOptionsThatUserDoesNotHaveAccessTo()
        {
            Assert.Inconclusive("Incomplete Test");
        }

        private string SetupValidUserAndAuthenticate()
        {
            var proxy = new AuthorisationProxy();
            var email = string.Format("{0}@integrationtest.com", Guid.NewGuid());
            var response = proxy.Signup(email, "test", "dude", "P@ssw0rd1");

            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsRequestSuccessfull);
            Assert.IsNotNull(response.DataObject);
            Assert.IsNotNull(response.DataObject.AccessGrant);
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.DataObject.AccessGrant.access_token));

            proxy.BearerToken = response.DataObject.AccessGrant.access_token;

            return response.DataObject.AccessGrant.access_token;
        }

	}
}
