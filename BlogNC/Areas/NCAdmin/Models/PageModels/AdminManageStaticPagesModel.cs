using BlogNC.Areas.Blog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Models.PageModels
{
    public class AdminManageStaticPagesModel
    {
        public List<StaticPage> Pages { get; set; }

        [Required(ErrorMessage = "There must be exactly one landing/home page")]
        public StaticPage HomePage { get
            {
                var num = Pages.Count(sp => sp.IsHomePage == true);
                if (num == 1)
                {
                    return Pages.Where(sp => sp.IsHomePage).First();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                foreach (var page in Pages)
                {
                    if (page.StaticPageId == value.StaticPageId)
                    {
                        page.IsHomePage = true;
                    }
                    else
                    {
                        page.IsHomePage = false;
                    }
                }
            }
        } 

    }
}
