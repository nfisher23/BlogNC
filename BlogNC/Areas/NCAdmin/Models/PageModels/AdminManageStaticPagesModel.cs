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
        public List<bool> HomePageMirror { get
            {
                return Pages.Select(sp => sp.IsHomePage).ToList();
            }
        }
    }
}
