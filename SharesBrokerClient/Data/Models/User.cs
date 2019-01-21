using System;

namespace SharesBrokerClient.Data.Models
{
    public class User
    {
        public string Credentials { get; set; }
        public ConnectionState ConnectionState { get; set; }
        public DateTime Expiry { get; set; }
    }
}
