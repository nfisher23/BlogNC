using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using BlogNC.Areas.Blog.Models;
using NSubstitute;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ModelsTests
{
    [TestFixture]
    public class BlogPostTemplateTests
    {
        [Test]
        public void GetUrlTitle_ReturnsNoSpecialCharacters()
        {
            var title = "Some Title? Is it?";
            BlogPostTemplate template = Substitute.For<BlogPostTemplate>();

            template.PageTitle = title;

            Assert.IsFalse(template.UrlTitle.Contains("?"));
        }

        [Test]
        public void GetUrlTitle_ReturnsNonEmptyString()
        {
            var title = "Some Example Title";
            BlogPostTemplate template = Substitute.For<BlogPostTemplate>();

            template.PageTitle = title;

            Assert.IsFalse(string.IsNullOrEmpty(template.UrlTitle));
        }

        [Test]
        public void GetUrlTitle_ReturnsDashedPageTitle()
        {
            var title = "An Example Title";
            BlogPostTemplate template = Substitute.For<BlogPostTemplate>();

            template.PageTitle = title;

            Assert.AreEqual(template.UrlTitle, "An-Example-Title");
        }
    }
}
