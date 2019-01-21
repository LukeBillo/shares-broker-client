using System;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using SharesBrokerClient.Data;
using SharesBrokerClient.Data.Models;

namespace SharesBrokerClient.Services
{
    public class ConnectionService
    {
        public readonly string HealthEndpoint;

        public IObservable<User> User => _user.AsObservable();
        public User CurrentUser => _user.Value;

        private readonly BehaviorSubject<User> _user = new BehaviorSubject<User>(null);

        public ConnectionService(IConfiguration configuration)
        {
            HealthEndpoint = $"{configuration["Urls:SharesWebService"]}/health";
        }

        public async Task<ConnectionState> Connect(string credentials)
        {
            var connectedUser = new User { Credentials = credentials};

            try
            {
                var response = await HealthEndpoint
                    .WithHeader("Authorization", $"Basic {credentials}")
                    .AllowHttpStatus(HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized)
                    .GetAsync();


                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        connectedUser.ConnectionState = ConnectionState.Connected;
                        break;
                    case HttpStatusCode.Forbidden:
                        connectedUser.ConnectionState = ConnectionState.Forbidden;
                        break;
                    case HttpStatusCode.Unauthorized:
                        connectedUser.ConnectionState = ConnectionState.Unauthorized;
                        break;
                    default:
                        // should not be possible
                        connectedUser.ConnectionState = ConnectionState.Error;
                        break;
                }

            }
            catch (FlurlHttpException)
            {
                connectedUser.ConnectionState = ConnectionState.Error;
            }

            _user.OnNext(connectedUser);

            return connectedUser.ConnectionState;
        }

        public async Task UpdateConnectionState(ConnectionState state)
        {
            var connectedUser = await _user.FirstAsync();
            connectedUser.ConnectionState = state;

            _user.OnNext(connectedUser);
        }
    }
}
