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
        private readonly IGenericDal<CertificateCategoryLink> _linkDal;
        private readonly IMapper _mapper;
        private readonly Helpers.FileImageHelper _fileImageHelper;

        public CertificateManager(IGenericDal<Certificate> certificateDal,
                                  IGenericDal<CertificateCategory> categoryDal,
                                  IGenericDal<CertificateCategoryLink> linkDal,
                                  IMapper mapper,
                                  Helpers.FileImageHelper fileImageHelper)
        {
            _certificateDal = certificateDal;
            _categoryDal = categoryDal;
            _linkDal = linkDal;
            _mapper = mapper;
            _fileImageHelper = fileImageHelper;
        }

        public async Task<List<ResultCertificateDto>> TGetCertificateListAsync()
        {
            var values = await _certificateDal.GetListAsync();
            return await MapWithCategoriesAsync(values);
        }

        public async Task<List<ResultCertificateDto>> TGetFeaturedCertificateListAsync(int take = 6)
        {
            var values = await _certificateDal.GetByFilterAsync(x => x.IsFeatured);
            var result = await MapWithCategoriesAsync(values);
            return result.Take(take).ToList();
        }

        public async Task<ResultCertificateDto?> TGetCertificateByIdAsync(int id)
        {
            var value = await _certificateDal.GetByIdAsync(id);
            if (value == null) return null;

            var result = await MapWithCategoriesAsync(new List<Certificate> { value });
            return result.FirstOrDefault();
        }

        public async Task<UpdateCertificateDto?> TGetByIdAsync(int id)
        {
            var value = await _certificateDal.GetByIdAsync(id);
            if (value == null) return null;

            var dto = _mapper.Map<UpdateCertificateDto>(value);
            var links = await _linkDal.GetByFilterAsync(x => x.CertificateId == id);
            dto.CategoryIds = links.Select(x => x.CategoryId).ToList();
            return dto;
        }

        public async Task TCreateCertificateAsync(CreateCertificateDto createDto)
        {
            var value = _mapper.Map<Certificate>(createDto);

            // InsertAsync SaveChanges yapar; sonrasında value.Id veritabanının verdiği değerdir
            await _certificateDal.InsertAsync(value);

            await _linkDal.InsertRangeAsync(BuildLinks(value.Id, createDto.CategoryIds));
        }

        public async Task TUpdateCertificateAsync(UpdateCertificateDto updateDto)
        {
            var existingData = await _certificateDal.GetByIdAsync(updateDto.Id);
            if (existingData == null) return;

            _mapper.Map(updateDto, existingData);
            await _certificateDal.UpdateAsync(existingData);

            // Bağlantıları senkronla: sadece çıkarılanlar silinir, sadece yeni seçilenler eklenir
            var requested = updateDto.CategoryIds.Distinct().ToHashSet();
            var current = await _linkDal.GetByFilterAsync(x => x.CertificateId == updateDto.Id);

            var toRemove = current.Where(l => !requested.Contains(l.CategoryId)).ToList();
            var currentIds = current.Select(l => l.CategoryId).ToHashSet();
            var toAdd = requested.Where(categoryId => !currentIds.Contains(categoryId)).ToList();

            await _linkDal.DeleteRangeAsync(toRemove);
            await _linkDal.InsertRangeAsync(BuildLinks(updateDto.Id, toAdd));
        }

        public async Task TDeleteCertificateAsync(int id)
        {
            var value = await _certificateDal.GetByIdAsync(id);
            if (value == null) return;

            // Veritabanındaki FK da cascade ile siler; açıkça silmek niyeti kodda görünür kılar
            var links = await _linkDal.GetByFilterAsync(x => x.CertificateId == id);
            await _linkDal.DeleteRangeAsync(links);

            // Kayıt gidince yüklenen dosya diskte yetim kalmasın
            _fileImageHelper.DeleteFile(value.FileUrl);

            await _certificateDal.DeleteAsync(value);
        }

        // Formdan aynı Id iki kez gelirse ara tablodaki unique index patlamasın
        private static List<CertificateCategoryLink> BuildLinks(int certificateId, IEnumerable<int> categoryIds)
            => categoryIds.Distinct()
                          .Select(categoryId => new CertificateCategoryLink { CertificateId = certificateId, CategoryId = categoryId })
                          .ToList();

        // Kategoriler entity içinde tutulmuyor (navigation property yok, bkz. PortfolioDetail).
        // Kategori ve bağlantılar tek seferde çekilip bellekte eşleştiriliyor;
        // böylece her sertifika için ayrı sorgu (N+1) atılmıyor.
        private async Task<List<ResultCertificateDto>> MapWithCategoriesAsync(List<Certificate> certificates)
        {
            var result = _mapper.Map<List<ResultCertificateDto>>(certificates);
            if (result.Count == 0) return result;

            var certificateIds = result.Select(x => x.Id).ToList();
            var links = await _linkDal.GetByFilterAsync(x => certificateIds.Contains(x.CertificateId));
            var categories = (await _categoryDal.GetListAsync()).ToDictionary(x => x.Id);

            foreach (var item in result)
            {
                item.Categories = links
                    .Where(l => l.CertificateId == item.Id && categories.ContainsKey(l.CategoryId))
                    .Select(l => ToLabel(categories[l.CategoryId], categories))
                    .OrderBy(x => SortKey(x, categories))
                    .ThenBy(x => x.ParentId.HasValue ? 1 : 0)
                    .ThenBy(x => categories[x.Id].DisplayOrder)
                    .ToList();
            }

            // Sertifikalar her yerde yeniden eskiye sıralı gösterilir
            return result.OrderByDescending(x => x.IssueDate).ThenByDescending(x => x.Id).ToList();
        }

        // Etiketler sitedeki kategori sırasıyla aynı dizilsin: önce ait olduğu ana kategorinin sırası
        private static int SortKey(CertificateCategoryLabelDto label, Dictionary<int, CertificateCategory> all)
        {
            int mainId = label.ParentId ?? label.Id;
            return all.TryGetValue(mainId, out var main) ? main.DisplayOrder : int.MaxValue;
        }

        private static CertificateCategoryLabelDto ToLabel(CertificateCategory category, Dictionary<int, CertificateCategory> all)
        {
            string? parentName = null;
            if (category.ParentId.HasValue && all.TryGetValue(category.ParentId.Value, out var parent))
            {
                parentName = parent.Name;
            }

            return new CertificateCategoryLabelDto
            {
                Id = category.Id,
                Name = category.Name,
                Icon = category.Icon,
                ParentId = category.ParentId,
                ParentName = parentName
            };
        }
    }
}
