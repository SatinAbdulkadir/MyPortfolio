using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.ExperienceDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class CreateExperienceValidator : AbstractValidator<CreateExperienceDto>
    {
        public CreateExperienceValidator()
        {
            RuleFor(x => x.Head).NotEmpty().WithMessage("Şirket/Kurum adı boş geçilemez.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Ünvan alanı boş geçilemez.");
            RuleFor(x => x.DatePeriod).NotEmpty().WithMessage("Tarih aralığı boş geçilemez.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş geçilemez.")
                                       .MinimumLength(10).WithMessage("Lütfen en az 10 karakterlik bir açıklama giriniz.");
        }
    }
}