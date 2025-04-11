using TrackerApp.Core.Services.Interfaces;

namespace TrackerApp.Core.Services.Implementations
{
    public class SecretService : ISecretService
    {
        private readonly string _requestVerificationSecret;
        private readonly string _thunderForestSecret;

        public SecretService(string requestVerificationSecret, string thunderForestSecret)
        {
            _requestVerificationSecret = requestVerificationSecret;
            _thunderForestSecret = thunderForestSecret;
        }

        public string GetRequestVerificationSecret() => _requestVerificationSecret;

        public string GetThunderForestSecret() => _thunderForestSecret;
    }
}
