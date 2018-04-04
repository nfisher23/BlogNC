using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHostingEnvironment env)
            : base(options)
        {
            if (env.IsDevelopment())
            {

            }
            else if (env.IsProduction())
            {
                throw new NotImplementedException(); // He wasn't ready!
            }
        }

        public DbSet<BlogPostTemplate> Templates { get; set; }
        public DbSet<BlogPostPublished> Posts { get; set; }
        public DbSet<BlogPostDraft> Drafts { get; set; }
        public DbSet<StaticPage> StaticPages { get; set; }
    }
}
