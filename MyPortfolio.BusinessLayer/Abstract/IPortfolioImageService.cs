using MyPortfolio.BusinessLayer.Dtos.PortfolioImageDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IPortfolioImageService
    {
        Task<List<ResultPortfolioImageDto>> TGetListByDetailIdAsync(int portfolioDetailId);
        Task TCreatePortfolioImageAsync(CreatePortfolioImageDto createDto);
        Task TCreatePortfolioImagesAsync(List<CreatePortfolioImageDto> createDtos);
        Task TDeletePortfolioImageAsync(int id);
        Task TDeleteByDetailIdAsync(int portfolioDetailId);
        Task<ResultPortfolioImageDto?> TGetByIdAsync(int id);
    }
}
