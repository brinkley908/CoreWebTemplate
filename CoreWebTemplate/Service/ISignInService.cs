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
    public interface ISignInService
    {

     
        Task<bool> SignInAsync( LoginModel model );

        Task SignOutAsync();
    }
}
