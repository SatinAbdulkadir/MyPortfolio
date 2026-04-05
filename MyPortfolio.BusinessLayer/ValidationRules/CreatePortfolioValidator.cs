using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class CreatePortfolioValidator : AbstractValidator<CreatePortfolioDto>
    {
        public CreatePortfolioValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Proje başlığı boş geçilemez.");
            RuleFor(x => x.SubTitle).NotEmpty().WithMessage("Alt başlık (Teknoloji vb.) boş geçilemez.");
            //RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Proje görsel linki zorunludur.");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Proje (GitHub/Canlı) linki zorunludur.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Proje açıklaması boş geçilemez.");

            RuleFor(x => x.ImageFile).NotNull().WithMessage("Lütfen bir proje görseli seçiniz.");
        }
    }
}