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
        [Required]
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
        public int? MainNavPriority { get; set; } = 100;
        ///<summary>Lower number means earlier on the list</summary>
        public int? FooterPriority { get; set; } = 100;

        public void UpdatePage(StaticPage page)
        {
            StaticPageId = page.StaticPageId;
            PageTitle = page.PageTitle;
            FullContent = page.FullContent;
            InMainNav = page.InMainNav;
            InFooter = page.InFooter;
            MainNavPriority = page.MainNavPriority;
            FooterPriority = page.FooterPriority;
        }
    }
}
