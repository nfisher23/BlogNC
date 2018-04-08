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

        [Test]
        public void GetPostById_IdExists_Retrieves()
        {
            var published = new BlogPostPublished
            {
                PageTitle = "temp",
                BlogPostTemplateId = 15000
            };
            SharedDbContext.Posts.Add(published);
            SharedDbContext.SaveChanges();
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var post = repo.GetPostById(15000);

            Assert.IsNotNull(published);

            Assert.AreEqual(published, post);
        }

        [Test]
        public void GetPostById_IdDoesNotExist_ReturnsNull()
        {
            var published = new BlogPostPublished
            {
                PageTitle = "temp",
                BlogPostTemplateId = 15000
            };
            SharedDbContext.Posts.Add(published);
            SharedDbContext.SaveChanges();
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var post = repo.GetPostById(15005);

            Assert.IsNull(post);
        }

        [Test]
        public void GetDraftById_IdExists_Retrieves()
        {
            var draftToSave = new BlogPostDraft
            {
                PageTitle = "temp",
                BlogPostTemplateId = 15000
            };
            SharedDbContext.Drafts.Add(draftToSave);
            SharedDbContext.SaveChanges();
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var draftToRetrieve = repo.GetDraftById(draftToSave.BlogPostTemplateId);

            Assert.IsNotNull(draftToRetrieve);

            Assert.AreEqual(draftToRetrieve, draftToSave);
        }

        [Test]
        public void GetDraftById_IdDoesNotExist_ReturnsNull()
        {
            var draft = new BlogPostDraft
            {
                PageTitle = "temp",
                BlogPostTemplateId = 15000
            };

            SharedDbContext.Drafts.Add(draft);
            SharedDbContext.SaveChanges();

            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var shouldBeNull = repo.GetDraftById(15005);
            Assert.IsNull(shouldBeNull);
        }
        
        [Test]
        public void SavePublishedPost_ZeroId_AddsNewPost()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            BlogPostPublished published = new BlogPostPublished
            {
                BlogPostTemplateId = 0,
                FullContent = "some new full content for this test",
                PageTitle = "A Test Page Title"
            };

            var countBefore = repo.Posts.Count();

            repo.SavePublishedPost(published);
            Assert.IsTrue(repo.Posts.Count() > countBefore);
        }

        [Test]
        public void SavePublishedPost_ZeroId_ReturnsTrue()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            BlogPostPublished published = new BlogPostPublished
            {
                BlogPostTemplateId = 0,
                FullContent = "some new full content for this test",
                PageTitle = "A Test Page Title"
            };

            var countBefore = repo.Posts.Count();

            var shouldBeTrue = repo.SavePublishedPost(published);
            Assert.IsTrue(shouldBeTrue);
        }

        [Test]
        public void SavePublishedPost_IdZeroAndUrlAlreadyExists_ReturnsFalse()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            BlogPostPublished published = new BlogPostPublished
            {
                BlogPostTemplateId = 0,
                FullContent = "Whatever",
                PageTitle = "Post Title No 4"
            };

            var shouldBeFalse = repo.SavePublishedPost(published);
            Assert.IsFalse(shouldBeFalse);
        }

        [Test]
        public void SavePublishedPost_IdZeroAndUrlAlreadyExists_CountIsSame()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            BlogPostPublished published = new BlogPostPublished
            {
                BlogPostTemplateId = 0,
                FullContent = "Whatever",
                PageTitle = "Post Title No 7"
            };

            var countBefore = repo.Posts.Count();
            repo.SavePublishedPost(published);
            Assert.AreEqual(countBefore, repo.Posts.Count());
        }

        [Test]
        public void SavePublishedPost_SaveExistingPost_CountIsSame()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            var countBefore = repo.Posts.Count();
            BlogPostPublished repoPost = repo.Posts
                .Where(p => p.PageTitle.ToLower() == "post title no 4").FirstOrDefault();
            int id = repoPost.BlogPostTemplateId;
            repoPost.PageTitle = "A Completely New Page Title";
            repoPost.FullContent = "some completely new content";

            repo.SavePublishedPost(repoPost);

            Assert.AreEqual(countBefore, repo.Posts.Count());
        }

        [Test]
        public void SavePublishedPost_SaveExistingPost_ChangesRecorded()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            var countBefore = repo.Posts.Count();
            BlogPostPublished repoPost = repo.Posts
                .Where(p => p.PageTitle.ToLower() == "post title no 4").FirstOrDefault();
            int id = repoPost.BlogPostTemplateId;
            string newTitle = "A Completely New Page Title";
            string newContent = "some completely new content";

            repoPost.PageTitle = newTitle;
            repoPost.FullContent = newContent;

            repo.SavePublishedPost(repoPost);

            var newVals = repo.GetPostById(id);
            Assert.AreEqual(newTitle, newVals.PageTitle);
            Assert.AreEqual(newContent, newVals.FullContent);
        }



        [Test]
        public void SaveDraft_ZeroId_AddsNewDraft()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            BlogPostDraft draft = new BlogPostDraft
            {
                BlogPostTemplateId = 0,
                FullContent = "some new full content for this test",
                PageTitle = "A Test Page Title"
            };

            var countBefore = repo.Drafts.Count();

            repo.SaveDraft(draft);
            Assert.IsTrue(repo.Drafts.Count() > countBefore);
        }

        [Test]
        public void SaveDraft_ZeroId_ReturnsTrue()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            BlogPostDraft draft = new BlogPostDraft
            {
                BlogPostTemplateId = 0,
                FullContent = "some new full content for this test",
                PageTitle = "A Test Page Title"
            };

            var shouldBeTrue = repo.SaveDraft(draft);
            Assert.IsTrue(shouldBeTrue);
        }


        [Test]
        public void SaveDraft_IdZeroAndUrlAlreadyExists_ReturnsFalse()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            BlogPostDraft draft = new BlogPostDraft
            {
                BlogPostTemplateId = 0,
                FullContent = "Whatever",
                PageTitle = "Draft Title No 4"
            };

            var shouldBeFalse = repo.SaveDraft(draft);
            Assert.IsFalse(shouldBeFalse);
        }

        [Test]
        public void SaveDraft_IdZeroAndUrlAlreadyExists_CountIsSame()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            BlogPostDraft draft = new BlogPostDraft
            {
                BlogPostTemplateId = 0,
                FullContent = "Whatever",
                PageTitle = "Draft Title No 7"
            };

            var countBefore = repo.Drafts.Count();
            repo.SaveDraft(draft);
            Assert.AreEqual(countBefore, repo.Drafts.Count());
        }

        [Test]
        public void SaveDraft_SaveExistingDraft_CountIsSame()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            var countBefore = repo.Drafts.Count();
            BlogPostDraft repoDraft = repo.Drafts
                .Where(d => d.PageTitle.ToLower() == "draft title no 4").FirstOrDefault();
            int id = repoDraft.BlogPostTemplateId;
            repoDraft.PageTitle = "A Completely New Page Title";
            repoDraft.FullContent = "some completely new content";

            repo.SaveDraft(repoDraft);

            Assert.AreEqual(countBefore, repo.Drafts.Count());
        }

        [Test]
        public void SaveDraft_SaveExistingDraft_ChangesRecorded()
        {
            string newFullContent = "Some completely new full content";
            string newPageTitle = "Some completely new page title";
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var draftToChange = repo.Drafts.First();
            int id = draftToChange.BlogPostTemplateId;

            draftToChange.FullContent = newFullContent;
            draftToChange.PageTitle = newPageTitle;

            repo.SaveDraft(draftToChange);

            var draftToCheck = repo.GetDraftById(id);
            Assert.AreEqual(draftToChange.PageTitle, newPageTitle);
            Assert.AreEqual(draftToChange.FullContent, newFullContent);
        }

        [Test]
        public void UnPublishPostToDraft_RemovesPost()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var published = repo.Posts.First();
            var title = published.PageTitle;

            repo.UnPublishPostToDraft(published);

            var shouldBeNull = repo.Posts.Where(p => p.PageTitle == title).FirstOrDefault();
            Assert.IsNull(shouldBeNull);
        }

        [Test]
        public void UnPublishPostToDraft_NewIdInDraft()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var published = repo.Posts.First();
            var title = published.PageTitle;
            var id = published.BlogPostTemplateId;

            repo.UnPublishPostToDraft(published);

            var draft = repo.Drafts.Where(p => p.PageTitle == title).FirstOrDefault();
            Assert.IsNotNull(draft);
            Assert.AreEqual(draft.PageTitle, title);
            Assert.AreNotEqual(draft.BlogPostTemplateId, id);
            Assert.IsTrue(draft.BlogPostTemplateId > 0);
        }

        [Test]
        public void PublishDraftToPost_AddsToPosts()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var draft = repo.Drafts.First();
            var title = draft.PageTitle;

            repo.PublishDraftToPost(draft);

            var shouldBePost = repo.Posts.Where(d => d.PageTitle == title).FirstOrDefault();
            Assert.IsNotNull(shouldBePost);
        }

        [Test]
        public void PublishDraftToPost_RemovesFromDrafts()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var draft = repo.Drafts.First();
            var id = draft.BlogPostTemplateId;

            repo.PublishDraftToPost(draft);

            var shouldBeNull = repo.Drafts.Where(d => d.BlogPostTemplateId == id).FirstOrDefault();
            Assert.IsNull(shouldBeNull);
        }

        [Test]
        public void DeleteDraft_RemovesFromSet()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var id = repo.Drafts.Select(d => d.BlogPostTemplateId).First();
            var draft = repo.GetDraftById(id);
            var title = draft.PageTitle;

            repo.DeleteDraft(draft);

            var shouldBeNull = repo.GetDraftById(id);
            Assert.IsNull(shouldBeNull);
        }

        [Test]
        public void GetStaticPageById_GetsRealId()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var page1 = repo.StaticPages.First();
            var id = page1.StaticPageId;

            var page2 = repo.GetStaticPageById(id);

            Assert.AreEqual(page1, page2);
        }

        [Test]
        public void GetStaticPageById_NoRealId_ReturnsNull()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var page = repo.GetStaticPageById(-4);

            Assert.IsNull(page);
        }




        [Test]
        public void SaveStaticPage_ZeroId_AddsNew()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            var page = new StaticPage
            {
                PageTitle = "Title",
                StaticPageId = 0,
                FullContent = "content"
            };

            var countBefore = repo.StaticPages.Count();
            repo.SaveStaticPage(page);
            Assert.IsTrue(repo.StaticPages.Count() > countBefore);
        }

        [Test]
        public void SaveStaticPage_ZeroId_ReturnsTrue()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            var page = new StaticPage
            {
                PageTitle = "Title",
                StaticPageId = 0,
                FullContent = "content"
            };

            var result = repo.SaveStaticPage(page);
            Assert.IsTrue(result);
        }

        [Test]
        public void SaveStaticPage_IdZeroAndUrlAlreadyExists_ReturnsFalse()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            var page = new StaticPage
            {
                PageTitle = "Static Page No 4",
                StaticPageId = 0,
                FullContent = "content"
            };

            var result = repo.SaveStaticPage(page);
            Assert.IsFalse(result);
        }

        [Test]
        public void SaveStaticPage_IdZeroAndUrlAlreadyExists_CountIsSame()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            var page = new StaticPage
            {
                PageTitle = "Static Page No 4",
                StaticPageId = 0,
                FullContent = "content"
            };
            var countBefore = repo.StaticPages.Count();
            var result = repo.SaveStaticPage(page);

            Assert.AreEqual(countBefore, repo.StaticPages.Count());
        }

        [Test]
        public void SaveStaticPage_SaveExistingPage_CountIsSame()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            string newFullContent = "Some completely new full content";
            string newPageTitle = "Some completely new page title";

            var page = repo.StaticPages.First();

            page.PageTitle = newPageTitle;
            page.FullContent = newFullContent;

            var count = repo.StaticPages.Count();
            repo.SaveStaticPage(page);

            Assert.AreEqual(count, repo.StaticPages.Count());
        }

        [Test]
        public void SaveStaticPage_SaveExistingPage_ChangesRecorded()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);
            string newFullContent = "Some completely new full content";
            string newPageTitle = "Some completely new page title";

            var page = repo.StaticPages.First();

            page.PageTitle = newPageTitle;
            page.FullContent = newFullContent;

            var count = repo.StaticPages.Count();
            repo.SaveStaticPage(page);

            var newPage = repo.StaticPages.Where(sp => sp.PageTitle == newPageTitle).FirstOrDefault();

            Assert.IsNotNull(newPage);
            Assert.AreEqual(newPage.PageTitle, newPageTitle);
            Assert.AreEqual(newPage.FullContent, newFullContent);
        }

        [Test]
        public void DeleteStaticPage_Removes()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var page = repo.StaticPages.First();
            var id = page.StaticPageId;
            repo.DeleteStaticPage(page);

            var shouldBeNull = repo.GetStaticPageById(id);
            Assert.IsNull(shouldBeNull);
        }

        [Test]
        public void DeleteStaticPage_CountIsLess()
        {
            EFBlogPostRepository repo = new EFBlogPostRepository(SharedDbContext);

            var page = repo.StaticPages.First();

            var countBefore = repo.StaticPages.Count();
            repo.DeleteStaticPage(page);

            Assert.IsTrue(repo.StaticPages.Count() < countBefore);
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