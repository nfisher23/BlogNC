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

        ///<summary>Most recent first</summary>
        IQueryable<BlogPostPublished> GetAllPostsDescending();
        BlogPostPublished GetPostByUrlTitle(string urlTitle);
        BlogPostPublished GetPostById(int publishedPostId);
        IQueryable<BlogPostPublished> GetMostRecentPosts(int numToGet);
        ///<summary>True if successful, false if a post with the same title already exists</summary>
        bool SavePublishedPost(BlogPostPublished post);
        void UnPublishPostToDraft(BlogPostPublished published);
        void PublishDraftToPost(BlogPostDraft draft);
        void DeleteDraft(BlogPostDraft draft);

        IQueryable<BlogPostDraft> GetMostRecentDrafts(int numToGet);
        BlogPostDraft GetDraftById(int draftId);
        bool SaveDraft(BlogPostDraft draft);
        


        IQueryable<StaticPage> GetNavBarStaticPages();
        IQueryable<StaticPage> GetFooterStaticPages();
        StaticPage GetStaticPageByUrlTitle(string urlTitle);
        StaticPage GetStaticPageById(int id);
        ///<summary>Retrives the (hopefully single) home/landing page </summary>
        StaticPage GetHomePage();
        ///<summary>Lowest number (most important) first</summary>
        IQueryable<StaticPage> GetStaticPagesByPriorityAscending();
        bool SaveStaticPage(StaticPage page);
        ///<summary>Update information about the file, but not its full content</summary>
        void UpdateMetadata(StaticPage page);
        void DeleteStaticPage(StaticPage page);


    }
}
