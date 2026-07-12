using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace MyPortfolio.BusinessLayer.Helpers
{
    public class FileImageHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Güvenlik: sadece bilinen medya uzantıları kabul edilir.
        // Aksi halde .html/.svg gibi dosyalar yüklenip site domaini üzerinden
        // zararlı içerik servis edilebilir (stored XSS / oltalama sayfası).
        private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private static readonly string[] VideoExtensions = { ".mp4", ".webm" };

        // Optimizasyon ayarları: uzun kenar en fazla 1920px, WebP kalite 80.
        // 4-5 MB'lık telefon fotoğrafını ~100-300 KB'a indirir, gözle fark edilmez.
        private const int MaxImageDimension = 1920;
        private const int WebpQuality = 80;

        // wwwroot klasörüne erişmek için bu servisi istiyoruz
        public FileImageHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task<string?> UploadImageAsync(IFormFile imageFile, string folderName = "images")
            => UploadAsync(imageFile, folderName, ImageExtensions, optimizeImage: true);

        public Task<string?> UploadVideoAsync(IFormFile videoFile, string folderName = "videos")
            => UploadAsync(videoFile, folderName, VideoExtensions, optimizeImage: false);

        private async Task<string?> UploadAsync(IFormFile file, string folderName, string[] allowedExtensions, bool optimizeImage)
        {
            if (file == null || file.Length == 0) return null;

            // 1. Uzantı beyaz listesi kontrolü — listede yoksa dosya reddedilir (null döner)
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension)) return null;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            // 2. Resimler küçültülüp WebP'ye çevrilir (gif hariç — animasyon kaybolmasın).
            // Parse edilemeyen dosya (uzantısı .jpg yapılmış sahte içerik) null döner = reddedilir;
            // ham kaydetmeye DÜŞMEZ, böylece içerik doğrulaması da yapılmış olur.
            if (optimizeImage && extension != ".gif")
            {
                return await TrySaveOptimizedImageAsync(file, uploadsFolder, folderName);
            }

            // 3. Dosyaya benzersiz bir isim ver (Örn: unique-id.jpg)
            // Kullanıcının verdiği ismi kullanmıyoruz, Türkçe karakter falan sorun olur.
            string uniqueFileName = Guid.NewGuid().ToString() + extension;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 4. Veritabanına yazılacak "göreli" yolu dön (/images/unique-id.jpg)
            return $"/{folderName}/{uniqueFileName}";
        }

        // Resmi yeniden boyutlandırıp WebP olarak kaydeder.
        // Bonus güvenlik: içerik gerçekten resim değilse (uzantısı değiştirilmiş dosya)
        // ImageSharp parse edemez ve dosya olduğu gibi kaydedilmek yerine reddedilmiş olur.
        private async Task<string?> TrySaveOptimizedImageAsync(IFormFile file, string uploadsFolder, string folderName)
        {
            try
            {
                using var image = await Image.LoadAsync(file.OpenReadStream());

                // Telefon fotoğraflarındaki EXIF yön bilgisini uygula (yan yatmış fotoğraf sorunu)
                image.Mutate(x => x.AutoOrient());

                // Sadece büyükse küçült; küçük resmi büyütme
                if (image.Width > MaxImageDimension || image.Height > MaxImageDimension)
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(MaxImageDimension, MaxImageDimension)
                    }));
                }

                string uniqueFileName = Guid.NewGuid().ToString() + ".webp";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                await image.SaveAsWebpAsync(filePath, new WebpEncoder { Quality = WebpQuality });

                return $"/{folderName}/{uniqueFileName}";
            }
            catch (Exception)
            {
                // Bozuk/egzotik format: optimizasyon atlanır, çağıran orijinal kaydetmeye düşer
                return null;
            }
        }

        // Kayıt silinince/değiştirilince fiziksel dosyayı da temizler;
        // yoksa wwwroot'ta yetim dosyalar (özellikle büyük videolar) sınırsız birikir.
        public void DeleteFile(string? relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl) || !relativeUrl.StartsWith("/")) return;

            try
            {
                string webRoot = Path.GetFullPath(_webHostEnvironment.WebRootPath);
                string fullPath = Path.GetFullPath(Path.Combine(webRoot, relativeUrl.TrimStart('/')));

                // Path traversal koruması: hedef mutlaka wwwroot içinde kalmalı
                if (!fullPath.StartsWith(webRoot, StringComparison.OrdinalIgnoreCase)) return;

                if (File.Exists(fullPath)) File.Delete(fullPath);
            }
            catch (IOException)
            {
                // Dosya kilitliyse silme başarısız olabilir; asıl işlemi (DB silme) engellememeli
            }
        }
    }
}
