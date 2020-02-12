using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using Piranha.AspNetCore.Services;
using RazorBlog.Models;

namespace RazorBlog.Pages
{
    public class ArchiveModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly IApi _api;
        private readonly IModelLoader _loader;

        public BlogArchive Data { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="loader"></param>
        public ArchiveModel(IApi api, IModelLoader loader) : base()
        {
            _api = api;
            _loader = loader;;
        }

        /// <summary>
        /// Gets the model data.
        /// </summary>
        /// <param name="id">The page id</param>
        /// <param name="year">The optional year</param>
        /// <param name="month">The optional month</param>
        /// <param name="pagenum">The optional page number</param>
        /// <param name="category">The optional category</param>
        /// <param name="tag">The optional draft</param>
        /// <param name="draft">If the draft should be fetched</param>
        /// <returns>The result</returns>
        public async Task<IActionResult> OnGet(Guid id, int? year = null, int? month = null, int? pagenum = null,
            Guid? category = null, Guid? tag = null, bool draft = false)
        {
            Data = await _loader.GetPageAsync<Models.BlogArchive>(id, HttpContext.User, draft);

            if (Data != null)
            {
                Data.Archive = await _api.Archives.GetByIdAsync(id, pagenum, category, tag, year, month);

                return Page();
            }
            return NotFound();
        }
    }
}