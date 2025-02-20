using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TrackerApp.Core.DataAccess;
using TrackerApp.Core.Services.Implementations;
using TrackerApp.Core.Services.Interfaces;

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
