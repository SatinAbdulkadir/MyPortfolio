using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;

[Authorize]
public class AdminSocialMediaController : Controller
{
    private readonly ISocialMediaService _socialMediaService;
    private readonly IValidator<CreateSocialMediaDto> _createValidator;
    private readonly IValidator<UpdateSocialMediaDto> _updateValidator;

    public AdminSocialMediaController(ISocialMediaService socialMediaService,
                                      IValidator<CreateSocialMediaDto> createValidator,
                                      IValidator<UpdateSocialMediaDto> updateValidator)
    {
        _socialMediaService = socialMediaService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IActionResult> Index()
    {
        var values = await _socialMediaService.TGetSocialMediaListAsync();
        return View(values);
    }

    [HttpGet]
    public IActionResult CreateSocialMedia() => View();

    [HttpPost]
    public async Task<IActionResult> CreateSocialMedia(CreateSocialMediaDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }
        await _socialMediaService.TCreateSocialMediaAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteSocialMedia(int id)
    {
        await _socialMediaService.TDeleteSocialMediaAsync(id);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateSocialMedia(int id)
    {
        var value = await _socialMediaService.TGetByIdAsync(id);
        // Eğer tip uyuşmazlığı olursa (ResultDto gelirse) buraya Mapper lazım reis unutma!
        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSocialMedia(UpdateSocialMediaDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }
        await _socialMediaService.TUpdateSocialMediaAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }
}