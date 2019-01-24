using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming
// Capitalisation has been manually changed here
// due to Jersey being case-sensitive on query parameters.

namespace SharesBrokerClient.Data.Models
{
    public class SharesQuery
    {
        public string companySymbol { get; set; } = null;
        public string companyName { get; set; } = null;
        public int? availableSharedLessThan { get; set; } = null;
        public int? availableSharesMoreThan { get; set; } = null;
        public double? priceLessThan { get; set; } = null;
        public double? priceMoreThan { get; set; } = null;
        public DateTime? priceLastUpdatedBefore { get; set; } = null;
        public DateTime? priceLastUpdatedAfter { get; set; } = null;
        public string currency { get; set; } = null;
        public int? limit { get; set; } = null;
    }
}
