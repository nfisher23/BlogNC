using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.NCAdmin.Controllers;
using BlogNC.Areas.NCAdmin.Models.PageModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ControllerTests
{
    [TestFixture]
    public class AdminControllerTests
    {
        [Test]
        public void EditPublishedPost_EmptyTitle_DoesNotSaveInRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();
            
            var post = InValidPostFactory();
            var model = new AdminEditPostPublishedModel
            {
                Post = post
            };
            controller.ModelState.AddModelError("", "error");

            var result = controller.EditPublishedPost(model);

            mockRepo.DidNotReceive().SavePublishedPost(post);
        }

        [Test]
        public void EditPublishedPost_HasNonExistentTitle_SavesInRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var controller = new AdminController(mockRepo);

            controller.TempData = Substitute.For<ITempDataDictionary>();

            var post = ValidPostFactory();
            var model = new AdminEditPostPublishedModel
            {
                Post = post
            };

            var result = controller.EditPublishedPost(model);

            mockRepo.Received().SavePublishedPost(post);
        }

        [Test]
        public void EditBlogPostDraft_NoTitle_DoesNotSaveInRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var controller = new AdminController(mockRepo);

            var draft = new BlogPostDraft();
            draft.PageTitle = "";
            draft.FullContent = "something";
            var model = new AdminEditBlogPostDraftModel
            {
                Draft = draft
            };

            var result = controller.EditBlogPostDraft(model);

            mockRepo.DidNotReceive().SaveDraft(draft);
        }

        [Test]
        public void EditBlogPostDraft_HasNonExistentTitle_SavesInRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var controller = new AdminController(mockRepo);

            controller.TempData = Substitute.For<ITempDataDictionary>();

            var draft = ValidDraftFactory();
            var model = new AdminEditBlogPostDraftModel
            {
                Draft = draft
            };

            var result = controller.EditBlogPostDraft(model);

            mockRepo.Received().SaveDraft(draft);
        }

        [Test]
        public void UnPublishPost_ValidState_CallsUnpublishRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();

            var post = ValidPostFactory();
            var model = new AdminEditPostPublishedModel{ Post = post };
            var result = controller.UnPublishPost(model);

            mockRepo.Received().UnPublishPostToDraft(post);
        }

        [Test]
        public void UnPublishPost_InvalidState_DoesNotCallUnpublish()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();

            var post = InValidPostFactory();
            var model = new AdminEditPostPublishedModel { Post = post };
            controller.ModelState.AddModelError("", "error");
            var result = controller.UnPublishPost(model);
            mockRepo.DidNotReceive().UnPublishPostToDraft(post);
        }

        [Test]
        public void PublishDraftToPost_ValidState_CallsRepoPublish()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var draft = new BlogPostDraft { PageTitle = "Something", FullContent = "something..." };
            var model = new AdminEditBlogPostDraftModel
            {
                Draft = draft
            };

            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();
            var result = controller.PublishDraftToPost(model);

            mockRepo.Received().PublishDraftToPost(draft);
        }

        [Test]
        public void PublishDraftToPost_InValidState_DoesNotCallRepoPublish()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            
            var draft = new BlogPostDraft { PageTitle = "Something", FullContent = "something..." };
            var model = new AdminEditBlogPostDraftModel
            {
                Draft = draft
            };

            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();
            controller.ModelState.AddModelError("", "error");
            var result = controller.PublishDraftToPost(model);

            mockRepo.DidNotReceive().PublishDraftToPost(draft);
        }

        [Test]
        public void DeleteDraft_CallsDeleteDraftRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var draft = new BlogPostDraft { PageTitle = "Something", FullContent = "something..." };
            mockRepo.SaveDraft(draft);

            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();
            var result = controller.DeleteDraft(new AdminEditBlogPostDraftModel { Draft = draft });

            mockRepo.Received().DeleteDraft(draft);
        }

        [Test]
        public void EditStaticPage_ValidState_SavesInRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var page = new StaticPage { PageTitle = "Title", FullContent = "Content" };
            mockRepo.SaveStaticPage(page);

            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();
            var result = controller.EditStaticPage(new AdminEditStaticPageModel { Page = page });

            mockRepo.Received().SaveStaticPage(page);
        }

        [Test]
        public void EditStaticPage_InvalidState_DoesNotSaveInRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var page = new StaticPage { PageTitle = "", FullContent = "" };

            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();
            controller.ModelState.AddModelError("", "error");

            var result = controller.EditStaticPage(new AdminEditStaticPageModel { Page = page });

            mockRepo.DidNotReceive().SaveStaticPage(page);
        }

        [Test]
        public void DeleteStaticPage_CallsDeleteRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var page = new StaticPage { StaticPageId = 10, PageTitle = "Title", FullContent = "Something" };
            mockRepo.GetStaticPageById(page.StaticPageId).Returns(page);


            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();

            var result = controller.DeleteStaticPage(new AdminEditStaticPageModel { Page = page });

            mockRepo.Received().DeleteStaticPage(page);
        }

        [Test]
        public void DeleteStaticPage_HomePagePassedIn_DoesNotCallDeleteRepo()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var page = new StaticPage { StaticPageId = 10, PageTitle = "Title", FullContent = "Something",
            IsHomePage = true };
            mockRepo.GetStaticPageById(page.StaticPageId).Returns(page);


            var controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();

            var result = controller.DeleteStaticPage(new AdminEditStaticPageModel { Page = page });


            mockRepo.DidNotReceive().DeleteStaticPage(page);
        }

        [Test]
        public void ManageStaticPages_CallsSavePagesOnAll()
        {
            var mockRepo = Substitute.For<IBlogPostRepository>();
            var page1 = new StaticPage
            {
                PageTitle = "title2",
                FullContent = "content"
            };
            var page2 = new StaticPage
            {
                PageTitle = "title2",
                FullContent = "content"
            };
            var pages = new List<StaticPage>
            {
                page1,
                page2
            };
            AdminController controller = new AdminController(mockRepo);
            controller.TempData = Substitute.For<ITempDataDictionary>();
            var result = controller.ManageStaticPages(new AdminManageStaticPagesModel
            {
                Pages = pages
            });

            mockRepo.Received().SaveStaticPage(page1);
            mockRepo.Received().SaveStaticPage(page2);
        }

        private BlogPostPublished ValidPostFactory()
        {
            var post = new BlogPostPublished();
            post.PageTitle = "New Title";
            post.FullContent = "something";
            return post;
        }

        private BlogPostPublished InValidPostFactory()
        {
            var post = new BlogPostPublished();
            post.PageTitle = null;
            post.FullContent = "something";
            return post;
        }

        private BlogPostDraft ValidDraftFactory()
        {
            var draft = new BlogPostDraft();
            draft.PageTitle = "New Title";
            draft.FullContent = "something";
            return draft;
        }

        private BlogPostDraft InValidDraftFactory()
        {
            var draft = new BlogPostDraft();
            draft.PageTitle = "";
            draft.FullContent = "some content that doesn't matter";
            return draft;
        }
    }
}
