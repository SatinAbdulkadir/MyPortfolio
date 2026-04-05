using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.AboutDtos;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class AboutValidator : AbstractValidator<UpdateAboutDto>
    {
        public AboutValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık alanı boş geçilemez.");
            RuleFor(x => x.Title).MinimumLength(5).WithMessage("Başlık en az 5 karakter olmalıdır.");

            RuleFor(x => x.SubDescription).NotEmpty().WithMessage("Açıklama alanı boş geçilemez.");
            RuleFor(x => x.SubDescription).MaximumLength(200).WithMessage("Açıklama alanı en fazla 200 karakter olmalıdır.");


            RuleFor(x => x.Details).NotEmpty().WithMessage("Detay alanı boş geçilemez.");
            RuleFor(x => x.Details).MaximumLength(1000).WithMessage("Açıklama alanı en fazla 1000 karakter olmalıdır.");
        }
    }
}