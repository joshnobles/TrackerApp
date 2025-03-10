using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

            // register Auth0 authentication services
            builder.Services.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = builder.Configuration["Auth0:Domain"]!;
                options.ClientId = builder.Configuration["Auth0:ClientId"]!;
                options.Scope = "openid profile email";
            });

            // register handler for checking if a user has the admin role
            builder.Services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();

            // register policy for requiring admin role
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policy => policy.Requirements.Add(new IsAdminRequirement("Admin")));
            });

            // register Razor Pages framework and require authentication for access to pages in /Private folder
            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Private");
            });

            // register Entity Framework Core ORM
            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Development"));
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // map API endpoints to controllers
            app.MapControllers();

            app.MapRazorPages();

            app.Run();
        }
    }
}
