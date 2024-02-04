using System.Globalization;

namespace ProjectForFarmers.WebApi.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var culture = context.Request.Headers["Accept-Language"];

            if (string.IsNullOrEmpty(culture))
                culture = "uk-UA";

            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            await _next(context);
        }

    }

}
