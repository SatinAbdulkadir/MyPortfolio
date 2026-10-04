using Microsoft.AspNetCore.Identity;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class AppUserManager : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public AppUserManager(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userName, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return IdentityResult.Failed(_userManager.ErrorDescriber.DefaultError());

            // Eskiden PasswordHasher ile hash elle yazılıyordu: Identity'nin şifre kuralları
            // (8 karakter, rakam, sembol...) atlanıyor ve security stamp yenilenmiyordu,
            // yani şifre değişse de diğer cihazlardaki eski oturumlar açık kalıyordu.
            // ChangePasswordAsync mevcut şifreyi doğrular, kuralları uygular ve stamp'i yeniler.
            return await _userManager.ChangePasswordAsync(user, dto.CurrentPassword ?? "", dto.NewPassword ?? "");
        }

        public async Task<IdentityResult> ChangeUserNameAsync(string userName, ChangeUserNameDto dto)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return IdentityResult.Failed(_userManager.ErrorDescriber.DefaultError());

            if (!await _userManager.CheckPasswordAsync(user, dto.CurrentPassword ?? ""))
            {
                return IdentityResult.Failed(_userManager.ErrorDescriber.PasswordMismatch());
            }

            var newUserName = (dto.NewUserName ?? "").Trim();

            // SetUserNameAsync izin verilen karakterleri ve başka hesapta kullanılıp kullanılmadığını
            // denetler, normalize edilmiş adı ve security stamp'i de günceller
            return await _userManager.SetUserNameAsync(user, newUserName);
        }
    }
}
