using MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;

namespace MyPortfolio.WebUI.Models
{
    // Admin "Proje Detayları" listesinde her satır: proje + (varsa) detay kaydı
    public class AdminPortfolioDetailRowViewModel
    {
        public required ResultPortfolioDto Portfolio { get; set; }
        public ResultPortfolioDetailDto? Detail { get; set; }
    }
}
