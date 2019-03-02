using MvcWeb.Models.Regions;
using Piranha.AttributeBuilder;
using Piranha.Models;

namespace MvcWeb.Models
{
    [PostType(Title = "Blog post")]
    public class BlogPost  : Post<BlogPost>
    {
        /// <summary>
        /// Gets/sets the post heading.
        /// </summary>
        [Region]
        public Heading Heading { get; set; }
    }
}