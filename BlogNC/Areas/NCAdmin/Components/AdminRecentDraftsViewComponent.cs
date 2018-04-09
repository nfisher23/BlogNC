using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.NCAdmin.Models.ViewComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Components
{
    public class AdminRecentDraftsViewComponent : ViewComponent
    {
        IBlogPostRepository blogRepository;
        public AdminRecentDraftsViewComponent(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public IViewComponentResult Invoke(int numToGet = 5)
        {
            var recent = blogRepository.GetMostRecentDrafts(numToGet);
            AdminRecentDraftsViewModel model = new AdminRecentDraftsViewModel
            {
                Drafts = recent
            };
            return View(model);
        }

    }
}
