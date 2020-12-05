using CoreWebTemplate.Models.Entity;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreWebTemplate.Models;
using Microsoft.EntityFrameworkCore;
using CoreWebTemplate.Models.Entity.Blog;

namespace CoreWebTemplate.Repository
{
    public class PostsRepistory : DBRepository<BlogPost>, IPostsRepository
    {

        private readonly IConfigurationProvider _mappingConfig;
        private readonly IMapper _mapper;

        public PostsRepistory( BlogEntities repositoryPatternDemoContext, IMapper mapper, IConfigurationProvider mappingConfig ) : base( repositoryPatternDemoContext )
        {
            _mappingConfig = mappingConfig;
            _mapper = _mappingConfig.CreateMapper();
        }

        public async Task<List<BlogTitleModel>> GetTitles()
        {
            return await GetAll( "BlogComments" )
                 .OrderByDescending( x => x.PublishedOn )
                 .ProjectTo<BlogTitleModel>( _mappingConfig )
                 .ToListAsync();

        }

        public async Task<List<BlogPost>> GetPosts()
        {
            return await GetAll( "BlogComments" ).ToListAsync();
        }

        public async Task<BlogPost> GetPost( int id )
        {
            return await GetAll().FirstOrDefaultAsync( x => x.BlogPostID == id );
        }

    }
}
