using BlogNC.Areas.Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Components
{
    public class ArchivesViewComponent : ViewComponent
    {
        IBlogPostRepository blogRepository;
        public ArchivesViewComponent(IBlogPostRepository repo)
        {
            blogRepository = repo;
        } 

        public IViewComponentResult Invoke()
        {
            throw new NotImplementedException();
        }
    }
}
