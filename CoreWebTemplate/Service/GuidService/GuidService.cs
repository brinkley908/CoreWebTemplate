using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Service
{
    public class GuidService : IGuidService
    {
        private readonly Guid serviceGuid;

        public GuidService()
        {
            serviceGuid = Guid.NewGuid();
        }

        public string GetGuid()
        {
            return serviceGuid.ToString();
        }
    }
}
