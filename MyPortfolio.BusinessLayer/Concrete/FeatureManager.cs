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

        public async Task TUpdateFeatureAsync(UpdateFeatureDto updateFeatureDto)
        {
            // Önce tabloda herhangi bir veri var mı ona bakıyoruz (Id'den bağımsız)
            var values = await _featureDal.GetListAsync();
            var existingData = values.FirstOrDefault();

            if (existingData != null)
            {
                // 1. DURUM: Veri varsa güncelle
                // AutoMapper sayesinde Id hariç her şeyi üstüne yazar
                _mapper.Map(updateFeatureDto, existingData);
                await _featureDal.UpdateAsync(existingData);
            }
            else
            {
                // 2. DURUM: Tablo bomboşsa hata verme, yenisini oluştur!
                // Bu sayede "güncelleme yapmıyor" derdinden kurtuluruz
                var newFeature = _mapper.Map<Feature>(updateFeatureDto);
                await _featureDal.InsertAsync(newFeature);
            }
        }


    }
}