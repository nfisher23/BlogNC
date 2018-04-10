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
    public class PostController : Controller
    {
        IBlogPostRepository blogRepository;
        public PostController(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public IActionResult FindPost([FromRoute]string urlTitle)
        {
            var post = blogRepository.GetPostByUrlTitle(urlTitle);
            if (post == null)
                return this.NotFound();

            BlogPostViewModel model = new BlogPostViewModel
            {
                Post = post
            };
            return View("BlogPost", model);
        }
    }
}
