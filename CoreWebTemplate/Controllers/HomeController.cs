using System;
using System.Linq;
using System.Collections.Generic;
using CoreWebTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CoreWebTemplate.Service.SessionSorageService;
using CoreWebTemplate.Models.Account;
using Microsoft.AspNetCore.Authorization;

namespace CoreWebTemplate.Controllers
{
   [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ISessionStorageService _sessionStorage;

        public HomeController( ILogger<HomeController> logger, ISessionStorageService sessionStorageService )
        {
            _logger = logger;
            _sessionStorage = sessionStorageService;
        }

        public IActionResult Index()
        {
            _sessionStorage.LoadSession( HttpContext );

            var sessionId = _sessionStorage.Id;

            var id = _sessionStorage.SessionObjectService.Register
            (
                new UserModel
                {
                    UserId = 1,
                    Client = new ClientModel { ClientId = 1, ClientName = "LBR" }
                }, 
                sessionId 
            );

            _sessionStorage.SessionObjectService.AddObject( new SessionItemTask(sessionId)
            {
                 Item = Task.Delay(120)
            } );

            return View( new ModelBase { BanerText = _sessionStorage.Id } );
        }

        public IActionResult Privacy()
        => View( new ModelBase { BanerText = "Privacy Policy" } );

        [ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
        public IActionResult Error()
        => View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
    }
}
