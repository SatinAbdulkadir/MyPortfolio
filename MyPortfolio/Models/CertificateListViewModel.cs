using MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos;
using MyPortfolio.BusinessLayer.Dtos.CertificateDtos;

namespace MyPortfolio.WebUI.Models
{
    // /Certificate sayfası: ana kategori grupları, her grubun içinde alt kategori bölümleri.
    // Bir sertifika birden fazla kategorideyse sayfada her birinin altında ayrı ayrı görünür.
    public class CertificateListViewModel
    {
        // Sadece içinde (kendisinde ya da alt kategorilerinde) sertifika olan ana kategoriler
        public List<CertificateGroupViewModel> Groups { get; set; } = new();

        // Farklı sertifika sayısı (aynı sertifika iki grupta olsa da bir kez sayılır)
        public int TotalCount { get; set; }
    }

    public class CertificateGroupViewModel
    {
        public required ResultCertificateCategoryDto Category { get; set; }

        // Ana kategoriye doğrudan bağlanmış sertifikalar
        public List<ResultCertificateDto> Certificates { get; set; } = new();

        // Sadece içinde sertifika olan alt kategoriler
        public List<CertificateSubGroupViewModel> SubGroups { get; set; } = new();

        // Filtre çipinde gösterilen sayı: grup altındaki farklı sertifika adedi
        public int DistinctCount { get; set; }
    }

    public class CertificateSubGroupViewModel
    {
        public required ResultCertificateCategoryDto Category { get; set; }
        public List<ResultCertificateDto> Certificates { get; set; } = new();
    }
}
