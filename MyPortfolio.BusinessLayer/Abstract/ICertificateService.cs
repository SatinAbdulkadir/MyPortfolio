using MyPortfolio.BusinessLayer.Dtos.CertificateDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ICertificateService
    {
        // Tümü, tarihe göre yeniden eskiye sıralı; kategori adı/ikonu birleştirilmiş
        Task<List<ResultCertificateDto>> TGetCertificateListAsync();

        // Ana sayfadaki "öne çıkanlar" bölümü için işaretli olanlar
        Task<List<ResultCertificateDto>> TGetFeaturedCertificateListAsync(int take = 6);

        Task<ResultCertificateDto?> TGetCertificateByIdAsync(int id);
        Task<UpdateCertificateDto?> TGetByIdAsync(int id);
        Task TCreateCertificateAsync(CreateCertificateDto createDto);
        Task TUpdateCertificateAsync(UpdateCertificateDto updateDto);

        // Kaydı siler, varsa yüklenmiş dosyayı da diskten temizler
        Task TDeleteCertificateAsync(int id);

        // Kategori silinmeden önce "dolu mu" kontrolü için
        Task<int> TCountByCategoryAsync(int categoryId);
    }
}
