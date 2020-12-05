using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Service.SessionSorageService
{
    public abstract class SessionItem
    {
        public string Id { get; set; }

        private string _sessionId;
        public string SessionId { get { return _sessionId; } }

 
        public SessionItem( string sessionId )
        {
            Id = Guid.NewGuid().ToString();
            _sessionId = sessionId;
        }

    }
}
