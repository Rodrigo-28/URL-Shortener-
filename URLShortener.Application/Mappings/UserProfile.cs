using AutoMapper;
using URLShortener.Application.Dtos.Request;
using URLShortener.Application.Dtos.Responses;
using URLShortener.Domian.Models;

namespace URLShortener.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UrlDto, ShortenedUrl>()
               .ForMember(dest => dest.LongUrl, opt => opt.MapFrom(src => src.LongUrl))
               .ForMember(dest => dest.ShortUrl, opt => opt.Ignore()) // Ignorar ShortUrl en el mapeo
               .ForMember(dest => dest.Code, opt => opt.Ignore()) // Ignorar Code en el mapeo
               .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // Ignorar CreatedAt en el mapeo

            // Mapeo de ShortenedUrl a ShortenedUrlDto
            CreateMap<ShortenedUrl, ShortenedUrlDto>()
                .ForMember(dest => dest.ShortUrl, opt => opt.MapFrom(src => src.ShortUrl))
                .ForMember(dest => dest.LongUrl, opt => opt.MapFrom(src => src.LongUrl));
        }
    }
}
