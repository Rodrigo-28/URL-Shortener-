using Microsoft.EntityFrameworkCore;
using URLShortener.Domian.Common;
using URLShortener.Domian.Models;

namespace URLShortener.infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrl>(builder =>
            {
                builder
                    .Property(shortenedUrl => shortenedUrl.Code)
                    .HasMaxLength(ShortLinkSettings.Length);

                builder
                    .HasIndex(shortenedUrl => shortenedUrl.Code)
                    .IsUnique();
            });
        }
    }
}
