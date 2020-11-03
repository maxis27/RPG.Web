using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using RPG.Web.Middleware;
using RPG.Web.Models;

namespace RPG.Web
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options
                .UseMySql(_config.GetConnectionString("DbConnection"), mysqlOptions => mysqlOptions
                    .ServerVersion(new Version(10, 3, 22), ServerType.MariaDb)
            ));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => 
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(options => {
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.EnableEndpointRouting = false;
            }).AddXmlSerializerFormatters();
            
            services.AddScoped<IChangesRepository, SQLChangesRepository>();
            services.AddScoped<IWorkRepository, SQLWorkRepository>();
            services.AddScoped<ITavernRepository, SQLTavernRepository>();
            services.AddScoped<IMailRepository, SQLMailRepository>();
            services.AddScoped<IItemRepository, SQLItemRepository>();
            services.AddScoped<IInventoryRepository, SQLInventoryRepository>();
            services.AddScoped<ILocationRepository, SQLLocationRepository>();
            services.AddScoped<ITripRepository, SQLTripRepository>();
            services.AddScoped<ICommentRepository, SQLCommentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseGameMiddleware();

            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
