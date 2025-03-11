using System.ComponentModel.DataAnnotations.Schema;

namespace URLShortener.Domian.Models
{
    public class ShortenedUrl
    {
        [Column("shortUrl_id")]
        public Guid Id { get; set; }
        [Column("longUrl")]

        public string LongUrl { get; set; } = string.Empty;
        [Column("shortUrl")]


        public string ShortUrl { get; set; } = string.Empty;

        [Column("code")]

        public string Code { get; set; } = string.Empty;
        [Column("createdAt")]


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int AccessCount { get; set; } = 0;

        public List<UrlClick> Clicks { get; set; } = new List<UrlClick>();

    }
}