using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;
using MyPortfolio.EntityLayer.Concrete;
using MyPortfolio.BusinessLayer.Abstract;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IAppUserService
    {
        // Profil bilgilerini ekrana getirmek için (Get)
        Task<EditProfileDto> GetUserForEditAsync(string userName);

        // Profil bilgilerini güncellemek için (Post)
        Task<bool> UpdateUserProfileAsync(EditProfileDto editProfileDto, string userName);
    }
}
