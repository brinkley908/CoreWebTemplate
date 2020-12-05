using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Service.SessionSorageService
{
    public class SessionRegistration : SessionItem
    {

        public int ClientId { get; set; }

        public int UserId { get; set; }

        public DateTime Started { get; set; }

        public int TimeOut { get; set; }

        public bool Garbage
        {
            get
            {
                return DateTime.Now.Subtract( Started ).Hours >= 24;
            }
        }

        public bool Expired
        {
            get
            {
                return DateTime.Now.Subtract( Started ).Minutes >= TimeOut;
            }
        }

        public SessionRegistration( string sessionId ):base(sessionId)
        {
            TimeOut = 30;
            Started = DateTime.Now;
        }

    }
}
