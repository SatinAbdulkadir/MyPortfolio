namespace MyPortfolio.BusinessLayer.Dtos.CertificateDtos
{
    // Create ve Update DTO'larının ortak alanları: paylaşılan validasyon kuralları
    // (CertificateBaseValidator) bu arayüz üzerinden tek yerde tanımlanır.
    public interface ICertificateFormDto
    {
        string Title { get; }
        string Issuer { get; }
        DateTime IssueDate { get; }
        string? Description { get; }
        string? CredentialUrl { get; }
        int CategoryId { get; }
    }
}
