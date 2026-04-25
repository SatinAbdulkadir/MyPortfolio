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
        private readonly IMapper _mapper; 

        public ContactManager(IGenericDal<Contact> contactDal, IMapper mapper)
        {
            _contactDal = contactDal;
            _mapper = mapper;
        }

        public async Task<ResultContactDto> TGetContactAsync()
        {
            var values = await _contactDal.GetListAsync();
            var data = values.FirstOrDefault();
            return _mapper.Map<ResultContactDto>(data);
        }
        




        public async Task TUpdateContactAsync(UpdateContactDto updateContactDto)
        {
            
            var values = await _contactDal.GetListAsync();
            var existingData = values.FirstOrDefault();

            if (existingData != null)
            {
                
                _mapper.Map(updateContactDto, existingData);
                await _contactDal.UpdateAsync(existingData);
            }
        }
    }
}