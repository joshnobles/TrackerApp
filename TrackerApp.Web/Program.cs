using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using TrackerApp.Core.DataAccess;
using TrackerApp.Core.Services.Implementations;
using TrackerApp.Core.Services.Interfaces;
using TrackerApp.Web.Policies.Handlers;
using TrackerApp.Web.Policies.Requirements;

namespace TrackerApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // regiester authentication management API for getting user roles
            builder.Services.AddSingleton<IAuthenticationManagementApi, AuthenticationManagementApi>(service =>
                new AuthenticationManagementApi
                (
                    builder.Configuration["Auth0:Domain"]!,
                    builder.Configuration["Auth0:ClientId"]!,
                    builder.Configuration["Auth0:ClientSecret"]!,
                    builder.Configuration["Auth0:Audience"]!,
                    Convert.ToInt32(builder.Configuration["Auth0:AccessTokenLifetime"])
                )
            );

            builder.Services.AddSession();

            // register Auth0 authentication services
            builder.Services.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = builder.Configuration["Auth0:Domain"]!;
                options.ClientId = builder.Configuration["Auth0:ClientId"]!;
                options.Scope = "openid profile email";
            });

            // register handler for checking if a user has the admin role
            builder.Services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();

            // register handler for checking if a user is authenticated
            builder.Services.AddSingleton<IAuthorizationHandler, IsLoggedInHandler>();

            // register policy for requiring admin role
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("IsLoggedIn", policy => policy.Requirements.Add(new IsLoggedInRequirement()));
                options.AddPolicy("IsAdmin", policy => policy.Requirements.Add(new IsAdminRequirement("Admin")));
            });

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.Always;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // register Razor Pages framework and require authentication for access to pages in /Private folder
            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Callback");

                options.Conventions.AuthorizeFolder("/Private", "IsLoggedIn");
                options.Conventions.AuthorizePage("/Private/Admin", "IsAdmin");
            });

            // register Entity Framework Core ORM
            builder.Services.AddDbContext<Context>(options =>
            {
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 41));
             
                options.UseMySql(builder.Configuration.GetConnectionString("Default"), serverVersion);
            });

            // register secret service to hold API request verification secret
            builder.Services.AddScoped<ISecretService, SecretService>(service => 
                new SecretService(builder.Configuration["ApiRequestVerification:Secret"]!)
            );

            // register API controllers
            builder.Services.AddControllers();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            // map API endpoints to controllers
            app.MapControllers();

            app.MapRazorPages();

            app.Run();
        }
    }
}
