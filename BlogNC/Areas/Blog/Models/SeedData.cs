using BlogNC.Areas.NCAccount.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
            else if (environment.IsProduction())
            {
                using (ApplicationDbContext context
                    = provider.GetRequiredService<ApplicationDbContext>())
                {
                    context.Database.EnsureCreated();
                    if (context.StaticPages.Count() == 0)
                    {
                        context.StaticPages.Add(GenerateProductionStaticPage());
                        context.Drafts.Add(GenerateProductionDraft());
                    }
                    context.SaveChanges();
                }
            }
        }

        public static string DefaultUsername = "BlogNCUser";
        public static string DefaultPassword = "BlogNCDefaultPassword1!";

        public static async Task EnsureIdentityPopulated(IServiceProvider provider,
            IHostingEnvironment environment)
        {
            using (var context = provider.GetRequiredService<AppIdentityDbContext>())
            {
                var _userManager = provider.GetRequiredService<UserManager<AppUser>>();

                if (!_userManager.Users.Any())
                {
                    AppUser user = new AppUser
                    {
                        UserName = DefaultUsername
                    };
                    await _userManager.CreateAsync(user, DefaultPassword);
                }
            }
        }

        public static bool IsFirstSignIn(string username, string password)
        {
            return (username == DefaultUsername && password == DefaultPassword);
        }

        private static IQueryable<BlogPostPublished> GenerateSamplePosts()
        {
            List<BlogPostPublished> posts = new List<BlogPostPublished>();

            List<string> cats = new List<string>();


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
                p.AddCategories("Cat" + i, "BigCat");
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
                FullContent = "Welcome to BlogNC...",
                InMainNav = true,
                InFooter = true,
                MainNavPriority = 0,
                FooterPriority = 0,
                IsHomePage = true
            });

            pages.Add(new StaticPage
            {
                PageTitle = "About",
                FullContent = "About the author..",
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
                MainNavPriority = 100,
                FooterPriority = 2
            });

            pages.Add(new StaticPage
            {
                PageTitle = "Mission",
                FullContent = "Mission statement for BlogNC...",
                InMainNav = true,
                InFooter = false,
                MainNavPriority = 2,
                FooterPriority = 100
            });


            return pages.AsQueryable();
        }

        private static StaticPage GenerateProductionStaticPage()
        {
            var page = new StaticPage
            {
                PageTitle = "Welcome to BlogNC",
                FullContent = "<h3>Welcome</h3><br /><p>Welcome to BlogNC, an open source blogging" +
                " platform coded in ASP .NET Core MVC. I wanted to create a wordpress alternative " +
                "for anyone who knows how to program in C#, or for anyone who is a little annoyed at using " +
                "php all the time :)</p>" +
                "<p>This project is still very much under development. There are many features that should be " +
                "and will be added as time moves along. I appreciate your interest, and do hope that you find " +
                "it serves (most of) your needs. Because this project is open source, I do invite you to " +
                "contribute on <a href=\"https://github.com/nfisher23/BlogNC\">The BlogNC GitHub page</a>. We" +
                " appreciate any feedback you give us in advance!</p>" +
                "<p>To get started with BlogNC, review the aforementioned GitHub page for the latest instructions." +
                " We hope you have a wonderful time!</p>" +
                "<p>-Nick Fisher, BlogNC founder and voted most-handsome BlogNC developer for three weeks running</p>",
                InMainNav = true,
                MainNavPriority = 0,
                InFooter = true,
                FooterPriority = 0,
                IsHomePage = true
            };
            return page;
        }

        private static BlogPostDraft GenerateProductionDraft()
        {
            var draft = new BlogPostDraft
            {
                PageTitle = "A Sample Draft",
                FullContent = "<p>Welcome to BlogNC! This is a sample draft. You can " +
                "publish this draft if you want, or delete this one and create your own. The sky " +
                "is the limit (and software limitations, but that's less of a philisophical argument).</p>",
                Author = "You!",
                CategoriesDbCollection = "FirstCategory"
            };
            return draft;
        }
    }
}
