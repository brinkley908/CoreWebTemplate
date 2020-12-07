using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Html;
using CoreWebTemplate.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using CoreWebTemplate.Service.SessionSorageService;
using CoreWebTemplate.Service;

namespace CoreWebTemplate.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private readonly IHtmlHelper _helper;

        private ISessionStorageService _session;

        private readonly ISignInService _signInService;

        public AccountController( IHtmlHelper helper, ISignInService signInService, ISessionStorageService sessionStorageService )
        {
            _helper = helper;
            _session = sessionStorageService;
            _signInService = signInService;
        }

        public async Task<IActionResult> Login()
        {
            if ( User.Identity.IsAuthenticated )
            {
                return await Logout();
            }

            var model = LoginModel.GetModel( this, _helper );
            return View( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginModel model)
        {
            if ( User.Identity.IsAuthenticated )
            {
                return await Logout();
            }

            model = LoginModel.GetModel(model, this, _helper );

            if ( !ModelState.IsValid )
                return View( model );

            if ( await CheckLogin( model ) )
                return RedirectToAction( "index", "home" );

            else
                return View( model );
        }

        public async Task<bool> CheckLogin( LoginModel model )
        {
            //var signInUser = new SignInUser
            //{
            //    UserManager = _seedData.UserManager,
            //    UserRoleManager = _seedData.UserRoleManager,
            //    SignInManager = _seedData.SignInManager,
            //    IdentityOptions = _seedData.IdentityOptions
            //};

            //await _seedData.AddTestUsers();

            return await _signInService.SignInAsync( model ); 
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {

            _session.LoadSession( HttpContext );

            _session.SessionObjectService.CleanSessionObjects(_session.Id);

            _session.Clear();

            await _signInService.SignOutAsync();

            return RedirectToAction( "Login" );
        }

    }
}
