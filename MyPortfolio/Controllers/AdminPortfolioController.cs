using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;
using MyPortfolio.BusinessLayer.Helpers;
 // FileImageHelper'ı kullanabilmek için ekle

[Authorize]
public class AdminPortfolioController : Controller
{
    private readonly IPortfolioService _portfolioService;
    private readonly IPortfolioDetailService _portfolioDetailService;
    private readonly IValidator<CreatePortfolioDto> _createValidator;
    private readonly IValidator<UpdatePortfolioDto> _updateValidator;
    private readonly FileImageHelper _fileImageHelper;

    public AdminPortfolioController(IPortfolioService portfolioService,
                                    IPortfolioDetailService portfolioDetailService,
                                    IValidator<CreatePortfolioDto> createValidator,
                                    IValidator<UpdatePortfolioDto> updateValidator,
                                    FileImageHelper fileImageHelper)
    {
        _portfolioService = portfolioService;
        _portfolioDetailService = portfolioDetailService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _fileImageHelper = fileImageHelper;
    }

    public async Task<IActionResult> Index()
    {
        var values = await _portfolioService.TGetPortfolioListAsync();
        return View(values);
    }

    [HttpGet] public IActionResult CreatePortfolio() => View();

    [HttpPost]
    public async Task<IActionResult> CreatePortfolio(CreatePortfolioDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }


        if (dto.ImageFile != null)
        {
            dto.ImageUrl = await _fileImageHelper.UploadImageAsync(dto.ImageFile);

            // Beyaz listeye takıldıysa (resim değilse) null döner
            if (dto.ImageUrl == null)
            {
                ModelState.AddModelError("ImageFile", "Sadece resim dosyaları yüklenebilir (jpg, jpeg, png, gif, webp).");
                TempData["ValidationResult"] = "error";
                return View(dto);
            }
        }

        await _portfolioService.TCreatePortfolioAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdatePortfolio(int id)
    {
        var value = await _portfolioService.TGetByIdAsync(id);
       
        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePortfolio(UpdatePortfolioDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }


        if (dto.ImageFile != null)
        {
            var oldImageUrl = dto.ImageUrl;
            dto.ImageUrl = await _fileImageHelper.UploadImageAsync(dto.ImageFile);

            // Beyaz listeye takıldıysa (resim değilse) null döner
            if (dto.ImageUrl == null)
            {
                ModelState.AddModelError("ImageFile", "Sadece resim dosyaları yüklenebilir (jpg, jpeg, png, gif, webp).");
                TempData["ValidationResult"] = "error";
                dto.ImageUrl = oldImageUrl;
                return View(dto);
            }

            // Yeni görsel başarıyla yüklendi; eskisi diskte yetim kalmasın
            _fileImageHelper.DeleteFile(oldImageUrl);
        }

        await _portfolioService.TUpdatePortfolioAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeletePortfolio(int id)
    {
        var portfolio = await _portfolioService.TGetPortfolioByIdAsync(id);
        if (portfolio != null)
        {
            // Kaskad: önce detay sayfası + galeri (kayıtlar ve dosyalar), sonra kapak görseli, en son proje
            await _portfolioDetailService.TDeleteByPortfolioIdAsync(id);
            _fileImageHelper.DeleteFile(portfolio.ImageUrl);
            await _portfolioService.TDeletePortfolioAsync(id);
        }

        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }
}