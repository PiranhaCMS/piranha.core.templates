using Piranha.AttributeBuilder;
using Piranha.Models;

namespace EnterpriseWeb.Data.Pages
{
    [PageType(Title = "Standard page")]
    [PageTypeRoute(Title = "Default", Route = "/standardpage")]
    public class StandardPage  : Page<StandardPage>
    {
    }
}