using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models
{
    public static class SeedData
    {
        public static void EnsureBlogPopulated(IServiceProvider provider,
            IHostingEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                using (ApplicationDbContext context
                    = provider.GetRequiredService<ApplicationDbContext>())
                {
                    // development settings
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    context.Posts.AddRange(GenerateSamplePosts());
                    context.Drafts.AddRange(GenerateSampleDrafts());
                    context.SaveChanges();

                    // static pages
                    context.StaticPages.AddRange(GenerateStaticPages());
                    context.SaveChanges();
                }
            }
        }



        private static IQueryable<BlogPostPublished> GenerateSamplePosts()
        {
            List<BlogPostPublished> posts = new List<BlogPostPublished>();
            for (int i = 1; i < 12; i++)
            {
                BlogPostPublished p = new BlogPostPublished
                {
                    PageTitle = $"Post Title No {i}",
                    FullContent = $"<h3>A Sample</h3> <p>Content for post no {i}</p>",
                    DatePublished = new DateTime(2017, i, i),
                    TimeOfDayPublished = new TimeSpan(i, 0, 0),
                    Author = "Nick Fisher"
                };

                posts.Add(p);
            }
            return posts.AsQueryable();
        }

        private static IQueryable<BlogPostDraft> GenerateSampleDrafts()
        {
            List<BlogPostDraft> drafts = new List<BlogPostDraft>();
            for (int i = 1; i < 10; i++)
            {
                BlogPostDraft d = new BlogPostDraft
                {
                    PageTitle = $"Draft Title No {i}",
                    FullContent = $"Sample Content for post no {i}",
                    Author = "Nick Fisher"
                };

                drafts.Add(d);
            }

            return drafts.AsQueryable();
        }

        private static IQueryable<StaticPage> GenerateStaticPages()
        {
            List<StaticPage> pages = new List<StaticPage>();

            pages.Add(new StaticPage
            {
                PageTitle = "Home",
                FullContent = "Welcome to BlogNC",
                InMainNav = true,
                InFooter = true,
                MainNavPriority = 0,
                FooterPriority = 0
            });

            pages.Add(new StaticPage
            {
                PageTitle = "About",
                FullContent = "About the author",
                InMainNav = true,
                InFooter = true,
                MainNavPriority = 1,
                FooterPriority = 1
            });

            pages.Add(new StaticPage
            {
                PageTitle = "Contact",
                FullContent = "Contact Information for BlogNC...",
                InMainNav = false,
                InFooter = true,
                MainNavPriority = 1000,
                FooterPriority = 2
            });

            pages.Add(new StaticPage
            {
                PageTitle = "Mission",
                FullContent = "Mission statement for BlogNC...",
                InMainNav = true,
                InFooter = false,
                MainNavPriority = 2,
                FooterPriority = 1000
            });


            return pages.AsQueryable();
        }
    }
}
