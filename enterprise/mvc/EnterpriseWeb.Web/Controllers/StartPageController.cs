using EnterpriseWeb.Data.Pages;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using System;
using System.Threading.Tasks;

namespace EnterpriseWeb.Web.Controllers
{
    public class StartPageController : Controller
    {
        private readonly IApi _api;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>
        public StartPageController(IApi api) {
            _api = api;
        }

        /// <summary>
        /// Gets the page with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        public async Task<IActionResult> Index(Guid id) {
            var model = await _api.Pages.GetByIdAsync<StartPage>(id);

            return View(model);
        }
    }
}
