using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MyPortfolio.BusinessLayer.Dtos.CertificateDtos
{
    public class CreateCertificateDto : ICertificateFormDto
    {
        public required string Title { get; set; }
        public required string Issuer { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Today;
        public string? Description { get; set; }
        public string? CredentialUrl { get; set; }
        public bool IsFeatured { get; set; }

        // Formdaki onay kutularından gelir; sertifika seçilen her kategoride görünür
        public List<int> CategoryIds { get; set; } = new();

        // Yükleme sonucu doldurulur (bkz. FileImageHelper); form üzerinden gelmez
        public string? FileUrl { get; set; }
        public string? FileType { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? CertificateFile { get; set; }
    }
}
