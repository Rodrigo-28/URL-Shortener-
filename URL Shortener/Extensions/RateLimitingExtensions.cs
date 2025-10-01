using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;
using URL_Shortener.Options;

namespace URL_Shortener.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddAppRateLimiting(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RateLimitOptions>(configuration.GetSection("RateLimiting"));

            services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {
                    var opts = context.HttpContext.RequestServices
                        .GetRequiredService<IOptions<RateLimitOptions>>().Value;

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.Headers["Retry-After"] =
                        opts.Create?.RetryAfterSeconds.ToString() ?? "60";
                    context.HttpContext.Response.ContentType = "application/json";

                    await context.HttpContext.Response.WriteAsync("""
                    {"title":"Too Many Requests","status":429,"detail":"Rate limit exceeded for this endpoint. Try again later."}
                    """, token);
                };

                // Política: CreatePolicy (POST /)
                options.AddPolicy("CreatePolicy", httpContext =>
                {
                    var rl = httpContext.RequestServices
                        .GetRequiredService<IOptions<RateLimitOptions>>().Value;

                    var headerName = rl.ApiKeyHeader ?? "X-Api-Key";
                    var apiKey = httpContext.Request.Headers[headerName].FirstOrDefault();

                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    var partitionKey = !string.IsNullOrWhiteSpace(apiKey) ? $"apiKey:{apiKey}" : $"ip:{ip}";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = rl.Create?.PermitLimit ?? 5,
                            Window = TimeSpan.FromSeconds(rl.Create?.WindowSeconds ?? 60),
                            QueueLimit = rl.Create?.QueueLimit ?? 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                });
            });

            return services;
        }

        public static IApplicationBuilder UseAppRateLimiting(this IApplicationBuilder app)
        {
            return app.UseRateLimiter();
        }
    }
}
