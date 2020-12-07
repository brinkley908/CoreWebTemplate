using System;
using CoreWebTemplate.Models.AspNetUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreWebTemplate.Models;
using CoreWebTemplate.Models.Account;

namespace CoreWebTemplate.Service
{
    public class SignInService : ISignInService
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<UserRoleEntity> _roleManager;

        public SignInService(
           IOptions<IdentityOptions> identityOptions,
           SignInManager<UserEntity> signInManager,
           UserManager<UserEntity> userManager,
           RoleManager<UserRoleEntity> roleManager )
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Constants
        private const string ErrInvalidCredentials = "Invalid Username or Password";

        private const string ErrSignInNotAllowed = "The specified user is not allowed to sign in";
        #endregion

        #region Properties


        private ErrorAuthModel _error;
        public ErrorAuthModel Error { get { return _error; } }
        #endregion

        #region
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        #endregion

        #region SignIn
        public async Task<bool> SignInAsync( LoginModel model )
        {
            var user = await _userManager.FindByNameAsync( model.Username );
            if ( user == null )
                return SetError( "NotFound", ErrInvalidCredentials );

            if ( !await _signInManager.CanSignInAsync( user ) )
                return SetError( "NotAllowed", ErrSignInNotAllowed );

            if ( _userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync( user ) )
                return SetError( "Locked", ErrInvalidCredentials );

            if ( !await _userManager.CheckPasswordAsync( user, model.Password ) )
            {
                if ( _userManager.SupportsUserLockout )
                {
                    await _userManager.AccessFailedAsync( user );
                }

                return SetError( "PasswordFailed", ErrInvalidCredentials );
            }

            if ( _userManager.SupportsUserLockout )
                await _userManager.ResetAccessFailedCountAsync( user );

            var roles = new string[0];
            if ( _userManager.SupportsUserRole )
            {
                roles = ( await _userManager.GetRolesAsync( user ) ).ToArray();
            }

            var siginInResult = await _signInManager.PasswordSignInAsync( model.Username, model.Password, false, false );

            return siginInResult.Succeeded;
        }

        #endregion

        #region Function Library
        private bool SetError( string error, string message )
        {
            _error = new ErrorAuthModel
            {
                Error = error,
                Message = message
            };

            return false;
        }
        #endregion

    }
}
