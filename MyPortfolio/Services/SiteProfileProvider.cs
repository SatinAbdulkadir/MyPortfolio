using Microsoft.Extensions.Caching.Memory;
using MyPortfolio.BusinessLayer.Abstract;

namespace MyPortfolio.WebUI.Services
{
    // Site sahibinin SEO'da kullanılan bilgileri: ad, unvan, kısa tanıtım, sosyal profiller.
    public record SiteProfile(string Name, string JobTitle, string Summary, IReadOnlyList<string> SameAs);

    // Bilgiler sabit yazılmaz, veritabanından gelir (Vitrin Ayarları, Hakkımda, Sosyal Medya):
    // admin panelden adını/unvanını değiştirince başlıklar ve paylaşım kartları da değişir.
    // Her public sayfanın <head>'i bunu kullandığı için önbellekte tutulur; anahtar ContentCacheVersion'a
    // bağlı olduğundan admin bir şey kaydettiği anda tazelenir (ana sayfa önbelleğiyle aynı mekanizma).
    public class SiteProfileProvider
    {
        private readonly IFeatureService _featureService;
        private readonly IAboutService _aboutService;
        private readonly ISocialMediaService _socialMediaService;
        private readonly IMemoryCache _cache;
        private readonly ContentCacheVersion _cacheVersion;

        public SiteProfileProvider(IFeatureService featureService,
                                   IAboutService aboutService,
                                   ISocialMediaService socialMediaService,
                                   IMemoryCache cache,
                                   ContentCacheVersion cacheVersion)
        {
            _featureService = featureService;
            _aboutService = aboutService;
            _socialMediaService = socialMediaService;
            _cache = cache;
            _cacheVersion = cacheVersion;
        }

        public async Task<SiteProfile> GetAsync()
        {
            var key = "seo:site-profile:" + _cacheVersion.Current;

            return (await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);

                // Kayıtlar henüz girilmemişse (boş veritabanı) makul varsayılanlara düşülür
                var feature = await _featureService.GetFeatureForBannerAsync();
                var about = await _aboutService.TGetAboutAsync();
                var socials = await _socialMediaService.TGetSocialMediaListAsync();

                var name = feature?.Title?.Trim() is { Length: > 0 } n ? n : "Abdulkadir Satin";
                var jobTitle = feature?.Description?.Trim() is { Length: > 0 } j ? j : "Developer";
                var summary = about?.SubDescription?.Trim() ?? string.Empty;

                var sameAs = (socials ?? new())
                    .Select(s => s.Url)
                    .Where(u => u.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return new SiteProfile(name, jobTitle, summary, sameAs);
            }))!;
        }
    }
}
