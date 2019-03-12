using EnterpriseWeb.Data.Regions;
using Piranha.AttributeBuilder;
using Piranha.Models;

namespace EnterpriseWeb.Data.Pages
{
    [PageType(Title = "Blog archive", UseBlocks = false)]
    [PageTypeRoute(Title = "Default", Route = "/blog/listing")]
    public class BlogArchive  : ArchivePage<BlogArchive>
    {
        /// <summary>
        /// Gets/sets the archive heading.
        /// </summary>
        [Region]
        public HeadingRegion Heading { get; set; }
    }
}