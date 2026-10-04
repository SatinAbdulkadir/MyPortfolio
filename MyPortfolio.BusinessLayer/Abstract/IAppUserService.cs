using Microsoft.AspNetCore.Identity;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IAppUserService
    {
        Task<EditProfileDto> GetUserForEditAsync(string userName);

        // IdentityResult döner: başarısızlıkta neden (yanlış mevcut şifre, zayıf yeni şifre vb.)
        // Identity'nin kendi hata listesiyle panele taşınır. Eskiden sadece true/false dönüyordu.
        Task<IdentityResult> UpdateUserProfileAsync(EditProfileDto editProfileDto, string userName);
    }
}
