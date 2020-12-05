using CoreWebTemplate.Models;
using CoreWebTemplate.Models.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreWebTemplate.Service.SessionSorageService
{
    public interface ISessionStorageService: ISession
    {


        string SessionGuid { get; set; }

        ISessionObjectService SessionObjectService { get; }

        void LoadSession( HttpContext httpContext );

        void SetSessionGuid( string sessionGuid);

        void SetUser( UserModel user);

        UserModel User();

        void SetString( string key, string value );

        string GetString( string key );

        void SetInt32( string key, int value );

        int? GetInt32( string key );

        byte[] Get( string key );

        string ToJson( string key, object value);

        T FromJson<T>( string key );

    }
}
