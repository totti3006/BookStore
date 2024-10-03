using BookStore.Infrastructure.Context;

namespace BookStore.WebAPI.Extensions
{
    public static class InitializerExtensions
    {
        public static IHost InitializeDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();

            initializer.Initialize().Wait();

            return host;
        }
    }
}
