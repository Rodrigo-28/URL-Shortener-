using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using URLShortener.Application.Dtos.Request;
using URLShortener.Application.Dtos.Responses;
using URLShortener.Application.Interfaces;
using URLShortener.Domian.Interfaces;
using URLShortener.Domian.Models;

namespace URLShortener.Application.Services
{
    public class ShortenedUrlService : IShortenedUrlService
    {
        private readonly IShortenedUrlRepository _shortenedUrlRepository;
        private readonly IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private readonly ICodeGeneratorService _codeGeneratorService;

        public ShortenedUrlService(IShortenedUrlRepository shortenedUrlRepository,
            IMapper mapper, IHttpContextAccessor httpContextAccessor, IMemoryCache cache, ICodeGeneratorService codeGeneratorService)
        {
            this._shortenedUrlRepository = shortenedUrlRepository;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
            this._cache = cache;
            this._codeGeneratorService = codeGeneratorService;
        }
        //private string GenerateUniqueCode()
        //{
        //    var codeChars = new char[ShortLinkSettings.Length];
        //    int maxValue = ShortLinkSettings.Alphabet.Length;




        //    for (int i = 0; i < ShortLinkSettings.Length; i++)
        //    {
        //        var randomIndex = _random.Next(maxValue);
        //        codeChars[i] = ShortLinkSettings.Alphabet[randomIndex];
        //    }

        //    return new string(codeChars);
        //}

        public async Task<ShortenedUrlDto> Create(UrlDto urlDto)
        {
            if (!Uri.TryCreate(urlDto.LongUrl, UriKind.Absolute, out var inputUri))
            {
                throw new ArgumentException("La URL especificada no es válida.");

            }
            string uniqueCode;
            do
            {
                uniqueCode = _codeGeneratorService.Generate();
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

            //invalidar cache
            var cacheKey = $"url_{uniqueCode}";
            _cache.Remove(cacheKey);


            await _shortenedUrlRepository.Create(shortenedUrl);
            return _mapper.Map<ShortenedUrlDto>(shortenedUrl);

        }

        public async Task<ShortenedUrlDto> GetOne(string Code)
        {

            var cacheKey = $"url_{Code}";
            if (!_cache.TryGetValue(cacheKey, out ShortenedUrlDto shortenedUrlDto))
            {
                Console.WriteLine($"Cache MISS: {cacheKey} - Consultando BD...");
                var shortenedUrl = await _shortenedUrlRepository.GetOne(Code);
                if (shortenedUrl == null)
                {
                    throw new KeyNotFoundException("No se encontró la URL acortada.");
                }


                shortenedUrlDto = _mapper.Map<ShortenedUrlDto>(shortenedUrl);

                // configuracion cache
                AddToCache(cacheKey, shortenedUrlDto);


            }
            else
            {
                Console.WriteLine($"Cache HIT: {cacheKey}");
            }
            return shortenedUrlDto;




        }

        private void AddToCache(string cacheKey, ShortenedUrlDto shortenedUrlDto)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            _cache.Set(cacheKey, shortenedUrlDto, cacheOptions);
        }
    }
}
