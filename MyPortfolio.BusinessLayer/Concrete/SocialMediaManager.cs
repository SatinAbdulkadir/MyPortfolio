using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class SocialMediaManager : ISocialMediaService
    {
        private readonly IGenericDal<SocialMedia> _socialMediaDal;

        public SocialMediaManager(IGenericDal<SocialMedia> socialMediaDal)
        {
            _socialMediaDal = socialMediaDal;
        }

        public async Task<List<ResultSocialMediaDto>> TGetSocialMediaListAsync()
        {
            var values = await _socialMediaDal.GetListAsync();
            return values.Select(x => new ResultSocialMediaDto
            {
                SocialMediaId = x.Id,
                Title = x.Title,
                Url = x.Url,
                Icon = x.Icon
            }).ToList();
        }
    }
}