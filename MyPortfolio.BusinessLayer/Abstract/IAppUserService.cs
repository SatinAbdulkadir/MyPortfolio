using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;
using MyPortfolio.EntityLayer.Concrete;
using MyPortfolio.BusinessLayer.Abstract;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IAppUserService
    {
        
        Task<EditProfileDto> GetUserForEditAsync(string userName);

        
        Task<bool> UpdateUserProfileAsync(EditProfileDto editProfileDto, string userName);
    }
}
