namespace TrackerApp.Core.Services.Interfaces
{
    public interface IAuthenticationManagementApi
    {
        public Task<string[]> GetUserRolesAsync(string userID);
    }
}
