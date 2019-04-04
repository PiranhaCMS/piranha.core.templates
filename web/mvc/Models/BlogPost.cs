using MvcWeb.Models.Regions;
using Piranha.AttributeBuilder;
using Piranha.Models;

namespace MvcWeb.Models
{
    [PostType(Title = "Blog post")]
    public class BlogPost  : Post<BlogPost>
    {
        /// <summary>
        /// Gets/sets the post hero.
        /// </summary>
        [Region]
        public Hero Hero { get; set; }
    }
}