using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.CertificateDtos;
using MyPortfolio.BusinessLayer.Helpers;

namespace MyPortfolio.WebUI.Controllers
{
    [Authorize]
    public class AdminCertificateController : Controller
    {
        private const string FileRejectedMessage =
            "Sadece resim (jpg, jpeg, png, gif, webp) veya PDF dosyası yükleyebilirsin.";

        private readonly ICertificateService _certificateService;
        private readonly ICertificateCategoryService _categoryService;
        private readonly IValidator<CreateCertificateDto> _createValidator;
        private readonly IValidator<UpdateCertificateDto> _updateValidator;
        private readonly FileImageHelper _fileImageHelper;

        public AdminCertificateController(ICertificateService certificateService,
                                          ICertificateCategoryService categoryService,
                                          IValidator<CreateCertificateDto> createValidator,
                                          IValidator<UpdateCertificateDto> updateValidator,
                                          FileImageHelper fileImageHelper)
        {
            _certificateService = certificateService;
            _categoryService = categoryService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _fileImageHelper = fileImageHelper;
        }

        public async Task<IActionResult> Index()
            => View(await _certificateService.TGetCertificateListAsync());

        [HttpGet]
        public async Task<IActionResult> CreateCertificate()
        {
            // Kategori yoksa sertifika da eklenemez; kullanıcıyı boş dropdown'la baş başa bırakma
            if (!await PopulateCategoriesAsync()) return RedirectToAction("Index", "AdminCertificateCategory");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCertificate(CreateCertificateDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
                TempData["ValidationResult"] = "error";
                await PopulateCategoriesAsync();
                return View(dto);
            }

            if (dto.CertificateFile != null)
            {
                var upload = await _fileImageHelper.UploadCertificateFileAsync(dto.CertificateFile);

                // Beyaz listeye ya da PDF imza kontrolüne takıldıysa null döner
                if (upload.Url == null)
                {
                    ModelState.AddModelError("CertificateFile", FileRejectedMessage);
                    TempData["ValidationResult"] = "error";
                    await PopulateCategoriesAsync();
                    return View(dto);
                }

                dto.FileUrl = upload.Url;
                dto.FileType = upload.Type;
            }

            await _certificateService.TCreateCertificateAsync(dto);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCertificate(int id)
        {
            var value = await _certificateService.TGetByIdAsync(id);
            if (value == null) return NotFound();

            await PopulateCategoriesAsync();
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCertificate(UpdateCertificateDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
                TempData["ValidationResult"] = "error";
                await PopulateCategoriesAsync();
                return View(dto);
            }

            if (dto.CertificateFile != null)
            {
                var oldFileUrl = dto.FileUrl;
                var upload = await _fileImageHelper.UploadCertificateFileAsync(dto.CertificateFile);

                if (upload.Url == null)
                {
                    ModelState.AddModelError("CertificateFile", FileRejectedMessage);
                    TempData["ValidationResult"] = "error";
                    dto.FileUrl = oldFileUrl;
                    await PopulateCategoriesAsync();
                    return View(dto);
                }

                dto.FileUrl = upload.Url;
                dto.FileType = upload.Type;

                // Yeni dosya yüklendi; eskisi diskte yetim kalmasın
                _fileImageHelper.DeleteFile(oldFileUrl);
            }

            await _certificateService.TUpdateCertificateAsync(dto);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            // Yüklenen dosyanın silinmesi manager'ın işi (bkz. CertificateManager)
            await _certificateService.TDeleteCertificateAsync(id);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        // Kategori dropdown'ını doldurur; hiç kategori yoksa false döner
        private async Task<bool> PopulateCategoriesAsync()
        {
            var categories = await _categoryService.TGetCategoryListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            if (categories.Count == 0)
            {
                TempData["WarningMessage"] = "Önce en az bir sertifika kategorisi oluşturmalısın.";
                return false;
            }

            return true;
        }
    }
}
