using System;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using shares_broker_client.Config;
using shares_broker_client.Data;

namespace shares_broker_client.Services
{
    public class ConnectionService
    {
        private static readonly Lazy<ConnectionService> Singleton = new Lazy<ConnectionService>(() => new ConnectionService());
        public static ConnectionService Instance => Singleton.Value;

        private static readonly string HealthEndpoint = $"{Configuration.SharesWebServiceUrl}/health";

        private ConnectionService() {}

        public async Task<ConnectionStatus> Connect(string username, string password)
        {
            try
            {
                var response = await HealthEndpoint
                    .WithBasicAuth(username, password)
                    .GetAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                    return ConnectionStatus.Connected;
            }
            catch (FlurlHttpException flurlHttpException)
            {
                var httpStatusCode = flurlHttpException.Call.HttpStatus.GetValueOrDefault();

                switch (httpStatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        return ConnectionStatus.Forbidden;
                    case HttpStatusCode.Unauthorized:
                        return ConnectionStatus.Unauthorized;
                    default:
                        return ConnectionStatus.Unknown;
                }
            }

            // should never be reached.
            return ConnectionStatus.Unknown;
        }
    }
}
