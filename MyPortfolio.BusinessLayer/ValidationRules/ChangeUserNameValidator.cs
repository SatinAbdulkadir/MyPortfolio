using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class ChangeUserNameValidator : AbstractValidator<ChangeUserNameDto>
    {
        public ChangeUserNameValidator()
        {
            RuleFor(x => x.NewUserName).NotEmpty().WithMessage("Yeni kullanıcı adı boş geçilemez.")
                                       .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
                                       .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakteri aşamaz.");

            // İzin verilen karakterler (Türkçe harf ve boşluk yok) Identity'nin kendi kuralıdır;
            // SetUserNameAsync uygular, mesaj TurkishIdentityErrorDescriber'dan gelir
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Değişikliği onaylamak için mevcut şifrenizi girmelisiniz.");
        }
    }
}
