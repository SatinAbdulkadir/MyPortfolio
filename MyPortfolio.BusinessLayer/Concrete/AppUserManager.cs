using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class AppUserManager : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AppUserManager(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<EditProfileDto> GetUserForEditAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return _mapper.Map<EditProfileDto>(user);
        }

        public async Task<IdentityResult> UpdateUserProfileAsync(EditProfileDto editProfileDto, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return IdentityResult.Failed(_userManager.ErrorDescriber.DefaultError());

            var currentPassword = editProfileDto.CurrentPassword ?? "";
            var changingPassword = !string.IsNullOrEmpty(editProfileDto.Password);

            // Mevcut şifre her değişiklikte istenir (formdaki "Değişiklikleri onaylamak için zorunludur").
            // Eskiden sadece yeni şifre girilince soruluyordu; e-posta şifresiz değiştirilebiliyordu.
            // Şifre değişiyorsa bu doğrulamayı aşağıdaki ChangePasswordAsync kendisi yapar.
            if (!changingPassword && !await _userManager.CheckPasswordAsync(user, currentPassword))
            {
                return IdentityResult.Failed(_userManager.ErrorDescriber.PasswordMismatch());
            }

            // Profil alanları (ad, soyad, e-posta, görsel) önce nesneye işlenir;
            // aşağıdaki iki çağrıdan hangisi çalışırsa kullanıcıyı bu alanlarla birlikte kaydeder
            _mapper.Map(editProfileDto, user);

            if (changingPassword)
            {
                // Eskiden PasswordHasher ile hash elle yazılıyordu: Identity'nin şifre kuralları
                // (8 karakter, rakam, sembol...) atlanıyor ve security stamp yenilenmiyordu,
                // yani şifre değişse de diğer cihazlardaki eski oturumlar açık kalıyordu.
                // ChangePasswordAsync mevcut şifreyi doğrular, kuralları uygular ve stamp'i yeniler.
                // Başarısız olursa hiçbir şey kaydedilmez (profil alanları dahil).
                return await _userManager.ChangePasswordAsync(user, currentPassword, editProfileDto.Password!);
            }

            return await _userManager.UpdateAsync(user);
        }
    }
}
