using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Project_GrandeTravel.Models;
using Project_GrandeTravel.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Project_GrandeTravel
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            services.AddScoped<IRepository<Category>, BaseRepository<Category>>();
            services.AddScoped<IRepository<Package>, BaseRepository<Package>>();
            services.AddScoped<IRepository<CustomerProfile>, BaseRepository<CustomerProfile>>();
            services.AddScoped<IRepository<ProviderProfile>, BaseRepository<ProviderProfile>>();
            services.AddScoped<IRepository<Order>, BaseRepository<Order>>();
            services.AddScoped<IRepository<MyUser>, BaseRepository<MyUser>>();
            services.AddScoped<IRepository<Feedback>, BaseRepository<Feedback>>();

            services.AddIdentity<MyUser, IdentityRole>
            (
                config =>
                {
                    config.Cookies.ApplicationCookie.AccessDeniedPath = "/Account/AccessDenied";
                }
            ).AddEntityFrameworkStores<GrandeTravelDbContext>();
            services.AddDbContext<GrandeTravelDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, GrandeTravelDbContext context, UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(context, userManager, roleManager);
        }
    }
}
