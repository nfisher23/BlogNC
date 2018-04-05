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
            SharedDbContext.StaticPages.AddRange(Generate10StaticPages());
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

        [Test]
        public void GetAllStaticPages_RetrievesAll()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var pages = repo.StaticPages;
            var titles = pages.Select(p => p.PageTitle).ToList();
            for (int i = 1; i < 10; i++)
            {
                Assert.Contains($"Static Page No {i}", titles);
            }
        }

        [Test]
        public void GetNavBarStaticPages_GetsPages()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var pages = repo.GetNavBarStaticPages();

            var titles = pages.Select(p => p.PageTitle).ToList();
            Assert.AreEqual(pages.Count(), 5);
            // should have only odds
            for (int i = 1; i < 10; i += 2)
            {
                Assert.Contains($"Static Page No {i}", titles);
                Assert.IsFalse(titles.Contains($"Static Page No {i - 1}"));
            }
        }

        [Test]
        public void GetNavBarStaticPages_CorrectOrder()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var pages = repo.GetNavBarStaticPages().ToList();

            for (int i = 0; i < 4; i++)
            {
                Assert.IsTrue(pages[i].MainNavPriority < pages[i+1].MainNavPriority);
            }
        }

        [Test]
        public void GetFooterStaticPages_GetsPages()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var pages = repo.GetFooterStaticPages();

            var titles = pages.Select(p => p.PageTitle).ToList();
            Assert.AreEqual(pages.Count(), 4);
            // should have only evens
            for (int i = 2; i < 10; i += 2)
            {
                Assert.Contains($"Static Page No {i}", titles);
                Assert.IsFalse(titles.Contains($"Static Page No {i - 1}"));
            }
        }

        [Test]
        public void GetFooterStaticPages_CorrectOrder()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var pages = repo.GetFooterStaticPages().ToList();

            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(pages[i].FooterPriority < pages[i + 1].FooterPriority);
            }
        }

        [Test]
        public void GetStaticPageByUrlTitle_IsThere_ReturnsRealPost()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var page = repo.GetStaticPageByUrlTitle("Static-Page-No-2");

            Assert.IsNotNull(page);
            Assert.AreEqual(page.PageTitle, "Static Page No 2");
        }

        [Test]
        public void GetStaticPageByUrlTitle_IsNotThere_ReturnsNull()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var notAPage = repo.GetStaticPageByUrlTitle("Not-A-Real-Title");

            Assert.IsNull(notAPage);
        }

        [Test]
        public void GetStaticPageByUrlTitle_AnyCaseWorks()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var shouldGetPage = repo.GetStaticPageByUrlTitle("STAtIc-pAgE-NO-5");

            Assert.IsNotNull(shouldGetPage);
            Assert.AreEqual(shouldGetPage.PageTitle, "Static Page No 5");
        }

        [Test]
        public void GetMostRecentPosts_CountIsCorrect()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var posts = repo.GetMostRecentPosts(8).ToList();
            Assert.AreEqual(posts.Count, 8);


            posts = repo.GetMostRecentPosts(6).ToList();
            Assert.AreEqual(posts.Count, 6);
        }

        [Test]
        public void GetMostRecentPosts_OrderIsCorrect()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var ordered = repo.GetMostRecentPosts(5).ToList();

            Assert.AreEqual(ordered.Count, 5);
            for (int i = 0; i < 4; i++)
            {
                Assert.IsTrue(ordered[i].DateTimePublished >= ordered[i + 1].DateTimePublished);
            }
        }

        [Test]
        public void GetMostRecentPosts_TooManyInArgument_ReturnsLessThanFull()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var posts = repo.GetMostRecentPosts(25).ToList();

            Assert.IsTrue(posts.Count < 25);
        }

        [Test]
        public void GetMostRecentDrafts_CountIsCorrect()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var drafts = repo.GetMostRecentDrafts(7).ToList();
            Assert.AreEqual(7, drafts.Count);

            drafts = repo.GetMostRecentDrafts(4).ToList();
            Assert.AreEqual(4, drafts.Count);
        }
        
        [Test]
        public void GetMostRecentDrafts_OrderIsCorrect()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var ordered = repo.GetMostRecentDrafts(5).ToList();

            Assert.AreEqual(ordered.Count, 5);
            for (int i = 0; i < 4; i++)
            {
                Assert.IsTrue(ordered[i].LastEdit >= ordered[i + 1].LastEdit);
            }
        }

        [Test]
        public void GetMostRecentDrafts_TooManyInArgument_ReturnsLessThanFull()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var drafts = repo.GetMostRecentDrafts(25).ToList();

            Assert.IsTrue(drafts.Count < 25);
        }

        [Test]
        public void GetStaticPagesByPriorityAscending_GetsAllPages()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var pages = repo.GetStaticPagesByPriorityAscending();

            Assert.AreEqual(pages.Count(), 9);
        }

        [Test]
        public void GetStaticPagesByPriorityAscending_OrderedByMainNav()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var pages = repo.GetStaticPagesByPriorityAscending().ToList();

            for (int i = 0; i < pages.Count; i++)
            {
                Assert.IsTrue(pages[i].MainNavPriority <= pages[i].MainNavPriority);
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
                    FullContent = $"Sample Content for post no {i}",
                    LastEdit = new DateTime(2017, i, i)
                };

                drafts.Add(d);
            }

            return drafts.AsQueryable();
        }

        private static IQueryable<StaticPage> Generate10StaticPages()
        {
            List<StaticPage> pages = new List<StaticPage>();
            bool navToggle = true;
            for (int i = 1; i < 10; i++)
            {
                StaticPage sp = new StaticPage
                {
                    PageTitle = $"Static Page No {i}",
                    FullContent = $"Sample Content for sample page no {i}",
                    InMainNav = navToggle,
                    InFooter = !navToggle,
                    MainNavPriority = 10 - i,
                    FooterPriority = i
                };
                navToggle = !navToggle;
                
                pages.Add(sp);
            }
            return pages.AsQueryable();
        }
    }
}