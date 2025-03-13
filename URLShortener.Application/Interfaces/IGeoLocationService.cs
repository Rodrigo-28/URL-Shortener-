namespace URLShortener.Application.Interfaces
{
    public interface IGeoLocationService
    {
        Task<string> GetCountryFromIp(string ipAddress);
    }
}
