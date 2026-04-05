using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class UpdateSocialMediaValidator : AbstractValidator<UpdateSocialMediaDto>
    {
        public UpdateSocialMediaValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Platform adı boş geçilemez.");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Link alanı boş geçilemez.");
            RuleFor(x => x.Icon).NotEmpty().WithMessage("İkon alanı boş geçilemez.");
        }
    }
}