﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models
{
    public interface IBlogPostRepository
    {
        IQueryable<BlogPostPublished> Posts { get; }
        IQueryable<BlogPostDraft> Drafts { get; }
        IQueryable<StaticPage> StaticPages { get; }

        BlogPostPublished GetPostByUrlTitle(string urlTitle);
        IQueryable<BlogPostPublished> GetAllPostsDescending();
        IQueryable<StaticPage> GetNavBarStaticPages();
        IQueryable<StaticPage> GetFooterStaticPages();
    }
}
