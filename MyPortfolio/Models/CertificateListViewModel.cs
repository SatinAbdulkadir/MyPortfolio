using MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos;
using MyPortfolio.BusinessLayer.Dtos.CertificateDtos;

namespace MyPortfolio.WebUI.Models
{
    // /Certificate sayfası: sertifikalar kategori kategori gruplanıp listelenir,
    // üstteki filtre çipleri de aynı kategori listesinden üretilir.
    public class CertificateListViewModel
    {
        public List<ResultCertificateCategoryDto> Categories { get; set; } = new();

        // Sadece içinde sertifika olan kategoriler; anahtar kategori Id'si
        public Dictionary<int, List<ResultCertificateDto>> CertificatesByCategory { get; set; } = new();

        public int TotalCount { get; set; }
    }
}
