using CoreWebTemplate.Models.AspNetUsers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var host = CreateWebHostBuilder( args ).Build();
            InitializeDatabase( host );
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder( string[] args ) =>
            WebHost.CreateDefaultBuilder( args )
                .UseStartup<Startup>();

        public static void InitializeDatabase( IWebHost host )
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var users = new List<UserEntity>();
            services.GetService<IConfiguration>().GetSection( "AspNetUser" ).Bind( users );

            try
            {
                SeedData.InitializeAsync( services, users ).Wait();
            }
            catch ( Exception ex )
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError( ex, "An error occurred seeding the database." );
            }
        }
    }
}
