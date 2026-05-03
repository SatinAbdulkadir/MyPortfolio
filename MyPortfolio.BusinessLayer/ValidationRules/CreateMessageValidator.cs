using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.MessageDtos;

public class CreateMessageValidator : AbstractValidator<CreateMessageDto>
{
    public CreateMessageValidator()
    {
        RuleFor(x => x.NameSurname)
            .NotEmpty().WithMessage("Ad Soyad boş geçilemez.")
            .MaximumLength(100).WithMessage("Ad Soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Mail adresi boş geçilemez.")
            .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi girin.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Konu boş geçilemez.")
            .MaximumLength(100).WithMessage("Konu en fazla 100 karakter olabilir.");

        RuleFor(x => x.MessageDetail)
            .NotEmpty().WithMessage("Mesaj içeriği boş geçilemez.")
            .MaximumLength(2000).WithMessage("Mesajınız çok uzun. Lütfen en fazla 2000 karakter girin.");
    }
}