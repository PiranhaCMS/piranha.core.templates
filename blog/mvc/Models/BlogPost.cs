using Piranha.AttributeBuilder;
using Piranha.Models;

namespace MvcBlog.Models
{
    [PostType(Title = "Blog post")]
    public class BlogPost : Post<BlogPost>
    {
        /// <summary>
        /// Gets/sets the heading.
        /// </summary>
        [Region]
        public Regions.Heading Heading { get; set; }
    }
}