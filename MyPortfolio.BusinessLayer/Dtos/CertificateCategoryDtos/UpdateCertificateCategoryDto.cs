namespace MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos
{
    public class UpdateCertificateCategoryDto : ICertificateCategoryFormDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }
    }
}
