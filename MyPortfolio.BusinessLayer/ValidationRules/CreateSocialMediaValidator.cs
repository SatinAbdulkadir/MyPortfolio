using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class CreateSocialMediaValidator : AbstractValidator<CreateSocialMediaDto>
    {
        public CreateSocialMediaValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Platform adı boş geçilemez (Örn: LinkedIn).");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Link alanı boş geçilemez.");
            RuleFor(x => x.Icon).NotEmpty().WithMessage("İkon alanı boş geçilemez (Örn: fab fa-instagram).");
        }
    }
}