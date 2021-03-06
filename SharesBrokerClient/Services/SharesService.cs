﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using SharesBrokerClient.Data;
using SharesBrokerClient.Data.Models;
using System;
using Flurl.Util;

namespace SharesBrokerClient.Services
{
    public class SharesService
    {
        public readonly string SharesEndpoint;

        private readonly ConnectionService _connectionService;
        private User _user;

        public SharesService(IConfiguration configuration, ConnectionService connectionService)
        {
            _connectionService = connectionService;
            _connectionService.User.Subscribe(user => { _user = user; });

            SharesEndpoint = $"{configuration["Urls:SharesWebService"]}/shares";
        }

        public async Task<List<CompanyShare>> GetShares(SharesQuery sharesQuery)
        {
            try
            {
                return await SharesEndpoint
                    .WithHeader("Authorization", $"Basic {_user.Credentials}")
                    .SetQueryParams(sharesQuery)
                    .GetJsonAsync<List<CompanyShare>>();
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
                return new List<CompanyShare>();
            }
        }

        public async Task BuyShare(BuyShareRequest request)
        {
            try
            {
                await SharesEndpoint
                    .WithHeader("Authorization", $"Basic {_user.Credentials}")
                    .PostJsonAsync(request);
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
            }
        }
    }
}
