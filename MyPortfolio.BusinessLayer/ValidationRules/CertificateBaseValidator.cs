using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.CertificateDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    // Create ve Update validator'larının ortak kuralları: tek yerde tanımlanır ki
    // bir kural değişince (örn. açıklama uzunluğu) iki dosya sessizce ayrışmasın.
    public abstract class CertificateBaseValidator<T> : AbstractValidator<T>
        where T : class, ICertificateFormDto
    {
        protected CertificateBaseValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Sertifika adı boş geçilemez.")
                                 .MaximumLength(200).WithMessage("Sertifika adı 200 karakteri aşamaz.");

            RuleFor(x => x.Issuer).NotEmpty().WithMessage("Sertifikayı veren kurum boş geçilemez.")
                                  .MaximumLength(150).WithMessage("Kurum adı 150 karakteri aşamaz.");

            RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("Lütfen en az bir kategori seçiniz.");

            RuleFor(x => x.IssueDate).NotEmpty().WithMessage("Sertifika tarihi boş geçilemez.")
                                     .LessThanOrEqualTo(_ => DateTime.Today.AddDays(1))
                                     .WithMessage("Sertifika tarihi gelecekte bir gün olamaz.");

            RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Açıklama 1000 karakteri aşamaz.");

            // Doğrulama linki girildiyse http(s) ile başlayan geçerli bir adres olmalı
            RuleFor(x => x.CredentialUrl)
                .Must(url => string.IsNullOrWhiteSpace(url)
                             || url.StartsWith("http://") || url.StartsWith("https://"))
                .WithMessage("Doğrulama linki http:// veya https:// ile başlamalıdır.");
        }
    }
}
