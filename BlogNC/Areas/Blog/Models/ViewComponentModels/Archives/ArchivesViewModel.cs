using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Models.ViewComponentModels.Archives
{
    public class ArchivesViewModel
    {
        public List<MonthAndYearPostBucket> BucketsOfPostsDescending
        {
            get
            {
                if (_bucketsDescending == null || _bucketsDescending.Count == 0)
                    _bucketsDescending = GetBuckets();

                return _bucketsDescending;
            }
        }

        List<MonthAndYearPostBucket> _bucketsDescending = new List<MonthAndYearPostBucket>();

        IBlogPostRepository blogRepository;
        public ArchivesViewModel(IBlogPostRepository repo)
        {
            blogRepository = repo;
        }


        private List<MonthAndYearPostBucket> GetBuckets()
        {
            _bucketsDescending = new List<MonthAndYearPostBucket>();

            var descendingPosts = blogRepository.GetAllPostsDescending();

            List<MonthAndYearPostBucket> buckets = new List<MonthAndYearPostBucket>();
            MonthAndYearPostBucket bucket = null;
            foreach (var post in descendingPosts)
            {
                var candidate = new MonthAndYearPostBucket(post);
                if (buckets.Count > 0)
                {
                    var latest = buckets[buckets.Count - 1];
                    if (latest.MonthAndYear == candidate.MonthAndYear)
                    {
                        latest.TryAddPost(post);
                    }
                    else
                    {
                        buckets.Add(candidate);
                    }
                }
                else
                    buckets.Add(candidate);
            }
            // final add
            if (bucket != null)
                buckets.Add(bucket);

            return buckets;
        }
    }
}
