using Microsoft.Extensions.DependencyInjection;
using URLShortener.Application.Interfaces;
using URLShortener.Application.Services;

namespace URLShortener.Application.Extensions
{
    public static class ApplicationExtension
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IShortenedUrlService, ShortenedUrlService>();
            return services;
        }
    }
}
