using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ISocialMediaService
    {
        Task<List<ResultSocialMediaDto>> TGetSocialMediaListAsync();
        Task TCreateSocialMediaAsync(CreateSocialMediaDto createSocialMediaDto);
        Task TUpdateSocialMediaAsync(UpdateSocialMediaDto updateSocialMediaDto);
        Task TDeleteSocialMediaAsync(int id);
        Task<UpdateSocialMediaDto> TGetByIdAsync(int id);
    }
}