namespace MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos
{
    public class CreateCertificateCategoryDto : ICertificateCategoryFormDto
    {
        public required string Name { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }

        // Boşsa ana kategori, doluysa seçilen ana kategorinin alt kategorisi
        public int? ParentId { get; set; }
    }
}
