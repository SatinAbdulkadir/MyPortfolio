namespace MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos
{
    // Create ve Update DTO'larının ortak alanları: paylaşılan validasyon kuralları
    // (PortfolioDetailBaseValidator) bu arayüz üzerinden tek yerde tanımlanır.
    public interface IPortfolioDetailFormDto
    {
        int PortfolioId { get; }
        string DetailDescription { get; }
        string? Technologies { get; }
        string? VideoUrl { get; }
    }
}
