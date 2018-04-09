using BlogNC.Areas.Blog.Controllers;
using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ControllerTests
{
    [TestFixture]
    public class StaticPagesControllerTests
    {
        [Test]
        public void FindStaticPage_PageExists_ReturnsPage()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAppDatabase")
                .Options;

            var stub = Substitute.For<IHostingEnvironment>();
            var context = new ApplicationDbContext(options, stub);
            context.Database.EnsureCreated();
            context.StaticPages.AddRange(GenerateStaticPages());
            context.SaveChanges();

            IBlogPostRepository repo = new EFBlogPostRepository(context);
            var controller = new StaticPagesController(repo);
            var result = controller.FindStaticPage("Static-Page-No-2");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Model is StaticPageTemplateModel);
            Assert.IsTrue(((StaticPageTemplateModel)(result.Model)).Page.FullContent.Contains("Sample Content for"));

            context.Database.EnsureDeleted();
        }


        private static IQueryable<StaticPage> GenerateStaticPages()
        {
            List<StaticPage> pages = new List<StaticPage>();
            bool navToggle = true;

            StaticPage h = new StaticPage
            {
                PageTitle = "Home"
            };
            pages.Add(h);

            for (int i = 1; i < 10; i++)
            {
                StaticPage sp = new StaticPage
                {
                    PageTitle = $"Static Page No {i}",
                    FullContent = $"Sample Content for sample page no {i}",
                    InMainNav = navToggle,
                    MainNavPriority = 10 - i
                };
                navToggle = !navToggle;
                pages.Add(sp);
            }
            return pages.AsQueryable();
        }

    }
}
