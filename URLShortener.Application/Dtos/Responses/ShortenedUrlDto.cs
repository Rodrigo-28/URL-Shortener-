namespace URLShortener.Application.Dtos.Responses
{
    public class ShortenedUrlDto
    {
        public string ShortUrl { get; set; }
        public string LongUrl { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
