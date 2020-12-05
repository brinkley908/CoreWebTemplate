using CoreWebTemplate.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using CoreWebTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CoreWebTemplate.Service.SessionSorageService;

namespace CoreWebTemplate.Controllers
{
    public class TestController : Controller
    {

        private ISessionStorageService _sessionStorageService;

        public TestController( ISessionStorageService sessionStorageService )
        {
            _sessionStorageService = sessionStorageService;
        }

        public IActionResult Index()
        {
            _sessionStorageService.LoadSession( HttpContext );

           var obs= _sessionStorageService.SessionObjectService.GetObjects<SessionItemTask>( _sessionStorageService.Id );

            foreach(SessionItemTask obj in obs )
            {
                //var t = await ((Task)obj.Task)
            }

            return View( new ModelBase { BanerText = _sessionStorageService?.Id ?? "Session Not Found"} );
        }
    }
}
