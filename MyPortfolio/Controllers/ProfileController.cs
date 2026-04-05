using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;

[Authorize]
public class ProfileController : Controller
{
    private readonly IAppUserService _appUserService;
    private readonly IValidator<EditProfileDto> _validator;

    public ProfileController(IAppUserService appUserService, IValidator<EditProfileDto> validator)
    {
        _appUserService = appUserService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userName = User.Identity?.Name;
        if (string.IsNullOrEmpty(userName)) return RedirectToAction("Index", "Login");

        var model = await _appUserService.GetUserForEditAsync(userName);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(EditProfileDto editProfileDto)
    {
        var userName = User.Identity?.Name;
        if (string.IsNullOrEmpty(userName)) return RedirectToAction("Index", "Login");

        // Validasyon kontrolü
        var validationResult = await _validator.ValidateAsync(editProfileDto);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors) { ModelState.AddModelError(error.PropertyName, error.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(editProfileDto);
        }

        var result = await _appUserService.UpdateUserProfileAsync(editProfileDto, userName);

        if (result)
        {
            TempData["ValidationResult"] = "success";
            // Güvenlik için tekrar login'e atıyoruz (Şifre değişmiş olabilir)
            return RedirectToAction("Index", "Login");
        }

        ModelState.AddModelError("", "Mevcut şifreniz hatalı veya bir sorun oluştu.");
        TempData["ValidationResult"] = "error";
        return View(editProfileDto);
    }
}