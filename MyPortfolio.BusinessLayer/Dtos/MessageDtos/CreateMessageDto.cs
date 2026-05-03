namespace MyPortfolio.BusinessLayer.Dtos.MessageDtos
{
    public class CreateMessageDto
    {
        public required string NameSurname { get; set; }
        public required string Subject { get; set; }
        public required string Email { get; set; }
        public required string MessageDetail { get; set; }
    }
}