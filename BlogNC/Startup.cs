﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogNC.Areas.NCAccount.Models;
using BlogNC.Areas.Blog.Infrastructure;
using BlogNC.Areas.Blog.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogNC
{
    public class Startup
    {
        IConfiguration AppSettingsConfiguration;
        IHostingEnvironment CurrentEnvironment;
        public Startup(IConfiguration config, IHostingEnvironment environment)
        {
            AppSettingsConfiguration = config;
            CurrentEnvironment = environment;
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            var blogContextConnectionString = AppSettingsConfiguration.GetConnectionString("AppDbContext");
            var identityContextConnectionString = AppSettingsConfiguration.GetConnectionString("AppIdentityDbContext");
            

            services.AddSingleton<IHostingEnvironment>(CurrentEnvironment);
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(blogContextConnectionString));
            services.AddDbContext<AppIdentityDbContext>(o => o.UseSqlite(identityContextConnectionString));
            services.AddTransient<IBlogPostRepository, EFBlogPostRepository>();
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<RazorViewEngineOptions>(opts =>
            {
                // for preview draft functionality
                opts.AreaViewLocationFormats.Insert(0, "Areas/Blog/Views/Post/{0}" + RazorViewEngine.ViewExtension);
                opts.AreaViewLocationFormats.Insert(0, "Areas/Blog/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddMvc();
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            var blogContextConnectionString = AppSettingsConfiguration.GetConnectionString("AppDbContext");

            // SQLite does not have support for a large number of built in EF Core migrations--
            // when we move to production we will use a local PostgreSQL DB and cross 
            // this bridge once the MVR is completed
            throw new NotImplementedException();
            //services.AddDbContext<ApplicationDbContext>(o => o.UseNpgSql(blogContextConnectionString));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction())
            {
                app.UseExceptionHandler("/ErrorHandlingPage");
            }


            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "",
                    template: "Blog/{urlTitle?}",
                    defaults: new { area = "Blog", controller = "Post", action = "FindPost" });

                routes.MapRoute(
                    name: "",
                    template: "NCAdmin/{action=Home}",
                    defaults: new { area = "NCAdmin", controller = "Admin" });

                routes.MapRoute(
                    name: "",
                    template: "Account/{action=Login}",
                    defaults: new { area = "NCAccount", controller = "Account" });

                routes.MapRoute(
                    name: "",
                    template: "ErrorHandlingPage",
                    defaults: new
                    {
                        area = "Blog",
                        controller = "StaticPages",
                        action = "ErrorHandlingPage"
                    });

                routes.MapRoute(
                    name: "",
                    template: "{urlTitle?}",
                    defaults: new { area = "Blog", controller = "StaticPages",
                        action = "FindStaticPage"});



            });
        }
    }
}
