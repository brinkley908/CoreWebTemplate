using NUnit.Framework;
using CoreWebTemplate;
using CoreWebTemplate.Models;
using CoreWebTemplate.Models.Entity.Blog;
using CoreWebTemplate.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using blog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Tests
{

    public class Tests
    {


        private IServiceCollection services;
        private ServiceProvider serviceProvider;


        [SetUp]
        public void Setup()
        {
            services = new ServiceCollection();
            services.AddAutoMapper( options => options.AddProfile<MappingProfile>() );

            var connectionLocal = "Server=localhost;Trusted_Connection=True;database=blog;pooling=true;MultipleActiveResultSets=True";

            services
                .AddDbContext<BlogEntities>(
                    options =>
                    {
                        options.UseSqlServer( connectionLocal );
                    } );

            services.AddScoped<IBlogService, BlogService>();

            serviceProvider = services.BuildServiceProvider();

        }

        [TearDown]
        public void TearDown()
        {
            serviceProvider?.Dispose();
        }

        [Test]
        public async Task TestBlogService_Pass()
        {

            var svc = serviceProvider.GetService<IBlogService>();
            var results = await svc.GetTitlesAsync(); 

            var expected = true;
            var actual = results?.BlogTitles != null;

            Assert.AreEqual( expected, actual );
        }

        [Test]
        public async Task TestBlogService_Fail()
        {

            var svc = serviceProvider.GetService<IBlogService>();
            var actual = await svc.GetPostAsync( -100 );

            var expected = new BlogPostModel
            {
                BlogPostID = 100
            };

            Assert.AreNotEqual( expected, actual );
        }


    }
}