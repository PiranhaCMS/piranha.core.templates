using Piranha.AttributeBuilder;
using Piranha.Models;

namespace RazorBlog.Models
{
    [PostType(Title = "Blog post")]
    public class BlogPost : Post<BlogPost>
    {
        /// <summary>
        /// Gets/sets the hero.
        /// </summary>
        [Region(Display = RegionDisplayMode.Setting)]
        public Regions.Hero Hero { get; set; }
    }
}