using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.CertificateDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class CertificateManager : ICertificateService
    {
        private readonly IGenericDal<Certificate> _certificateDal;
        private readonly IGenericDal<CertificateCategory> _categoryDal;
        private readonly IMapper _mapper;
        private readonly Helpers.FileImageHelper _fileImageHelper;

        public CertificateManager(IGenericDal<Certificate> certificateDal,
                                  IGenericDal<CertificateCategory> categoryDal,
                                  IMapper mapper,
                                  Helpers.FileImageHelper fileImageHelper)
        {
            _certificateDal = certificateDal;
            _categoryDal = categoryDal;
            _mapper = mapper;
            _fileImageHelper = fileImageHelper;
        }

        public async Task<List<ResultCertificateDto>> TGetCertificateListAsync()
        {
            var values = await _certificateDal.GetListAsync();
            return await MapWithCategoryAsync(values);
        }

        public async Task<List<ResultCertificateDto>> TGetFeaturedCertificateListAsync(int take = 6)
        {
            var values = await _certificateDal.GetByFilterAsync(x => x.IsFeatured);
            var result = await MapWithCategoryAsync(values);
            return result.Take(take).ToList();
        }

        public async Task<ResultCertificateDto?> TGetCertificateByIdAsync(int id)
        {
            var value = await _certificateDal.GetByIdAsync(id);
            if (value == null) return null;

            var result = await MapWithCategoryAsync(new List<Certificate> { value });
            return result.FirstOrDefault();
        }

        public async Task<UpdateCertificateDto?> TGetByIdAsync(int id)
        {
            var value = await _certificateDal.GetByIdAsync(id);
            return value == null ? null : _mapper.Map<UpdateCertificateDto>(value);
        }

        public async Task TCreateCertificateAsync(CreateCertificateDto createDto)
        {
            var value = _mapper.Map<Certificate>(createDto);
            await _certificateDal.InsertAsync(value);
        }

        public async Task TUpdateCertificateAsync(UpdateCertificateDto updateDto)
        {
            var existingData = await _certificateDal.GetByIdAsync(updateDto.Id);
            if (existingData != null)
            {
                _mapper.Map(updateDto, existingData);
                await _certificateDal.UpdateAsync(existingData);
            }
        }

        public async Task TDeleteCertificateAsync(int id)
        {
            var value = await _certificateDal.GetByIdAsync(id);
            if (value == null) return;

            // Kayıt gidince yüklenen dosya wwwroot'ta yetim kalmasın
            _fileImageHelper.DeleteFile(value.FileUrl);

            await _certificateDal.DeleteAsync(value);
        }

        public async Task<int> TCountByCategoryAsync(int categoryId)
        {
            var values = await _certificateDal.GetByFilterAsync(x => x.CategoryId == categoryId);
            return values.Count;
        }

        // Kategori adı/ikonu entity'de tutulmuyor (navigation property yok, bkz. PortfolioDetail).
        // Kategori tablosu birkaç satır olduğu için tek sorguyla çekilip bellekte eşleştiriliyor;
        // böylece her sertifika için ayrı sorgu (N+1) atılmıyor.
        private async Task<List<ResultCertificateDto>> MapWithCategoryAsync(List<Certificate> certificates)
        {
            var result = _mapper.Map<List<ResultCertificateDto>>(certificates);
            if (result.Count == 0) return result;

            var categories = (await _categoryDal.GetListAsync()).ToDictionary(x => x.Id);

            foreach (var item in result)
            {
                if (categories.TryGetValue(item.CategoryId, out var category))
                {
                    item.CategoryName = category.Name;
                    item.CategoryIcon = category.Icon;
                }
            }

            // Sertifikalar her yerde yeniden eskiye sıralı gösterilir
            return result.OrderByDescending(x => x.IssueDate).ThenByDescending(x => x.Id).ToList();
        }
    }
}
