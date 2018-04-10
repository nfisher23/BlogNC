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
    public class AdminEditBlogPostDraftModelTests
    {
        [Test]
        public void GetDraft_FillsSelectedCats()
        {
            List<CategoryCheckBox> boxes = new List<CategoryCheckBox>();
            boxes.Add(new CategoryCheckBox { Category = "Cat1", Selected = true });
            boxes.Add(new CategoryCheckBox { Category = "Cat2", Selected = true });
            boxes.Add(new CategoryCheckBox { Category = "Cat3", Selected = false});
            boxes.Add(new CategoryCheckBox { Category = "Cat4", Selected = true });

            var mockRepo = Substitute.For<IBlogPostRepository>();
            mockRepo.GetAllCategoriesUsed(false)
                .Returns(new List<string> { "Cat1", "Cat2", "Cat3", "Cat4" });

            var model = new AdminEditBlogPostDraftModel(mockRepo);
            model.Draft = new BlogPostDraft
            {
                PageTitle = "Title"
            };
            model.Draft.CategoriesDbCollection = "Cat1,Cat3";
            model.CategoriesSelected = boxes;

            Assert.Contains("Cat2", model.Draft.Categories);
            Assert.Contains("Cat4", model.Draft.Categories);
            Assert.Contains("Cat1", model.Draft.Categories);

            Assert.IsFalse(model.Draft.Categories.Contains("Cat3"));
            Assert.AreEqual(model.Draft.Categories.Count, 3);
        }
    }
}
