﻿using BlogNC.Areas.Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Models.ViewComponentModels
{
    public class AdminRecentPostsViewModel
    {
        public IEnumerable<BlogPostPublished> Posts { get; set; }
    }
}
