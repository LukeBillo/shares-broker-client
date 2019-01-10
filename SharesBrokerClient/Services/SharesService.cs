using System;
using System.Collections.Generic;
using System.Net;
using Flurl.Http;
using SharesBrokerClient.Config;
using SharesBrokerClient.Data;
using SharesBrokerClient.Data.Models;

namespace SharesBrokerClient.Services
{
    public class SharesService
    {
        private static readonly Lazy<SharesService> Singleton = new Lazy<SharesService>(() => new SharesService());
        public static SharesService Instance => Singleton.Value;

        private static readonly string SharesEndpoint = $"{Configuration.SharesWebServiceUrl}/shares";
        private static readonly User User = ConnectionService.Instance.ConnectedUser;

        private SharesService() {}

        public List<CompanyShare> GetShares()
        {
            try
            {
                var response = SharesEndpoint
                    .WithBasicAuth(User.Username, User.Password)
                    .GetJsonAsync<List<CompanyShare>>();

                return response.Result;
            }
            catch (FlurlHttpException flurlHttpException)
            {
                var httpStatus = flurlHttpException.Call.HttpStatus;

                switch (httpStatus)
                {
                    case HttpStatusCode.Forbidden:
                        ConnectionService.Instance.UpdateConnectionState(ConnectionState.Forbidden);
                        break;
                    case HttpStatusCode.Unauthorized:
                        ConnectionService.Instance.UpdateConnectionState(ConnectionState.Unauthorized);
                        break;
                    default:
                        ConnectionService.Instance.UpdateConnectionState(ConnectionState.Error);
                        break;
                }

                // Empty list if errored
                return new List<CompanyShare>();
            }
        }
    }
}
