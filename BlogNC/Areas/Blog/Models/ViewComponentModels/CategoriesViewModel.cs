using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models.ViewComponentModels
{
    public class CategoriesViewModel
    {
        public Dictionary<string, List<BlogPostPublished>> SortedPublished
        {
            get
            {
                if (_sortedPublished == null || _sortedPublished.Count == 0)
                {
                    FillCategories();
                }
                return _sortedPublished;
            }
            set
            {
                _sortedPublished = value;
            }
        }

        private Dictionary<string, List<BlogPostPublished>> _sortedPublished;

        IBlogPostRepository blogRepository;
        public CategoriesViewModel(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }



        private void FillCategories()
        {
            _sortedPublished = new Dictionary<string, List<BlogPostPublished>>();
            var posts = blogRepository.GetAllPostsDescending();
            var cats = blogRepository.GetAllCategoriesUsed(true);
            foreach (var cat in cats)
            {
                _sortedPublished.Add(cat, posts.Where(p => p.Categories.Contains(cat)).ToList());
            }
            
        }
    }
}
