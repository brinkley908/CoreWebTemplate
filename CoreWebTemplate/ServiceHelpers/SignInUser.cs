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
    public class SignInUser : ISignInUser
    {
        #region Constants
        private const string ErrInvalidCredentials = "Invalid Username or Password";

        private const string ErrSignInNotAllowed = "The specified user is not allowed to sign in";
        #endregion

        #region Properties
        public List<UserEntity> Users { get; set; }

        public UserManager<UserEntity> UserManager { get; set; }

        public RoleManager<UserRoleEntity> UserRoleManager { get; set; }

        public SignInManager<UserEntity> SignInManager { get; set; }

        public IOptions<IdentityOptions> IdentityOptions { get; set; } 

        private ErrorAuthModel _error;
        public ErrorAuthModel Error { get { return _error; }  }
        #endregion

        #region
        public async Task SignOutAsync()
        {
            await SignInManager.SignOutAsync();
        }
        #endregion

        #region SignIn
        public async Task<bool> SignInAsync( LoginModel model )
        {
            var user = await UserManager.FindByNameAsync( model.Username );
            if ( user == null )
                return SetError( "NotFound", ErrInvalidCredentials );

            if ( !await SignInManager.CanSignInAsync( user ) )
                return SetError( "NotAllowed", ErrSignInNotAllowed );

            if ( UserManager.SupportsUserLockout && await UserManager.IsLockedOutAsync( user ) )
                return SetError( "Locked", ErrInvalidCredentials );

            if ( !await UserManager.CheckPasswordAsync( user, model.Password ) )
            {
                if ( UserManager.SupportsUserLockout )
                {
                    await UserManager.AccessFailedAsync( user );
                }

                return SetError( "PasswordFailed", ErrInvalidCredentials );
            }

            if ( UserManager.SupportsUserLockout )
                await UserManager.ResetAccessFailedCountAsync( user );

            var roles = new string[0];
            if ( UserManager.SupportsUserRole )
            {
                roles = ( await UserManager.GetRolesAsync( user ) ).ToArray();
            }
          
            var siginInResult = await SignInManager.PasswordSignInAsync( model.Username, model.Password, false, false );

            return siginInResult.Succeeded;
        }

        private static void AddRolesToPrincipal( ClaimsPrincipal principal, string[] roles )
        {
            var identity = principal.Identity as ClaimsIdentity;

            var alreadyHasRolesClaim = identity.Claims.Any( c => c.Type == "role" );

            if ( !alreadyHasRolesClaim && roles.Any() )
                identity.AddClaims( roles.Select( r => new Claim( "role", r ) ) );

            var newPrincipal = new System.Security.Claims.ClaimsPrincipal( identity );
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
