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
                    FullContent = $"Sample Content for post no {i}",
                    DatePublished = new DateTime(2017, i, i),
                    TimeOfDayPublished = new TimeSpan(i, 0, 0)
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
                    FullContent = $"Sample Content for post no {i}"
                };

                drafts.Add(d);
            }

            return drafts.AsQueryable();
        }


    }
}
