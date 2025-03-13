namespace URLShortener.Application.Dtos.Responses
{
    public class ShortenedUrlStatsDto
    {
        public int TotalClicks { get; set; } // Total de clics
        public List<CountryCountDto> TopCountries { get; set; } // Países más frecuentes
        public Dictionary<string, int> Devices { get; set; } // Dispositivos más usados
    }
}
