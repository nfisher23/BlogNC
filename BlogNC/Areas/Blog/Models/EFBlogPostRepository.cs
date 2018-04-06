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

        public BlogPostDraft GetDraftById(int draftId)
        {
            return AppDbContext.Drafts
                .Where(draft => draft.BlogPostTemplateId == draftId)
                .FirstOrDefault();
        }

        public bool SavePublishedPost(BlogPostPublished post)
        {
            if (post.BlogPostTemplateId == 0)
            {
                var count = AppDbContext.Posts
                    .Count(p => p.UrlTitle.ToLower() == post.UrlTitle.ToLower());

                if (count == 0)
                {
                    AppDbContext.Posts.Add(post);
                    AppDbContext.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            else
            {
                var postToUpdate = GetPostById(post.BlogPostTemplateId);
                if (postToUpdate != null)
                {
                    postToUpdate.UpdatePost(post);
                    AppDbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool SaveDraft(BlogPostDraft draft)
        {
            if (draft.BlogPostTemplateId == 0)
            {
                var count = AppDbContext.Drafts
                    .Count(p => p.UrlTitle.ToLower() == draft.UrlTitle.ToLower());

                if (count == 0)
                {
                    AppDbContext.Drafts.Add(draft);
                    AppDbContext.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            else
            {
                var draftToUpdate = GetDraftById(draft.BlogPostTemplateId);
                if (draftToUpdate != null)
                {
                    draftToUpdate.UpdatePost(draft);
                    AppDbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
