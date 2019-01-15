using System.Net;
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

        public User ConnectedUser { get; private set; }
        public ConnectionState ConnectionState { get; private set; }

        public ConnectionService(IConfiguration configuration)
        {
            HealthEndpoint = $"{configuration["Urls:SharesWebService"]}/health";
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
