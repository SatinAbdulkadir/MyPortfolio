namespace MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos
{
    public class ResultCertificateCategoryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }

        // Kategoriye bağlı sertifika sayısı: admin listesinde gösterilir ve
        // "dolu kategoriyi silme" uyarısını tetikler (entity'de yok, manager doldurur)
        public int CertificateCount { get; set; }
    }
}
