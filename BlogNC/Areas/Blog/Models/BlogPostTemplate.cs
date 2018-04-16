using BlogNC.Areas.Blog.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models
{
    ///<summary>Base class for Posts (published) and Drafts (works-in-progress)</summary>
    public abstract class BlogPostTemplate
    {
        public int BlogPostTemplateId { get; set; } // for entity framework

        [Required]
        [Display(Name = "Page Title")]
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
                UrlTitle = GetUrlTitle(value); // set url title every time
            }
        }
        public string UrlTitle { get; private set; }
        [Display(Name = "Full Html Content", Description = "Enter your html here")]
        public string FullContent { get; set; }
        public string Author { get; set; }

        // EF Core does not yet (as of EF Core 2.0) support 
        // many-to-many relationships and will probably never support collections of
        // primitive types. The workaround here, using an entity class for the join table:
        // https://docs.microsoft.com/en-us/ef/core/modeling/relationships#many-to-many
        // can allegedly run into problems when you try to migrate schemas.
        // To avoid falling down this rabbit hole we'll take a slight 
        // performance hit when querying for categories, which 
        // will probably not be noticeable for the vast majority of blog applications anyway.
        [NotMapped]
        public List<string> Categories { get; protected set; } = new List<string>();

        ///<summary>Used to workaround EF problem described above</summary>
        public string CategoriesDbCollection
        {
            get
            {
                return string.Join(",", this.Categories);
            }
            set
            {
                this.Categories = value.Split(',').ToList();
            }
        }

        private string _pageTitle;


        ///<summary>Follow this-is-a-blog-post convention without special characters</summary>
        public static string GetUrlTitle(string currentPageTitle)
        {
            return UrlHelper.GetUrlTitleFromPageTitle(currentPageTitle);
        }

        ///<summary>True if post has same ID</summary>
        public virtual bool UpdatePost(BlogPostTemplate newData)
        {
            if (newData.BlogPostTemplateId == this.BlogPostTemplateId)
            {
                this.PageTitle = newData.PageTitle;
                this.FullContent = newData.FullContent;
                this.Author = newData.Author;
                Categories = newData.Categories;
                return true;
            }
            return false;
        }

        public void AddCategory(string category)
        {
            if (!Categories.Contains(category))
            {
                Categories.Add(category);
            }
        }

        public void AddCategories(params string[] categories)
        {
            foreach (var cat in categories)
            {
                AddCategory(cat);
            }
        }

        public void RemoveCategory(string category)
        {
            for (int i = 0; i < Categories.Count; i++)
            {
                if (Categories[i] == category)
                {
                    Categories.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
