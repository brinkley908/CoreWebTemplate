using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CoreWebTemplate.Models.Entity.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CoreWebTemplate.Service;
using AutoMapper;
using blog.Infrastructure;
using CoreWebTemplate.Service.SessionSorageService;
using CoreWebTemplate.Models.AspNetUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using CoreWebTemplate.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddDatabaseContexts( this IServiceCollection services )
        => services.AddDbContext<BlogEntities>( options => { options.UseSqlServer( "Server=192.168.1.65;Trusted_Connection=True;database=blog;pooling=true;MultipleActiveResultSets=True" ); } )
                   .AddDbContext<MemoryIdentities>( options =>
                      {
                          options.UseInMemoryDatabase( "aspnet" );
                      } );

        public static IServiceCollection AddSecurity( this IServiceCollection services )
        {
            //services.AddOpenIddict()
            //        .AddCore( options =>
            //        {
            //            options.UseEntityFrameworkCore()
            //                .UseDbContext<MemoryIdentities>()
            //                .ReplaceDefaultEntities<string>();
            //        } )
            //        .AddServer( options =>
            //        {
            //            options.UseMvc();
            //            options.EnableTokenEndpoint( "/token" );
            //            options.AllowPasswordFlow();
            //            options.AcceptAnonymousClients();
            //        } )
            //        .AddValidation();

            //services.Configure<IdentityOptions>( options =>
            //{
            //    options.ClaimsIdentity.UserNameClaimType = "name"; //OpenIdConnectConstants.Claims.Name;
            //    options.ClaimsIdentity.UserIdClaimType = "sub"; //OpenIdConnectConstants.Claims.Subject;
            //    options.ClaimsIdentity.RoleClaimType = "role";// OpenIdConnectConstants.Claims.Role;
            //} );

            //services.AddAuthentication( options =>
            //{
            //    options.DefaultScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            //} );

            services.AddAuthentication();
            services.AddIdentity<UserEntity, UserRoleEntity>()
            .AddEntityFrameworkStores<MemoryIdentities>()
            .AddSignInManager<SignInManager<UserEntity>>();

            //var builder = services.AddIdentityCore<UserEntity>();
            //builder = new IdentityBuilder(
            //    builder.UserType,
            //    typeof( UserRoleEntity ),
            //    builder.Services );

            //builder.AddRoles<UserRoleEntity>()
            //    .AddEntityFrameworkStores<MemoryIdentities>()
            //    .AddDefaultTokenProviders()
            //    .AddSignInManager<SignInManager<UserEntity>>();

            return services;
        }


        public static IServiceCollection AddServices( this IServiceCollection services, IConfiguration configuration )
         => services.Configure<List<UserEntity>>( configuration.GetSection( "AspNetUser" ) )

                    .AddScoped( typeof( IDBRepository<> ), typeof( DBRepository<> ) )
                    .AddScoped<IPostsRepository, PostsRepistory>()
                    .AddScoped<ISignInService, SignInService>()
                    .AddScoped<IBlogService, BlogService>();
        //.AddScoped<IHtmlHelperFactory, HtmlHelperFactory>();


        public static IServiceCollection AddSessionStorage( this IServiceCollection services )
        => services.AddDistributedMemoryCache()
                   .AddSession( options =>
                   {
                       //options.IdleTimeout = TimeSpan.FromSeconds( 10 );
                       //options.Cookie.HttpOnly = true;
                       options.Cookie.IsEssential = true;
                   } )
                   .AddSingleton<ISessionObjectService, SessionObjectService>()
                   .AddScoped<ISessionStorageService, SessionStorageService>();




        public static IServiceCollection AddMapper( this IServiceCollection services )
        => services.AddAutoMapper( options =>
                   {
                       options.AddProfile<MappingProfile>();
                   } );


    }
}
