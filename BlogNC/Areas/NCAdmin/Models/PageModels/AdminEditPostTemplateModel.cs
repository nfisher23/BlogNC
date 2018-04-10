using BlogNC.Areas.Blog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Models.PageModels
{
    public abstract class AdminEditPostTemplateModel
    {
        IBlogPostRepository blogRepository;

        public AdminEditPostTemplateModel(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public List<CategoryCheckBox> CategoriesSelected
        {
            get
            {
                if (_catsSelected.Count == 0
                    && blogRepository != null)
                {
                    var allCats = blogRepository.GetAllCategoriesUsed(false);
                    foreach (var cat in allCats)
                    {
                        _catsSelected.Add(new CategoryCheckBox
                        {
                            Category = cat,
                            Selected = IsSelected(cat)
                        });
                    }
                }
                return _catsSelected;
            }
            set
            {
                UpdateBlogPostWithNewCats(value);
                _catsSelected = value;
            }
        }

        private List<CategoryCheckBox> _catsSelected = new List<CategoryCheckBox>();
        public abstract bool IsSelected(string category);
        public abstract void UpdateBlogPostWithNewCats(List<CategoryCheckBox> boxesChecked);
    }

    [NotMapped]
    public class CategoryCheckBox
    {
        public string Category { get; set; }
        public bool Selected { get; set; }
    }

}
