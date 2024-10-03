using BookStore.WebAPI.Middlewares;

namespace BookStore.WebAPI.Extensions
{
    public static class MiddlewareExtension
    {
        public static void UseCustomGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();
        }

        public static void UseCustomAuthenticate(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthenticateMiddleware>();
        }
    }
}
