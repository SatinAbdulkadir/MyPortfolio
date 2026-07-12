using MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;
using MyPortfolio.BusinessLayer.Dtos.PortfolioImageDtos;

namespace MyPortfolio.WebUI.Models
{
    // Public proje detay sayfasının tüm verisi tek modelde toplanır
    public class PortfolioDetailViewModel
    {
        public required ResultPortfolioDto Portfolio { get; set; }

        // Detay kaydı henüz girilmemiş olabilir; sayfa temel bilgilerle yine de açılır
        public ResultPortfolioDetailDto? Detail { get; set; }

        public List<ResultPortfolioImageDto> GalleryImages { get; set; } = new();
        public List<ResultPortfolioDto> OtherProjects { get; set; } = new();
    }
}
