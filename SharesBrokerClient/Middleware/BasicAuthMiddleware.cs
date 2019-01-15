using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SharesBrokerClient.Data;
using SharesBrokerClient.Services;

namespace SharesBrokerClient.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ConnectionService connectionService)
        {
            string authHeader = context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic"))
            {
                context.Response.Redirect("/login");
                return;
            }

            var encodedUsernamePassword = authHeader
                .Substring("Basic".Length)
                .Trim();

            var decodedHeader = Convert.FromBase64String(encodedUsernamePassword).ToString().Split(":");
            var username = decodedHeader[0];
            var password = decodedHeader[1];

            if (connectionService.ConnectedUser != null)
            {
                if (connectionService.ConnectedUser.Username == username &&
                    connectionService.ConnectedUser.Password == password)
                {
                    await _next.Invoke(context);
                }
            }

            var connectionState = await connectionService.Connect(username, password);

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

            await _next.Invoke(context);
        }
    }
}