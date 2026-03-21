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
            // Business katmanından projelerin listesini çekiyoruz
            var values = await _portfolioService.TGetPortfolioListAsync();

            // Listeyi HTML tarafına (View) gönderiyoruz
            return View(values);
        }
    }
}
