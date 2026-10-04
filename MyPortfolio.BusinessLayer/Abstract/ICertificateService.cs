using MyPortfolio.BusinessLayer.Dtos.CertificateDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ICertificateService
    {
        // Tümü, tarihe göre yeniden eskiye sıralı; bağlı kategorileri doldurulmuş
        Task<List<ResultCertificateDto>> TGetCertificateListAsync();

        // Ana sayfadaki "öne çıkanlar" bölümü için işaretli olanlar
        Task<List<ResultCertificateDto>> TGetFeaturedCertificateListAsync(int take = 6);

        Task<ResultCertificateDto?> TGetCertificateByIdAsync(int id);
        Task<UpdateCertificateDto?> TGetByIdAsync(int id);
        Task TCreateCertificateAsync(CreateCertificateDto createDto);
        Task TUpdateCertificateAsync(UpdateCertificateDto updateDto);

        // Kaydı ve kategori bağlantılarını siler, varsa yüklenmiş dosyayı ve önizlemesini de diskten temizler
        Task TDeleteCertificateAsync(int id);

        // Önizlemesi olmayan PDF'ler için önizleme üretir (özellik eklenmeden önce yüklenenler için).
        // Kaç tanesi üretildi, kaç tanesi üretilemedi döner.
        Task<(int Created, int Failed)> TGenerateMissingPreviewsAsync();
    }
}
