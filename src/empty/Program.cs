using Microsoft.EntityFrameworkCore;
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

var builder = WebApplication.CreateBuilder(args);

builder.AddPiranha(options =>
{
    /**
     * This will enable automatic reload of .cshtml
     * without restarting the application. However since
     * this adds a slight overhead it should not be
     * enabled in production.
     */
    options.AddRazorRuntimeCompilation = true;

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

    var connectionString = builder.Configuration.GetConnectionString("piranha");
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UsePiranha(options =>
{
    // Initialize Piranha
    App.Init(options.Api);

    // Build content types
    new ContentTypeBuilder(options.Api)
        .AddAssembly(typeof(Program).Assembly)
        .Build()
        .DeleteOrphans();

    // Configure Tiny MCE
    EditorConfig.FromFile("editorconfig.json");

    options.UseManager();
    options.UseTinyMCE();
    options.UseIdentity();
});

app.Run();