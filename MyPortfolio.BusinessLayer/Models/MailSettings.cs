namespace MyPortfolio.BusinessLayer.Models
{
    public class MailSettings
    {
        public required string SenderEmail { get; set; }
        public required string Password { get; set; }
        public required string Server { get; set; }
        public required int Port { get; set; }
        public required string ReceiverEmail { get; set; } 
    }
}