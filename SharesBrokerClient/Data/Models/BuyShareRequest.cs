using Newtonsoft.Json;

namespace SharesBrokerClient.Data.Models
{
    public class BuyShareRequest
    {
        [JsonProperty("buyerUsername")]
        public string BuyerUsername { get; set; }

        [JsonProperty("companySymbol")]
        public string CompanySymbol { get; set; }

        [JsonProperty("numberOfSharesToBuy")]
        public int NumberOfSharesToBuy { get; set; }
    }
}