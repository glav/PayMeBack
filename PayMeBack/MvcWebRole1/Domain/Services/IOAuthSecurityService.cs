using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.PayMeBack.Core;
using Glav.PayMeBack.Core.Domain;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public interface IOAuthSecurityService
	{
		OAuthAuthorisationGrantResponse AuthorisePasswordCredentialsGrant(string username, string password, string scope);
		string CreateNewHashedToken();
		string CreateHashedTokenFromInput(string input);
		bool IsAccessTokenValid(string token);
		OAuthAuthorisationGrantResponse RefreshAccessToken(string refreshToken, string scope);
		User SignIn(string email, string password);
	}
}
