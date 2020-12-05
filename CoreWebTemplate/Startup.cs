using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CoreWebTemplate
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services
                .AddDatabaseContexts()
                .AddSecurity()
                .AddServices(Configuration)
                .AddSessionStorage()
                .AddMapper();

            services.AddControllersWithViews();
       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

             app.UseAuthentication();

             app.UseStatusCodePages( async context =>
            {
                var response = context.HttpContext.Response;

                if ( response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                    response.StatusCode == (int)HttpStatusCode.Forbidden ||
                    response.StatusCode == (int)HttpStatusCode.Forbidden )
                    response.Redirect( "/account/login" );

                if(response.StatusCode == (int)HttpStatusCode.Gone)
                    response.Redirect( "/account/logout" );

                response.Redirect( "/account/login" );

            } );

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}" );
             } );
        }
    }
}
