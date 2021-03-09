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
            startPage.MetaTitle = "Piranha CMS - Open Source, Cross Platform ASP.NET Core CMS";
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
            post3.Title = "What's New In 9";
            post3.MetaKeywords = "Piranha, Version, Information";
            post3.MetaDescription = "Here you can find information about what's included in the current release.";
            post3.PrimaryImage = images["bird-s-eye-view-photography-of-lighted-city-3573383.jpg"];
            post3.Excerpt = "Here you can find information about what's included in the current release.";
            post3.Published = DateTime.Now;

            post3.Blocks.Add(new HtmlBlock
            {
                Body =
                    "<p class=\"lead\">Big thanks to <a href=\"https://github.com/mikaellindemann\">@axunonb</a>, <a href=\"https://github.com/mikaellindemann\">@eferfolja</a>, <a href=\"https://github.com/mikaellindemann\">@FelipePergher</a>, <a href=\"https://github.com/mikaellindemann\">@jensbrak</a>, <a href=\"https://github.com/mikaellindemann\">@jfaquinojr</a>, <a href=\"https://github.com/mikaellindemann\">@MehranDHN</a>, <a href=\"https://github.com/mikaellindemann\">@raziee</a> and <a href=\"https://github.com/mikaellindemann\">@tfritzke</a> for their contributions and all of the people who has helped with translating the manager.</p>"
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
                            "  <li>Add Index, NoIndex and Follow to page/post settings <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1000\">#1000</a></li>" +
                            "  <li>Add possibility to set configuration on Regions & Fields <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1233\">#1233</a></li>" +
                            "  <li>Add ColorField <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1302\">#1302</a></li>" +
                            "  <li>Specify allowed blocks by content type <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1318\">#1318</a></li>" +
                            "  <li>Update package dependencies <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1319\">#1319</a></li>" +
                            "  <li>Drop old application setup extension methods <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1323\">#1323</a></li>" +
                            "  <li>Provide generic content structure <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1335\">#1335</a></li>" +
                            "  <li>Add NewtonSoft SerializerSettings globally in startup when PiranhaManager is present <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1354\">#1354</a></li>" +
                            "  <li>Upgrade to .NET 5 <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1400\">#1400</a></li>" +
                            "  <li>Add support for connection pooling <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1428\">#1428</a></li>" +
                            "  <li>Add null checks in IMediaHelper for deleted images <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1462\">#1462</a></li>" +
                            "  <li>Culture specific characters for Slovenian <a href=\"https://github.com/PiranhaCMS/piranha.core/pull/1479\">#1479</a></li>" +
                            "  <li>Unescaped hash \"#\" in media filename <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1478\">#1478</a></li>" +
                            "  <li>Warning messages from EF Core <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1490\">#1490</a></li>" +
                            "  <li>Add hooks for content <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1511\">#1511</a></li>" +
                            "  <li>Add pt-BR special characters to slugs <a href=\"https://github.com/PiranhaCMS/piranha.core/pull/1520\">#1520</a></li>" +
                            "  <li>ifferentiate models for page & post comments <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1525\">#1525</a></li>" +
                            "</ul>" +
                            "<h4>Manager</h4>" +
                            "<ul>" +
                            "  <li>PrimaryImage & Excerpt taking too much space <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1290\">#1290</a></li>" +
                            "  <li>Route update in manager for .NET 5 <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1324\">#1324</a></li>" +
                            "  <li>Indesired elements with TinyMCE 5.0.3 <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1379\">#1379</a></li>" +
                            "  <li>Upgrade to TinyMCE 5.6.2 <a href=\"https://github.com/PiranhaCMS/piranha.core/pull/1427\">#1427</a></li>" +
                            "  <li>Add RTL Layout support <a href=\"https://github.com/PiranhaCMS/piranha.core/pull/1449\">#1449</a></li>" +
                            "  <li>Add quick buttons in manager to expand/collapse all pages in all sitemaps <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1460\">#1460</a></li>" +
                            "  <li>Config option for outlining blocks in the manager <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1489\">#1489</a></li>" +
                            "  <li>Add support for editor width on regions and blocks <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1514\">#1514</a></li>" +
                            "</ul>" +
                            "<h4>Bugfixes</h4>" +
                            "<ul>" +
                            "  <li>ContentId of Comments not set in OnAfterSave hook <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1338\">#1338</a></li>" +
                            "  <li>CommentCount should only look at approved comments <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1346\">#1346</a></li>" +
                            "  <li>OnValidate hook not properly attached <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1347\">#1347</a></li>" +
                            "  <li>Duplicate slugs give an internal server error. <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1369\">#1369</a></li>" +
                            "  <li>Update Open Graph tag rendering <a href=\"https://github.com/PiranhaCMS/piranha.core/pull/1373\">#1373</a></li>" +
                            "  <li>Dragging media folder into itself make it disappear <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1409\">#1409</a></li>" +
                            "  <li>Unable to clear Post tags <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1411\">#1411</a></li>" +
                            "  <li>InitManager isn't called for fields in blocks <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1142014\">#1420</a></li>" +
                            "  <li>Meta-tags are render incorrectly <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1421\">#1421</a></li>" +
                            "  <li>When creating new Site, the Language doesn't load the Default language. <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1429\">#1429</a></li>" +
                            "  <li>MarkdownField not working properly in List Region <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1486\">#1486</a></li>" +
                            "  <li>Custom view/component for BlockGroup is not supported in the current state of ContentTypeService <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1498\">#1498</a></li>" +
                            "  <li>Select2 and remote data throwing error, jQuery slim doesn't include ajax(). <a href=\"https://github.com/PiranhaCMS/piranha.core/issues/1506\">#1506</a></li>" +
                            "</ul>"
                    },
                    new ImageBlock
                    {
                        Body = images["person-looking-at-phone-and-at-macbook-pro-1181244.jpg"]
                    }
                }
            });

            await _api.Posts.SaveAsync(post3);

            var comment = new Piranha.Models.PostComment
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
