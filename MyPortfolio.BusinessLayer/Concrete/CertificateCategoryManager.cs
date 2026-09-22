using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class CertificateCategoryManager : ICertificateCategoryService
    {
        private readonly IGenericDal<CertificateCategory> _categoryDal;
        private readonly IGenericDal<Certificate> _certificateDal;
        private readonly IMapper _mapper;

        public CertificateCategoryManager(IGenericDal<CertificateCategory> categoryDal,
                                          IGenericDal<Certificate> certificateDal,
                                          IMapper mapper)
        {
            _categoryDal = categoryDal;
            _certificateDal = certificateDal;
            _mapper = mapper;
        }

        public async Task<List<ResultCertificateCategoryDto>> TGetCategoryListAsync()
        {
            var categories = await _categoryDal.GetListAsync();
            var certificates = await _certificateDal.GetListAsync();

            var result = _mapper.Map<List<ResultCertificateCategoryDto>>(categories);

            foreach (var item in result)
            {
                item.CertificateCount = certificates.Count(c => c.CategoryId == item.Id);
            }

            // Önce elle verilen sıra, eşitlikte alfabetik
            return result.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name).ToList();
        }

        public async Task<UpdateCertificateCategoryDto?> TGetByIdAsync(int id)
        {
            var value = await _categoryDal.GetByIdAsync(id);
            return value == null ? null : _mapper.Map<UpdateCertificateCategoryDto>(value);
        }

        public async Task TCreateCategoryAsync(CreateCertificateCategoryDto createDto)
        {
            var value = _mapper.Map<CertificateCategory>(createDto);
            await _categoryDal.InsertAsync(value);
        }

        public async Task TUpdateCategoryAsync(UpdateCertificateCategoryDto updateDto)
        {
            var existingData = await _categoryDal.GetByIdAsync(updateDto.Id);
            if (existingData != null)
            {
                _mapper.Map(updateDto, existingData);
                await _categoryDal.UpdateAsync(existingData);
            }
        }

        public async Task<bool> TDeleteCategoryAsync(int id)
        {
            var value = await _categoryDal.GetByIdAsync(id);
            if (value == null) return false;

            // Dolu kategori silinirse sertifikalar hiçbir grupta görünmez hale gelirdi;
            // bu yüzden önce taşınmaları/silinmeleri isteniyor
            var linked = await _certificateDal.GetByFilterAsync(x => x.CategoryId == id);
            if (linked.Count > 0) return false;

            await _categoryDal.DeleteAsync(value);
            return true;
        }
    }
}
