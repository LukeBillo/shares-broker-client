using System;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SharesBrokerClient.Data;
using SharesBrokerClient.Data.Models;
using SharesBrokerClient.Services;

namespace SharesBrokerClient.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private const int FiveMinutes = 5;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ConnectionService connectionService)
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

            var currentUser = connectionService.CurrentUser;

            if (currentUser != null)
            {
                if (currentUser.Expiry <= DateTime.Now && 
                    currentUser.Credentials == encodedUsernamePassword)
                {
                    await _next.Invoke(context);
                }
            }

            var connectionState = await connectionService.Connect(encodedUsernamePassword);

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

            currentUser = connectionService.CurrentUser;
            currentUser.Expiry = DateTime.Now.AddMinutes(FiveMinutes);

            await _next.Invoke(context);
        }
    }
}