using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingServer
{
    internal class CantFindAddressException : Exception
    {
        public CantFindAddressException(String errorMessage) : base(errorMessage) { }
    }
}
