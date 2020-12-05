using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Service.SessionSorageService
{
    public class SessionItemTask : SessionItem
    {
        public SessionItemTask( string sessionId ) : base( sessionId ) { }

        public Task Item { get; set; }
    }
}
