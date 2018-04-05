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
            throw new NotImplementedException();
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
    }
}
