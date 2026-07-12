using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    // Create ve Update validator'larının ortak kuralları: tek yerde tanımlanır ki
    // bir kural değişince (örn. minimum uzunluk) iki dosya sessizce ayrışmasın.
    public abstract class PortfolioDetailBaseValidator<T> : AbstractValidator<T>
        where T : class, IPortfolioDetailFormDto
    {
        protected PortfolioDetailBaseValidator()
        {
            RuleFor(x => x.PortfolioId).GreaterThan(0).WithMessage("Geçerli bir proje seçilmelidir.");
            RuleFor(x => x.DetailDescription).NotEmpty().WithMessage("Detay açıklaması boş geçilemez.")
                                             .MinimumLength(20).WithMessage("Detay açıklaması en az 20 karakter olmalıdır.");
            RuleFor(x => x.Technologies).MaximumLength(500).WithMessage("Teknoloji listesi 500 karakteri aşamaz.");

            // Video linki girildiyse http(s) ile başlayan bir adres veya site içi /videos yolu olmalı
            RuleFor(x => x.VideoUrl)
                .Must(url => string.IsNullOrWhiteSpace(url)
                             || url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("/"))
                .WithMessage("Video linki geçerli bir URL olmalıdır (http, https veya site içi yol).");
        }
    }
}
