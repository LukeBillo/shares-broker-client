using System.Collections.Generic;
using System.Net;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using SharesBrokerClient.Data;
using SharesBrokerClient.Data.Models;

namespace SharesBrokerClient.Services
{
    public class SharesService
    {
        public readonly string SharesEndpoint;

        private readonly ConnectionService _connectionService;
        private readonly User _user;

        public SharesService(IConfiguration configuration, ConnectionService connectionService)
        {
            _connectionService = connectionService;
            _user = _connectionService.ConnectedUser;

            SharesEndpoint = $"{configuration["Urls:SharesWebService"]}/shares";
        }

        public List<CompanyShare> GetShares()
        {
            try
            {
                var response = SharesEndpoint
                    .WithBasicAuth(_user.Username, _user.Password)
                    .GetJsonAsync<List<CompanyShare>>();

                return response.Result;
            }
            catch (FlurlHttpException flurlHttpException)
            {
                var httpStatus = flurlHttpException.Call.HttpStatus;

                switch (httpStatus)
                {
                    case HttpStatusCode.Forbidden:
                        _connectionService.UpdateConnectionState(ConnectionState.Forbidden);
                        break;
                    case HttpStatusCode.Unauthorized:
                        _connectionService.UpdateConnectionState(ConnectionState.Unauthorized);
                        break;
                    default:
                        _connectionService.UpdateConnectionState(ConnectionState.Error);
                        break;
                }

                // Empty list if errored
                return new List<CompanyShare>();
            }
        }
    }
}
