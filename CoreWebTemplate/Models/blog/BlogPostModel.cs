using CoreWebTemplate.Models.Entity.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Models
{
    public class BlogPostModel : ModelBase
    {
        public int BlogPostID { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public int Shares { get; set; }

        public int Likes { get; set; }

        public DateTime PublishedOn { get; set; }

        public virtual List<BlogComment> BlogComments { get; set; } = new List<BlogComment>();
    }
}
