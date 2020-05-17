using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;

namespace MvcBlog.Models
{
    [PostType(Title = "Blog post")]
    public class BlogPost : Post<BlogPost>
    {
    }
}