using System.Text.Json;

namespace BookStore.Infrastructure.Utils
{
    internal class JsonFileReader
    {
        public static async Task<List<T>> ReadAsync<T>(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            return await JsonSerializer.DeserializeAsync<List<T>>(stream, options) ?? new List<T>();
        }

    }
}
