using CoreWebTemplate.Models;
using CoreWebTemplate.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Service
{
    public interface IBlogService
    {

        Task<BlogListModel> GetTitlesAsync();

        Task<BlogPostModel> GetPostAsync(int id);

    }
}
