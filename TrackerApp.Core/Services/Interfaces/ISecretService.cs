namespace TrackerApp.Core.Services.Interfaces
{
    public interface ISecretService
    {
        public string GetRequestVerificationSecret();
        public string GetThunderForestSecret();
    }
}
