using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;

namespace MyPortfolio.WebUI.Models
{
    // Hesap Güvenliği sayfası: iki bağımsız form (kullanıcı adı, şifre).
    // Form alanları "UserNameForm." / "PasswordForm." önekiyle gönderilir, böylece
    // hata olduğunda sadece ilgili kartın altında görünür (bkz. ProfileController).
    public class AccountSettingsViewModel
    {
        public string CurrentUserName { get; set; } = string.Empty;
        public ChangeUserNameDto UserNameForm { get; set; } = new();
        public ChangePasswordDto PasswordForm { get; set; } = new();
    }
}
