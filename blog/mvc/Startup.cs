﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Piranha;
using Piranha.AspNetCore.Identity.SQLite;

namespace MvcBlog
{
    public class Startup
    {
        /// <summary>
        /// The application config.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="env">The current hosting environment</param>
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.ModelBinderProviders.Insert(0, new Piranha.Manager.Binders.AbstractModelBinderProvider());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddPiranhaApplication();
            services.AddPiranhaFileStorage();
            services.AddPiranhaImageSharp();
            services.AddPiranhaManager();
            services.AddPiranhaMemoryCache();

            //
            // Setup Piranha & Asp.Net Identity for EF SQLite
            //
            services.AddPiranhaEF(options =>
                options.UseSqlite(Configuration.GetConnectionString("piranha")));
            services.AddPiranhaIdentityWithSeed<IdentitySQLiteDb>(options =>
                options.UseSqlite(Configuration.GetConnectionString("piranha")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApi api)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Initialize Piranha
            App.Init();

            // Configure cache level
            App.CacheLevel = Piranha.Cache.CacheLevel.Basic;

            // Build content types
            var pageTypeBuilder = new Piranha.AttributeBuilder.PageTypeBuilder(api)
                .AddType(typeof(Models.BlogArchive))
                .AddType(typeof(Models.StandardPage));
            pageTypeBuilder.Build()
                .DeleteOrphans();
            var postTypeBuilder = new Piranha.AttributeBuilder.PostTypeBuilder(api)
                .AddType(typeof(Models.BlogPost));
            postTypeBuilder.Build()
                .DeleteOrphans();
            var siteTypeBuilder = new Piranha.AttributeBuilder.SiteTypeBuilder(api)
                .AddType(typeof(Models.BlogSite));
            siteTypeBuilder.Build()
                .DeleteOrphans();

            // Register middleware
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UsePiranha();
            app.UsePiranhaManager();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
