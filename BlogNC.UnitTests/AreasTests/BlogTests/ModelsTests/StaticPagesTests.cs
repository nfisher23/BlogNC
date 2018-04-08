using BlogNC.Areas.Blog.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ModelsTests
{
    [TestFixture]
    public class StaticPagesTests
    {
        [Test]
        public void UpdateStaticPage_UpdatesAllVals()
        {
            StaticPage p1 = new StaticPage
            {
                PageTitle = "Title 1",
                FullContent = "Content 1",
                InFooter = true,
                InMainNav = true,
                FooterPriority = 100,
                MainNavPriority = 100,
                StaticPageId = 100
            };

            string newtitle = "Title 2";
            string newcontent = "Content 2";
            bool newfoot = false;
            bool newnav = false;
            int newNavP = 250;
            int newFootP = 25;
            int newID = 13;

            StaticPage p2 = new StaticPage
            {
                PageTitle = newtitle,
                FullContent = newcontent,
                InFooter = newfoot,
                InMainNav = newnav,
                FooterPriority = newFootP,
                MainNavPriority = newNavP,
                StaticPageId = newID
            };

            p1.UpdatePage(p2);

            Assert.AreEqual(newtitle, p1.PageTitle);
            Assert.AreEqual(newcontent, p1.FullContent);
            Assert.AreEqual(newfoot, p1.InFooter);
            Assert.AreEqual(newnav, p1.InMainNav);
            Assert.AreEqual(newFootP, p1.FooterPriority);
            Assert.AreEqual(newNavP, p1.MainNavPriority);
            Assert.AreEqual(newID, p1.StaticPageId);
        }
    }
}
