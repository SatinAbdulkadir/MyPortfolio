using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;

public interface IPortfolioService
{
    Task<List<ResultPortfolioDto>> TGetPortfolioListAsync();
    Task TCreatePortfolioAsync(CreatePortfolioDto createDto);
    Task TUpdatePortfolioAsync(UpdatePortfolioDto updateDto);
    Task TDeletePortfolioAsync(int id);
    Task<UpdatePortfolioDto> TGetByIdAsync(int id);
}