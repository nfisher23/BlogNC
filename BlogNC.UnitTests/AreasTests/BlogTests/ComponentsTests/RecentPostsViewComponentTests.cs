using BlogNC.Areas.Blog.Models;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BlogNC.Areas.Blog.Components;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using BlogNC.Areas.Blog.Models.ViewModels;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ComponentsTests
{
    [TestFixture]
    public class RecentPostsViewComponentTests
    {
        [Test]
        public void Invoke_EnsureModelMatchesUp()
        {
            IBlogPostRepository repo = Substitute.For<IBlogPostRepository>();
            repo.Posts.Returns(GenerateFakePosts());

            RecentPostsViewComponent comp = new RecentPostsViewComponent(repo);
            ViewViewComponentResult result = (ViewViewComponentResult)comp.Invoke();

            Assert.IsTrue(result.ViewData.Model is RecentPostsViewModel);
        }

        [Test]
        public void Invoke_GetsCorrectNumber()
        {
            IBlogPostRepository repo = Substitute.For<IBlogPostRepository>();
            repo.Posts.Returns(GenerateFakePosts());

            RecentPostsViewComponent comp = new RecentPostsViewComponent(repo);
            ViewViewComponentResult result = (ViewViewComponentResult)comp.Invoke(4);

            var model = (RecentPostsViewModel)result.ViewData.Model;
            Assert.AreEqual(model.Posts.Count(), 4);
        }

        [Test]
        public void Invoke_PostsInMostRecentFirstOrder()
        {
            IBlogPostRepository repo = Substitute.For<IBlogPostRepository>();
            repo.Posts.Returns(GenerateFakePosts());

            RecentPostsViewComponent comp = new RecentPostsViewComponent(repo);
            ViewViewComponentResult result = (ViewViewComponentResult)comp.Invoke(4);

            var model = (RecentPostsViewModel)result.ViewData.Model;
            var posts = model.Posts.ToList();
            for (int i = 0; i < posts.Count - 1; i++)
            {
                Assert.IsTrue(posts[i].DatePublished > posts[i + 1].DatePublished);
            }

        }



        private IQueryable<BlogPostPublished> GenerateFakePosts()
        {
            List<BlogPostPublished> posts = new List<BlogPostPublished>();
            for (int i = 0; i < 10; i++)
            {
                posts.Add(new BlogPostPublished
                {
                    PageTitle = $"Title no {i}",
                    FullContent = "<p>Some blog post content</p>",
                    DatePublished = new DateTime(1 + i, 1, 1)
                });
            }

            return posts.AsQueryable();
        }
    }
}
