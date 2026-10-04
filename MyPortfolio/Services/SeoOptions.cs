namespace MyPortfolio.WebUI.Services
{
    // SEO ayarları. appsettings'te "Seo" bölümüyle ezilebilir; yoksa buradaki varsayılan kullanılır.
    public class SeoOptions
    {
        // Canonical, Open Graph, sitemap gibi mutlak adres gereken her yerde kullanılan ana adres.
        // www'siz ve https: site www'yi buraya yönlendirir (bkz. Program.cs), Google tek adres görür.
        public string BaseUrl { get; set; } = "https://abdulkadirsatin.com.tr";

        // Sayfaya özel görsel yoksa paylaşım kartında (LinkedIn, WhatsApp...) gösterilen görsel
        public string DefaultImagePath { get; set; } = "/images/og-default.png";

        public string Absolute(string path)
            => path.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? path : BaseUrl.TrimEnd('/') + "/" + path.TrimStart('/');
    }
}
