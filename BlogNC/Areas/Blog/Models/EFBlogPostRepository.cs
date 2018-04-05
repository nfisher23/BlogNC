using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models
{
    public class EFBlogPostRepository : IBlogPostRepository
    {
        private ApplicationDbContext AppDbContext;
        public EFBlogPostRepository(ApplicationDbContext context)
        {
            AppDbContext = context;
        }

        public IQueryable<BlogPostPublished> Posts
        {
            get
            {
                return AppDbContext.Posts;
            }
        }

        public IQueryable<BlogPostDraft> Drafts
        {
            get
            {
                return AppDbContext.Drafts;
            }
        }

        public IQueryable<StaticPage> StaticPages
        {
            get
            {
                return AppDbContext.StaticPages;
            }
        }

        public IQueryable<BlogPostPublished> GetAllPostsDescending()
        {
            return AppDbContext.Posts.OrderByDescending(p => p.DateTimePublished);
        }

        public IQueryable<StaticPage> GetFooterStaticPages()
        {
            return AppDbContext.StaticPages.Where(sp => sp.InFooter)
                .OrderBy(sp => sp.FooterPriority);
        }


        public IQueryable<StaticPage> GetNavBarStaticPages()
        {
            return AppDbContext.StaticPages.Where(sp => sp.InMainNav)
                .OrderBy(sp => sp.MainNavPriority);
        }

        public BlogPostPublished GetPostByUrlTitle(string urlTitle)
        {
            return AppDbContext.Posts
                .Where(p => p.UrlTitle.ToLower() == urlTitle.ToLower())
                .FirstOrDefault();
        }

        public StaticPage GetStaticPageByUrlTitle(string urlTitle)
        {
            return AppDbContext.StaticPages
                .Where(p => p.UrlTitle.ToLower() == urlTitle.ToLower())
                .FirstOrDefault();
        }

        public IQueryable<BlogPostDraft> GetMostRecentDrafts(int numToGet)
        {
            return AppDbContext.Drafts
                .OrderByDescending(d => d.LastEdit).Take(numToGet);
        }

        public IQueryable<BlogPostPublished> GetMostRecentPosts(int numToGet)
        {
            return AppDbContext.Posts
                .OrderByDescending(p => p.DateTimePublished)
                .Take(numToGet);
        }

        public IQueryable<StaticPage> GetStaticPagesByPriorityAscending()
        {
            return AppDbContext.StaticPages
                .OrderBy(sp => sp.MainNavPriority);
        }

        public BlogPostPublished GetPostById(int publishedPostId)
        {
            return AppDbContext.Posts
                .Where(post => post.BlogPostTemplateId == publishedPostId)
                .FirstOrDefault();
        }
    }
}
