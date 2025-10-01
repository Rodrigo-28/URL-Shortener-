namespace URLShortener.Application.Dtos.Request
{
    public class UrlDto
    {

        public string LongUrl { get; set; }
        //si es null o <= 0, el link no expira
        public int? TtlMinutes { get; set; }



    }
}
