namespace MyPortfolio.BusinessLayer.Helpers
{
    // Video linki çözümleme mantığı: view içinde @functions olarak durması yerine
    // burada yaşar ki test edilebilsin ve başka yerlerden (admin önizleme vb.) kullanılabilsin.
    public static class VideoUrlHelper
    {
        // watch?v=, youtu.be/, shorts/, live/, embed/ ve v/ formatlarını tanır;
        // host karşılaştırması büyük/küçük harf duyarsızdır (YouTube.com da geçerli).
        public static string? GetYouTubeEmbedUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return null;

            string host = uri.Host.ToLowerInvariant();
            bool isYouTube = host == "youtube.com" || host.EndsWith(".youtube.com")
                          || host == "youtu.be"
                          || host == "youtube-nocookie.com" || host.EndsWith(".youtube-nocookie.com");
            if (!isYouTube) return null;

            string? videoId = null;
            var segments = uri.AbsolutePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (host == "youtu.be")
            {
                if (segments.Length >= 1) videoId = segments[0];
            }
            else if (segments.Length >= 2 && (segments[0] is "shorts" or "embed" or "live" or "v"))
            {
                videoId = segments[1];
            }
            else
            {
                // watch?v=ID — v parametresi sorgunun herhangi bir sırasında olabilir
                foreach (var param in uri.Query.TrimStart('?').Split('&'))
                {
                    if (param.StartsWith("v=")) { videoId = param[2..]; break; }
                }
            }

            return string.IsNullOrWhiteSpace(videoId)
                ? null
                : $"https://www.youtube-nocookie.com/embed/{videoId}";
        }

        // Site içi yol (/videos/x.mp4) veya doğrudan medya dosyası linki mi?
        public static bool IsDirectVideoFile(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            return url.StartsWith("/")
                || url.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase)
                || url.EndsWith(".webm", StringComparison.OrdinalIgnoreCase);
        }
    }
}
