using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.ViewComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Components
{
    public class RecentPostsViewComponent : ViewComponent
    {
        IBlogPostRepository blogRepository;
        public RecentPostsViewComponent(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public IViewComponentResult Invoke(int numToTake = 7)
        {
            RecentPostsViewModel model = new RecentPostsViewModel
            {
                Posts = blogRepository.Posts
                                      .OrderByDescending(p => p.DateTimePublished)
                                      .Take(numToTake)
            };

            return View(model);
        }
    }
}
