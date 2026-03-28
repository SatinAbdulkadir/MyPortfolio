using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.ContactDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class ContactManager : IContactService
    {
        private readonly IGenericDal<Contact> _contactDal;
        private readonly IMapper _mapper; // Mapper enjekte edildi

        public ContactManager(IGenericDal<Contact> contactDal, IMapper mapper)
        {
            _contactDal = contactDal;
            _mapper = mapper;
        }

        public async Task<ResultContactDto> TGetContactAsync()
        {
            var values = await _contactDal.GetListAsync();
            var data = values.FirstOrDefault(); // Sadece ilk veriyi alıyoruz
            return _mapper.Map<ResultContactDto>(data);
        }
        




        public async Task TUpdateContactAsync(UpdateContactDto updateContactDto)
        {
            // Veritabanındaki tek kaydı alıyoruz
            var values = await _contactDal.GetListAsync();
            var existingData = values.FirstOrDefault();

            if (existingData != null)
            {
                // Kurumsal Mapping: DTO'yu mevcut Entity üzerine bindir
                _mapper.Map(updateContactDto, existingData);
                await _contactDal.UpdateAsync(existingData);
            }
        }
    }
}