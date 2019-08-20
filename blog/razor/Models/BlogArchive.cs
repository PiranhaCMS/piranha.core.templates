using Piranha.AttributeBuilder;
using Piranha.Models;

namespace RazorBlog.Models
{
    [PageType(Title = "Blog archive", UseBlocks = false)]
    public class BlogArchive  : ArchivePage<BlogArchive>
    {
    }
}