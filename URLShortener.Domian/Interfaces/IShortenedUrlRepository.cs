using URLShortener.Domian.Models;

namespace URLShortener.Domian.Interfaces
{
    public interface IShortenedUrlRepository
    {
        public Task<ShortenedUrl> Create(ShortenedUrl shortenedUrl);
        public Task<ShortenedUrl> GetOne(string ShortenedUrlCode);
    }
}
