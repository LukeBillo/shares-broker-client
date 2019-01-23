using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using SharesBrokerClient.Data;
using SharesBrokerClient.Data.Models;

namespace SharesBrokerClient.Services
{
    public class UserSharesService
    {
        public readonly string UserSharesEndpoint;

        private readonly ConnectionService _connectionService;
        private User _user;

        public UserSharesService(IConfiguration configuration, ConnectionService connectionService)
        {
            _connectionService = connectionService;
            _connectionService.User.Subscribe(user => { _user = user; });

            UserSharesEndpoint = $"{configuration["Urls:SharesWebService"]}/user/shares";
        }

        public async Task<List<UserShare>> GetUserShares(string username, string currency)
        {
            try
            {
                var response = await UserSharesEndpoint
                    .WithHeader("Authorization", $"Basic {_user.Credentials}")
                    .SetQueryParam("username", username)
                    .SetQueryParam("currency", currency)
                    .GetJsonAsync<List<UserShare>>();

                return response;
            }
            catch (FlurlHttpException flurlHttpException)
            {
                var httpStatus = flurlHttpException.Call.HttpStatus;

                switch (httpStatus)
                {
                    case HttpStatusCode.Forbidden:
                        await _connectionService.UpdateConnectionState(ConnectionState.Forbidden);
                        break;
                    case HttpStatusCode.Unauthorized:
                        await _connectionService.UpdateConnectionState(ConnectionState.Unauthorized);
                        break;
                    default:
                        await _connectionService.UpdateConnectionState(ConnectionState.Error);
                        break;
                }

                // Empty list if errored
                return new List<UserShare>();
            }
        }
    }
}
