using Microsoft.Extensions.DependencyInjection;
using URLShortener.Domian.Interfaces;
using URLShortener.infrastructure.Repositories;

namespace URLShortener.infrastructure.Extensions
{
    public static class InfrastructureExtension
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IShortenedUrlRepository, ShortenedUrlRepository>();
            services.AddTransient<IUrlClickRepository, UrlClickRepository>();

            return services;
        }
    }
}
