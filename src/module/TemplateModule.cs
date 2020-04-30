using Piranha;
using Piranha.Extend;
using Piranha.Manager;
using Piranha.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateModule
{
    public class TemplateModule : IModule
    {
        private readonly List<PermissionItem> _permissions = new List<PermissionItem>
        {
            new PermissionItem { Name = Permissions.TemplateModule, Title = "List TemplateModule content", Category = "TemplateModule", IsInternal = true },
            new PermissionItem { Name = Permissions.TemplateModuleAdd, Title = "Add TemplateModule content", Category = "TemplateModule", IsInternal = true },
            new PermissionItem { Name = Permissions.TemplateModuleEdit, Title = "Edit TemplateModule content", Category = "TemplateModule", IsInternal = true },
            new PermissionItem { Name = Permissions.TemplateModuleDelete, Title = "Delete TemplateModule content", Category = "TemplateModule", IsInternal = true }
        };

        /// <summary>
        /// 
        /// </summary>
        public string Author => "";

        /// <summary>
        /// 
        /// </summary>
        public string Name => "";

        /// <summary>
        /// 
        /// </summary>
        public string Version => Utils.GetAssemblyVersion(GetType().Assembly);

        /// <summary>
        /// 
        /// </summary>
        public string Description => "";

        /// <summary>
        /// 
        /// </summary>
        public string PackageUrl => "";

        /// <summary>
        /// 
        /// </summary>
        public string IconUrl => "/manager/PiranhaModule/piranha-logo.png";

        public void Init()
        {
            // Register permissions
            foreach (var permission in _permissions)
            {
                App.Permissions["PiranhaModule"].Add(permission);
            }

            // Add manager menu items
            Menu.Items["PiranhaModule"].Items.Add(new MenuItem
            {
                InternalId = "PiranhaModule",
                Name = "PiranhaModule",
                Route = "~/manager/PiranhaModule",
                Policy = Permissions.TemplateModule,
                Css = "fas fa-box"
            });
        }
    }
}
