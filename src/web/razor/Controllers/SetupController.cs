using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Extend.Blocks;
using Piranha.Models;
using RazorWeb.Models;

namespace RazorWeb.Controllers
{
    /// <summary>
    /// This controller is only used when the project is first started
    /// and no pages has been added to the database. Feel free to remove it.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SetupController : Controller
    {
        private readonly IApi _api;

        public SetupController(IApi api)
        {
            _api = api;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/seed")]
        public async Task<IActionResult> Seed()
        {
            var images = new Dictionary<string, Guid>();

            // Get the default site
            var site = await _api.Sites.GetDefaultAsync();

            // Add media assets
            foreach (var image in Directory.GetFiles("seed"))
            {
                var info = new FileInfo(image);
                var id = Guid.NewGuid();
                images.Add(info.Name, id);

                using (var stream = System.IO.File.OpenRead(image))
                {
                    await _api.Media.SaveAsync(new Piranha.Models.StreamMediaContent()
                    {
                        Id = id,
                        Filename = info.Name,
                        Data = stream
                    });
                }
            }

            // Add blog page
            var blogPage = await StandardArchive.CreateAsync(_api);
            blogPage.Id = Guid.NewGuid();
            blogPage.SiteId = site.Id;
            blogPage.Title = "Blog Archive";
            blogPage.NavigationTitle = "Blog";
            blogPage.MetaKeywords = "Purus, Amet, Ullamcorper, Fusce";
            blogPage.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet.";
            blogPage.PrimaryImage = images["woman-in-blue-long-sleeve-dress-standing-beside-brown-wooden-4100766.jpg"];
            blogPage.Excerpt = "Keep yourself updated with the latest and greatest news. All of this knowledge is at your fingertips, what are you waiting for?";
            blogPage.Published = DateTime.Now;

            await _api.Pages.SaveAsync(blogPage);

            // Add docs page
            var docsPage = await StandardPage.CreateAsync(_api);
            docsPage.Id = Guid.NewGuid();
            docsPage.SiteId = site.Id;
            docsPage.SortOrder = 1;
            docsPage.Title = "Read The Docs";
            docsPage.NavigationTitle = "Docs";
            docsPage.MetaKeywords = "Purus, Amet, Ullamcorper, Fusce";
            docsPage.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet.";
            docsPage.RedirectUrl = "https://piranhacms.org/docs";
            docsPage.RedirectType = RedirectType.Temporary;
            docsPage.PrimaryImage = images["man-in-red-jacket-standing-on-the-stairs-4390730.jpg"];
            docsPage.Excerpt = "Ready to get started! Then head over to our official documentation and learn more about Piranha and how to use it.";
            docsPage.Published = DateTime.Now;

            await _api.Pages.SaveAsync(docsPage);


            // Add start page
            var startPage = await StandardPage.CreateAsync(_api);
            startPage.Id = Guid.NewGuid();
            startPage.SiteId = site.Id;
            startPage.Title = "Welcome To Piranha CMS";
            startPage.NavigationTitle = "Home";
            startPage.MetaKeywords = "Purus, Amet, Ullamcorper, Fusce";
            startPage.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet.";
            startPage.PrimaryImage = images["cute-business-kids-working-on-project-together-surfing-3874121.jpg"];
            startPage.Excerpt = "Welcome to your brand new website. To show some of the features that you have available at your fingertips we have created some example content for you.";
            startPage.Published = DateTime.Now;

            startPage.Blocks.Add(new HtmlBlock
            {
                Body =
                    "<h2>Because First Impressions Last</h2>" +
                    "<p class=\"lead\">All pages and posts you create have a primary image and excerpt available that you can use both to create nice looking headers for your content, but also when listing or linking to it on your site. These fields are totally optional and can be disabled for each content type.</p>"
            });
            startPage.Blocks.Add(new ColumnBlock
            {
                Items = new List<Block>()
                {
                    new ImageBlock
                    {
                        Aspect = new SelectField<ImageAspect>
                        {
                            Value = ImageAspect.Widescreen
                        },
                        Body = images["concentrated-little-kids-taking-notes-in-organizer-and-3874109.jpg"]
                    },
                    new HtmlBlock
                    {
                        Body =
                            "<h3>Add, Edit & Rearrange</h3>" +
                            "<p class=\"lead\">Build your content with our powerful and modular block editor that allows you to add, rearrange and layout your content with ease.</p>" +
                            "<p>New content blocks can be installed or created in your project and will be available to use across all content functions. Build complex regions for all of the fixed content you want on your content types.</p>"
                    }
                }
            });
            startPage.Blocks.Add(new HtmlBlock
            {
                Body =
                    "<h3>Cross-Link Your Content</h3>" +
                    "<p>With our new Page and Post Link blocks it's easier than ever to promote, and link to your content across the site. Simple select the content you want to reference and simply use it's basic fields including Primary Image & Excerpt to display it.</p>"
            });
            startPage.Blocks.Add(new ColumnBlock
            {
                Items = new List<Block>
                {
                    new PageBlock
                    {
                        Body = blogPage
                    },
                    new PageBlock
                    {
                        Body = docsPage
                    }
                }
            });
            startPage.Blocks.Add(new HtmlBlock
            {
                Body =
                    "<h2>Share Your Images</h2>" +
                    "<p>An image says more that a thousand words. With our <strong>Image Gallery</strong> you can easily create a gallery or carousel and share anything you have available in your media library or download new images directly on your page by just dragging them to your browser.</p>"
            });
            startPage.Blocks.Add(new ImageGalleryBlock
            {
                Items = new List<Block>
                {
                    new ImageBlock
                    {
                        Body = images["cheerful-diverse-colleagues-working-on-laptops-in-workspace-3860809.jpg"]
                    },
                    new ImageBlock
                    {
                        Body = images["smiling-woman-working-in-office-with-coworkers-3860641.jpg"]
                    },
                    new ImageBlock
                    {
                        Body = images["diverse-group-of-colleagues-having-meditation-together-3860619.jpg"]
                    }
                }
            });

            await _api.Pages.SaveAsync(startPage);

            // Add blog posts
            var post1 = await StandardPost.CreateAsync(_api);
            post1.BlogId = blogPage.Id;
            post1.Category = "Magna";
            post1.Tags.Add("Euismod", "Ridiculus");
            post1.Title = "Tortor Magna Ultricies";
            post1.MetaKeywords = "Nibh, Vulputate, Venenatis, Ridiculus";
            post1.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet. Maecenas faucibus mollis interdum.";
            post1.PrimaryImage = images["smiling-woman-working-in-office-with-coworkers-3860641.jpg"];
            post1.Excerpt = "Maecenas faucibus mollis interdum. Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec sed odio dui.";
            post1.Published = DateTime.Now;

            post1.Blocks.Add(new HtmlBlock
            {
                Body =
                    "<p>Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Vestibulum id ligula porta felis euismod semper. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam quis risus eget urna mollis ornare vel eu leo. Nullam id dolor id nibh ultricies vehicula ut id elit. Nullam quis risus eget urna mollis ornare vel eu leo. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.</p>" +
                    "<p>Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum. Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Maecenas sed diam eget risus varius blandit sit amet non magna. Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas faucibus mollis interdum. Cras mattis consectetur purus sit amet fermentum. Donec ullamcorper nulla non metus auctor fringilla.</p>" +
                    "<p>Sed posuere consectetur est at lobortis. Maecenas faucibus mollis interdum. Sed posuere consectetur est at lobortis. Morbi leo risus, porta ac consectetur ac, vestibulum at eros. Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas faucibus mollis interdum.</p>"
            });
            await _api.Posts.SaveAsync(post1);

            var post2 = await StandardPost.CreateAsync(_api);
            post2.BlogId = blogPage.Id;
            post2.Category = "Tristique";
            post2.Tags.Add("Euismod", "Ridiculus");
            post2.Title = "Sollicitudin Risus Dapibus";
            post2.MetaKeywords = "Nibh, Vulputate, Venenatis, Ridiculus";
            post2.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet. Maecenas faucibus mollis interdum.";
            post2.PrimaryImage = images["concentrated-little-kids-taking-notes-in-organizer-and-3874109.jpg"];
            post2.Excerpt = "Donec sed odio dui. Maecenas faucibus mollis interdum. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.";
            post2.Published = DateTime.Now;

            post2.Blocks.Add(new HtmlBlock
            {
                Body =
                    "<p>Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Vestibulum id ligula porta felis euismod semper. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam quis risus eget urna mollis ornare vel eu leo. Nullam id dolor id nibh ultricies vehicula ut id elit. Nullam quis risus eget urna mollis ornare vel eu leo. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.</p>" +
                    "<p>Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum. Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Maecenas sed diam eget risus varius blandit sit amet non magna. Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas faucibus mollis interdum. Cras mattis consectetur purus sit amet fermentum. Donec ullamcorper nulla non metus auctor fringilla.</p>" +
                    "<p>Sed posuere consectetur est at lobortis. Maecenas faucibus mollis interdum. Sed posuere consectetur est at lobortis. Morbi leo risus, porta ac consectetur ac, vestibulum at eros. Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas faucibus mollis interdum.</p>"
            });
            await _api.Posts.SaveAsync(post2);

            var post3 = await StandardPost.CreateAsync(_api);
            post3.Id = Guid.NewGuid();
            post3.BlogId = blogPage.Id;
            post3.Category = "Piranha";
            post3.Tags.Add("Development", "Release Info");
            post3.Title = "What's New In 8.3";
            post3.MetaKeywords = "Nibh, Vulputate, Venenatis, Ridiculus";
            post3.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet. Maecenas faucibus mollis interdum.";
            post3.PrimaryImage = images["bird-s-eye-view-photography-of-lighted-city-3573383.jpg"];
            post3.Excerpt = "For this release we've put our focus on bugfixes and functionality that has been requested by our users to increase productivity in development projects.";
            post3.Published = DateTime.Now;

            post3.Blocks.Add(new HtmlBlock
            {
                Body =
                    "<p class=\"lead\">Please note that if you have implemented a custom <code>IStorage</code> service in your application you will need to upgrade it as the interfaces have been updated for issues #1124.</p>"
            });
            post3.Blocks.Add(new ColumnBlock
            {
                Items = new List<Block>
                {
                    new HtmlBlock
                    {
                        Body =
                            "<h4>Core</h4>" +
                            "<ul>" +
                            "  <li>Make sure empty slugs are not generated (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/566\" target=\"_blank\">#566</a>)</li>" +
                            "  <li>Optional Authentication in REST API (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/841\" target=\"_blank\">#841</a>)</li>" +
                            "  <li>Add author to QuoteBlock (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1037\" target=\"_blank\">#1037</a>)</li>" +
                            "  <li>Option to Remove GUID from filename in media uploads (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1124\" target=\"_blank\">#1124</a>)</li>" +
                            "  <li>Add SourceLink (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1153\" target=\"_blank\">#1153</a>)</li>" +
                            "  <li>Automatically clean media filenames (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1156\" target=\"_blank\">#1156</a>)</li>" +
                            "  <li>Add RazorPage model for Archive Pages (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1167\" target=\"_blank\">#1167</a>)</li>" +
                            "  <li>Add PrimaryImage and Excerpt to Pages as well (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1175\" target=\"_blank\">#1175</a>)</li>" +
                            "  <li>Add PageLink block (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1179\" target=\"_blank\">#1179</a>)</li>" +
                            "  <li>Add PostLink block (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1180\" target=\"_blank\">#1180</a>)</li>" +
                            "  <li>Add preferred image aspect for ImageBlock (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1191\" target=\"_blank\">#1191</a>)</li>" +
                            "</ul>" +
                            "<h4>Manager</h4>" +
                            "<ul>" +
                            "  <li>Excerpt as HTML (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1150\" target=\"_blank\">#1150</a>)</li>" +
                            "  <li>Support TinyMCE extended_valid_elements (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1154\" target=\"_blank\">#1154</a>)</li>" +
                            "  <li>Perform a Sign Out before Signing In to the manager (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1159\" target=\"_blank\">#1159</a>)</li>" +
                            "  <li>Move &amp; delete multiple media files in the manager (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1174\" target=\"_blank\">#1174</a>)</li>" +
                            "  <li>Media data not serialized for Mediafield (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1183\" target=\"_blank\">#1183</a>)</li>" +
                            "</ul>" +
                            "<h4>Bugfixes</h4>" +
                            "<ul>" +
                            "  <li>Fix 404 when fetching empty-image.png from wrong path. (<a href=\"https://github.com/PiranhaCMS/piranha.core/pull/1177\" target=\"_blank\">#1177</a>)</li>" +
                            "  <li>Unable to move post blocks (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1178\" target=\"_blank\">#1178</a>)</li>" +
                            "  <li>Cannot remove missing permissions from post/page (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1182\" target=\"_blank\">#1182</a>)</li>" +
                            "  <li>Urls in sitemap doesn't consider prefixes (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1083\" target=\"_blank\">#1083</a>)</li>" +
                            "  <li>Uploaded media is not cleared from the mediapicker (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1189\" target=\"_blank\">#1189</a>)</li>" +
                            "  <li>Multiple HtmlString results in TinyMCE not loading (<a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1203\" target=\"_blank\">#1203</a>)</li>" +
                            "</ul>"
                    },
                    new ImageBlock
                    {
                        Body = images["person-looking-at-phone-and-at-macbook-pro-1181244.jpg"]
                    }
                }
            });

            await _api.Posts.SaveAsync(post3);

            var comment = new Piranha.Models.Comment
            {
                Author = "Håkan Edling",
                Email = "hakan@tidyui.com",
                Url = "http://piranhacms.org",
                Body = "Awesome to see that the project is up and running! Now maybe it's time to start customizing it to your needs. You can find a lot of information in the official docs.",
                IsApproved = true
            };
            await _api.Posts.SaveCommentAsync(post3.Id, comment);

            return Redirect("~/");
        }
    }
}
