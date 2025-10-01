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

        public ShortenedUrlService(IShortenedUrlRepository shortenedUrlRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this._shortenedUrlRepository = shortenedUrlRepository;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
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
            var shortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{uniqueCode}"; z

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
    }
}
