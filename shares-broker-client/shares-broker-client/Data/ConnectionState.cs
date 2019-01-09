using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharesBrokerClient.Data
{
    public enum ConnectionState
    {
        NotConnected,
        Connected,
        Unauthorized,
        Forbidden,
        Unreachable,
        Error
    }
}
