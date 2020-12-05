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
    public class SessionStorageService : ISessionStorageService
    {

        private const string USER_NAME = "user";
        private const string SESSION_GUID = "SessionGuid";


        public  ISession _session;

        public readonly ISessionObjectService _sessionObjectService;

        public SessionStorageService( ISessionObjectService sessionObjectService )
        {

            _sessionObjectService = sessionObjectService;

            //SetSessionGuid( _session.Id );
            
        }

        public void LoadSession(HttpContext httpContext)
        {
            _session = httpContext.Session;
            SetSessionGuid( _session.Id );
        }

        public ISessionObjectService SessionObjectService { get { return _sessionObjectService; } }

        public string Id { get { return _session.Id; } }

        public bool IsAvailable { get { return _session.IsAvailable; } }

        public string SessionGuid { get { return _session.GetString( SESSION_GUID ); } set { SetSessionGuid( value ); } }

        public void SetSessionGuid( string sessionGuid ) => _session.SetString( SESSION_GUID, sessionGuid );

        public void SetUser( UserModel user ) => ToJson( USER_NAME, user );

        public UserModel User() => FromJson<UserModel>( USER_NAME );

        public void SetString( string key, string value ) => _session.SetString(key, value);

        public string GetString( string key ) => _session.GetString( key );

        public void SetInt32( string key, int value ) => _session.SetInt32( key, value );

        public int? GetInt32( string key ) => _session.GetInt32( key );

        public byte[] Get( string key ) => _session.Get( key );

        public bool TryGetValue( string key, out byte[] value ) => _session.TryGetValue( key, out value );

        public void Set( string key, byte[] value ) => _session.Set( key, value );

        public void Remove( string key ) => _session.Remove( key );

        public IEnumerable<string> Keys { get { return _session.Keys; } }

        public void Clear() => _session.Clear();

        public async Task CommitAsync( CancellationToken cancellationToken = default ) => await _session.CommitAsync( cancellationToken );

        public async Task LoadAsync( CancellationToken cancellationToken = default ) => await _session.LoadAsync( cancellationToken );

        public string ToJson( string key, object value )
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject( value );
            _session.SetString( key, str );
            return str;
        }

        public T FromJson<T>( string key )
        {
            var str = _session.GetString( key );
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>( str );
        }
    }
}
