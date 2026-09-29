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

        // Kategori ilişkisi: projede navigation property kullanılmıyor (bkz. PortfolioDetail),
        // kategori bilgisi manager içinde ayrı sorguyla birleştirilir
        public required int CategoryId { get; set; }

        // Ana sayfadaki "öne çıkan sertifikalar" bölümünde görünsün mü
        public bool IsFeatured { get; set; }
    }
}
