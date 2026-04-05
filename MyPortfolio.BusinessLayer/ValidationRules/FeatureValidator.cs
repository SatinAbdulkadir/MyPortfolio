using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class FeatureValidator : AbstractValidator<UpdateFeatureDto>
    {
        public FeatureValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Ad-Soyad/Başlık alanı boş geçilemez.");
            RuleFor(x => x.Title).MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş geçilemez.");
            RuleFor(x => x.Description).MaximumLength(250).WithMessage("Açıklama 250 karakterden fazla olamaz.");
        }
    }
}