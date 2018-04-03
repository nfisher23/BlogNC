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
            Assert.AreEqual(SharedDbContext.Posts.Count(), 9);

            var titles = SharedDbContext.Posts.Select(p => p.PageTitle).ToList();

            for (int i = 1; i < 10; i++)
                Assert.Contains($"Post Title No {i}", titles);
        }

        [Test]
        public void GetDrafts_DraftsAreThere()
        {
            Assert.AreEqual(SharedDbContext.Drafts.Count(), 9);

            var titles = SharedDbContext.Drafts.Select(p => p.PageTitle).ToList();

            for (int i = 1; i < 10; i++)
                Assert.Contains($"Draft Title No {i}", titles);
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
