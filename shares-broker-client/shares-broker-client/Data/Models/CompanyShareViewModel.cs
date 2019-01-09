using System;

namespace SharesBrokerClient.Data.Models
{
    public class CompanyShareViewModel
    {
        public string CompanyName { get; set; }
        public string CompanySymbol { get; set; }
        public int NumberOfShares { get; set; }
        public string Currency { get; set; }
        public double Value { get; set; }
        //public DateTime LastUpdated { get; set; }

        public CompanyShareViewModel(CompanyShare companyShare)
        {
            CompanyName = companyShare.CompanyName;
            CompanySymbol = companyShare.CompanySymbol;
            NumberOfShares = companyShare.NumberOfShares;
            Currency = companyShare.SharePrice.Currency;
            Value = companyShare.SharePrice.Value;
            //LastUpdated = companyShare.SharePrice.LastUpdated;
        }
    }
}
