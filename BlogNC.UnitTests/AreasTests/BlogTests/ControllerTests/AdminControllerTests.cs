﻿using NUnit.Framework;
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