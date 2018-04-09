using BlogNC.Areas.Blog.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using System.Text;
using BlogNC.Areas.Blog.Models.ViewComponentModels;

namespace BlogNC.UnitTests.AreasTests.BlogTests.ModelsTests
{
    [TestFixture]
    public class CategoriesViewModelTests
    {
        [Test]
        public void DictGetter_Works()
        {
            IBlogPostRepository repo = Substitute.For<IBlogPostRepository>();
            var posts = Generate10Posts();
            repo.GetAllPostsDescending().Returns(posts);
            repo.GetAllCategoriesUsed()
                .Returns(posts.SelectMany(p => p.Categories).Distinct().ToList());

            CategoriesViewModel model = new CategoriesViewModel(repo);
            var sorted = model.SortedPublished;

            Assert.AreEqual(sorted.Count, 10);
            Assert.AreEqual(sorted["OtherCat"].Count, 9);
            for (int i = 1; i < 10; i ++)
            {
                Assert.AreEqual(sorted["Cat" + i].Count, 1);
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
                p.AddCategories("Cat" + i, "OtherCat");

                posts.Add(p);
            }
            return posts.AsQueryable();
        }

    }
}
