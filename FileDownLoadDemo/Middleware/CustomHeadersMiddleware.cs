namespace FileDownLoadDemo.Middleware
{
    public class CustomHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append("Cache-Control", "no-cache");
                context.Response.Headers.Append("Cache-Control", "no-store");

                if (!context.Response.Headers.ContainsKey("Pragma"))
                {
                    context.Response.Headers.Add("Pragma", "no-cache");
                }

                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000;includeSubDomains");
                context.Response.Headers.Add("Via", "HTTP/1.1 Inner_Proxy_WK,HTTP/1.1 Outer_Proxy_WK");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Nom-Cp-Id", "WM/ePlatform");
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
