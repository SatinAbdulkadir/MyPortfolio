using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;
using MyPortfolio.EntityLayer.Concrete;
using MyPortfolio.WebUI.Models;

// Hesap Güvenliği: giriş kullanıcı adı ve şifre değiştirme.
// (Eskiden burada ad/soyad/e-posta/fotoğraf alanları da vardı; sitede hiçbir yerde
// kullanılmadıkları için kaldırıldı.)
[Authorize]
public class ProfileController : Controller
{
    private const string UserNamePrefix = nameof(AccountSettingsViewModel.UserNameForm);
    private const string PasswordPrefix = nameof(AccountSettingsViewModel.PasswordForm);

    private readonly IAppUserService _appUserService;
    private readonly IValidator<ChangePasswordDto> _passwordValidator;
    private readonly IValidator<ChangeUserNameDto> _userNameValidator;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public ProfileController(IAppUserService appUserService,
                             IValidator<ChangePasswordDto> passwordValidator,
                             IValidator<ChangeUserNameDto> userNameValidator,
                             UserManager<AppUser> userManager,
                             SignInManager<AppUser> signInManager)
    {
        _appUserService = appUserService;
        _passwordValidator = passwordValidator;
        _userNameValidator = userNameValidator;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Index() => View(BuildModel());

    [HttpPost]
    public async Task<IActionResult> ChangePassword([Bind(Prefix = PasswordPrefix)] ChangePasswordDto dto)
    {
        var userName = User.Identity!.Name!;

        var validation = await _passwordValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors) { ModelState.AddModelError($"{PasswordPrefix}.{error.PropertyName}", error.ErrorMessage); }
            return InvalidForm(passwordForm: dto);
        }

        var result = await _appUserService.ChangePasswordAsync(userName, dto);
        if (!result.Succeeded)
        {
            // Yanlış mevcut şifre → "Mevcut şifre" alanı; şifre kuralları → "Yeni şifre" alanı
            foreach (var error in result.Errors)
            {
                var field = error.Code == nameof(IdentityErrorDescriber.PasswordMismatch) ? nameof(dto.CurrentPassword) : nameof(dto.NewPassword);
                ModelState.AddModelError($"{PasswordPrefix}.{field}", error.Description);
            }
            return InvalidForm(passwordForm: dto);
        }

        await RefreshSessionAsync(userName);
        TempData["AccountMessage"] = "Şifren değiştirildi. Diğer cihazlardaki açık oturumlar kapatılacak.";
        TempData["ValidationResult"] = "success";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeUserName([Bind(Prefix = UserNamePrefix)] ChangeUserNameDto dto)
    {
        var userName = User.Identity!.Name!;

        var validation = await _userNameValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors) { ModelState.AddModelError($"{UserNamePrefix}.{error.PropertyName}", error.ErrorMessage); }
            return InvalidForm(userNameForm: dto);
        }

        var result = await _appUserService.ChangeUserNameAsync(userName, dto);
        if (!result.Succeeded)
        {
            // Yanlış şifre → şifre alanı; geçersiz/kullanılan kullanıcı adı → kullanıcı adı alanı
            foreach (var error in result.Errors)
            {
                var field = error.Code == nameof(IdentityErrorDescriber.PasswordMismatch) ? nameof(dto.CurrentPassword) : nameof(dto.NewUserName);
                ModelState.AddModelError($"{UserNamePrefix}.{field}", error.Description);
            }
            return InvalidForm(userNameForm: dto);
        }

        // Kullanıcı adı değişti: oturum çerezi eski adı taşıyor, yenisiyle tazelenir
        var newUserName = dto.NewUserName!.Trim();
        await RefreshSessionAsync(newUserName);
        TempData["AccountMessage"] = $"Kullanıcı adın \"{newUserName}\" olarak değiştirildi. Bir sonraki girişte bunu kullan.";
        TempData["ValidationResult"] = "success";
        return RedirectToAction(nameof(Index));
    }

    private AccountSettingsViewModel BuildModel(ChangeUserNameDto? userNameForm = null, ChangePasswordDto? passwordForm = null)
        => new()
        {
            CurrentUserName = User.Identity?.Name ?? string.Empty,
            UserNameForm = userNameForm ?? new(),
            PasswordForm = passwordForm ?? new()
        };

    // Hatalı formda girilen şifreler geri doldurulmaz (tarayıcıda tutulmasın), sadece hata gösterilir
    private IActionResult InvalidForm(ChangeUserNameDto? userNameForm = null, ChangePasswordDto? passwordForm = null)
    {
        TempData["ValidationResult"] = "error";
        if (userNameForm != null) userNameForm.CurrentPassword = null;
        if (passwordForm != null) passwordForm = new();
        return View(nameof(Index), BuildModel(userNameForm, passwordForm));
    }

    // Şifre ya da kullanıcı adı değişince Identity security stamp'i yeniler. Bu tarayıcının çerezi
    // yeni stamp'le tazelenir ki kullanıcı kendi oturumundan atılmasın; diğer cihazlardaki
    // eski oturumlar Identity'nin bir sonraki stamp kontrolünde düşer.
    private async Task RefreshSessionAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user != null) await _signInManager.RefreshSignInAsync(user);
    }
}
