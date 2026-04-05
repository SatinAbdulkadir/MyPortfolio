using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class CreateTestimonialValidator : AbstractValidator<CreateTestimonialDto>
    {
        public CreateTestimonialValidator()
        {
            RuleFor(x => x.NameSurname).NotEmpty().WithMessage("Ad Soyad alanı boş geçilemez.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Ünvan alanı boş geçilemez (Örn: CEO, Yazılım Mühendisi).");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Yorum alanı boş geçilemez.")
                                       .MinimumLength(10).WithMessage("Yorum en az 10 karakter olmalıdır.");
        }
    }
}