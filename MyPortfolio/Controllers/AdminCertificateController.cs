using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos;
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
            // Kategori yoksa sertifika da eklenemez; kullanıcıyı boş seçim listesiyle baş başa bırakma
            var categories = await LoadCategoriesAsync();
            if (categories.Count == 0)
            {
                TempData["WarningMessage"] = "Önce en az bir sertifika kategorisi oluşturmalısın.";
                return RedirectToAction("Index", "AdminCertificateCategory");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCertificate(CreateCertificateDto dto)
        {
            var categories = await LoadCategoriesAsync();
            dto.CategoryIds = KeepExistingCategoryIds(dto.CategoryIds, categories);

            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
                TempData["ValidationResult"] = "error";
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

            await LoadCategoriesAsync();
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCertificate(UpdateCertificateDto dto)
        {
            var categories = await LoadCategoriesAsync();
            dto.CategoryIds = KeepExistingCategoryIds(dto.CategoryIds, categories);

            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
                TempData["ValidationResult"] = "error";
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
            // Bağlantıların ve yüklenen dosyanın silinmesi manager'ın işi (bkz. CertificateManager)
            await _certificateService.TDeleteCertificateAsync(id);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        // Önizleme özelliği gelmeden önce yüklenmiş PDF'ler için bir kerelik toplu önizleme üretimi
        [HttpPost]
        public async Task<IActionResult> GeneratePreviews()
        {
            var (created, failed) = await _certificateService.TGenerateMissingPreviewsAsync();

            if (failed > 0)
            {
                TempData["WarningMessage"] = $"{created} önizleme oluşturuldu, {failed} PDF için oluşturulamadı (bu kartlar önizlemesiz, kompakt görünür).";
            }
            else
            {
                TempData["ValidationResult"] = "success";
            }

            return RedirectToAction("Index");
        }

        // Formdaki kategori onay kutuları için ağaç sırasında liste (ana → alt kategoriler)
        private async Task<List<ResultCertificateCategoryDto>> LoadCategoriesAsync()
        {
            var categories = await _categoryService.TGetCategoryListAsync();
            ViewBag.Categories = categories;
            return categories;
        }

        // Elle değiştirilmiş bir formdan var olmayan kategori Id'si gelirse
        // ara tablodaki FK hata vermesin: sadece gerçekten var olanlar kalır
        private static List<int> KeepExistingCategoryIds(List<int> requested, List<ResultCertificateCategoryDto> categories)
        {
            var existing = categories.Select(c => c.Id).ToHashSet();
            return requested.Where(existing.Contains).Distinct().ToList();
        }
    }
}
