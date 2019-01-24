using System;
using Microsoft.AspNetCore.Mvc;
using SharesBrokerClient.Data.Models;
using SharesBrokerClient.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SharesBrokerClient.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private const int HalfAnHour = 30;
        private User _user;

        public AuthController(ConnectionService connectionService)
        {
            _user = connectionService.CurrentUser;
            connectionService.User.Subscribe(user => _user = user);
        }

        [HttpPost]
        public void Post()
        {
            _user.Expiry = DateTime.Now.AddMinutes(HalfAnHour);
        }
    }
}
