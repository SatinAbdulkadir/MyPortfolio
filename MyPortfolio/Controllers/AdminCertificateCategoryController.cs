using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos;

namespace MyPortfolio.WebUI.Controllers
{
    [Authorize]
    public class AdminCertificateCategoryController : Controller
    {
        private readonly ICertificateCategoryService _categoryService;
        private readonly IValidator<CreateCertificateCategoryDto> _createValidator;
        private readonly IValidator<UpdateCertificateCategoryDto> _updateValidator;

        public AdminCertificateCategoryController(ICertificateCategoryService categoryService,
                                                  IValidator<CreateCertificateCategoryDto> createValidator,
                                                  IValidator<UpdateCertificateCategoryDto> updateValidator)
        {
            _categoryService = categoryService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IActionResult> Index()
            => View(await _categoryService.TGetCategoryListAsync());

        [HttpGet] public IActionResult CreateCertificateCategory() => View();

        [HttpPost]
        public async Task<IActionResult> CreateCertificateCategory(CreateCertificateCategoryDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
                TempData["ValidationResult"] = "error";
                return View(dto);
            }

            await _categoryService.TCreateCategoryAsync(dto);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCertificateCategory(int id)
        {
            var value = await _categoryService.TGetByIdAsync(id);
            if (value == null) return NotFound();

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCertificateCategory(UpdateCertificateCategoryDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
                TempData["ValidationResult"] = "error";
                return View(dto);
            }

            await _categoryService.TUpdateCategoryAsync(dto);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCertificateCategory(int id)
        {
            // Dolu kategori silinmez: sertifikalar hiçbir grupta görünmez hale gelirdi
            var deleted = await _categoryService.TDeleteCategoryAsync(id);
            if (!deleted)
            {
                TempData["WarningMessage"] = "Bu kategoride sertifika var. Önce sertifikaları başka bir kategoriye taşı veya sil.";
                return RedirectToAction("Index");
            }

            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }
    }
}
