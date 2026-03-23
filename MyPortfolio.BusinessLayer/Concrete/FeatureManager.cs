using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class FeatureManager : IFeatureService
    {
        private readonly IGenericDal<Feature> _featureDal;
        private readonly IMapper _mapper; // AutoMapper enjekte edildi

        public FeatureManager(IGenericDal<Feature> featureDal, IMapper mapper)
        {
            _featureDal = featureDal;
            _mapper = mapper;
        }

        public async Task<ResultFeatureDto> GetFeatureForBannerAsync()
        {
            var values = await _featureDal.GetListAsync();
            var data = values.FirstOrDefault();

            // ESKİ: Manuel atama ameleliği
            // YENİ: Tek satır kurumsal dönüşüm
            return _mapper.Map<ResultFeatureDto>(data);
        }

        public async Task TUpdateFeatureAsync(ResultFeatureDto resultFeatureDto)
        {
            // Admin paneline gelince burayı da _mapper.Map ile tek satırda halledeceğiz reis.
            throw new NotImplementedException();
        }
    }
}