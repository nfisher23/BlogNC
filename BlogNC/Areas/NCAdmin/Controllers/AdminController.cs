using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.PageModels;
using BlogNC.Areas.NCAdmin.Models.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Controllers
{
    [Area("NCAdmin")]
    [Authorize]
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
                model = new AdminEditPostPublishedModel(blogRepository)
                {
                    Post = new BlogPostPublished
                    {
                        DateTimePublished = DateTime.Now,
                    }
                };
            }
            else
            {
                model = new AdminEditPostPublishedModel(blogRepository)
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
                model = new AdminEditBlogPostDraftModel(blogRepository)
                {
                    Draft = new BlogPostDraft()
                };
            }
            else
            {
                model = new AdminEditBlogPostDraftModel(blogRepository)
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
                return View(nameof(EditPublishedPost), model);
            }
        }

        [HttpPost]
        public IActionResult UnPublishPostById(int publishedPostId)
        {
            var post = blogRepository.GetPostById(publishedPostId);
            if (post != null)
            {
                blogRepository.UnPublishPostToDraft(post);
                TempData["message"] = "The selected post was successfully moved to drafts";
                return RedirectToAction("Home");
            }
            else
            {
                TempData["message"] = "Your request could not be completed";
                return View(nameof(EditPublishedPost), new AdminEditPostPublishedModel
                (blogRepository)
                {
                    Post = post
                });
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

        [HttpPost]
        public IActionResult DeleteDraftById(int blogPostDraftId)
        {
            var draft = blogRepository.GetDraftById(blogPostDraftId);
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
                Pages = pages.ToList()
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

        [HttpGet]
        public ViewResult ManagePublishedPosts()
        {
            var posts = blogRepository.GetAllPostsDescending().ToList();
            var model = new AdminManagePublishedPostsModel
            {
                Posts = posts
            };
            return View(model);
        }

        [HttpGet]
        public ViewResult ManageDrafts()
        {
            var drafts = blogRepository.GetMostRecentDrafts(1000);
            var model = new AdminManageDraftsModel
            {
                Drafts = drafts.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddCategoryToPost(AdminEditPostPublishedModel model, string newCategory)
        {
            model.CategoriesSelected.Add(new CategoryCheckBox
            {
                Category = newCategory,
                Selected = true
            });
            return View(nameof(EditPublishedPost), model);
        }

        [HttpPost]
        public IActionResult AddCategoryToDraft(AdminEditBlogPostDraftModel model, string newCategory)
        {
            model.CategoriesSelected.Add(new CategoryCheckBox
            {
                Category = newCategory,
                Selected = true
            });
            return View(nameof(EditBlogPostDraft), model);
        }

        [HttpPost]
        public IActionResult PreviewDraft(AdminEditBlogPostDraftModel model)
        {
            // save and redirect
            if (ModelState.IsValid)
            {
                blogRepository.SaveDraft(model.Draft);
            }
            return RedirectToAction(nameof(PreviewDraft), new { draftId = model.Draft.BlogPostTemplateId });
        }

        [HttpGet]
        public IActionResult PreviewDraft(int draftId)
        {
            var post = new BlogPostPublished();
            var draft = blogRepository.GetDraftById(draftId);
            post.PublishDraftToPost(draft);
            BlogPostViewModel postModel = new BlogPostViewModel
            {
                Post = post
            };
            return View("BlogPost", postModel);
        }

        [HttpPost]
        public IActionResult UpdateDraft(AdminEditBlogPostDraftModel model)
        {
            if (ModelState.IsValid)
            {
                blogRepository.SaveDraft(model.Draft);
                TempData["message"] = "Your draft was successfully updated";
            }
            else
                TempData["message"] = "Your requested action could not be completed";

            return RedirectToAction(nameof(EditBlogPostDraft), new { blogPostDraftId = model.Draft.BlogPostTemplateId });
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
