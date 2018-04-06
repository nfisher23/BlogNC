﻿using BlogNC.Areas.Blog.Models;
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
                model = new AdminEditPostPublishedModel
                {
                    Post = new BlogPostPublished
                    {
                        DateTimePublished = DateTime.Now,
                    }
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
        public IActionResult EditPublishedPost(AdminEditPostPublishedModel model)
        {
            var post = model.Post;
            if (string.IsNullOrEmpty(post.PageTitle))
            {
                ModelState.AddModelError("", "Your blog post must have a page title");
                return View();
            }
            if (blogRepository.SavePublishedPost(post))
            {
                TempData["message"] = "Blog Post successfully saved to the database";
                return RedirectToAction("Home");
            }
            else
            {
                TempData["message"] = "A post with that title already exists. " +
                    "Your changes could not be saved";
                ModelState.AddModelError("", "A post with that title already exists");
                return View();
            }
        }

        [HttpGet]
        public ViewResult EditBlogPostDraft(int blogPostDraftId)
        {
            var repoDraft = blogRepository.GetDraftById(blogPostDraftId);
            AdminEditBlogPostDraftModel model;
            if (repoDraft == null)
            {
                model = new AdminEditBlogPostDraftModel
                {
                    Draft = new BlogPostDraft()
                };
            }
            else
            {
                model = new AdminEditBlogPostDraftModel
                {
                    Draft = repoDraft
                };
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult EditBlogPostDraft(AdminEditBlogPostDraftModel model)
        {
            var draft = model.Draft;
            if (string.IsNullOrEmpty(draft.PageTitle))
            {
                ModelState.AddModelError("", "Your draft must have a page title");
                return View();
            }
            if (blogRepository.SaveDraft(draft))
            {
                TempData["message"] = "Your draft successfully saved to the database";
                return RedirectToAction("Home");
            }
            else
            {
                TempData["message"] = "A post with that title already exists. " +
                    "Your changes could not be saved";
                ModelState.AddModelError("", "A post with that title already exists");
                return View();
            }
        }

        [HttpPost]
        public IActionResult UnPublishPost(AdminEditPostPublishedModel model)
        {
            if (ModelState.IsValid)
            {
                blogRepository.UnPublishPostToDraft(model.Post);
                TempData["message"] = "The selected post was successfully moved to drafts";
                return RedirectToAction("Home");
            }
            else
            {
                TempData["message"] = "Your request could not be completed";
                return View(nameof(EditPublishedPost),model);
            }
        }
    }
}
