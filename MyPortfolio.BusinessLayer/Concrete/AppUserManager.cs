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

        public async Task<bool> UpdateUserProfileAsync(EditProfileDto editProfileDto, string userName)
        {
            // userName null ise zaten işlem yapamayız
            if (string.IsNullOrEmpty(userName)) return false;

            var user = await _userManager.FindByNameAsync(userName);

            // Eğer kullanıcı veritabanında bulunamazsa (null ise)
            if (user == null) return false;

            // Şifre kontrolü yaparken 'editProfileDto.CurrentPassword' ün null olmadığını garanti et
            if (!string.IsNullOrEmpty(editProfileDto.Password))
            {
                var currentPassword = editProfileDto.CurrentPassword ?? ""; // null ise boş string yap
                var checkPassword = await _userManager.CheckPasswordAsync(user, currentPassword);

                if (!checkPassword) return false;

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, editProfileDto.Password);
            }

            _mapper.Map(editProfileDto, user);
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}