using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Mevcut şifrenizi girmelisiniz.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Yeni şifre boş geçilemez.");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword).WithMessage("Yeni şifreler birbiriyle uyuşmuyor.");

            // Uzunluk/karmaşıklık burada tekrar yazılmaz: tek kaynak Program.cs'teki Identity
            // şifre ayarları. ChangePasswordAsync onları uygular, mesajlar TurkishIdentityErrorDescriber'dan gelir.
            // (Eski EditProfileValidator'da bu kurallar RuleSet içindeydi ve hiç çalışmıyordu.)
        }
    }
}
