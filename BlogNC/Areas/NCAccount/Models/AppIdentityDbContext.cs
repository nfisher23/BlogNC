using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAccount.Models
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> opts,
            IHostingEnvironment env): base(opts)
        {
            if (env.IsDevelopment())
            {
                Database.EnsureCreated();
            }
            else if (env.IsProduction())
            {
                Database.EnsureCreated();
            }
        }
    }
}
