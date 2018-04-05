using BlogNC.Areas.Blog.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models
{
    public class StaticPage
    {
        public int StaticPageId { get; set; }
        public string PageTitle { get; set; }
        public string UrlTitle
        {
            get
            {
                return UrlHelper.GetUrlTitleFromPageTitle(PageTitle);
            }
        }

        public string FullContent { get; set; }



        public bool InMainNav { get; set; }
        public bool InFooter { get; set; }
        ///<summary>Lower number means earlier on the list</summary>
        [Range(0, int.MaxValue, ErrorMessage = "The priority cannot be lower than zero")]
        public int MainNavPriority { get; set; } = int.MaxValue;
        ///<summary>Lower number means earlier on the list</summary>
        [Range(0, int.MaxValue, ErrorMessage = "The priority cannot be lower than zero")]
        public int FooterPriority { get; set; } = int.MaxValue;
    }
}
