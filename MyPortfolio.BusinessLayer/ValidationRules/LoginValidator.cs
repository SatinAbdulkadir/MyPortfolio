using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adını girmeden geçemezsin reis.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifreni yazmayı unuttun.");
        }
    }
}