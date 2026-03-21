using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

public class PortfolioManager : IPortfolioService
{
    private readonly IGenericDal<Portfolio> _portfolioDal;
    public PortfolioManager(IGenericDal<Portfolio> portfolioDal) { _portfolioDal = portfolioDal; }

    public async Task<List<ResultPortfolioDto>> TGetPortfolioListAsync()
    {
        var values = await _portfolioDal.GetListAsync();
        return values.Select(x => new ResultPortfolioDto
        {
            PortfolioId = x.Id,
            Title = x.Title,
            SubTitle = x.SubTitle,
            ImageUrl = x.ImageUrl,
            Url = x.Url,
            Description = x.Description
        }).ToList();
    }
}