using EnterpriseWeb.Data.Pages;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using System;
using System.Threading.Tasks;

namespace EnterpriseWeb.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IApi _api;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>
        public BlogController(IApi api) {
            _api = api;
        }

        /// <summary>
        /// Gets the blog archive with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        /// <param name="year">The optional year</param>
        /// <param name="month">The optional month</param>
        /// <param name="page">The optional page</param>
        /// <param name="category">The optional category</param>
        /// <param name="tag">The optional tag</param>
        public async Task<IActionResult> Listing(Guid id, int? year = null, int? month = null, int? page = null, 
            Guid? category = null, Guid? tag = null)
        {
            var model = await _api.Pages.GetByIdAsync<BlogArchive>(id);
            model.Archive = await _api.Archives.GetByIdAsync(id, page, category, tag, year, month);

            return View(model);
        }

        /// <summary>
        /// Gets the post with the given id.
        /// </summary>
        /// <param name="id">The unique post id</param>
        [Route("post")]
        public async Task<IActionResult> Post(Guid id) {
            var model = await _api.Posts.GetByIdAsync<BlogPost>(id);

            return View(model);
        }
    }
}
