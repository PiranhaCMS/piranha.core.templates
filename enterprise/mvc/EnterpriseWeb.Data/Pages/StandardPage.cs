using EnterpriseWeb.Data.Regions;
using Piranha.AttributeBuilder;
using Piranha.Models;

namespace EnterpriseWeb.Data.Pages
{
    [PageType(Title = "Standard page")]
    [PageTypeRoute(Title = "Default", Route = "/standardpage")]
    public class StandardPage  : Page<StandardPage>
    {
        /// <summary>
        /// Gets/sets the page header.
        /// </summary>
        [Region(Display = RegionDisplayMode.Setting)]
        public HeroRegion Hero { get; set; }
    }
}