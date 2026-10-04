using MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ICertificateCategoryService
    {
        // Ağaç sırasında düz liste: her ana kategoriyi hemen ardından kendi alt kategorileri izler.
        // İkisi de kendi içinde DisplayOrder'a göre sıralı; sertifika ve alt kategori sayıları dolu.
        Task<List<ResultCertificateCategoryDto>> TGetCategoryListAsync();
        Task<UpdateCertificateCategoryDto?> TGetByIdAsync(int id);
        Task TCreateCategoryAsync(CreateCertificateCategoryDto createDto);
        Task TUpdateCategoryAsync(UpdateCertificateCategoryDto updateDto);

        // İki seviye kuralı: üst kategori mevcut bir ana kategori olmalı, kendisi olamaz,
        // alt kategorisi olan bir kategori başka birinin altına taşınamaz.
        // Kural ihlalinde hata mesajı, geçerliyse null döner. categoryId yeni kayıtta null.
        Task<string?> TValidateParentAsync(int? categoryId, int? parentId);

        // Kategoriye bağlı sertifika ya da alt kategori varsa silmez ve false döner
        Task<bool> TDeleteCategoryAsync(int id);
    }
}
