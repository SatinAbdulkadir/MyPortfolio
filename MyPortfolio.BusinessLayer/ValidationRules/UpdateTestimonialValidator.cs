using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class UpdateTestimonialValidator : AbstractValidator<UpdateTestimonialDto>
    {
        public UpdateTestimonialValidator()
        {
            RuleFor(x => x.NameSurname).NotEmpty().WithMessage("Ad Soyad alanı boş geçilemez.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Ünvan alanı boş geçilemez.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Yorum alanı boş geçilemez.");
        }
    }
}