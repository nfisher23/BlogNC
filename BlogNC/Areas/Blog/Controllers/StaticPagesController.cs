using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Controllers
{
    public class StaticPagesController : Controller
    {
        IBlogPostRepository blogRepository;
        public StaticPagesController(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public ViewResult FindStaticPage(string urlTitle)
        {
            if (urlTitle == null || urlTitle == "")
                urlTitle = blogRepository.GetHomePage().UrlTitle;

            var page = blogRepository.GetStaticPageByUrlTitle(urlTitle);
            StaticPageTemplateModel model = new StaticPageTemplateModel
            {
                Page = page
            };
            return View("StaticPageTemplate", model);
        }
    }
}
