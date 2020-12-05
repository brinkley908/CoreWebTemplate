using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using CoreWebTemplate.Models;
using CoreWebTemplate.Models.Entity.Blog;

namespace blog.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BlogPost, BlogPostModel>()
                .ForMember( d => d.BlogComments, o => o.MapFrom( src => src.BlogComments.ToList() ) );

            CreateMap<BlogPost, BlogTitleModel>()
                .ForMember( d => d.CommentCount, o => o.MapFrom( src => src.BlogComments.Count ) );

        }
    }
}
