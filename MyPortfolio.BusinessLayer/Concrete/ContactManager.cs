using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.ContactDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

public class ContactManager : IContactService
{
    private readonly IGenericDal<Contact> _contactDal;
    public ContactManager(IGenericDal<Contact> contactDal) { _contactDal = contactDal; }

    public async Task<ResultContactDto> TGetContactAsync()
    {
        var values = await _contactDal.GetListAsync();
        var data = values.FirstOrDefault();
        if (data != null)
        {
            return new ResultContactDto
            {
                ContactId = data.Id, 
                Title = data.Title,
                Description = data.Description,
                Phone1 = data.Phone1,
                Phone2 = data.Phone2,
                Email1 = data.Email1,
                Email2 = data.Email2,
                Address = data.Address
            };
        }
        return null!;
    }
}