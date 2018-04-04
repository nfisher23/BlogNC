using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models.ViewComponentModels.Archives
{
    public class MonthAndYearPostBucket
    {
        public DateTime MonthAndYear
        {
            get
            {
                return new DateTime(FullYear, Month, 1);
            }
        }

        public int Month { get; }
        public int FullYear { get; }
        public string DisplayName { get
            {
                return $"{MonthAndYear.ToString("MMMM yyyy")}";
            }
        }

        public string IdName { get
            {
                return $"Archives-{MonthAndYear.ToString("MMM-yyyy")}";
            }
        }

        public List<BlogPostPublished> Posts { get; } = new List<BlogPostPublished>();

        public MonthAndYearPostBucket(BlogPostPublished post)
        {
            Month = post.DatePublished.Month;
            FullYear = post.DatePublished.Year;
            TryAddPost(post);
        }

        ///<summary>Tries to add to the bucket. 
        ///Returns true if it matches with the model and addition was successful. False if not</summary>
        public bool TryAddPost(BlogPostPublished post)
        {
            if (Month == post.DatePublished.Month
                && FullYear == post.DatePublished.Year)
            {
                Posts.Add(post);
                return true;
            }
            return false;
        }
    }
}
