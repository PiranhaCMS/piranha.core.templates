using Microsoft.AspNetCore.Mvc;
using MvcBlog.Models;
using Piranha;
using System;
using System.Threading.Tasks;

namespace MvcBlog.Controllers
{
    public class CmsController : Controller
    {
        private readonly IApi _api;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>
        public CmsController(IApi api) 
        {
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
        [Route("archive")]
        public async Task<IActionResult> Archive(Guid id, int? year = null, int? month = null, int? page = null, 
            Guid? category = null, Guid? tag = null)
        {
            var model = await _api.Pages.GetByIdAsync<BlogArchive>(id);

            model.Archive = await _api.Archives.GetByIdAsync(id, page, category, tag, year, month);

            return View(model);
        }

        /// <summary>
        /// Gets the page with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        [Route("page")]
        public async Task<IActionResult> Page(Guid id)
        {
            var model = await _api.Pages.GetByIdAsync<Models.StandardPage>(id);

            return View(model);
        }

        /// <summary>
        /// Gets the page with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        [Route("pagewide")]
        public async Task<IActionResult> PageWide(Guid id)
        {
            var model = await _api.Pages.GetByIdAsync<Models.StandardPage>(id);

            return View(model);
        }

        /// <summary>
        /// Gets the post with the given id.
        /// </summary>
        /// <param name="id">The unique post id</param>
        ///
        [Route("post")]
        public async Task<IActionResult> Post(Guid id)
        {
            var model = await _api.Posts.GetByIdAsync<Models.BlogPost>(id);

            return View(model);
        }
    }
}
