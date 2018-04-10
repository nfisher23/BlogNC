using BlogNC.Areas.Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Models.PageModels
{
    public class AdminEditPostPublishedModel : AdminEditPostTemplateModel
    {
        public AdminEditPostPublishedModel(IBlogPostRepository repo) 
            : base(repo)
        { }

        public AdminEditPostPublishedModel(): base(null) { } // for model binding only

        public BlogPostPublished Post { get; set; }

        public override bool IsSelected(string category)
        {
            return Post.Categories.Contains(category);
        }

        public override void UpdateBlogPostWithNewCats(List<CategoryCheckBox> boxesChecked)
        {
            foreach (var cat in boxesChecked)
            {
                if (cat.Selected)
                    Post.AddCategory(cat.Category);
                else
                    Post.RemoveCategory(cat.Category);
            }
        }
    }
}
