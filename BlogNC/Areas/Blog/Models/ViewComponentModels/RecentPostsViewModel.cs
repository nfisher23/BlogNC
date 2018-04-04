using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models.ViewComponentModels
{
    public class RecentPostsViewModel
    {
        public IQueryable<BlogPostPublished> Posts { get; set; }
    }
}
