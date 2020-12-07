using AutoMapper.Configuration;
using CoreWebTemplate.Models.AspNetUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate
{
    public class SeedData
    {


        public static async Task InitializeAsync( IServiceProvider services, List<UserEntity> users )
        {


            await AddTestUsers(
                services.GetRequiredService<RoleManager<UserRoleEntity>>(),
                services.GetRequiredService<UserManager<UserEntity>>(),
              users
                );
        }

        public static async Task AddTestUsers( RoleManager<UserRoleEntity> roleManager, UserManager<UserEntity> userManager, List<UserEntity> users )
        {
            if ( roleManager.Roles.Any() || userManager.Users.Any() )
                return;


            await roleManager.CreateAsync( new UserRoleEntity( "Admin" ) { Id = Guid.NewGuid().ToString() } );


            foreach ( var user in users )
            {
                var newUser = new UserEntity
                {

                    Id = Guid.NewGuid().ToString(),
                    Email = user.Email,
                    UserName = user.UserName
                };

                await userManager.CreateAsync( newUser, "LetMeIn2020!!" );
                await userManager.AddToRoleAsync( newUser, "Admin" );
                await userManager.UpdateAsync( newUser );
            }



        }


    }


}
