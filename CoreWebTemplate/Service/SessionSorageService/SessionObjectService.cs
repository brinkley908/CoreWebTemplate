using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebTemplate.Models.Account;

namespace CoreWebTemplate.Service.SessionSorageService
{
    public class SessionObjectService : ISessionObjectService
    {

        public List<SessionItem> Values { get; set; }

        public List<SessionRegistration> Registry { get; set; }

        public SessionObjectService()
        {
            Values = new List<SessionItem>();
            Registry = new List<SessionRegistration>();
        }

        public string Register( UserModel user, string sessionId )
        {
            if ( user == null || string.IsNullOrEmpty( sessionId ) ) return string.Empty;

            var registration = RegistryItem( sessionId );

            if ( registration != null ) return registration.Id;

            registration = new SessionRegistration( sessionId )
            {
                ClientId = user.Client?.ClientId ?? 0,
                UserId = user.UserId,
            };

            lock ( Registry ) Registry.Add( registration );

            return registration.Id;
        }

        public bool Registered( string sessionId, bool withLock = true )
        {
            if ( withLock )

                lock ( Registry )
                    return Registry.Any( x => x.SessionId.Equals( sessionId, StringComparison.OrdinalIgnoreCase ) );

            else

                return Registry.Any( x => x.SessionId.Equals( sessionId, StringComparison.OrdinalIgnoreCase ) );
        }

        public SessionRegistration RegistryItem( string sessionId, bool withLock = true )
        {
            if ( withLock )

                lock ( Registry )
                    return Registry.FirstOrDefault( x => x.SessionId.Equals( sessionId, StringComparison.OrdinalIgnoreCase ) );

            else

                return Registry.FirstOrDefault( x => x.SessionId.Equals( sessionId, StringComparison.OrdinalIgnoreCase ) );
        }

        public void RegistryRemove( string sessionId, bool withLock = true )
        {
            if ( withLock )

                lock ( Registry )
                {
                    var item = RegistryItem( sessionId );

                    if ( item != null )
                        Registry.Remove( item );
                }

            else
            {
                var item = RegistryItem( sessionId );

                if ( item != null )
                    Registry.Remove( item );
            }
        }

        public List<SessionRegistration> RegistryExpired( int? top = null )
        {
            lock ( Registry )

                if ( top == null )

                    return Registry
                        .Where( x => x.Expired || x.Garbage )
                        .ToList();
                else

                    return Registry
                        .Where( x => x.Expired || x.Garbage )
                        .Take( top.Value )
                        .ToList();
        }

        public bool AddObject( SessionItem item )
        {

            if ( string.IsNullOrEmpty( item.SessionId ) || string.IsNullOrEmpty( item.Id ) ) return false;

            if ( !Registered( item.SessionId ) )  return false;

            lock ( Values )
            {
                if ( Values.Any( x => x.SessionId.Equals( item.SessionId, StringComparison.OrdinalIgnoreCase ) && x.Id.Equals( item.Id, StringComparison.OrdinalIgnoreCase ) ) )
                    return false;

                Values.Add( item );
            }

            return true;
        }

        public SessionItem GetObject( string id, string sessionId )
        {
            var objects = GetObjects( sessionId );
            return objects.FirstOrDefault( x => x.Id.Equals( id, StringComparison.OrdinalIgnoreCase ) );
        }

        public List<SessionItem> GetObjects( string sessionId )
        {
            lock ( Values )

                return Values
                    .Where( x => x.SessionId.Equals( sessionId, StringComparison.OrdinalIgnoreCase ) )
                    .ToList();
        }

        public List<SessionItem> GetObjects<T>( string sessionId )
        {
            lock ( Values )

                return Values
                    .Where( x => x.SessionId.Equals( sessionId, StringComparison.OrdinalIgnoreCase ) && x is T )
                    .ToList();
        }

        public bool RemoveObject( SessionItem item )
        {
            if ( string.IsNullOrEmpty( item.SessionId ) ) return false;

            lock ( Values )
            {
                if ( Values.Contains( item ) )
                    return false;

                return Values.Remove( item );
            }
        }

        public bool RemoveObject( string id, string sessionId )
        {

            if ( string.IsNullOrEmpty( id ) || string.IsNullOrEmpty( sessionId ) ) return false;

            lock ( Values )
            {
                var item = GetObject( id, sessionId );

                if ( item != null )
                    return Values.Remove( item );
            }

            return false;
        }

        public bool RemoveObjects( string sessionId )
        {
            lock ( Values )
            {
                var items = Values
                      .Where( x => x.SessionId.Equals( sessionId, StringComparison.OrdinalIgnoreCase ) )
                      .ToList();

                foreach ( var item in items )
                    Values.Remove( item );
            }

            return true;
        }

        public bool RemoveObjects<T>( string sessionId )
        {
            lock ( Values )
            {
                var items = Values
                    .Where( x => x.SessionId.Equals( sessionId, StringComparison.OrdinalIgnoreCase ) && x is T )
                    .ToList();

                foreach ( var item in items )
                    Values.Remove( item );
            }

            return true;
        }

        public int CleanObjects( int? top = null )
        {

            var expired = RegistryExpired( top );

            if ( !expired.Any() ) return 0;

            var count = 0;

            foreach ( var exp in expired )
                count += CleanSessionObjects( exp.SessionId );


            return count;
        }

        public int CleanSessionObjects( string sessionId )
        {
            var objects = GetObjects( sessionId );
            var count = objects.Count;

            RegistryRemove( sessionId );

            if ( count > 0 )
                lock ( Values )

                    foreach ( var obj in objects )
                        Values.Remove( obj );

            return count;
        }

    }
}
