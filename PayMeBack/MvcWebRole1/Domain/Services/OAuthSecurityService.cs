using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Glav.PayMeBack.Web.Data;
using Glav.CacheAdapter.Core;
using Glav.PayMeBack.Core;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class OAuthSecurityService : IOAuthSecurityService
	{
		private ICrudRepository _crudRepository;
		private ICacheProvider _cacheProvider;

		private const string CacheKeyTokenEntry = "OAuthTokenEntry_{0}";

		public OAuthSecurityService(ICrudRepository crudRepository, ICacheProvider cacheProvider)
		{
			_crudRepository = crudRepository;
			_cacheProvider = cacheProvider;
		}
		public OAuthAuthorisationGrantResponse AuthorisePasswordCredentialsGrant(string emailAddress, string password, string scope)
		{
			var response = new OAuthAuthorisationGrantResponse();
			response.IsSuccessfull = false;
			response.ErrorDetails = new OAuthGrantRequestError();
			response.AccessGrant = new OAuthAccessTokenGrant();

			var user = _crudRepository.GetSingle<UserDetail>(u => u.EmailAddress == emailAddress);

			if (user != null)
			{
				try
				{
					var validUser = SignIn(emailAddress, password);
					var isScopeValid = ValidateScopeForUse(validUser, scope);

					if (validUser != null)
					{
						if (!isScopeValid)
						{
							response.ErrorDetails.error = OAuthErrorResponseCode.InvalidScope;
						}
						else
						{
							var tokenEntry = new OAuthToken();
							tokenEntry.AssociatedUserId = validUser.Id;
							tokenEntry.AccessToken = CreateNewHashedToken();
							tokenEntry.AccessTokenExpiry = DateTime.UtcNow.AddMinutes(OAuthConfig.AccessTokenExpiryInMinutes);
							tokenEntry.RefreshToken = CreateNewHashedToken();
							tokenEntry.RefreshTokenExpiry = OAuthConfig.RefreshTokenExpiryInDays == 0 ? DateTime.MaxValue : DateTime.UtcNow.AddDays(OAuthConfig.RefreshTokenExpiryInDays);
							PopulateScopeForToken(tokenEntry, scope);

							_crudRepository.Insert<OAuthToken>(tokenEntry);
							_cacheProvider.Add(GetCacheKeyForTokenRecord(tokenEntry.AccessToken), tokenEntry.AccessTokenExpiry, tokenEntry);

							response.IsSuccessfull = true;
							MapOAuthTokenToAccessTokenResponse(tokenEntry, response);

							return response;
						}
					}
					else
					{
						response.ErrorDetails.error = OAuthErrorResponseCode.InvalidClient;
					}
				}
				catch (Exception ex)
				{
					//TODO: Proper response code required here
					response.ErrorDetails.error = OAuthErrorResponseCode.InvalidClient;
				}
			}
			else
			{
				response.ErrorDetails.error = OAuthErrorResponseCode.InvalidClient;
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
			response.AccessGrant.expires_in = OAuthConfig.AccessTokenExpiryInMinutes * 60;  // convert to seconds
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
				var tokenEntry = _crudRepository.GetSingle<OAuthToken>(t => t.RefreshToken == refreshToken);
				if (tokenEntry != null)
				{
					var oldToken = tokenEntry.AccessToken;
					var validUser =new User(_crudRepository.GetSingle<UserDetail>(u => u.Id == tokenEntry.AssociatedUserId));
					
					var isScopeValid = ValidateScopeForUse(validUser, scope);

					if (!isScopeValid)
					{
						response.ErrorDetails.error = OAuthErrorResponseCode.InvalidScope;
					}
					else
					{

						if (DateTime.UtcNow < tokenEntry.RefreshTokenExpiry)
						{
							tokenEntry.AccessToken = CreateNewHashedToken();
							tokenEntry.AccessTokenExpiry = DateTime.UtcNow.AddMinutes(30);
							_cacheProvider.InvalidateCacheItem(GetCacheKeyForTokenRecord(oldToken));
							_crudRepository.Update<OAuthToken>(tokenEntry);
							_cacheProvider.Add(GetCacheKeyForTokenRecord(tokenEntry.AccessToken), tokenEntry.AccessTokenExpiry, tokenEntry);

							response.IsSuccessfull = true;
							MapOAuthTokenToAccessTokenResponse(tokenEntry, response);
						}
						else
						{
							response.ErrorDetails.error = OAuthErrorResponseCode.InvalidClient;
						}
					}
				}
				else
				{
					response.ErrorDetails.error = OAuthErrorResponseCode.InvalidGrant;
				}
			}
			catch (Exception ex)
			{
				response.ErrorDetails.error = OAuthErrorResponseCode.InvalidClient;
			}

			return response;
		}

		public bool IsAccessTokenValid(string token)
		{
			var tokenEntry = _cacheProvider.Get<OAuthToken>(GetCacheKeyForTokenRecord(token), DateTime.Now.AddMinutes(OAuthConfig.AccessTokenExpiryInMinutes), () =>
				{
					return _crudRepository.GetSingle<OAuthToken>(t => t.AccessToken == token);
				});

			if (tokenEntry != null && tokenEntry.AccessTokenExpiry > DateTime.UtcNow)
			{
			    return true;
			}

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
				return false;
			}

			var authScopesPresented = scope.ToScopeArray();
			if (authScopesPresented.Length == 0)
			{
				return false;
			}

			// Currently we only assert that a valid scope is present
			var isValidPresent = authScopesPresented.Count(s => s.ScopeType == AuthorisationScopeType.Modify 
							|| s.ScopeType == AuthorisationScopeType.UberUser
							|| s.ScopeType == AuthorisationScopeType.Readonly) > 0;
			return isValidPresent;
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
			var userDetail = _crudRepository.GetSingle<UserDetail>(u => u.EmailAddress == email);
			if (userDetail != null)
			{
				var currentPwd = userDetail.Password;
				if (currentPwd == CreateHashedTokenFromInput(password))
				{
					return new User(userDetail);
				}
			}
			return null;
		}

	}
}
