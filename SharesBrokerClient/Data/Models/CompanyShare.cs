using System;

namespace SharesBrokerClient.Data.Models
{
    public class CompanyShare
    {
        public string CompanyName { get; set; }
        public string CompanySymbol { get; set; }
        public long NumberOfShares { get; set; }
        public SharePrice SharePrice { get; set; }
    }
}
