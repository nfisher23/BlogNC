using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.ViewComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        IBlogPostRepository blogRepository;
        public NavigationViewComponent(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public IViewComponentResult Invoke()
        {
            IEnumerable<StaticPage> pages = blogRepository.GetNavBarStaticPages().AsEnumerable();
            NavigationViewModel model = new NavigationViewModel
            {
                QualifyingStaticPages = pages
            };

            return View(model);
        }
    }
}
