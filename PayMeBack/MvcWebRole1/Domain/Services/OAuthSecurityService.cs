using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class OAuthSecurityService : IOAuthSecurityService
	{
		private ISecurityRepository _securityRepository;
		//private ICacheProvider _cacheProvider;
		private IUserRepository _userRepository;

		private const string CacheKeyTokenEntry = "OAuthTokenEntry_{0}";

		public OAuthSecurityService(ISecurityRepository securityRepository, IUserRepository userRepository)
		{
			_securityRepository = securityRepository;
			//_cacheProvider = cacheProvider;
			_userRepository = userRepository;
		}
		public OAuthAuthorisationGrantResponse AuthorisePasswordCredentialsGrant(string emailAddress, string password, string scope)
		{
			var response = new OAuthAuthorisationGrantResponse();
			response.IsSuccessfull = false;
			response.ErrorDetails = new OAuthGrantRequestError();
			response.AccessGrant = new OAuthAccessTokenGrant();

			var user = _userRepository.GetUser(emailAddress);

			if (user != null)
			{
				try
				{
					var validUser = SignIn(emailAddress, password);
					var isScopeValid = ValidateScopeForUse(validUser, scope);

					if (validUser != null)
					{
						var tokenEntry = new OAuthToken();
						tokenEntry.AssociatedUserId = validUser.Id;
						tokenEntry.AccessToken = CreateNewHashedToken();
						tokenEntry.AccessTokenExpiry = DateTime.UtcNow.AddMinutes(30);
						tokenEntry.RefreshToken = CreateNewHashedToken();
						tokenEntry.RefreshTokenExpiry = DateTime.UtcNow.AddYears(1);
						PopulateScopeForToken(tokenEntry, scope);

						_securityRepository.Insert(tokenEntry);
						//_cacheProvider.Add(GetCacheKeyForTokenRecord(tokenEntry.AccessToken), tokenEntry.AccessTokenExpiry, tokenEntry);

						response.IsSuccessfull = true;
						MapOAuthTokenToAccessTokenResponse(tokenEntry, response);

						return response;
					}
					else
					{
						response.ErrorDetails.error = "invalid_client";
					}
				}
				catch (Exception ex)
				{
					//TODO: Proper response code required here
					response.ErrorDetails.error = "invalid_client";
				}
			}


			return response;
		}

		/// <summary>
		/// This method will populate the token scope with a fully qualified scope value. Initially the client may request FULL access
		/// but we must also return the clients valid scope such as valid file id's that can be used and are applicable for the
		/// requested level of access.
		/// </summary>
		/// <param name="tokenEntry"></param>
		private void PopulateScopeForToken(OAuthToken tokenEntry, string requestedScope)
		{
			List<AuthorisationScope> scopeList = new List<AuthorisationScope>();
			if (!string.IsNullOrWhiteSpace(requestedScope))
			{
				scopeList.AddRange(requestedScope.ToScopeArray());
			}

				// Just return the provided valid scope
				tokenEntry.Scope = string.IsNullOrWhiteSpace(requestedScope) ? string.Empty : requestedScope.ToAuthorisationScope().ToTextValue();
		}

		private void MapOAuthTokenToAccessTokenResponse(OAuthToken tokenEntry, OAuthAuthorisationGrantResponse response)
		{
			response.AccessGrant.access_token = tokenEntry.AccessToken;
			response.AccessGrant.expires_in = 30 * 60;  // convert to seconds
			response.AccessGrant.refresh_token = tokenEntry.RefreshToken;
			response.AccessGrant.token_type = OAuthTokenType.Bearer;
			response.AccessGrant.scope = tokenEntry.Scope;
		}

		public OAuthAuthorisationGrantResponse RefreshAccessToken(string refreshToken, string scope)
		{
			var response = new OAuthAuthorisationGrantResponse();
			response.IsSuccessfull = false;
			response.ErrorDetails = new OAuthGrantRequestError();
			response.AccessGrant = new OAuthAccessTokenGrant();

			try
			{
				var tokenEntry = _securityRepository.GetTokenDataByRefreshToken(refreshToken);
				if (tokenEntry != null)
				{
					var oldToken = tokenEntry.AccessToken;
					//TODO: Need to ensure we get a proper userInfo and validate scope
					var validUser =new User(_userRepository.GetUser(tokenEntry.AssociatedUserId));
					
					ValidateScopeForUse(validUser, scope);
					

					if (DateTime.UtcNow < tokenEntry.RefreshTokenExpiry)
					{
						tokenEntry.AccessToken = CreateNewHashedToken();
						tokenEntry.AccessTokenExpiry = DateTime.UtcNow.AddMinutes(30);
						//_cacheProvider.InvalidateCacheItem(GetCacheKeyForTokenRecord(oldToken));
						_securityRepository.Update(tokenEntry);
						//_cacheProvider.Add(GetCacheKeyForTokenRecord(tokenEntry.AccessToken), tokenEntry.AccessTokenExpiry, tokenEntry);

						response.IsSuccessfull = true;
						MapOAuthTokenToAccessTokenResponse(tokenEntry, response);
					}
					else
					{
						//TODO: Proper response code required here
						response.ErrorDetails.error = "invalid_client";
					}
				}
				else
				{
					response.ErrorDetails.error = "invalid_grant";
				}
			}
			catch (Exception ex)
			{
				//TODO: Proper response code required here
				response.ErrorDetails.error = "invalid_client";
			}

			return response;
		}

		public bool IsAccessTokenValid(string token)
		{
			//var tokenEntry = _cacheProvider.InnerCache.Get<OAuthToken>(GetCacheKeyForTokenRecord(token));
			//if (tokenEntry != null && tokenEntry.AccessTokenExpiry > DateTime.UtcNow)
			//{
			//    return true;
			//}

			return false;
		}

		private string GetCacheKeyForTokenRecord(string token)
		{
			return string.Format(CacheKeyTokenEntry, token);
		}

		private bool ValidateScopeForUse(User validUser, string scope)
		{
			if (validUser == null || string.IsNullOrWhiteSpace(scope))
			{
				throw new System.Security.SecurityException("Invalid User");
			}

			var authScopesPresented = scope.ToScopeArray();
			if (authScopesPresented.Length == 0)
			{
				throw new System.Security.SecurityException("No authorisation scope presented");
			}

			// Currently we only support FULL scope
			var isFullScopePresent = authScopesPresented.Count(s => s.ScopeType == AuthorisationScopeType.Full) > 0;
			return isFullScopePresent;
		}

		public string CreateNewHashedToken()
		{
			var newAccessToken = Guid.NewGuid();
			return CreateHashedTokenFromInput(newAccessToken.ToString());
		}
		public string CreateHashedTokenFromInput(string input)
		{
			var hashAlgorithm = new SHA256Managed();
			var messageInBytes = UnicodeEncoding.Unicode.GetBytes(input);

			var hashedValue = hashAlgorithm.ComputeHash(messageInBytes);
			return System.Web.HttpServerUtility.UrlTokenEncode(hashedValue);
		}

		public User SignIn(string email, string password)
		{
			var currentPwd = _userRepository.GetUserPassword(email);
			if (CreateHashedTokenFromInput(currentPwd) == CreateHashedTokenFromInput(password))
			{
				return new User(_userRepository.GetUser(email));
			}
			return null;
		}

	}
}
