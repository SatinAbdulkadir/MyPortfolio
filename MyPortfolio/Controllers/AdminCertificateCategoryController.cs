using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
        public async Task<IActionResult> CreateCertificateCategory()
        {
            await PopulateParentCategoriesAsync(null);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCertificateCategory(CreateCertificateCategoryDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }

            // İki seviye kuralı veritabanı gerektirdiği için validator'da değil burada
            var parentError = await _categoryService.TValidateParentAsync(null, dto.ParentId);
            if (parentError != null) ModelState.AddModelError(nameof(dto.ParentId), parentError);

            if (!result.IsValid || parentError != null)
            {
                TempData["ValidationResult"] = "error";
                await PopulateParentCategoriesAsync(null);
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

            await PopulateParentCategoriesAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCertificateCategory(UpdateCertificateCategoryDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }

            var parentError = await _categoryService.TValidateParentAsync(dto.Id, dto.ParentId);
            if (parentError != null) ModelState.AddModelError(nameof(dto.ParentId), parentError);

            if (!result.IsValid || parentError != null)
            {
                TempData["ValidationResult"] = "error";
                await PopulateParentCategoriesAsync(dto.Id);
                return View(dto);
            }

            await _categoryService.TUpdateCategoryAsync(dto);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCertificateCategory(int id)
        {
            // Dolu kategori silinmez: sertifikalar ya da alt kategoriler sahipsiz kalırdı
            var deleted = await _categoryService.TDeleteCategoryAsync(id);
            if (!deleted)
            {
                TempData["WarningMessage"] = "Bu kategoride sertifika ya da alt kategori var. Önce onları taşı veya sil.";
                return RedirectToAction("Index");
            }

            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        // Üst kategori dropdown'ı: yalnızca ana kategoriler seçilebilir (iki seviye kuralı).
        // Düzenlenen kategorinin kendisi listeden çıkarılır.
        private async Task PopulateParentCategoriesAsync(int? editingId)
        {
            var categories = await _categoryService.TGetCategoryListAsync();
            var parents = categories.Where(c => c.IsMain && c.Id != editingId).ToList();
            ViewBag.ParentCategories = new SelectList(parents, "Id", "Name");
        }
    }
}
