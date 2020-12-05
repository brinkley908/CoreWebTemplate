using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Models
{
    public class BlogListModel : ModelBase
    {

        public int Id { get; set; }

        public int Shares { get; set; }

        public int Likes { get; set; }

        public List<BlogTitleModel> BlogTitles { get; set; }
    }
}
