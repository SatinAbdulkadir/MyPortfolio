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
        private readonly IGenericDal<CertificateCategoryLink> _linkDal;
        private readonly IMapper _mapper;

        public CertificateCategoryManager(IGenericDal<CertificateCategory> categoryDal,
                                          IGenericDal<CertificateCategoryLink> linkDal,
                                          IMapper mapper)
        {
            _categoryDal = categoryDal;
            _linkDal = linkDal;
            _mapper = mapper;
        }

        public async Task<List<ResultCertificateCategoryDto>> TGetCategoryListAsync()
        {
            var categories = await _categoryDal.GetListAsync();
            var links = await _linkDal.GetListAsync();

            var all = _mapper.Map<List<ResultCertificateCategoryDto>>(categories);
            var byId = all.ToDictionary(x => x.Id);

            foreach (var item in all)
            {
                item.CertificateCount = links.Count(l => l.CategoryId == item.Id);
                item.ChildCount = all.Count(c => c.ParentId == item.Id);

                if (item.ParentId.HasValue && byId.TryGetValue(item.ParentId.Value, out var parent))
                {
                    item.ParentName = parent.Name;
                }
            }

            // Ağaç sırası: ana kategori, ardından onun alt kategorileri
            var result = new List<ResultCertificateCategoryDto>();
            foreach (var main in SortForDisplay(all.Where(x => x.IsMain)))
            {
                result.Add(main);
                result.AddRange(SortForDisplay(all.Where(x => x.ParentId == main.Id)));
            }

            return result;
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

        public async Task<string?> TValidateParentAsync(int? categoryId, int? parentId)
        {
            // Ana kategori olarak kaydediliyor: her zaman geçerli
            if (!parentId.HasValue) return null;

            if (categoryId.HasValue && parentId.Value == categoryId.Value)
                return "Bir kategori kendisinin üst kategorisi olamaz.";

            var parent = await _categoryDal.GetByIdAsync(parentId.Value);
            if (parent == null)
                return "Seçilen üst kategori bulunamadı.";

            // Derinlik 2: üst kategori de bir alt kategoriyse üçüncü seviye oluşurdu
            if (parent.ParentId.HasValue)
                return "Alt kategorinin altına kategori eklenemez; sadece ana kategoriler üst kategori olabilir.";

            // Alt kategorisi olan bir ana kategori başkasının altına taşınırsa
            // onun alt kategorileri üçüncü seviyeye düşerdi
            if (categoryId.HasValue)
            {
                var children = await _categoryDal.GetByFilterAsync(x => x.ParentId == categoryId.Value);
                if (children.Count > 0)
                    return "Bu kategorinin alt kategorileri var; başka bir kategorinin altına taşınamaz.";
            }

            return null;
        }

        public async Task<bool> TDeleteCategoryAsync(int id)
        {
            var value = await _categoryDal.GetByIdAsync(id);
            if (value == null) return false;

            // Dolu kategori silinirse bazı sertifikalar hiçbir grupta görünmez hale gelebilirdi
            var links = await _linkDal.GetByFilterAsync(x => x.CategoryId == id);
            if (links.Count > 0) return false;

            // Ana kategori silinirse alt kategorileri sahipsiz kalırdı
            var children = await _categoryDal.GetByFilterAsync(x => x.ParentId == id);
            if (children.Count > 0) return false;

            await _categoryDal.DeleteAsync(value);
            return true;
        }

        // Önce elle verilen sıra, eşitlikte alfabetik
        private static IEnumerable<ResultCertificateCategoryDto> SortForDisplay(IEnumerable<ResultCertificateCategoryDto> items)
            => items.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name);
    }
}
