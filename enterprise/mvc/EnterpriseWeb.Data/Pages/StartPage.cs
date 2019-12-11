using EnterpriseWeb.Data.Regions;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using System.Collections.Generic;

namespace EnterpriseWeb.Data.Pages
{
    [PageType(Title = "Start page")]
    [PageTypeRoute(Title = "Default", Route = "/startpage")]
    public class StartPage : Page<StartPage>
    {
        /// <summary>
        /// Gets/sets the page hero.
        /// </summary>
        [Region(Display = RegionDisplayMode.Setting)]
        public HeroRegion Hero { get; set; }

        /// <summary>
        /// Gets/sets the available teasers.
        /// </summary>
        [Region(ListTitle = "Title")]
        public IList<TeaserRegion> Teasers { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StartPage() {
            Teasers = new List<TeaserRegion>();
        }
    }
}