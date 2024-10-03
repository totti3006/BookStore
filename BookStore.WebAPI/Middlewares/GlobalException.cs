namespace BookStore.WebAPI.Middlewares
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalException> _logger;

        public GlobalException(RequestDelegate next, ILogger<GlobalException> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Code to be executed before the next middleware
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // problem occur
                // record the exception
                _logger.LogError($"Error from exception: {ex.Message}");
                // write the response for client
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("An error occurred");
            }
        }
    }
}
