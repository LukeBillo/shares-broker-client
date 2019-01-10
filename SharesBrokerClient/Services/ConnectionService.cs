using System;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using SharesBrokerClient.Config;
using SharesBrokerClient.Data;
using SharesBrokerClient.Data.Models;

namespace SharesBrokerClient.Services
{
    public class ConnectionService
    {
        private static readonly Lazy<ConnectionService> Singleton = new Lazy<ConnectionService>(() => new ConnectionService());
        public static ConnectionService Instance => Singleton.Value;

        private static readonly string HealthEndpoint = $"{Configuration.SharesWebServiceUrl}/health";

        public User ConnectedUser { get; private set; }
        public ConnectionState ConnectionState { get; private set; }

        private ConnectionService()
        {
            ConnectionState = ConnectionState.NotConnected;
        }

        public async Task<ConnectionState> Connect(string username, string password)
        {
            try
            {
                var response = await HealthEndpoint
                    .WithBasicAuth(username, password)
                    .AllowHttpStatus(HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized)
                    .GetAsync();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        ConnectedUser = new User { Username = username, Password = password };
                        ConnectionState = ConnectionState.Connected;
                        break;
                    case HttpStatusCode.Forbidden:
                        ConnectionState = ConnectionState.Forbidden;
                        break;
                    case HttpStatusCode.Unauthorized:
                        ConnectionState = ConnectionState.Unauthorized;
                        break;
                    default:
                        // should not be possible
                        ConnectionState = ConnectionState.Error;
                        break;
                }
            }
            catch (FlurlHttpException)
            {
                ConnectionState = ConnectionState.Error;
            }

            return ConnectionState;
        }

        public void UpdateConnectionState(ConnectionState state)
        {
            ConnectionState = state;
        }
    }
}
