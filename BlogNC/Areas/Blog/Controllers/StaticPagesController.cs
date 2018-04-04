using BlogNC.Areas.Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Controllers
{
    public class StaticPagesController : Controller
    {
        IBlogPostRepository blogRepository;
        public StaticPagesController(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }

        public ViewResult Index()
        {
            // temp placeholder to develop other functionality
            return View();
        }

        public ViewResult FindStaticPage(string urlTitle)
        {
            if (urlTitle == null || urlTitle == "")
                return View("Index");

            // final product will look for user created static page entries
            throw new NotImplementedException();
        }
    }
}
