using CoreWebTemplate.Models.AspNetUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Service.SeedData
{
    public interface ISeedData
    {

        List<UserEntity> Users { get; }
        UserManager<UserEntity> UserManager { get; }
        RoleManager<UserRoleEntity> UserRoleManager { get; }
        SignInManager<UserEntity> SignInManager { get; }
        IOptions<IdentityOptions> IdentityOptions { get; }

        Task AddTestUsers();

    }
}
