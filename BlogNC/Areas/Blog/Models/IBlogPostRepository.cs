using System;
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
        ///<summary>Most recent first</summary>
        IQueryable<BlogPostPublished> GetAllPostsDescending();
        IQueryable<StaticPage> GetNavBarStaticPages();
        IQueryable<StaticPage> GetFooterStaticPages();
        StaticPage GetStaticPageByUrlTitle(string urlTitle);

        IQueryable<BlogPostPublished> GetMostRecentPosts(int numToGet);
        IQueryable<BlogPostDraft> GetMostRecentDrafts(int numToGet);
        ///<summary>Lowest number (most important) first</summary>
        IQueryable<StaticPage> GetStaticPagesByPriorityAscending();

    }
}
