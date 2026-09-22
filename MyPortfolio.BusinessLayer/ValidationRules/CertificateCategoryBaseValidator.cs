using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public abstract class CertificateCategoryBaseValidator<T> : AbstractValidator<T>
        where T : class, ICertificateCategoryFormDto
    {
        protected CertificateCategoryBaseValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori adı boş geçilemez.")
                                .MaximumLength(100).WithMessage("Kategori adı 100 karakteri aşamaz.");

            RuleFor(x => x.Icon).MaximumLength(100).WithMessage("İkon sınıfı 100 karakteri aşamaz.");

            RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0).WithMessage("Sıra değeri negatif olamaz.");
        }
    }
}
