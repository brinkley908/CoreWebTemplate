using CoreWebTemplate.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreWebTemplate.Models.Entity.Blog;
using CoreWebTemplate.Repository;

namespace CoreWebTemplate.Service
{
    public class BlogService : IBlogService
    {
       
        private readonly IPostsRepository _context;
        private readonly IConfigurationProvider _mappingConfig;
        private readonly IMapper _mapper;


        public BlogService( IPostsRepository context, IMapper mapper, IConfigurationProvider mappingConfig )
        {

            _context = context;
            _mappingConfig = mappingConfig;
            _mapper = _mappingConfig.CreateMapper();

        }

        public async Task<BlogPostModel> GetPostAsync( int id )
        {
            var entity = await _context.GetPost( id );


            return entity == null
                ?
                new BlogPostModel { BlogComments = new List<BlogComment>() }
                :
                _mapper.Map<BlogPostModel>( entity );
        }

        public async Task<BlogListModel> GetTitlesAsync()
        {

            var titles = await _context.GetTitles();

            Random r = new Random();

            foreach ( var title in titles )
            {
                title.Shares = r.Next( 0, 50 );
                title.Likes = r.Next( 0, 100 );
            }

            return new BlogListModel
            {
                BlogTitles = titles
            };
        }

    }
}
