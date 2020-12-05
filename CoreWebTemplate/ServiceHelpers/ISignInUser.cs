using System;
using System.Linq;
using CoreWebTemplate.Models.AspNetUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreWebTemplate.Models.Account;

namespace CoreWebTemplate.Service
{
    public interface ISignInUser
    {

        List<UserEntity> Users { get; set; }

        UserManager<UserEntity> UserManager { get; set; }

        RoleManager<UserRoleEntity> UserRoleManager { get; set; }

        SignInManager<UserEntity> SignInManager { get; set; }

        IOptions<IdentityOptions> IdentityOptions { get; set; }

        Task<bool> SignInAsync( LoginModel model );

        Task SignOutAsync();
    }
}
