using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.NCAdmin.Models.ViewComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Components
{
    public class AdminStaticPagesViewComponent : ViewComponent
    {
        IBlogPostRepository blogRepository;
        public AdminStaticPagesViewComponent(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public IViewComponentResult Invoke()
        {
            var pages = blogRepository.GetStaticPagesByPriorityAscending();
            var model = new AdminStaticPagesViewModel
            {
                AllPages = pages
            };
            return View(model);
        }

    }
}
