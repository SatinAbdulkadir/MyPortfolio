using MyPortfolio.EntityLayer.Common;

namespace MyPortfolio.EntityLayer.Concrete
{
    public class Certificate : BaseEntity
    {
        public required string Title { get; set; }

        // Sertifikayı veren kurum: BTK Akademi, Udemy, Coursera...
        public required string Issuer { get; set; }

        public DateTime IssueDate { get; set; }

        public string? Description { get; set; }

        // Credly/Coursera gibi doğrulama sayfasının linki; boş geçilebilir
        public string? CredentialUrl { get; set; }

        // Yüklenen dosyanın yolu: /certificates/x.webp veya /certificates/x.pdf
        public string? FileUrl { get; set; }

        // "image" veya "pdf": arayüz görseli mi büyütecek yoksa yeni sekmede mi açacak
        public string? FileType { get; set; }

        // PDF'in ilk sayfasından üretilen WebP önizleme (/certificates/x.preview.webp).
        // Kartta görsel olarak gösterilir; üretilemediyse boş kalır ve kart kompakt görünür.
        public string? PreviewUrl { get; set; }

        // Kategoriler ara tablo üzerinden bağlanır (bkz. CertificateCategoryLink):
        // bir sertifika birden fazla kategoride görünebilir

        // Ana sayfadaki "öne çıkan sertifikalar" bölümünde görünsün mü
        public bool IsFeatured { get; set; }
    }
}
