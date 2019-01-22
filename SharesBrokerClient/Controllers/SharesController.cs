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

        /*@QueryParam("companySymbol") String companySymbol,
            @QueryParam("companyName") String companyName,
        @QueryParam("availableSharesLessThan") Integer availableSharedLessThan,
            @QueryParam("availableSharesMoreThan") Integer availableSharesMoreThan,
        @QueryParam("priceLessThan") Double priceLessThan,
            @QueryParam("priceMoreThan") Double priceMoreThan,
        @QueryParam("priceLastUpdatedBefore") Date priceLastUpdatedBefore,
            @QueryParam("priceLastUpdatedAfter") Date priceLastUpdatedAfter,
        @QueryParam("currency") String currency,
            @QueryParam("limit") Integer limit*/

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
    }
}
