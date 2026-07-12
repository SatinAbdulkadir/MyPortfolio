using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    // Ortak kurallar PortfolioDetailBaseValidator'dan gelir; buraya sadece Update'e özgü kural eklenir
    public class UpdatePortfolioDetailValidator : PortfolioDetailBaseValidator<UpdatePortfolioDetailDto>
    {
        public UpdatePortfolioDetailValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Geçerli bir detay kaydı gereklidir.");
        }
    }
}
