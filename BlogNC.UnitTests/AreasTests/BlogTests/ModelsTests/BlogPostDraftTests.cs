using BlogNC.Areas.Blog.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ModelsTests
{
    [TestFixture]
    public class BlogPostDraftTests
    {
        [Test]
        public void UpdatePost_WrongId_DoesNothingAndReturnsFalse()
        {
            BlogPostDraft d1 = new BlogPostDraft
            {
                BlogPostTemplateId = 5,
                PageTitle = "Some Title",
                FullContent = "FullContent"
            };
            BlogPostDraft d2 = new BlogPostDraft
            {
                BlogPostTemplateId = 15,
                PageTitle = "Some Title 2",
                FullContent = "FullContent 2"
            };

            d1.UpdatePost(d2);

            Assert.AreNotEqual(d1.BlogPostTemplateId, 15);
            Assert.AreNotEqual(d1.PageTitle, d2.PageTitle);
            Assert.AreNotEqual(d1.FullContent, d2.FullContent);
        }

        [Test]
        public void UpdatePost_RealUpdate_GetsNewValuesLoaded()
        {
            var title = "Some First Title";
            var FullContent = "Some First Full Content";


            BlogPostDraft d1 = new BlogPostDraft
            {
                BlogPostTemplateId = 15,
                PageTitle = "Some Title 2",
                FullContent = "FullContent 2",
                LastEdit = new DateTime(2017, 6, 1)
                
            };
            BlogPostDraft d2 = new BlogPostDraft
            {
                BlogPostTemplateId = 15,
                PageTitle = title,
                FullContent = FullContent,
                LastEdit = new DateTime(2017, 9, 1)
            };

            d1.UpdatePost(d2);

            Assert.AreEqual(d1.PageTitle, title);
            Assert.AreEqual(d1.FullContent, FullContent);
            Assert.AreEqual(d1.LastEdit, new DateTime(2017, 9, 1));
        }



        [Test]
        public void UnPublishToDraft_RetainsValues()
        {
            string content = "Some Full Content";
            string title = "Some Full Content";

            BlogPostPublished post = new BlogPostPublished
            {
                FullContent = content,
                PageTitle = title,
                BlogPostTemplateId = 40
            };

            BlogPostDraft draft = new BlogPostDraft();
            draft.UnPublishToDraft(post);

            Assert.AreEqual(draft.BlogPostTemplateId, 0);
            Assert.AreEqual(draft.FullContent, content);
            Assert.AreEqual(draft.PageTitle, title);
        }
    }
}
