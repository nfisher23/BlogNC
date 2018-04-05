using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.NCAdmin.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Controllers
{
    public class AdminController : Controller
    {
        IBlogPostRepository blogRepository;
        public AdminController(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public ViewResult Home()
        {
            return View();
        }

        [HttpGet]
        public ViewResult EditPublishedPost(int publishedPostId)
        {
            var post = blogRepository.GetPostById(publishedPostId);
            AdminEditPostPublishedModel model;
            if (post == null)
            {
                ModelState.AddModelError("", "There was an internal error finding your post. " +
                    "This is either a mistake on our part or you requested a resource that is not there");
                model = new AdminEditPostPublishedModel
                {
                    Post = new BlogPostPublished()
                };
            }
            else
            {
                model = new AdminEditPostPublishedModel
                {
                    Post = post
                };
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult EditPublishedPost(BlogPostPublished post)
        {
            throw new NotImplementedException();
        }
    }
}
