using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.NCAdmin.Models.PageModels;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogNC.UnitTests.AreasTests.NCAdminTests.ModelsTests.PageModelsTests
{
    [TestFixture]
    public class AdminEditPostPublishedTests
    {
        [Test]
        public void GetPost_FillsSelectedCats()
        {
            List<CategoryCheckBox> boxes = new List<CategoryCheckBox>();
            boxes.Add(new CategoryCheckBox { Category = "Cat1", Selected = true });
            boxes.Add(new CategoryCheckBox { Category = "Cat2", Selected = true });
            boxes.Add(new CategoryCheckBox { Category = "Cat3", Selected = false });
            boxes.Add(new CategoryCheckBox { Category = "Cat4", Selected = true });

            var mockRepo = Substitute.For<IBlogPostRepository>();
            mockRepo.GetAllCategoriesUsed(false)
                .Returns(new List<string> { "Cat1", "Cat2", "Cat3", "Cat4" });

            var model = new AdminEditPostPublishedModel(mockRepo);
            model.Post = new BlogPostPublished
            {
                PageTitle = "Title"
            };
            model.Post.CategoriesDbCollection = "Cat1,Cat3";
            model.CategoriesSelected = boxes;

            Assert.Contains("Cat2", model.Post.Categories);
            Assert.Contains("Cat4", model.Post.Categories);
            Assert.Contains("Cat1", model.Post.Categories);

            Assert.IsFalse(model.Post.Categories.Contains("Cat3"));
            Assert.AreEqual(model.Post.Categories.Count, 3);
        }
    }
}
