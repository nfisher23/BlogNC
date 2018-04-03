using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models
{
    ///<summary>Base class for Posts (published) and Drafts (works-in-progress)</summary>
    public abstract class BlogPostTemplate
    {
        public int BlogPostTemplateId { get; set; } // for entity framework

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
        public string FullContent { get; set; }


        // EF Core does not yet (as of EF Core 2.0) support 
        // many-to-many relationships and will probably never support 
        // primitive types. The workaround here, using a entity class for the join table:
        // https://docs.microsoft.com/en-us/ef/core/modeling/relationships#many-to-many
        // can allegedly run into problems when you try to migrate schemas.
        // To avoid falling down this rabbit hole we'll just take a slight 
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
        private string GetUrlTitle(string currentPageTitle)
        {
            string _urlTitle = "";
            foreach (var letter in currentPageTitle)
            {
                if (char.IsLetterOrDigit(letter))
                {
                    _urlTitle += letter;
                }
                else if (letter == ' ')
                {
                    _urlTitle += "-";
                }
            }
            return _urlTitle;
        }
    }
}
