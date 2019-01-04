using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shares_broker_client.Data
{
    public enum ConnectionStatus
    {
        Connected,
        Unauthorized,
        Forbidden,
        Unknown
    }
}
