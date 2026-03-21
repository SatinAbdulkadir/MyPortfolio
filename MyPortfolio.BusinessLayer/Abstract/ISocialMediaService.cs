using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ISocialMediaService
    {
        Task<List<ResultSocialMediaDto>> TGetSocialMediaListAsync();
    }
}