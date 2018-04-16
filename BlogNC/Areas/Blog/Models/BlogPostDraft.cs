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
            LastEdit = DateTime.Now;
        }

        
        public DateTime TimeStarted { get; }

        public DateTime LastEdit { get; set; }

        public bool UpdatePost(BlogPostDraft newData)
        {
            if (base.UpdatePost(newData) == false)
                return false;

            LastEdit = newData.LastEdit;
            return true;
        }

        public void UnPublishToDraft(BlogPostPublished published)
        {
            this.BlogPostTemplateId = published.BlogPostTemplateId;
            base.UpdatePost(published);
            this.BlogPostTemplateId = 0;
        }
    }
}
