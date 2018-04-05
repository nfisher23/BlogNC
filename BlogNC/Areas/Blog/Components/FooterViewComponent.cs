using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.ViewComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Components
{
    public class FooterViewComponent : ViewComponent
    {
        IBlogPostRepository blogRepository;
        public FooterViewComponent(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public IViewComponentResult Invoke()
        {
            var footers = blogRepository.GetFooterStaticPages();
            FooterViewModel model = new FooterViewModel
            {
                QualifyingStaticPages = footers
            };
            return View(model);
        }
    }
}
