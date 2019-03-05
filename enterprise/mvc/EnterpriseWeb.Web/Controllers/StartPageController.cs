using EnterpriseWeb.Data.Pages;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using System;

namespace EnterpriseWeb.Web.Controllers
{
    public class StartPageController : Controller
    {
        private readonly IApi api;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>
        public StartPageController(IApi api) {
            this.api = api;
        }

        /// <summary>
        /// Gets the page with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        public IActionResult Index(Guid id) {
            var model = api.Pages.GetById<StartPage>(id);

            return View(model);
        }
    }
}
