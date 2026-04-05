using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class EditProfileValidator : AbstractValidator<EditProfileDto>
    {
        public EditProfileValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad alanı boş geçilemez.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad alanı boş geçilemez.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta alanı boş geçilemez.")
                                 .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            // Şifre alanı doluysa kontrolleri yap (Şifre değişmeyecekse boş kalabilir)
            RuleSet("PasswordChange", () => {
                RuleFor(x => x.Password).MinimumLength(6).WithMessage("Yeni şifre en az 6 karakter olmalıdır.")
                                       .When(x => !string.IsNullOrEmpty(x.Password));

                RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Yeni şifreler birbiriyle uyuşmuyor.")
                                              .When(x => !string.IsNullOrEmpty(x.Password));

                RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Şifre değiştirmek için mevcut şifrenizi girmelisiniz.")
                                              .When(x => !string.IsNullOrEmpty(x.Password));
            });
        }
    }
}