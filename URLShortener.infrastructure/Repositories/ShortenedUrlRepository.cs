using Microsoft.EntityFrameworkCore;
using URLShortener.Domian.Interfaces;
using URLShortener.Domian.Models;
using URLShortener.infrastructure.Contexts;

namespace URLShortener.infrastructure.Repositories
{
    public class ShortenedUrlRepository : IShortenedUrlRepository
    {
        private readonly ApplicationDbContext _context;

        public ShortenedUrlRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<ShortenedUrl> Create(ShortenedUrl shortenedUrl)
        {
            _context.ShortenedUrls.Add(shortenedUrl);
            await _context.SaveChangesAsync();
            return shortenedUrl;
        }

        public async Task<ShortenedUrl> GetOne(string ShortenedUrlCode)
        {
            var shortenedUrl = await _context.ShortenedUrls.SingleOrDefaultAsync(x => x.Code == ShortenedUrlCode);

            return shortenedUrl;
        }
    }
}
