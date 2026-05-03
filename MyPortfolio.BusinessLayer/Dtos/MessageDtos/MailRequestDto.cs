namespace MyPortfolio.BusinessLayer.Dtos.MessageDtos
{
    public class MailRequestDto
    {
        public required string NameSurname { get; set; } // Formdaki Ad Soyad
        public required string Subject { get; set; } // Formdaki Email (Reply-To için)
        public required string Email { get; set; } // Konu
        public required string MessageDetail { get; set; } // Mesaj
    }
}