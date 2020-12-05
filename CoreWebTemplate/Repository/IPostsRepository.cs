
using CoreWebTemplate.Models;
using CoreWebTemplate.Models.Entity.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Repository
{
    public interface IPostsRepository : IDBRepository<BlogPost>
    {

        Task<List<BlogPost>> GetPosts();

        Task<BlogPost> GetPost( int id );

        Task<List<BlogTitleModel>> GetTitles();

    }
}
