using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MyPortfolio.BusinessLayer.Helpers
{
    public class FileImageHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        // wwwroot klasörüne erişmek için bu servisi istiyoruz
        public FileImageHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string folderName = "images")
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            // 1. Resme benzersiz bir isim ver (Örn: unique-id.jpg)
            // Kullanıcının verdiği ismi kullanmıyoruz, Türkçe karakter falan sorun olur.
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

            // 2. Sunucudaki tam yolu oluştur (wwwroot/images/unique-id.jpg)
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderName);

            // Klasör yoksa oluştur reis
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // 3. Dosyayı sunucuya kaydet
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // 4. Veritabanına yazılacak "göreli" yolu dön (/images/unique-id.jpg)
            return $"/{folderName}/{uniqueFileName}";
        }
    }
}