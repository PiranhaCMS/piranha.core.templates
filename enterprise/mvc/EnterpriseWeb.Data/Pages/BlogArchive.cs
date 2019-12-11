using EnterpriseWeb.Data.Regions;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;

namespace EnterpriseWeb.Data.Pages
{
    [PageType(Title = "Blog archive", UseBlocks = false, IsArchive = true)]
    [PageTypeRoute(Title = "Default", Route = "/blog/listing")]
    public class BlogArchive  : Page<BlogArchive>
    {
        /// <summary>
        /// Gets/sets the page header.
        /// </summary>
        [Region(Display = RegionDisplayMode.Setting)]
        public HeroRegion Hero { get; set; }

        /// <summary>
        /// Gets/sets the archive.
        /// </summary>
        public PostArchive<DynamicPost> Archive { get; set; }
    }
}