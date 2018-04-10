using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class StaticPagesController : Controller
    {
        IBlogPostRepository blogRepository;
        public StaticPagesController(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public IActionResult FindStaticPage(string urlTitle)
        {
            if (urlTitle == null || urlTitle == "")
                urlTitle = blogRepository.GetHomePage().UrlTitle;

            var page = blogRepository.GetStaticPageByUrlTitle(urlTitle);
            if (page == null)
                return this.NotFound();

            StaticPageTemplateModel model = new StaticPageTemplateModel
            {
                Page = page
            };
            return View("StaticPageTemplate", model);
        }


        public ViewResult ErrorHandlingPage()
        {
            return View();
        }
    }
}
