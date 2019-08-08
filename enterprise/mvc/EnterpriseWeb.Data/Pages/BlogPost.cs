using EnterpriseWeb.Data.Regions;
using Piranha.AttributeBuilder;
using Piranha.Models;

namespace EnterpriseWeb.Data.Pages
{
    [PostType(Title = "Blog post")]
    [PageTypeRoute(Title = "Default", Route = "/blog/post")]
    public class BlogPost  : Post<BlogPost>
    {
        /// <summary>
        /// Gets/sets the hero region.
        /// </summary>
        [Region(Display = RegionDisplayMode.Setting)]
        public HeroRegion Hero { get; set; }
    }
}