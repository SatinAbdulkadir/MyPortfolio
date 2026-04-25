using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.WebUI.ViewComponents
{
    public class _PortfolioComponentPartial:ViewComponent
    {
        private readonly IPortfolioService _portfolioService;

        public _PortfolioComponentPartial(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var values = await _portfolioService.TGetPortfolioListAsync();

           
            return View(values);
        }
    }
}
