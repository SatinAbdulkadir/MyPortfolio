using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.ContactDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class ContactValidator : AbstractValidator<UpdateContactDto>
    {
        public ContactValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Sayfa başlığı boş geçilemez.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş geçilemez.");

            RuleFor(x => x.Email1).NotEmpty().WithMessage("Birinci e-posta adresi zorunludur.")
                                  .EmailAddress().WithMessage("Geçerli bir e-posta formatı giriniz.");

            
            RuleFor(x => x.Address).NotEmpty().WithMessage("Açık adres alanı boş geçilemez.");
        }
    }
}