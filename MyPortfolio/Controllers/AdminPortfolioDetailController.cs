using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos;
using MyPortfolio.BusinessLayer.Dtos.PortfolioImageDtos;
using MyPortfolio.BusinessLayer.Helpers;
using MyPortfolio.WebUI.Models;

[Authorize]
public class AdminPortfolioDetailController : Controller
{
    private const string VideoTypeError = "Sadece video dosyaları yüklenebilir (mp4, webm).";

    private readonly IPortfolioDetailService _portfolioDetailService;
    private readonly IPortfolioImageService _portfolioImageService;
    private readonly IPortfolioService _portfolioService;
    private readonly IValidator<CreatePortfolioDetailDto> _createValidator;
    private readonly IValidator<UpdatePortfolioDetailDto> _updateValidator;
    private readonly FileImageHelper _fileImageHelper;

    public AdminPortfolioDetailController(IPortfolioDetailService portfolioDetailService,
                                          IPortfolioImageService portfolioImageService,
                                          IPortfolioService portfolioService,
                                          IValidator<CreatePortfolioDetailDto> createValidator,
                                          IValidator<UpdatePortfolioDetailDto> updateValidator,
                                          FileImageHelper fileImageHelper)
    {
        _portfolioDetailService = portfolioDetailService;
        _portfolioImageService = portfolioImageService;
        _portfolioService = portfolioService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _fileImageHelper = fileImageHelper;
    }

    // Tüm projeleri detay durumuyla birlikte listeler (detayı olmayan da görünür ki eklenebilsin)
    public async Task<IActionResult> Index()
    {
        var portfolios = await _portfolioService.TGetPortfolioListAsync();
        var details = await _portfolioDetailService.TGetPortfolioDetailListAsync();

        var rows = portfolios.Select(p => new AdminPortfolioDetailRowViewModel
        {
            Portfolio = p,
            Detail = details.FirstOrDefault(d => d.PortfolioId == p.Id)
        }).ToList();

        return View(rows);
    }

    [HttpGet]
    public async Task<IActionResult> CreatePortfolioDetail(int portfolioId)
    {
        var portfolio = await _portfolioService.TGetPortfolioByIdAsync(portfolioId);
        if (portfolio == null) return RedirectToAction("Index");

        // Aynı projeye ikinci detay açılmasın; varsa düzenlemeye yönlendir
        var existing = await _portfolioDetailService.TGetByPortfolioIdAsync(portfolioId);
        if (existing != null) return RedirectToAction("UpdatePortfolioDetail", new { id = existing.Id });

        ViewBag.PortfolioTitle = portfolio.Title;
        var dto = new CreatePortfolioDetailDto { PortfolioId = portfolioId, DetailDescription = "" };
        return View(dto);
    }

    [HttpPost]
    [RequestSizeLimit(104857600)] // Video yüklemesi için 100 MB üst sınır
    public async Task<IActionResult> CreatePortfolioDetail(CreatePortfolioDetailDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            AddValidationErrors(result);
            await PopulateCreateViewBagAsync(dto.PortfolioId);
            return View(dto);
        }

        var (videoUrl, videoRejected) = await TryUploadVideoAsync(dto.VideoFile, dto.VideoUrl);
        if (videoRejected)
        {
            ModelState.AddModelError("VideoFile", VideoTypeError);
            TempData["ValidationResult"] = "error";
            await PopulateCreateViewBagAsync(dto.PortfolioId);
            return View(dto);
        }
        dto.VideoUrl = videoUrl;

        var detailId = await _portfolioDetailService.TCreatePortfolioDetailAsync(dto);
        await UploadGalleryFilesAsync(dto.GalleryFiles, detailId);

        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdatePortfolioDetail(int id)
    {
        var value = await _portfolioDetailService.TGetByIdAsync(id);
        if (value == null) return RedirectToAction("Index");

        await PopulateUpdateViewBagAsync(value.PortfolioId, id);
        return View(value);
    }

    [HttpPost]
    [RequestSizeLimit(104857600)]
    public async Task<IActionResult> UpdatePortfolioDetail(UpdatePortfolioDetailDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            AddValidationErrors(result);
            await PopulateUpdateViewBagAsync(dto.PortfolioId, dto.Id);
            return View(dto);
        }

        var (videoUrl, videoRejected) = await TryUploadVideoAsync(dto.VideoFile, dto.VideoUrl);
        if (videoRejected)
        {
            ModelState.AddModelError("VideoFile", VideoTypeError);
            TempData["ValidationResult"] = "error";
            await PopulateUpdateViewBagAsync(dto.PortfolioId, dto.Id);
            return View(dto);
        }
        dto.VideoUrl = videoUrl;

        await _portfolioDetailService.TUpdatePortfolioDetailAsync(dto);
        await UploadGalleryFilesAsync(dto.GalleryFiles, dto.Id);

        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    // Kaskad temizlik (galeri kayıtları + dosyalar + video dosyası) servis katmanında yapılır
    [HttpPost]
    public async Task<IActionResult> DeletePortfolioDetail(int id)
    {
        await _portfolioDetailService.TDeletePortfolioDetailAsync(id);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    // Galeri resmini siler (kayıt + dosya), düzenleme sayfasına geri döner
    [HttpPost]
    public async Task<IActionResult> DeletePortfolioImage(int id, int detailId)
    {
        await _portfolioImageService.TDeletePortfolioImageAsync(id);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("UpdatePortfolioDetail", new { id = detailId });
    }

    // --- Ortak yardımcılar: Create/Update ve hata yollarında tekrar eden işler tek yerde ---

    private void AddValidationErrors(FluentValidation.Results.ValidationResult result)
    {
        foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
        TempData["ValidationResult"] = "error";
    }

    private async Task PopulateCreateViewBagAsync(int portfolioId)
    {
        var portfolio = await _portfolioService.TGetPortfolioByIdAsync(portfolioId);
        ViewBag.PortfolioTitle = portfolio?.Title;
    }

    private async Task PopulateUpdateViewBagAsync(int portfolioId, int detailId)
    {
        await PopulateCreateViewBagAsync(portfolioId);
        ViewBag.GalleryImages = await _portfolioImageService.TGetListByDetailIdAsync(detailId);
    }

    // Dosya yüklendiyse o kazanır ve eski yerel video diskten silinir; yüklenmediyse mevcut link korunur.
    // rejected=true → dosya beyaz listeye takıldı (mp4/webm değil).
    private async Task<(string? videoUrl, bool rejected)> TryUploadVideoAsync(IFormFile? videoFile, string? currentUrl)
    {
        if (videoFile == null) return (currentUrl, false);

        var newUrl = await _fileImageHelper.UploadVideoAsync(videoFile);
        if (newUrl == null) return (currentUrl, true);

        // Yeni video geldi; eski yerel dosya yetim kalmasın (harici linklere dokunulmaz)
        if (currentUrl != null && currentUrl.StartsWith("/"))
        {
            _fileImageHelper.DeleteFile(currentUrl);
        }

        return (newUrl, false);
    }

    // Galeri: geçerli resimler tek veritabanı turunda eklenir; beyaz listeye takılan
    // dosyalar sessizce yutulmaz, admin'e uyarı toast'ı ile bildirilir.
    private async Task UploadGalleryFilesAsync(List<IFormFile>? files, int detailId)
    {
        if (files == null || files.Count == 0) return;

        var newImages = new List<CreatePortfolioImageDto>();
        var rejectedNames = new List<string>();

        foreach (var file in files)
        {
            var imageUrl = await _fileImageHelper.UploadImageAsync(file);
            if (imageUrl != null)
            {
                newImages.Add(new CreatePortfolioImageDto { PortfolioDetailId = detailId, ImageUrl = imageUrl });
            }
            else
            {
                rejectedNames.Add(file.FileName);
            }
        }

        await _portfolioImageService.TCreatePortfolioImagesAsync(newImages);

        if (rejectedNames.Count > 0)
        {
            TempData["WarningMessage"] = $"Şu dosyalar resim olmadığı için galeriye eklenmedi: {string.Join(", ", rejectedNames)}";
        }
    }
}
