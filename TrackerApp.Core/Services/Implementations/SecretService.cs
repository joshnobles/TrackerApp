using TrackerApp.Core.Services.Interfaces;

namespace TrackerApp.Core.Services.Implementations
{
    public class SecretService : ISecretService
    {
        private readonly string _secret;

        public SecretService(string secret)
        {
            _secret = secret;
        }

        public string GetSecret() => _secret;
    }
}
