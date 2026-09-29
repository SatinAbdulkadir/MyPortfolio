namespace MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos
{
    // Create ve Update DTO'larının ortak alanları: paylaşılan validasyon kuralları
    // (CertificateCategoryBaseValidator) bu arayüz üzerinden tek yerde tanımlanır.
    public interface ICertificateCategoryFormDto
    {
        string Name { get; }
        string? Icon { get; }
        int DisplayOrder { get; }
    }
}
