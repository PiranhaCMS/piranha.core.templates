using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Piranha;
using Piranha.AspNetCore;
using TemplateModule;

public static class TemplateModuleExtensions
{
    /// <summary>
    /// Adds the TemplateModule module.
    /// </summary>
    /// <param name="serviceBuilder"></param>
    /// <returns></returns>
    public static PiranhaServiceBuilder UseTemplateModule(this PiranhaServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.AddTemplateModule();

        return serviceBuilder;
    }

    /// <summary>
    /// Uses the TemplateModule module.
    /// </summary>
    /// <param name="applicationBuilder">The current application builder</param>
    /// <returns>The builder</returns>
    public static PiranhaApplicationBuilder UseTemplateModule(this PiranhaApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Builder.UseTemplateModule();

        return applicationBuilder;
    }

    /// <summary>
    /// Adds the TemplateModule module.
    /// </summary>
    /// <param name="services">The current service collection</param>
    /// <returns>The services</returns>
    public static IServiceCollection AddTemplateModule(this IServiceCollection services)
    {
        // Add the TemplateModule module
        Piranha.App.Modules.Register<Module>();

        // Setup authorization policies
        services.AddAuthorization(o =>
        {
            // TemplateModule policies
            o.AddPolicy(Permissions.TemplateModule, policy =>
            {
                policy.RequireClaim(Permissions.TemplateModule, Permissions.TemplateModule);
            });

            // TemplateModule add policy
            o.AddPolicy(Permissions.TemplateModuleAdd, policy =>
            {
                policy.RequireClaim(Permissions.TemplateModule, Permissions.TemplateModule);
                policy.RequireClaim(Permissions.TemplateModuleAdd, Permissions.TemplateModuleAdd);
            });

            // TemplateModule edit policy
            o.AddPolicy(Permissions.TemplateModuleEdit, policy =>
            {
                policy.RequireClaim(Permissions.TemplateModule, Permissions.TemplateModule);
                policy.RequireClaim(Permissions.TemplateModuleEdit, Permissions.TemplateModuleEdit);
            });

            // TemplateModule delete policy
            o.AddPolicy(Permissions.TemplateModuleDelete, policy =>
            {
                policy.RequireClaim(Permissions.TemplateModule, Permissions.TemplateModule);
                policy.RequireClaim(Permissions.TemplateModuleDelete, Permissions.TemplateModuleDelete);
            });
        });

        // Return the service collection
        return services;
    }

    /// <summary>
    /// Uses the TemplateModule.
    /// </summary>
    /// <param name="builder">The application builder</param>
    /// <returns>The builder</returns>
    public static IApplicationBuilder UseTemplateModule(this IApplicationBuilder builder)
    {
        return builder.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "TemplateModule.assets.dist"),
            RequestPath = "/manager/TemplateModule"
        });
    }

    /// <summary>
    /// Static accessor to TemplateModule module if it is registered in the Piranha application.
    /// </summary>
    /// <param name="modules">The available modules</param>
    /// <returns>The TemplateModule module</returns>
    public static Module TemplateModule(this Piranha.Runtime.AppModuleList modules)
    {
        return modules.Get<Module>();
    }
}
