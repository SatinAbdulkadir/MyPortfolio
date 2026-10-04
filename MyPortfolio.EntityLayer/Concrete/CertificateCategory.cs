using MyPortfolio.EntityLayer.Common;

namespace MyPortfolio.EntityLayer.Concrete
{
    // Sertifikaların gruplandığı başlık: "Yazılım", "Front End", ".NET Developer Pozisyonu" gibi.
    // İsimleri sabit liste değil, admin panelinden yönetilir.
    // İki seviyelidir: ParentId boşsa ana kategori, doluysa bir ana kategorinin alt kategorisi.
    public class CertificateCategory : BaseEntity
    {
        public required string Name { get; set; }

        // Font Awesome sınıfı (örn. "fa fa-code"); boşsa arayüz varsayılan ikon kullanır
        public string? Icon { get; set; }

        // Sitede grupların görünme sırası; küçük olan önce gelir
        public int DisplayOrder { get; set; }

        // Üst (ana) kategori. Derinlik 2 ile sınırlı: üst kategorinin kendi üstü olamaz
        // (kural CertificateCategoryManager.TValidateParentAsync'te uygulanır)
        public int? ParentId { get; set; }
    }
}
