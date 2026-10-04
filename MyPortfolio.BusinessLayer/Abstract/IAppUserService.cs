using Microsoft.AspNetCore.Identity;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    // Hesap güvenliği işlemleri. IdentityResult döner: başarısızlıkta neden (yanlış mevcut şifre,
    // zayıf yeni şifre, geçersiz kullanıcı adı vb.) Identity'nin kendi hata listesiyle panele taşınır.
    public interface IAppUserService
    {
        Task<IdentityResult> ChangePasswordAsync(string userName, ChangePasswordDto dto);
        Task<IdentityResult> ChangeUserNameAsync(string userName, ChangeUserNameDto dto);
    }
}
