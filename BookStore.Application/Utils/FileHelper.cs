using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Utils
{
    internal class FileHelper
    {
        public static async Task<bool> UploadFile(IFormFile? ufile, string path, string fileName)
        {
            if (ufile != null && ufile.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory() + path, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ufile.CopyToAsync(fileStream);
                }
                return true;
            }
            return false;
        }
    }
}
