using Microsoft.AspNetCore.Mvc;
using URLShortener.Application.Dtos.Responses;
using URLShortener.Application.Interfaces;

namespace URL_Shortener.Controllers
{
    public class UrlStatsController : ControllerBase
    {
        private readonly IShortenedUrlService _shortenedUrlService;

        public UrlStatsController(IShortenedUrlService shortenedUrlService)
        {
            this._shortenedUrlService = shortenedUrlService;
        }
        [HttpGet("{code}/stats")]
        public async Task<ActionResult<ShortenedUrlStatsDto>> GetStats(string code)
        {
            try
            {
                var stats = await _shortenedUrlService.GetStats(code);
                return stats != null ? Ok(stats) : NotFound();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
