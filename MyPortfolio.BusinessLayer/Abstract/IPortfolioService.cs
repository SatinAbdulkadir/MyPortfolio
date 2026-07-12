using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;

public interface IPortfolioService
{
    Task<List<ResultPortfolioDto>> TGetPortfolioListAsync();
    Task TCreatePortfolioAsync(CreatePortfolioDto createDto);
    Task TUpdatePortfolioAsync(UpdatePortfolioDto updateDto);
    Task TDeletePortfolioAsync(int id);
    Task<UpdatePortfolioDto> TGetByIdAsync(int id);

    // Public detay sayfası için: entity'yi Result DTO olarak döner
    Task<ResultPortfolioDto?> TGetPortfolioByIdAsync(int id);
}