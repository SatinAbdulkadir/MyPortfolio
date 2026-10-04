using MyPortfolio.EntityLayer.Common;

namespace MyPortfolio.EntityLayer.Concrete
{
    // Sertifika ↔ kategori ara tablosu: bir sertifika birden fazla kategoride görünebilir
    // (örn. C# sertifikası hem "Yazılım › Backend" hem ".NET Developer Pozisyonu" altında)
    public class CertificateCategoryLink : BaseEntity
    {
        public required int CertificateId { get; set; }
        public required int CategoryId { get; set; }
    }
}
