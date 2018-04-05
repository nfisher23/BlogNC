using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.NCAdmin.Models.ViewComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Components
{
    public class AdminRecentPostsViewComponent : ViewComponent
    {
        IBlogPostRepository blogRepository;
        public AdminRecentPostsViewComponent(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public IViewComponentResult Invoke(int numToGet = 5)
        {
            var recent = blogRepository.GetMostRecentPosts(numToGet);
            AdminRecentPostsViewModel model = new AdminRecentPostsViewModel
            {
                Posts = recent
            };
            return View(model);
        }

    }
}
