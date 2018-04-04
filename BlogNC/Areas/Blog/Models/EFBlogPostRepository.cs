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

        public BlogPostPublished GetPostByUrlTitle(string urlTitle)
        {
            return AppDbContext.Posts
                .Where(p => p.UrlTitle.ToLower() == urlTitle.ToLower()).FirstOrDefault();
        }

    }
}
