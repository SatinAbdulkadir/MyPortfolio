using MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IPortfolioDetailService
    {
        Task<List<ResultPortfolioDetailDto>> TGetPortfolioDetailListAsync();
        Task<ResultPortfolioDetailDto?> TGetByPortfolioIdAsync(int portfolioId);
        Task<UpdatePortfolioDetailDto?> TGetByIdAsync(int id);
        Task<int> TCreatePortfolioDetailAsync(CreatePortfolioDetailDto createDto);
        Task TUpdatePortfolioDetailAsync(UpdatePortfolioDetailDto updateDto);
        Task TDeletePortfolioDetailAsync(int id);

        // Portfolio silinirken çağrılır: detay + galeri kayıtlarını ve dosyalarını temizler
        Task TDeleteByPortfolioIdAsync(int portfolioId);
    }
}
