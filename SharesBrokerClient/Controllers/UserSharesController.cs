using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharesBrokerClient.Data.Models;
using SharesBrokerClient.Services;

namespace SharesBrokerClient.Controllers
{
    [Route("api/user/shares")]
    public class UserSharesController : Controller
    {
        private readonly UserSharesService _userSharesService;

        public UserSharesController(UserSharesService userSharesService)
        {
            _userSharesService = userSharesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string username, [FromQuery] string currency)
        {
            var userShares = await _userSharesService.GetUserShares(username, currency);
            return Ok(userShares);
        }
    }
}
