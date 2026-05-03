using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.MessageDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IGenericDal<Message> _messageDal;
        private readonly IMapper _mapper;

        public MessageManager(IGenericDal<Message> messageDal, IMapper mapper)
        {
            _messageDal = messageDal;
            _mapper = mapper;
        }

        // Metodun adını veritabanı işlemine uygun hale getirdik.
        // IMessageService içindeki imzanı da "AddMessageAsync" olarak güncellemelisin.
        public async Task AddMessageAsync(CreateMessageDto dto)
        {
            var messageEntity = _mapper.Map<Message>(dto);
            await _messageDal.InsertAsync(messageEntity);
        }
    }
}