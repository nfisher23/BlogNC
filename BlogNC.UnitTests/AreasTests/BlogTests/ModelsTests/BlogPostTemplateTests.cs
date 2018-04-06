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

        [Test]
        public void SetCategoriesDbCollection_SetsCategoriesRight()
        {
            // This will verify that EF Core handles it as expected
            BlogPostTemplate template = Substitute.For<BlogPostTemplate>();

            template.CategoriesDbCollection = "Cat1,Cat2,Cat3";


            Assert.Contains("Cat1", template.Categories);
            Assert.Contains("Cat2", template.Categories);
            Assert.Contains("Cat3", template.Categories);
        }

        [Test]
        public void UpdatePost_UpdatesAllFields()
        {
            BlogPostTemplate t1 = new FakeBlogPostTemplateSubClass();
            t1.BlogPostTemplateId = 5;
            t1.Author = "Nick";
            t1.FullContent = "Some Content";
            t1.PageTitle ="A Page Title";
            t1.CategoriesDbCollection = "Cat1,Cat2,Cat3";

            BlogPostTemplate t2 = new FakeBlogPostTemplateSubClass();
            t2.BlogPostTemplateId = 5;
            t2.Author = "Nick2";
            t2.FullContent = "Some Content 2";
            t2.PageTitle = "A Page Title 2";
            var catTwos= "Cat12,Cat22,Cat32";

            t2.CategoriesDbCollection = catTwos;
            var update = t1.UpdatePost(t2);

            Assert.IsTrue(update);

            Assert.AreEqual(t1.BlogPostTemplateId, 5);
            Assert.AreEqual(t1.Author, "Nick2");
            Assert.AreEqual(t1.FullContent, "Some Content 2");
            Assert.AreEqual(t1.PageTitle, "A Page Title 2");
            Assert.AreEqual(t1.Categories, new List<string> { "Cat12", "Cat22", "Cat32" });
        }

        [Test]
        public void UpdatePost_WrongId_ReturnsFalseAndDoesNotUpdate()
        {
            BlogPostTemplate t1 = new FakeBlogPostTemplateSubClass();
            t1.BlogPostTemplateId = 5;
            t1.Author = "Nick";
            t1.FullContent = "Some Content";
            t1.PageTitle = "A Page Title";
            t1.CategoriesDbCollection = "Cat1,Cat2,Cat3";

            BlogPostTemplate t2 = new FakeBlogPostTemplateSubClass();
            t2.BlogPostTemplateId = 15; // wrong id
            t2.Author = "Nick2";
            t2.FullContent = "Some Content 2";
            t2.PageTitle = "A Page Title 2";
            var catTwos = "Cat12,Cat22,Cat32";

            t2.CategoriesDbCollection = catTwos;

            var update = t1.UpdatePost(t2);

            Assert.IsFalse(update);

            Assert.AreEqual(t1.BlogPostTemplateId, 5);
            Assert.AreNotEqual(t1.Author, "Nick2");
            Assert.AreNotEqual(t1.FullContent, "Some Content 2");
            Assert.AreNotEqual(t1.PageTitle, "A Page Title 2");
            Assert.AreNotEqual(t1.Categories, new List<string> { "Cat12", "Cat22", "Cat32" });
        }

        private class FakeBlogPostTemplateSubClass : BlogPostTemplate
        {
            public override bool UpdatePost(BlogPostTemplate newData)
            {
                return base.UpdatePost(newData);
            }
        }
        
    }
}
