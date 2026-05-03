using MyPortfolio.BusinessLayer.Dtos.MessageDtos;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestDto mailRequestDto);
    }
}