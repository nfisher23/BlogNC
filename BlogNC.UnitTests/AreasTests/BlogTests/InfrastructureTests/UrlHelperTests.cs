using BlogNC.Areas.Blog.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogNC.UnitTests.AreasTests.BlogTests.InfrastructureTests
{
    [TestFixture]
    public class UrlHelperTests
    {
        // these first three are redundant, but a necessary additional layer of confirmation
        [Test]
        public void GetUrlTitleFromPageTitle_ReturnsNoSpecialCharacters()
        {
            var title = "Some Title? Is it?";

            var urlTitle = UrlHelper.GetUrlTitleFromPageTitle(title);

            Assert.IsFalse(urlTitle.Contains("?"));
        }

        [Test]
        public void GetUrlTitleFromPageTitle_ReturnsNonEmptyString()
        {
            var title = "Some Example Title";

            var urlTitle = UrlHelper.GetUrlTitleFromPageTitle(title);

            Assert.IsFalse(string.IsNullOrEmpty(urlTitle));
        }

        [Test]
        public void GetUrlTitleFromPageTitle_ReturnsDashedPageTitle()
        {
            var title = "An Example Title";

            var urlTitle = UrlHelper.GetUrlTitleFromPageTitle(title);

            Assert.AreEqual(urlTitle, "An-Example-Title");
        }

    }
}
