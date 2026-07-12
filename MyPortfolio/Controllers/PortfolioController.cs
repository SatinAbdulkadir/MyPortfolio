using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.WebUI.Models;

namespace MyPortfolio.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IPortfolioDetailService _portfolioDetailService;
        private readonly IPortfolioImageService _portfolioImageService;

        public PortfolioController(IPortfolioService portfolioService,
                                   IPortfolioDetailService portfolioDetailService,
                                   IPortfolioImageService portfolioImageService)
        {
            _portfolioService = portfolioService;
            _portfolioDetailService = portfolioDetailService;
            _portfolioImageService = portfolioImageService;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var portfolio = await _portfolioService.TGetPortfolioByIdAsync(id);
            if (portfolio == null) return NotFound();

            var detail = await _portfolioDetailService.TGetByPortfolioIdAsync(id);

            var model = new PortfolioDetailViewModel
            {
                Portfolio = portfolio,
                Detail = detail
            };

            if (detail != null)
            {
                model.GalleryImages = await _portfolioImageService.TGetListByDetailIdAsync(detail.Id);
            }

            // Sayfa sonundaki "Diğer Projeler" şeridi: mevcut proje hariç tüm projeler
            var allProjects = await _portfolioService.TGetPortfolioListAsync();
            model.OtherProjects = allProjects.Where(p => p.Id != id).ToList();

            return View(model);
        }
    }
}
