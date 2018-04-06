using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        ///<summary>Made model binding easier to split it up</summary>
        [NotMapped]
        public DateTime DateTimePublished
        {
            get
            {
                return DatePublished.Add(TimeOfDayPublished);
            }
            set
            {
                DatePublished = value.Date;
                TimeOfDayPublished = value.TimeOfDay;
            }
        }

        private DateTime _datePublished;

        public bool UpdatePost(BlogPostPublished newData)
        {
            if (base.UpdatePost(newData) == false)
            {
                return false;
            }
            TimeOfDayPublished = newData.TimeOfDayPublished;
            DatePublished = newData.DatePublished;
            return true;
        }
    }
}
