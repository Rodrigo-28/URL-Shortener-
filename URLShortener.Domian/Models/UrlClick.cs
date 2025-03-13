namespace URLShortener.Domian.Models
{
    public class UrlClick
    {
        public Guid Id { get; set; }
        public Guid ShortenedUrlId { get; set; }
        public DateTime ClickedAt { get; set; } = DateTime.Now;
        public string IpAddress { get; set; } = "UnKnown";
        public string Device { get; set; } = "UnKnown";
        public string Country { get; set; } = "Unknown";

        public ShortenedUrl ShortenedUrl { get; set; }
    }
}
