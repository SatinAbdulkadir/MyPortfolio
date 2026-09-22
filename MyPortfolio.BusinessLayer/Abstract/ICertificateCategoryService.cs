using MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ICertificateCategoryService
    {
        // DisplayOrder'a göre sıralı döner; her kategorinin sertifika sayısı doldurulur
        Task<List<ResultCertificateCategoryDto>> TGetCategoryListAsync();
        Task<UpdateCertificateCategoryDto?> TGetByIdAsync(int id);
        Task TCreateCategoryAsync(CreateCertificateCategoryDto createDto);
        Task TUpdateCategoryAsync(UpdateCertificateCategoryDto updateDto);

        // Kategoriye bağlı sertifika varsa silmez ve false döner (sertifikalar sahipsiz kalmasın)
        Task<bool> TDeleteCategoryAsync(int id);
    }
}
