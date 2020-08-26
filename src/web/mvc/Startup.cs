using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Piranha;
using Piranha.AttributeBuilder;
#if (UseSQLServer)
using Piranha.AspNetCore.Identity.SQLServer;
using Piranha.Data.EF.SQLServer;
#elif (UseMySql)
using Piranha.AspNetCore.Identity.MySQL;
using Piranha.Data.EF.MySql;
#elif (UsePostgreSql)
using Piranha.AspNetCore.Identity.PostgreSQL;
using Piranha.Data.EF.PostgreSql;
#else
using Piranha.AspNetCore.Identity.SQLite;
using Piranha.Data.EF.SQLite;
#endif
using Piranha.Manager.Editor;

namespace MvcWeb
{
    public class Startup
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="configuration">The current configuration</param>
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Service setup
            services.AddPiranha(options =>
            {
                options.AddRazorRuntimeCompilation = true;

#if (UseBlobStorage)
                options.UseBlobStorage(_config.GetConnectionString("blobstorage"));
#else
                options.UseFileStorage(naming: Piranha.Local.FileStorageNaming.UniqueFolderNames);
#endif
                options.UseImageSharp();
                options.UseManager();
                options.UseTinyMCE();
                options.UseMemoryCache();
#if (UseSQLServer)
                options.UseEF<SQLServerDb>(db =>
                    db.UseSqlServer(_config.GetConnectionString("piranha")));
                options.UseIdentityWithSeed<IdentitySQLServerDb>(db =>
                    db.UseSqlServer(_config.GetConnectionString("piranha")));
#elif (UseMySql)
                options.UseEF<MySqlDb>(db =>
                    db.UseMySql(_config.GetConnectionString("piranha")));
                options.UseIdentityWithSeed<IdentityMySQLDb>(db =>
                    db.UseMySql(_config.GetConnectionString("piranha")));
#elif (UsePostgreSql)
                options.UseEF<PostgreSqlDb>(db =>
                    db.UseNpgsql(_config.GetConnectionString("piranha")));
                options.UseIdentityWithSeed<IdentityPostgreSQLDb>(db =>
                    db.UseNpgsql(_config.GetConnectionString("piranha")));
#else
                options.UseEF<SQLiteDb>(db =>
                    db.UseSqlite(_config.GetConnectionString("piranha")));
                options.UseIdentityWithSeed<IdentitySQLiteDb>(db =>
                    db.UseSqlite(_config.GetConnectionString("piranha")));
#endif

                /***
                 * Here you can configure the different permissions
                 * that you want to use for securing content in the
                 * application.
                options.UseSecurity(o =>
                {
                    o.UsePermission("WebUser", "Web User");
                });
                 */
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApi api)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Initialize Piranha
            App.Init(api);

            // Build content types
            new ContentTypeBuilder(api)
                .AddAssembly(typeof(Startup).Assembly)
                .Build()
                .DeleteOrphans();

            // Configure Tiny MCE
            EditorConfig.FromFile("editorconfig.json");

            // Middleware setup
            app.UsePiranha(options => {
                options.UseManager();
                options.UseTinyMCE();
                options.UseIdentity();
            });
        }
    }
}
