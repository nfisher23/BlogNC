using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.NCAdmin.Models.PageModels;
using Microsoft.AspNetCore.Http;
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
            var page = blogRepository.GetStaticPageById(model.Page.StaticPageId);
            if (page.IsHomePage)
            {
                ModelState.AddModelError("", "You cannot delete the home page. " +
                    "To Delete this page, first go to the \"Manage Static Pages\" link, " +
                    "then select a different home page");
                return View("EditStaticPage");
            }
            blogRepository.DeleteStaticPage(page);
            TempData["message"] = "Your static page was deleted";
            return RedirectToAction("Home");
        }

        [HttpGet]
        public ViewResult ManageStaticPages()
        {
            var pages = blogRepository.GetStaticPagesByPriorityAscending();
            var model = new AdminManageStaticPagesModel
            {
                Pages = pages.ToList(),
                HomePageMirror = pages.Select(sp => sp.IsHomePage).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult ManageStaticPages(AdminManageStaticPagesModel model)
        {
            IFormCollection form = this.Request.Form;
            FillInSelectedHomePage(model, form);
            if (ModelState.IsValid)
            {
                var pages = model.Pages;
                foreach (var p in pages)
                {
                    blogRepository.UpdateMetadata(p);
                }
                TempData["message"] = "Your static page configuration was successfully updated";
                return RedirectToAction(nameof(ManageStaticPages));
            }
            else
            {
                return View(model);
            }
        }

        // ick
        private void FillInSelectedHomePage(AdminManageStaticPagesModel model,
            IFormCollection form)
        {
            for (int i = 0; i < model.Pages.Count; i++)
            {
                var page = model.Pages[i];
                page.IsHomePage = false;
                if (page.PageTitle == form["homepageselector"])
                {
                    page.IsHomePage = true;
                }
            }
        }
    }
}
