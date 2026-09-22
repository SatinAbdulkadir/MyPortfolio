using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.WebUI.Models;

namespace MyPortfolio.WebUI.Controllers
{
    public class CertificateController : Controller
    {
        private readonly ICertificateService _certificateService;
        private readonly ICertificateCategoryService _categoryService;

        public CertificateController(ICertificateService certificateService,
                                     ICertificateCategoryService categoryService)
        {
            _certificateService = certificateService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var certificates = await _certificateService.TGetCertificateListAsync();
            var categories = await _categoryService.TGetCategoryListAsync();

            var grouped = certificates
                .GroupBy(x => x.CategoryId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var model = new CertificateListViewModel
            {
                // Boş kategoriler sayfada yer kaplamasın; çipler de buradan üretilir
                Categories = categories.Where(c => grouped.ContainsKey(c.Id)).ToList(),
                CertificatesByCategory = grouped,
                TotalCount = certificates.Count
            };

            return View(model);
        }
    }
}
