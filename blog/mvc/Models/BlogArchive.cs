using Piranha.AttributeBuilder;
using Piranha.Models;

namespace MvcBlog.Models
{
    [PageType(Title = "Blog archive", UseBlocks = false, IsArchive = true)]
    public class BlogArchive  : Page<BlogArchive>
    {
        /// <summary>
        /// Gets/sets the post archive.
        /// </summary>
        public PostArchive<PostInfo> Archive { get; set; }
    }
}