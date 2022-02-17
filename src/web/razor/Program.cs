using System.Reflection;
//using Microsoft.AspNetCore.HttpOverrides;
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

builder.Services.AddPiranha(options =>
{
    /*
     * This will enable automatic reload of .cshtml
     * without restarting the application. However since
     * this adds a slight overhead it should not be
     * enabled in production.
     */
    options.AddRazorRuntimeCompilation = true;

    options.UseCms();
    options.UseManager();

#if (UseBlobStorage)
    options.UseBlobStorage(builder.Configuration.GetConnectionString("blobstorage"), naming: Piranha.Azure.BlobStorageNaming.UniqueFolderNames);
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
    options.UseEF<MySqlDb>(db => db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    options.UseIdentityWithSeed<IdentityMySQLDb>(db => db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
#elif (UsePostgreSql)
    options.UseEF<PostgreSqlDb>(db => db.UseNpgsql(connectionString));
    options.UseIdentityWithSeed<IdentityPostgreSQLDb>(db => db.UseNpgsql(connectionString));
#else
    options.UseEF<SQLiteDb>(db => db.UseSqlite(connectionString));
    options.UseIdentityWithSeed<IdentitySQLiteDb>(db => db.UseSqlite(connectionString));
#endif

    /*
     * Here you can configure the different permissions
     * that you want to use for securing content in the
     * application.
    options.UseSecurity(o =>
    {
        o.UsePermission("WebUser", "Web User");
    });
     */

    /*
     * Here you can specify the login url for the front end
     * application. This does not affect the login url of
     * the manager interface.
    options.LoginUrl = "login";
     */
});

var app = builder.Build();

// For application behind a reverse proxy, accept forwarded headers
// Nginx: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-6.0
// Apache: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-6.0
//app.UseForwardedHeaders(new ForwardedHeadersOptions
//{
//    ForwardedHeaders = ForwardedHeaders.All
//});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    var api = scope.ServiceProvider.GetRequiredService<IApi>();

    // Initialize Piranha
    App.Init(api);

    // Build content types
    await (await new ContentTypeBuilder(api)
            .AddAssembly(Assembly.GetExecutingAssembly())
            .BuildAsync())
        .DeleteOrphansAsync();
}

// Configure Tiny MCE
EditorConfig.FromFile("editorconfig.json");

// Middleware setup
app.UsePiranha(options =>
{
    options.UseManager();
    options.UseTinyMCE();
    options.UseIdentity();
});

await app.RunAsync();