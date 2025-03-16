using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using TrackerApp.Core.Services.Interfaces;

namespace TrackerApp.Core.Services.Implementations
{
    public class AuthenticationManagementApi : IAuthenticationManagementApi
    {
        private readonly string _domain;
        private readonly string _clientID;
        private readonly string _clientSecret;
        private readonly string _audience;
        private readonly int _accessTokenLifetime;

        private string? _accessToken;

        private DateTime? _accessTokenGenerationTime;

        public AuthenticationManagementApi(string domain, string clientID, string clientSecret, string audience, int accessTokenLifetime)
        {
            _domain = domain;
            _clientID = clientID;
            _clientSecret = clientSecret;
            _audience = audience;
            _accessTokenLifetime = accessTokenLifetime;

            _accessToken = null;
            _accessTokenGenerationTime = null;
        }

        public async Task<string[]> GetUserRolesAsync(string userID)
        {
            await CheckAccessToken();

            using var client = new ManagementApiClient(_accessToken, _domain);

            var roles = await client.Users.GetRolesAsync(userID);

            return [.. roles.Select(r => r.Name)];
        }

        private async Task CheckAccessToken()
        {
            if (string.IsNullOrWhiteSpace(_accessToken) || _accessTokenGenerationTime is null)
                await RotateTokenAsync();

            var timeAlive = (DateTime.Now - _accessTokenGenerationTime) + TimeSpan.FromSeconds(5);

            var lifetime = TimeSpan.FromSeconds(_accessTokenLifetime);
            
            if (timeAlive >= lifetime)
                await RotateTokenAsync();
        }

        private async Task RotateTokenAsync()
        {
            using var client = new AuthenticationApiClient(_domain);

            var request = new ClientCredentialsTokenRequest()
            {
                ClientId = _clientID,
                ClientSecret = _clientSecret,
                Audience = _audience
            };

            _accessTokenGenerationTime = DateTime.Now;

            var response = await client.GetTokenAsync(request);

            _accessToken = response.AccessToken;
        }

    }
}
