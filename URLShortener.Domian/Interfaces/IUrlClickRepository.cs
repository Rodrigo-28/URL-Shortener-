using URLShortener.Domian.Models;

namespace URLShortener.Domian.Interfaces
{
    public interface IUrlClickRepository
    {
        public Task AddClick(UrlClick urlClick);

        public Task<ShortenedUrl> GetWithClicks(string code);
    }
}
