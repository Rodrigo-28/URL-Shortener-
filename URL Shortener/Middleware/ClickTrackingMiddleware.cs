using URLShortener.Application.Interfaces;

namespace URL_Shortener.Middleware
{
    public class ClickTrackingMiddleware
    {
        private readonly RequestDelegate _next;

        public ClickTrackingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task InvokeAsync(HttpContext context, IShortenedUrlService urlService)
        {
            await _next(context);
            if (context.Request.Path.HasValue && context.Request.Path.Value.Length > 1)
            {
                var code = context.Request.Path.Value.Substring(1);
                await urlService.RegisterClick(code);
            }
        }
    }
}
