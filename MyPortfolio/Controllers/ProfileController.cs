using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;
using MyPortfolio.EntityLayer.Concrete;

[Authorize]
public class ProfileController : Controller
{
    private readonly IAppUserService _appUserService;
    private readonly IValidator<EditProfileDto> _validator;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public ProfileController(IAppUserService appUserService,
                             IValidator<EditProfileDto> validator,
                             UserManager<AppUser> userManager,
                             SignInManager<AppUser> signInManager)
    {
        _appUserService = appUserService;
        _validator = validator;
        _userManager = userManager;
        _signInManager = signInManager;
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

        if (!result.Succeeded)
        {
            // Identity'nin hata listesi ilgili alanın altında gösterilir:
            // yanlış mevcut şifre → "Mevcut Şifre", şifre kuralları (PasswordTooShort, PasswordRequires...) → "Yeni Şifre"
            foreach (var error in result.Errors)
            {
                var field = error.Code == nameof(IdentityErrorDescriber.PasswordMismatch) ? nameof(EditProfileDto.CurrentPassword)
                          : error.Code.StartsWith("Password") ? nameof(EditProfileDto.Password)
                          : string.Empty;
                ModelState.AddModelError(field, error.Description);
            }
            TempData["ValidationResult"] = "error";
            return View(editProfileDto);
        }

        // Şifre değiştiyse security stamp yenilendi: bu tarayıcının çerezi de yeni stamp'le
        // tazelenir ki kullanıcı kendi oturumundan atılmasın. Diğer cihazlardaki eski oturumlar
        // ise Identity'nin bir sonraki stamp kontrolünde düşer. (Identity'nin kendi ChangePassword
        // sayfası da aynı şeyi yapar.)
        if (!string.IsNullOrEmpty(editProfileDto.Password))
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null) await _signInManager.RefreshSignInAsync(user);
        }

        // Eskiden başarıda /Login/Index'e yönlendiriyordu; LoginKey olmadan orası 404 veriyor
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }
}
