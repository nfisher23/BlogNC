using BlogNC.Areas.Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Models.PageModels
{
    public class AdminManagePublishedPostsModel
    {
        public List<BlogPostPublished> Posts { get; set; }
    }
}
