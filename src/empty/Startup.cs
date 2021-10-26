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

namespace Empty
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
                options.UseCms();
                options.UseManager();
                
#if (UseBlobStorage)
                options.UseBlobStorage(_config.GetConnectionString("blobstorage"), naming: Piranha.Azure.BlobStorageNaming.UniqueFolderNames);
#else
                options.UseFileStorage(naming: Piranha.Local.FileStorageNaming.UniqueFolderNames);
#endif
                options.UseImageSharp();
                options.UseTinyMCE();
                options.UseMemoryCache();

                var connectionString = _config.GetConnectionString("piranha");
#if (UseSQLServer)
                options.UseEF<SQLServerDb>(db => db.UseSqlServer(connectionString));
                options.UseIdentityWithSeed<IdentitySQLServerDb>(db => db.UseSqlServer(connectionString));
#elif (UseMySql)
                options.UseEF<MySqlDb>(db =>
                    db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
                options.UseIdentityWithSeed<IdentityMySQLDb>(db =>
                    db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
#elif (UsePostgreSql)
                options.UseEF<PostgreSqlDb>(db => db.UseNpgsql(connectionString));
                options.UseIdentityWithSeed<IdentityPostgreSQLDb>(db => db.UseNpgsql(connectionString));
#else
                options.UseEF<SQLiteDb>(db => db.UseSqlite(connectionString));
                options.UseIdentityWithSeed<IdentitySQLiteDb>(db => db.UseSqlite(connectionString));
#endif

                /**
                 * Here you can configure the different permissions
                 * that you want to use for securing content in the
                 * application.
                options.UseSecurity(o =>
                {
                    o.UsePermission("WebUser", "Web User");
                });
                 */

                /**
                 * Here you can specify the login url for the front end
                 * application. This does not affect the login url of
                 * the manager interface.
                options.LoginUrl = "login";
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
