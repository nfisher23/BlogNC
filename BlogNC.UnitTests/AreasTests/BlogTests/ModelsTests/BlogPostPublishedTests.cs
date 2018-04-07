using BlogNC.Areas.Blog.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ModelsTests
{
    [TestFixture]
    public class BlogPostPublishedTests
    {
        [Test]
        public void UpdatePost_UpdatesNewValsAndReturnsTrue()
        {
            BlogPostPublished p1 = new BlogPostPublished
            {
                BlogPostTemplateId = 19,
                TimeOfDayPublished = new TimeSpan(5, 0, 0),
                DatePublished = new DateTime(2017, 6, 1)
            };

            var time2 = new TimeSpan(16, 0, 0);
            var date2 = new DateTime(2017, 10, 1);
            BlogPostPublished p2 = new BlogPostPublished
            {
                BlogPostTemplateId = 19
            };
            p2.TimeOfDayPublished = time2;
            p2.DatePublished = date2;

            var update = p1.UpdatePost(p2);

            Assert.IsTrue(update);

            Assert.AreEqual(p1.TimeOfDayPublished, time2);
            Assert.AreEqual(p1.DatePublished, date2);
        }

        [Test]
        public void UpdatePost_WrongId_DoesNotUpdateAndReturnsFalse()
        {

            BlogPostPublished p1 = new BlogPostPublished();
            p1.BlogPostTemplateId = 26;
            p1.TimeOfDayPublished = new TimeSpan(5, 0, 0);
            p1.DatePublished = new DateTime(2017, 6, 1);

            BlogPostPublished p2 = new BlogPostPublished();
            p2.BlogPostTemplateId = 19;
            var time2 = new TimeSpan(16, 0, 0);
            var date2 = new DateTime(2017, 10, 1);
            p2.TimeOfDayPublished = time2;
            p2.DatePublished = date2;

            var update = p1.UpdatePost(p2);

            Assert.IsFalse(update);

            Assert.AreNotEqual(p1.TimeOfDayPublished, time2);
            Assert.AreNotEqual(p1.DatePublished, date2);
        }

        [Test]
        public void PublishDraftToPost_RetainsValues()
        {
            string title = "Some Example Title";
            string content = "Some Example Full Content";
            string author = "Steven Jobison";

            var draft = new BlogPostDraft
            {
                PageTitle = title,
                Author = author,
                FullContent = content
            };

            var post = new BlogPostPublished();
            post.PublishDraftToPost(draft);

            Assert.AreEqual(title, post.PageTitle);
            Assert.AreEqual(content, post.FullContent);
            Assert.AreEqual(author, post.Author);
        }
    }
}
