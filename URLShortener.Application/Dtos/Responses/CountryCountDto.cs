namespace URLShortener.Application.Dtos.Responses
{
    public class CountryCountDto
    {
        public string Country { get; set; } // Nombre del país
        public int Count { get; set; } // Número de clics desde ese país
    }
}
