using System;

namespace SharesBrokerClient.Data.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ConnectionState ConnectionState { get; set; }
        public DateTime Expiry { get; set; }
    }
}
