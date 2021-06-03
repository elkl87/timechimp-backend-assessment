using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.DTO;
using TimeChimp.Backend.Assessment.Services;

namespace TimeChimp.Backend.Assessment.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    public class RssFeedController : Controller
    {
        private readonly IFeedService FeedService;
        private readonly ILogger<RssFeedController> Logger;

        public RssFeedController(IFeedService feedService, ILogger<RssFeedController> logger)
        {
            FeedService = feedService;
            Logger = logger;
        }

        /// <summary>
        /// Endpoint to query rss feed
        /// </summary>
        /// <param name="feedName">Please select one of the values: film, tech, sport</param>
        /// <param name="cancellationToken"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortByPublishedDate"></param>
        /// <returns>One rss feed</returns>
        /// <response code="200">Returns rss feed</response>
        /// <response code="400">If parameters are invalid</response>
        /// <response code="404">If the rss feed is not found</response>
        /// <response code="500">If failed with exception</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetRssFeedAsync(string feedName, CancellationToken cancellationToken, int page = 1, int pageSize = 3, bool sortByPublishedDate = false)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Logger.LogDebug($"{nameof(GetRssFeedAsync)} has been invoked");

            if (string.IsNullOrEmpty(feedName) || page < 1 || pageSize < 1)
            {
                return BadRequest();
            }

            try
            {
                var result = await FeedService.GetRssFeedByNameAsync(feedName, cancellationToken);

                if (sortByPublishedDate)
                {
                    var sortedResult = result.items.OrderByDescending(f => f.Published);
                    result.items = sortedResult;
                }

                var count = result.items.Count();
                var items = result.items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
                IndexViewModel viewModel = new IndexViewModel
                {
                    PageViewModel = pageViewModel,
                    Items = items
                };

                return Ok(viewModel);
            }
            catch (ArgumentException ex)
            {
                Logger.LogError($"{nameof(GetRssFeedAsync)} has been failed with exception {ex.Message}");
                return NotFound($"Rss feed with name {feedName} was not found ");
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(GetRssFeedAsync)} has been failed with exception {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
