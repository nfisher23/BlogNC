using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.ViewComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        IBlogPostRepository blogRepository;
        public CategoriesViewComponent(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public IViewComponentResult Invoke()
        {
            var model = new CategoriesViewModel(blogRepository);
            return View(model);
        }
    }
}
