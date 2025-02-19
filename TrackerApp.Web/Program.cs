using Auth0.AspNetCore.Authentication;

namespace TrackerApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // register Auth0 authentication services
            builder.Services.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = builder.Configuration["Auth0:Domain"]!;
                options.ClientId = builder.Configuration["Auth0:ClientId"]!;
            });

            // register Razor Pages framework and require authentication for access to /Private folder
            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Private");
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
