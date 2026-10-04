using System.Security.Cryptography;

namespace MyPortfolio.WebUI.Security
{
    // İçerik Güvenlik Politikası (CSP): tarayıcıya sayfanın hangi kaynaklardan script, stil, font,
    // görsel ve iframe yükleyebileceğini söyler. Sayfaya bir şekilde zararlı script sokulsa bile (XSS)
    // tarayıcı listede olmayan kaynağı ya da nonce'u olmayan inline script'i çalıştırmaz.
    //
    // Yeni bir CDN/servis eklenirse buraya da eklenmeli, yoksa tarayıcı onu engeller
    // (tarayıcı konsolunda "Refused to load ... Content Security Policy" uyarısı görünür).
    public static class ContentSecurityPolicy
    {
        private const string NonceKey = "csp-nonce";

        // Her isteğe yeni, tahmin edilemez bir nonce. Sadece bu değeri taşıyan inline <script>'ler çalışır;
        // view'larda: <script nonce="@Context.GetCspNonce()">
        public static string CreateNonce(HttpContext context)
        {
            // Hex (0-9, A-F): Base64'teki "+" karakterini Razor HTML niteliğinde "&#x2B;" diye kodluyordu.
            // Tarayıcı bunu çözdüğü için çalışıyordu ama hex'te hiç kodlama olmaz; 128 bit rastgelelik aynı.
            var nonce = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
            context.Items[NonceKey] = nonce;
            return nonce;
        }

        public static string GetCspNonce(this HttpContext context)
            => context.Items[NonceKey] as string ?? string.Empty;

        public static string Build(string nonce) => string.Join("; ", new[]
        {
            // Aşağıda ayrıca belirtilmeyen her şey: sadece kendi alan adımız
            "default-src 'self'",

            // Script: kendi dosyalarımız, nonce'lu inline script'ler, jsDelivr (Bootstrap, Swiper,
            // SweetAlert2) ve Cloudflare Turnstile. 'unsafe-inline' YOK: inline script'in nonce'u olmalı.
            $"script-src 'self' 'nonce-{nonce}' https://cdn.jsdelivr.net https://challenges.cloudflare.com",

            // Stil: 'unsafe-inline' bilinçli olarak açık. Sayfalarda çok sayıda style="..." var ve
            // Bootstrap/Swiper/SweetAlert2 çalışırken stil enjekte ediyor. Stil enjeksiyonunun riski
            // script'e göre çok düşük; asıl korumayı script-src sağlıyor.
            "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://unpkg.com",

            // Font: Google Fonts, Font Awesome (cdnjs), Boxicons (unpkg); data: → Swiper ikon fontu CSS'e gömülü
            "font-src 'self' data: https://fonts.gstatic.com https://cdnjs.cloudflare.com https://unpkg.com https://cdn.jsdelivr.net",

            // Görsel ve video: referans/proje görselleri ya da video linkleri dış bir https adresinden gelebilir
            "img-src 'self' data: https:",
            "media-src 'self' https:",

            // iframe: Turnstile doğrulama kutusu ve proje sayfalarındaki YouTube videoları
            "frame-src https://challenges.cloudflare.com https://www.youtube-nocookie.com https://www.youtube.com",

            // fetch/AJAX: sadece kendi sunucumuz (iletişim formu)
            "connect-src 'self'",

            // Eklenti (Flash vb.) yok; <base> etiketiyle adres kaçırma yok; formlar sadece kendi sunucumuza
            "object-src 'none'",
            "base-uri 'self'",
            "form-action 'self'",

            // Site başka sitelerin iframe'ine gömülemez (X-Frame-Options: DENY'ın modern karşılığı)
            "frame-ancestors 'none'"
        });
    }
}
