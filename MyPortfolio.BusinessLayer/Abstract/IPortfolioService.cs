using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;

public interface IPortfolioService
{
    Task<List<ResultPortfolioDto>> TGetPortfolioListAsync();
}