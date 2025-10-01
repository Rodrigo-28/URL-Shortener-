using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using URLShortener.Application.Dtos.Request;
using URLShortener.Application.Dtos.Responses;
using URLShortener.Application.Interfaces;

namespace URL_Shortener.Controllers
{
    [Route("")]
    [ApiController]
    public class URLShortenerController : ControllerBase
    {
        private readonly IShortenedUrlService _shortenedUrlService;

        public URLShortenerController(IShortenedUrlService shortenedUrlService)
        {
            this._shortenedUrlService = shortenedUrlService;
        }
        [HttpPost]
        [EnableRateLimiting("CreatePolicy")]
        public async Task<ActionResult<ShortenedUrlDto>> Create([FromBody] UrlDto urlDto)
        {
            if (urlDto == null)
            {
                return BadRequest("La solicitud no puede estar vacía.");
            }
            try
            {
                var shortenedUrlDto = await _shortenedUrlService.Create(urlDto);
                return Ok(shortenedUrlDto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{code}")]
        public async Task<ActionResult<ShortenedUrlDto>> Get(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("El código no puede estar vacío.");
            };
            try
            {
                var shortenedUrl = await _shortenedUrlService.GetOne(code);
                if (shortenedUrl == null)
                {
                    return NotFound("La URL acortada no existe.");
                };
                return Redirect(shortenedUrl.LongUrl);
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }
        }
    }
}
