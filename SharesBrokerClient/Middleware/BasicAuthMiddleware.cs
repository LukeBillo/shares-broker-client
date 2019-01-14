using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SharesBrokerClient.Data;
using SharesBrokerClient.Services;

namespace SharesBrokerClient.Middleware
{
    public class BasicAuthMiddleware : IMiddleware
    {
        private readonly ConnectionService _connectionService;

        public BasicAuthMiddleware(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string authHeader = context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic"))
            {
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                return;
            }

            var encodedUsernamePassword = authHeader
                .Substring("Basic".Length)
                .Trim();

            var decodedHeader = Convert.FromBase64String(encodedUsernamePassword).ToString().Split(":");
            var username = decodedHeader[0];
            var password = decodedHeader[1];

            if (_connectionService.ConnectedUser != null)
            {
                if (_connectionService.ConnectedUser.Username == username &&
                    _connectionService.ConnectedUser.Password == password)
                {
                    await next.Invoke(context);
                }
            }

            var connectionState = await _connectionService.Connect(username, password);

            if (connectionState != ConnectionState.Connected)
            {
                switch (connectionState)
                {
                    case ConnectionState.Unauthorized:
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        return;
                    case ConnectionState.Forbidden:
                        context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                        return;
                    case ConnectionState.Unreachable:
                        context.Response.StatusCode = (int) HttpStatusCode.ServiceUnavailable;
                        return;
                    default:
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        return;
                }
            }

            await next.Invoke(context);
        }
    }
}