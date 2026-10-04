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

            // Her değişiklik mevcut şifreyle onaylanır (formdaki açıklamayla aynı kural)
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Değişiklikleri onaylamak için mevcut şifrenizi girmelisiniz.");

            // DİKKAT: Bu kurallar eskiden RuleSet("PasswordChange") içindeydi. RuleSet'teki kurallar
            // düz ValidateAsync(dto) çağrısında hiç çalışmaz; yani tekrar alanı kontrol edilmiyordu.
            // Uzunluk/karmaşıklık burada yazılmaz: tek kaynak Program.cs'teki Identity şifre ayarları.
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Yeni şifreler birbiriyle uyuşmuyor.")
                                           .When(x => !string.IsNullOrEmpty(x.Password));
        }
    }
}
