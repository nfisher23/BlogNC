using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.Blog.Models.ViewComponentModels.Archives;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ModelsTests.ArchivesTests
{
    [TestFixture]
    public class MonthAndYearPostBucketTests
    {
        [Test]
        public void TryAddPost_NotMatching_DoesntAdd()
        {
            BlogPostPublished first = new BlogPostPublished
            {
                PageTitle = "Title",
                DatePublished = new DateTime(2017, 1, 1)
            };

            BlogPostPublished second = new BlogPostPublished
            {
                PageTitle = "Title",
                DatePublished = new DateTime(2017, 3, 1) // different month
            };

            MonthAndYearPostBucket bucket = new MonthAndYearPostBucket(first);
            bucket.TryAddPost(second);
            Assert.IsTrue(bucket.Posts.Count == 1);
            Assert.IsTrue(bucket.Posts[0].DatePublished == new DateTime(2017, 1, 1));
        }

        [Test]
        public void TryAddPost_NotMatching_ReturnsFalse()
        {
            BlogPostPublished first = new BlogPostPublished
            {
                PageTitle = "Title",
                DatePublished = new DateTime(2017, 1, 1)
            };

            BlogPostPublished second = new BlogPostPublished
            {
                PageTitle = "Title",
                DatePublished = new DateTime(2017, 3, 1) // different month
            };

            MonthAndYearPostBucket bucket = new MonthAndYearPostBucket(first);
            Assert.IsFalse(bucket.TryAddPost(second));
        }
    }
}
