using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models
{
    public class BlogPostDraft : BlogPostTemplate
    {
        public BlogPostDraft()
        {
            TimeStarted = DateTime.Now;
        }

        public DateTime TimeStarted { get; set; }

        public DateTime LastEdit { get; set; }
    }
}
