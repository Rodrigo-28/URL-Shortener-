using AutoMapper;
using Microsoft.AspNetCore.Http;
using URLShortener.Application.Dtos.Request;
using URLShortener.Application.Dtos.Responses;
using URLShortener.Application.Interfaces;
using URLShortener.Domian.Common;
using URLShortener.Domian.Interfaces;
using URLShortener.Domian.Models;

namespace URLShortener.Application.Services
{
    public class ShortenedUrlService : IShortenedUrlService
    {
        private readonly IShortenedUrlRepository _shortenedUrlRepository;
        private readonly IMapper _mapper;
        private readonly Random _random = new Random();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGeoLocationService _geoLocationService;
        private readonly IUrlClickRepository _urlClickRepository;

        public ShortenedUrlService(IShortenedUrlRepository shortenedUrlRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IGeoLocationService geoLocationService, IUrlClickRepository urlClickRepository)
        {
            this._shortenedUrlRepository = shortenedUrlRepository;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
            this._geoLocationService = geoLocationService;
            this._urlClickRepository = urlClickRepository;
        }
        private string GenerateUniqueCode()
        {
            var codeChars = new char[ShortLinkSettings.Length];
            int maxValue = ShortLinkSettings.Alphabet.Length;




            for (int i = 0; i < ShortLinkSettings.Length; i++)
            {
                var randomIndex = _random.Next(maxValue);
                codeChars[i] = ShortLinkSettings.Alphabet[randomIndex];
            }

            return new string(codeChars);
        }

        public async Task<ShortenedUrlDto> Create(UrlDto urlDto)
        {
            if (!Uri.TryCreate(urlDto.LongUrl, UriKind.Absolute, out var inputUri))
            {
                throw new ArgumentException("La URL especificada no es válida.");

            }
            string uniqueCode;
            do
            {
                uniqueCode = GenerateUniqueCode();
            } while (await _shortenedUrlRepository.GetOne(uniqueCode) != null);

            // contexto http
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("No se pudo acceder al contexto HTTP.");
            };
            //construir url corta
            var shortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{uniqueCode}";

            var shortenedUrl = new ShortenedUrl()
            {
                Id = Guid.NewGuid(),
                LongUrl = urlDto.LongUrl,
                Code = uniqueCode,
                ShortUrl = shortUrl,
                CreatedAt = DateTime.UtcNow
            };
            await _shortenedUrlRepository.Create(shortenedUrl);
            return _mapper.Map<ShortenedUrlDto>(shortenedUrl);

        }

        public async Task<ShortenedUrlDto> GetOne(string Code)
        {
            var shortenedUrl = await _shortenedUrlRepository.GetOne(Code);

            if (shortenedUrl == null)
            {
                throw new KeyNotFoundException("No se encontró la URL acortada.");
            }
            return _mapper.Map<ShortenedUrlDto>(shortenedUrl);
        }

        public async Task RegisterClick(string Code)
        {
            Console.WriteLine($"Intentando registrar click para código: {Code}");
            var shortenedUrl = await _shortenedUrlRepository.GetOne(Code);
            if (shortenedUrl == null)
            {
                Console.WriteLine("URL no encontrada");
                return;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            var ipAddress = httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "0.0.0.0";

            string country;
            if (ipAddress == "127.0.0.1" || ipAddress == "::1")
            {
                country = "Localhost";
            }
            else
            {
                country = await _geoLocationService.GetCountryFromIp(ipAddress);
            }



            // var country = await _geoLocationService.GetCountryFromIp(ipAddress);
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            if (string.IsNullOrEmpty(userAgent))
            {
                userAgent = "Unknown";
            }

            var device = userAgent.Contains("Mobile") ? "Mobile" : "Desktop";

            var click = new UrlClick
            {
                ShortenedUrlId = shortenedUrl.Id,
                IpAddress = ipAddress,
                Country = country,
                Device = device,
                ClickedAt = DateTime.UtcNow

            };
            shortenedUrl.AccessCount++;
            await _urlClickRepository.AddClick(click);
            Console.WriteLine("Click registrado exitosamente");
        }

        public async Task<ShortenedUrlStatsDto> GetStats(string code)
        {
            var shortenedUrl = await _urlClickRepository.GetWithClicks(code);
            if (shortenedUrl != null) return null;

            var totalClicks = GetTotalClicks(shortenedUrl);
            var topCountries = GetTopCountries(shortenedUrl.Clicks);
            var devices = GetDeviceCounts(shortenedUrl.Clicks);

            return new ShortenedUrlStatsDto
            {
                TotalClicks = totalClicks,
                TopCountries = topCountries,
                Devices = devices
            };



        }

        private int GetTotalClicks(ShortenedUrl shortenedUrl)
        {
            return shortenedUrl.Clicks.Count;
        }
        private List<CountryCountDto> GetTopCountries(List<UrlClick> clicks)
        {
            //return clicks.GroupBy(c => c.Country)
            //    .Select(g => new CountryCountDto { Country = g.Key, Count = g.Count() })
            //    .OrderByDescending(c => c.Count)
            //    .Take(5)
            //    .ToList();

            var countryCounts = new Dictionary<string, int>();
            foreach (var click in clicks)
            {
                if (countryCounts.ContainsKey(click.Country))
                {
                    countryCounts[click.Country]++;
                }
                else
                {
                    countryCounts[click.Country] = 1;
                }
            }
            return countryCounts
                .Select(kvp => new CountryCountDto
                {
                    Country = kvp.Key,
                    Count = kvp.Value
                })
                .OrderByDescending(c => c.Count)
                .Take(5)
                .ToList();
        }
        private Dictionary<string, int> GetDeviceCounts(List<UrlClick> clicks)
        {
            return clicks
                .GroupBy(c => c.Device)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
