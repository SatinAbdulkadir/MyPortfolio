using MyPortfolio.BusinessLayer.Dtos.MessageDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IMessageService
    {
        Task AddMessageAsync(CreateMessageDto dto);
    }
}