using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models
{
    public class BlogPostPublished : BlogPostTemplate
    {
        public BlogPostPublished()
        { }

        public TimeSpan TimeOfDayPublished { get; set; }
        public DateTime DatePublished
        {
            get { return _datePublished.Date; }
            set { _datePublished = value.Date; }
        }


        private DateTime _datePublished;
    }
}
