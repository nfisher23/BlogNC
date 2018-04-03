using System;
using System.Collections.Generic;
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
        public List<string> Categories { get; protected set; } = new List<string>();


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
