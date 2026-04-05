using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class UpdatePortfolioValidator : AbstractValidator<UpdatePortfolioDto>
    {
        public UpdatePortfolioValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Proje başlığı boş geçilemez.");
            RuleFor(x => x.SubTitle).NotEmpty().WithMessage("Alt başlık boş geçilemez.");
            //RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Görsel linki zorunludur.");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Proje linki zorunludur.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş geçilemez.");

            RuleFor(x => x.ImageFile).NotNull().WithMessage("Lütfen bir proje görseli seçiniz.");
        }
    }
}