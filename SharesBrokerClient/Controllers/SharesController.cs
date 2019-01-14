using Microsoft.AspNetCore.Mvc;
using SharesBrokerClient.Services;

namespace SharesBrokerClient.Controllers
{
    [Route("api/[controller]")]
    public class SharesController : Controller
    {
        private readonly SharesService _sharesService;

        public SharesController(SharesService sharesService)
        {
            _sharesService = sharesService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_sharesService.GetShares());
        }
    }
}
