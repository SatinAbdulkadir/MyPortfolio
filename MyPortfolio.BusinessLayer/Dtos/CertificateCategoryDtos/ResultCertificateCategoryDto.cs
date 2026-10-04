namespace MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos
{
    public class ResultCertificateCategoryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }
        public int? ParentId { get; set; }

        // Aşağıdakiler entity'de yok, manager doldurur:
        public string? ParentName { get; set; }

        // Bu kategoriye doğrudan bağlı sertifika sayısı (alt kategoriler hariç)
        public int CertificateCount { get; set; }

        public int ChildCount { get; set; }

        public bool IsMain => ParentId == null;

        // Silinebilir mi: ne sertifikası ne alt kategorisi olmalı
        public bool CanDelete => CertificateCount == 0 && ChildCount == 0;

        // Admin listelerinde ve kartlarda gösterilecek tam ad: "Yazılım › Front End"
        public string FullName => ParentName == null ? Name : $"{ParentName} › {Name}";
    }
}
