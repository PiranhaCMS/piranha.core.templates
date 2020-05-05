using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;

namespace RazorBlog.Models
{
    [PostType(Title = "Blog post")]
    public class BlogPost : Post<BlogPost>
    {
    }
}