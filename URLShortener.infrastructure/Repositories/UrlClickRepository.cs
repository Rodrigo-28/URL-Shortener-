using Microsoft.EntityFrameworkCore;
using URLShortener.Domian.Interfaces;
using URLShortener.Domian.Models;
using URLShortener.infrastructure.Contexts;

namespace URLShortener.infrastructure.Repositories
{
    public class UrlClickRepository : IUrlClickRepository
    {
        private readonly ApplicationDbContext _context;

        public UrlClickRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task AddClick(UrlClick urlClick)
        {
            _context.urlClicks.Add(urlClick);
            await _context.SaveChangesAsync();
        }

        public async Task<ShortenedUrl?> GetWithClicks(string code)
        {
            return await _context.ShortenedUrls
                .Include(u => u.Clicks)
                .FirstOrDefaultAsync(u => u.Code == code);
        }
    }
}
