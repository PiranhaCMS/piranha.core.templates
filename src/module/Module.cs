using Piranha;
using Piranha.Extend;
using Piranha.Manager;
using Piranha.Security;

namespace TemplateModule;

public class Module : IModule
{
    private readonly List<PermissionItem> _permissions = new List<PermissionItem>
    {
        new PermissionItem { Name = Permissions.TemplateModule, Title = "List TemplateModule content", Category = "TemplateModule", IsInternal = true },
        new PermissionItem { Name = Permissions.TemplateModuleAdd, Title = "Add TemplateModule content", Category = "TemplateModule", IsInternal = true },
        new PermissionItem { Name = Permissions.TemplateModuleEdit, Title = "Edit TemplateModule content", Category = "TemplateModule", IsInternal = true },
        new PermissionItem { Name = Permissions.TemplateModuleDelete, Title = "Delete TemplateModule content", Category = "TemplateModule", IsInternal = true }
    };

    /// <summary>
    /// Gets the module author
    /// </summary>
    public string Author => "";

    /// <summary>
    /// Gets the module name
    /// </summary>
    public string Name => "";

    /// <summary>
    /// Gets the module version
    /// </summary>
    public string Version => Utils.GetAssemblyVersion(GetType().Assembly);

    /// <summary>
    /// Gets the module description
    /// </summary>
    public string Description => "";

    /// <summary>
    /// Gets the module package url
    /// </summary>
    public string PackageUrl => "";

    /// <summary>
    /// Gets the module icon url
    /// </summary>
    public string IconUrl => "/manager/PiranhaModule/piranha-logo.png";

    public void Init()
    {
        // Register permissions
        foreach (var permission in _permissions)
        {
            App.Permissions["TemplateModule"].Add(permission);
        }

        // Add manager menu items
        Menu.Items.Add(new MenuItem
        {
            InternalId = "TemplateModule",
            Name = "TemplateModule",
            Css = "fas fa-box"
        });
        Menu.Items["TemplateModule"].Items.Add(new MenuItem
        {
            InternalId = "TemplateModuleStart",
            Name = "Module Start",
            Route = "~/manager/templatemodule",
            Policy = Permissions.TemplateModule,
            Css = "fas fa-box"
        });
    }
}
