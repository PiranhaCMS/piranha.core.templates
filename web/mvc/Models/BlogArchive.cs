using MvcWeb.Models.Regions;
using Piranha.AttributeBuilder;
using Piranha.Models;

namespace MvcWeb.Models
{
    [PageType(Title = "Blog archive", UseBlocks = false)]
    public class BlogArchive  : ArchivePage<BlogArchive>
    {
        /// <summary>
        /// Gets/sets the archive heading.
        /// </summary>
        [Region]
        public Heading Heading { get; set; }
    }
}