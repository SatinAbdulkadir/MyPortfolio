using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.CertificateDtos;
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
            // Liste tarihe göre sıralı gelir; aşağıdaki filtreler bu sırayı korur
            var certificates = await _certificateService.TGetCertificateListAsync();
            var categories = await _categoryService.TGetCategoryListAsync();

            List<ResultCertificateDto> InCategory(int categoryId)
                => certificates.Where(c => c.Categories.Any(x => x.Id == categoryId)).ToList();

            var model = new CertificateListViewModel { TotalCount = certificates.Count };

            foreach (var main in categories.Where(c => c.IsMain))
            {
                var group = new CertificateGroupViewModel
                {
                    Category = main,
                    Certificates = InCategory(main.Id)
                };

                foreach (var child in categories.Where(c => c.ParentId == main.Id))
                {
                    var items = InCategory(child.Id);
                    if (items.Count > 0)
                    {
                        group.SubGroups.Add(new CertificateSubGroupViewModel { Category = child, Certificates = items });
                    }
                }

                // Aynı sertifika hem ana kategoride hem alt kategoride olabilir: çipte bir kez sayılsın
                group.DistinctCount = group.Certificates
                    .Concat(group.SubGroups.SelectMany(s => s.Certificates))
                    .Select(c => c.Id)
                    .Distinct()
                    .Count();

                // Boş ana kategoriler sayfada yer kaplamasın; çipler de buradan üretilir
                if (group.DistinctCount > 0) model.Groups.Add(group);
            }

            return View(model);
        }
    }
}
