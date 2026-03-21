using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class FeatureManager : IFeatureService
    {
        private readonly IGenericDal<Feature> _featureDal;

        // DI: DataAccess katmanındaki şablonu buraya enjekte ediyoruz
        public FeatureManager(IGenericDal<Feature> featureDal)
        {
            _featureDal = featureDal;
        }

        public async Task<ResultFeatureDto> GetFeatureForBannerAsync()
        {
            // Veriyi DataAccess'ten çekiyoruz
            var values = await _featureDal.GetListAsync();
            var data = values.FirstOrDefault(); // Şimdilik ilk kaydı alalım

            // Manuel dönüşüm (İleride AutoMapper ile otomatize edeceğiz)
            if (data != null)
            {
                return new ResultFeatureDto
                {
                    Title = data.Title,
                    Description = data.Description
                };
            }
            
                return null!;
        }

        public Task TUpdateFeatureAsync(ResultFeatureDto resultFeatureDto)
        {
            throw new NotImplementedException(); // Admin paneline gelince yazacağız
        }
    }
}