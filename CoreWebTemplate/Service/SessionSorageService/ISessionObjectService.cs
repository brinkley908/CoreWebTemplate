using CoreWebTemplate.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Service.SessionSorageService
{
    public interface ISessionObjectService
    {
        List<SessionItem> Values { get; set; }

        List<SessionRegistration> Registry { get; set; }

        bool Registered(string sessionId, bool withLock = true );

        string Register( UserModel user, string sessionId );

        SessionRegistration RegistryItem( string sessionId, bool withLock = true );

        void RegistryRemove( string sessionId, bool withLock = true );

        List<SessionRegistration> RegistryExpired( int? top = null );

        bool AddObject( SessionItem item );

        SessionItem GetObject( string id, string sessionId );

        public List<SessionItem> GetObjects( string sessionId );

        List<SessionItem> GetObjects<T>( string sessionId );

        bool RemoveObject( SessionItem item );

        bool RemoveObject( string id, string sessionId );

        bool RemoveObjects( string sessionId );

        bool RemoveObjects<T>( string sessionId );

        int CleanObjects( int? top = null );

        int CleanSessionObjects( string sessionId );


    }
}
