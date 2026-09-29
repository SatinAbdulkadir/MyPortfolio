using MyPortfolio.EntityLayer.Common;

namespace MyPortfolio.EntityLayer.Concrete
{
    // Sertifikaların gruplandığı başlık: "Yazılım Geliştirme", "Office & İşletim Sistemleri" gibi.
    // İsimleri sabit liste değil, admin panelinden yönetilir.
    public class CertificateCategory : BaseEntity
    {
        public required string Name { get; set; }

        // Font Awesome sınıfı (örn. "fa fa-code"); boşsa arayüz varsayılan ikon kullanır
        public string? Icon { get; set; }

        // Sitede grupların görünme sırası; küçük olan önce gelir
        public int DisplayOrder { get; set; }
    }
}
