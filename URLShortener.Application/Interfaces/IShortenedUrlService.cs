using URLShortener.Application.Dtos.Request;
using URLShortener.Application.Dtos.Responses;

namespace URLShortener.Application.Interfaces
{
    public interface IShortenedUrlService
    {
        Task<ShortenedUrlDto> Create(UrlDto urlDto);
        Task<ShortenedUrlDto> GetOne(string Code);
    }
}
