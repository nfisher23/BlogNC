using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.NCAdmin.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            if (ModelState.IsValid 
                && blogRepository.SavePublishedPost(model.Post))
            {
                TempData["message"] = "Blog Post successfully saved to the database";
                return RedirectToAction("Home");
            }
            else
            {
                TempData["message"] = "Your changes could not be saved";
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
                TempData["message"] = "Your draft was successfully saved to the database";
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
            var post = model.Post;
            if (ModelState.IsValid)
            {
                blogRepository.UnPublishPostToDraft(post);
                TempData["message"] = "The selected post was successfully moved to drafts";
                return RedirectToAction("Home");
            }
            else
            {
                TempData["message"] = "Your request could not be completed";
                return View(nameof(EditPublishedPost),model);
            }
        }

        [HttpPost] 
        public IActionResult PublishDraftToPost(AdminEditBlogPostDraftModel model)
        {
            var draft = model.Draft;
            if (ModelState.IsValid)
            {
                blogRepository.PublishDraftToPost(draft);
                TempData["message"] = "Your draft was successfully published";
                return RedirectToAction("Home");
            }
            else
            {
                TempData["message"] = "Your request could not be completed";
                return View(nameof(EditBlogPostDraft), model);
            }
        }

        [HttpPost]
        public IActionResult DeleteDraft(AdminEditBlogPostDraftModel model)
        {
            var draft = model.Draft;
            blogRepository.DeleteDraft(draft);
            TempData["message"] = "Your draft was deleted";
            return RedirectToAction("Home");
        }

        [HttpGet]
        public ViewResult EditStaticPage(int staticPageId)
        {
            var page = blogRepository.GetStaticPageById(staticPageId);
            var model = new AdminEditStaticPageModel
            {
                Page = page ?? new StaticPage()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditStaticPage(AdminEditStaticPageModel model)
        {
            var page = model.Page;
            if (ModelState.IsValid)
            {
                blogRepository.SaveStaticPage(page);
                TempData["message"] = "Your static page was saved to your database";
                return RedirectToAction("Home");
            }
            else
            {
                TempData["message"] = "Your requested action could not be completed";
                return View();
            }
        }

        [HttpPost]
        public IActionResult DeleteStaticPage(AdminEditStaticPageModel model)
        {
            var page = model.Page;
            blogRepository.DeleteStaticPage(page);
            TempData["message"] = "Your static page was deleted";
            return RedirectToAction("Home");
        }


    }
}
