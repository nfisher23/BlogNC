using BlogNC.Areas.Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Models.PageModels
{
    public class AdminEditBlogPostDraftModel : AdminEditPostTemplateModel
    {
        public AdminEditBlogPostDraftModel(IBlogPostRepository repo)
            : base(repo)
        { }

        public AdminEditBlogPostDraftModel() : base(null) { } // for model binding

        public BlogPostDraft Draft { get; set; }

        public override bool IsSelected(string category)
        {
            return Draft.Categories.Contains(category);
        }

        public override void UpdateBlogPostWithNewCats(List<CategoryCheckBox> boxesChecked)
        {
            foreach (var cat in boxesChecked)
            {
                if (cat.Selected)
                    Draft.AddCategory(cat.Category);
                else
                    Draft.RemoveCategory(cat.Category);
            }
        }
    }
}
