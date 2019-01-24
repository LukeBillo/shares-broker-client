using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharesBrokerClient.Data.Models;
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
        public async Task<IActionResult> Get(
            [FromQuery] string companySymbol,
            [FromQuery] string companyName,
            [FromQuery] int? availableSharedLessThan,
            [FromQuery] int? availableSharesMoreThan,
            [FromQuery] double? priceLessThan,
            [FromQuery] double? priceMoreThan,
            [FromQuery] DateTime? priceLastUpdatedBefore,
            [FromQuery] DateTime? priceLastUpdatedAfter,
            [FromQuery] string currency,
            [FromQuery] int? limit)
        {
            var sharesQuery = new SharesQuery
            {
                companySymbol = companySymbol,
                companyName = companyName,
                availableSharedLessThan = availableSharedLessThan,
                availableSharesMoreThan = availableSharesMoreThan,
                priceLessThan = priceLessThan,
                priceMoreThan = priceMoreThan,
                priceLastUpdatedBefore = priceLastUpdatedBefore,
                priceLastUpdatedAfter = priceLastUpdatedAfter,
                currency = currency,
                limit = limit
            };

            var shares = await _sharesService.GetShares(sharesQuery);
            return Ok(shares);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BuyShareRequest buyShareRequest)
        {
            await _sharesService.BuyShare(buyShareRequest);
            return Ok();
        }
    }
}
