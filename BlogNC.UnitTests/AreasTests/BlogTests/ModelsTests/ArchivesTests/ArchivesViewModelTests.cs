using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.ViewComponentModels.Archives;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ModelsTests.ArchivesTests
{
    [TestFixture]
    public class ArchivesViewModelTests
    {
        [Test]
        public void GetBucketsDescending_AreDescending()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAppDatabase")
                .Options;

            var stub = Substitute.For<IHostingEnvironment>();
            ApplicationDbContext context = new ApplicationDbContext(options, stub);
            context.Database.EnsureCreated();
            context.Posts.AddRange(GenerateDoublePosts());
            context.SaveChanges();

            IBlogPostRepository repo = new EFBlogPostRepository(context);
            ArchivesViewModel model = new ArchivesViewModel(repo);
            var buckets = model.BucketsOfPostsDescending;

            Assert.IsTrue(buckets.Count > 0); 
            Assert.IsTrue(buckets.Count == 9);

            for (int i = 0; i < buckets.Count - 1; i++)
            {
                Assert.IsTrue(buckets[i].MonthAndYear > buckets[i + 1].MonthAndYear);
            }

            context.Database.EnsureDeleted();
        }

        [Test]
        public void GetBucketsDescending_DoubleGroup_CountIsRight()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAppDatabase")
                .Options;   

            var stub = Substitute.For<IHostingEnvironment>();
            ApplicationDbContext context = new ApplicationDbContext(options, stub);
            context.Database.EnsureCreated();
            context.Posts.AddRange(GenerateDoublePosts());
            context.SaveChanges();

            IBlogPostRepository repo = new EFBlogPostRepository(context);
            ArchivesViewModel model = new ArchivesViewModel(repo);
            var buckets = model.BucketsOfPostsDescending;

            Assert.IsTrue(buckets.Count == 9);

            context.Database.EnsureDeleted();
        }

        [Test]
        public void GetBucketsDescending_SingleGroup_CountIsRight()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAppDatabase")
                .Options;

            var stub = Substitute.For<IHostingEnvironment>();
            ApplicationDbContext context = new ApplicationDbContext(options, stub);
            context.Database.EnsureCreated();
            context.Posts.AddRange(GenerateSinglePosts());
            context.SaveChanges();

            IBlogPostRepository repo = new EFBlogPostRepository(context);
            ArchivesViewModel model = new ArchivesViewModel(repo);
            var buckets = model.BucketsOfPostsDescending;

            Assert.IsTrue(buckets.Count == 9);

            context.Database.EnsureDeleted();
        }


        private static IQueryable<BlogPostPublished> GenerateDoublePosts()
        {
            List<BlogPostPublished> posts = new List<BlogPostPublished>();
            for (int i = 1; i < 10; i++)
            {
                BlogPostPublished p1 = new BlogPostPublished
                {
                    PageTitle = $"Post Title No {i} - 1",
                    FullContent = $"Sample Content for post no {i} - 1",
                    DatePublished = new DateTime(2018, i, i),
                    TimeOfDayPublished = new TimeSpan(i, 0, 0)
                };
                BlogPostPublished p2 = new BlogPostPublished
                {
                    PageTitle = $"Post Title No {i} - 2",
                    FullContent = $"Sample Content for post no {i} - 2",
                    DatePublished = new DateTime(2018, i, i + 10),
                    TimeOfDayPublished = new TimeSpan(i, 0, 0)
                };

                posts.Add(p1);
                posts.Add(p2);
            }

            return posts.AsQueryable();
        }

        private static IQueryable<BlogPostPublished> GenerateSinglePosts()
        {
            List<BlogPostPublished> posts = new List<BlogPostPublished>();
            for (int i = 1; i < 10; i++)
            {
                BlogPostPublished p = new BlogPostPublished
                {
                    PageTitle = $"Post Title No {i} - 1",
                    FullContent = $"Sample Content for post no {i} - 1",
                    DatePublished = new DateTime(2018, i, i),
                    TimeOfDayPublished = new TimeSpan(i, 0, 0)
                };

                posts.Add(p);
            }

            return posts.AsQueryable();
        }
    }
}
