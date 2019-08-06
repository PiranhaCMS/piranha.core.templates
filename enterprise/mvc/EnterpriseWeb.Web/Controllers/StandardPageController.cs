using EnterpriseWeb.Data.Pages;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using Piranha.AspNetCore.Services;
using System;
using System.Threading.Tasks;

namespace EnterpriseWeb.Web.Controllers
{
    public class StandardPageController : Controller
    {
        private readonly IApi _api;
        private readonly IModelLoader _loader;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>
        public StandardPageController(IApi api, IModelLoader loader)
        {
            _api = api;
            _loader = loader;
        }

        /// <summary>
        /// Gets the page with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        /// <param name="draft">If a draft is requested</param>
        public async Task<IActionResult> Index(Guid id, bool draft = false)
        {
            var model = await _loader.GetPage<StandardPage>(id, HttpContext.User, draft);

            return View(model);
        }
    }
}
