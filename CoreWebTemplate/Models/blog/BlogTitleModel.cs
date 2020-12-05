using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Models
{
    public class BlogTitleModel
    {

        public int BlogPostID { get; set; }

        public string Title { get; set; }

        public int CommentCount { get; set; }

        public int Shares { get; set; }

        public int Likes { get; set; }

        public DateTime PublishedOn { get; set; }

    }
}
