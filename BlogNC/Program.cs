using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogNC.Areas.Blog.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlogNC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            IHostingEnvironment env =
                (IHostingEnvironment)host.Services.GetService(typeof(IHostingEnvironment));

            SeedData.EnsureBlogPopulated(host.Services, env);
             
            host.Run(); 
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(opts => opts.ValidateScopes = false)                
                .Build();
    }
}
