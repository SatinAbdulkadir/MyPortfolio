using System.Collections.Concurrent;

namespace MyPortfolio.WebUI.Middlewares
{
    // Basit "sabit pencere" (fixed window) rate limiter.
    // Mantık: Her IP için sayaç tutulur. Pencere süresi (örn. 1 dk) dolunca sayaç sıfırlanır.
    // Pencere içinde limit aşılırsa istek 429 (Too Many Requests) ile reddedilir.
    // Sadece aşağıdaki Rules listesindeki POST endpoint'leri sınırlanır; sitenin geri kalanı etkilenmez.
    public class SimpleRateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        private sealed record RateRule(string PathPrefix, int PermitLimit, TimeSpan Window);

        private static readonly RateRule[] Rules =
        {
            // İletişim formu: IP başına dakikada 5 mesaj denemesi
            new("/Home/SendMessage", 5, TimeSpan.FromMinutes(1)),
            // Login: IP başına 5 dakikada 10 giriş denemesi (hesap kilidine ek katman)
            new("/Login", 10, TimeSpan.FromMinutes(5))
        };

        private sealed class Counter
        {
            public int Count;
            public DateTime WindowStart;
        }

        // Uygulama ömrü boyunca yaşayan sayaç tablosu: anahtar = "kural|IP"
        private static readonly ConcurrentDictionary<string, Counter> Counters = new();

        // Temizlik taraması en fazla dakikada bir çalışsın diye zaman kapısı
        private static DateTime _lastCleanupUtc = DateTime.MinValue;

        public SimpleRateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Cloudflare/reverse proxy arkasında RemoteIpAddress herkes için proxy'nin IP'si olur;
        // gerçek ziyaretçi IP'si header'da gelir. Aksi halde 6. masum ziyaretçi 429 yerdi.
        private static string ResolveClientIp(HttpContext context)
        {
            var cfIp = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(cfIp)) return cfIp;

            var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwarded)) return forwarded.Split(',')[0].Trim();

            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (HttpMethods.IsPost(context.Request.Method))
            {
                var rule = Rules.FirstOrDefault(r => context.Request.Path.StartsWithSegments(r.PathPrefix));

                if (rule != null)
                {
                    var ip = ResolveClientIp(context);
                    var key = $"{rule.PathPrefix}|{ip}";
                    var now = DateTime.UtcNow;

                    var counter = Counters.GetOrAdd(key, _ => new Counter { Count = 0, WindowStart = now });

                    bool rejected;
                    lock (counter)
                    {
                        // Pencere süresi dolduysa sayacı sıfırla, yeni pencere başlat
                        if (now - counter.WindowStart >= rule.Window)
                        {
                            counter.Count = 0;
                            counter.WindowStart = now;
                        }

                        counter.Count++;
                        rejected = counter.Count > rule.PermitLimit;
                    }

                    if (rejected)
                    {
                        // Boş gövdeli 429 döner; UseStatusCodePagesWithReExecute bunu temalı sayfaya çevirir
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        return;
                    }

                    // Bellek temizliği: tablo şiştiyse VE son taramadan 1+ dk geçtiyse
                    // süresi geçmiş pencereleri at (her istekte tam tarama yapılmasın)
                    if (Counters.Count > 1000 && now - _lastCleanupUtc >= TimeSpan.FromMinutes(1))
                    {
                        _lastCleanupUtc = now;
                        foreach (var entry in Counters)
                        {
                            if (now - entry.Value.WindowStart >= TimeSpan.FromMinutes(10))
                            {
                                Counters.TryRemove(entry.Key, out _);
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
