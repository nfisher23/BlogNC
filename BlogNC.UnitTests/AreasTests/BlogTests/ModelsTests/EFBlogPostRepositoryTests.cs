using BlogNC.Areas.Blog.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ModelsTests
{
    [TestFixture]
    public class EFBlogPostRepositoryTests
    {
        private ApplicationDbContext SharedDbContext;

        [SetUp]
        public void SetupDB()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAppDatabase")
                .Options;

            var stub = Substitute.For<IHostingEnvironment>();
            SharedDbContext = new ApplicationDbContext(options, stub);
            SharedDbContext.Database.EnsureCreated();
            SharedDbContext.Posts.AddRange(Generate10Posts());
            SharedDbContext.Drafts.AddRange(Generate10Drafts());
            SharedDbContext.SaveChanges();
        }

        [TearDown]
        public void TearDownDB()
        {
            SharedDbContext.Database.EnsureDeleted();
        }

        [Test]
        public void GetPosts_PostsAreThere()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            Assert.AreEqual(repo.Posts.Count(), 9);

            var titles = repo.Posts.Select(p => p.PageTitle).ToList();

            for (int i = 1; i < 10; i++)
                Assert.Contains($"Post Title No {i}", titles);
        }

        [Test]
        public void GetDrafts_DraftsAreThere()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            Assert.AreEqual(repo.Drafts.Count(), 9);

            var titles = repo.Drafts.Select(p => p.PageTitle).ToList();

            for (int i = 1; i < 10; i++)
                Assert.Contains($"Draft Title No {i}", titles);
        }

        [Test]
        public void GetPostByUrlTitle_IsThere_ReturnsRealPost()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var post = repo.GetPostByUrlTitle("Post-Title-No-2");

            Assert.IsNotNull(post);
            Assert.AreEqual(post.PageTitle, "Post Title No 2");
        }

        [Test]
        public void GetPostByUrlTitle_IsNotThere_ReturnsNull()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var notAPost = repo.GetPostByUrlTitle("Not-A-Real-Title");

            Assert.IsNull(notAPost);
        }

        [Test]
        public void GetPostByUrlTitle_AnyCaseWorks()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var shouldGetPost = repo.GetPostByUrlTitle("POsT-tiTLe-NO-5");

            Assert.IsNotNull(shouldGetPost);
            Assert.AreEqual(shouldGetPost.PageTitle, "Post Title No 5");
        }

        [Test]
        public void GetAllPostsDescending_CorrectOrder()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var shouldBeDescending = repo.GetAllPostsDescending().ToList();

            for (int i = 0; i < shouldBeDescending.Count - 1; i++)
            {
                Assert.IsTrue(shouldBeDescending[i].DateTimePublished 
                    >= shouldBeDescending[i + 1].DateTimePublished);
            }
        }




        private static IQueryable<BlogPostPublished> Generate10Posts()
        {
            List<BlogPostPublished> posts = new List<BlogPostPublished>();
            for (int i = 1; i < 10; i++)
            {
                BlogPostPublished p = new BlogPostPublished
                {
                    PageTitle = $"Post Title No {i}",
                    FullContent = $"Sample Content for post no {i}",
                    DatePublished = new DateTime(2018, i, i),
                    TimeOfDayPublished = new TimeSpan(i, 0, 0)
                };

                posts.Add(p);
            }

            return posts.AsQueryable();
        }

        private static IQueryable<BlogPostDraft> Generate10Drafts()
        {
            List<BlogPostDraft> drafts = new List<BlogPostDraft>();
            for (int i = 1; i < 10; i++)
            {
                BlogPostDraft d = new BlogPostDraft
                {
                    PageTitle = $"Draft Title No {i}",
                    FullContent = $"Sample Content for post no {i}"
                };

                drafts.Add(d);
            }

            return drafts.AsQueryable();
        }

    }
}
