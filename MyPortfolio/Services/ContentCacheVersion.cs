namespace MyPortfolio.WebUI.Services
{
    // Site içeriğinin "sürüm damgası". Cache tag helper'ları vary-by ile bu değere bağlanır:
    // admin bir şey değiştirince damga yenilenir, eski önbellek girdileri anında geçersizleşir
    // (süre dolmasını beklemeye gerek kalmaz). Singleton olarak kaydedilir.
    public class ContentCacheVersion
    {
        private long _version = DateTime.UtcNow.Ticks;

        public string Current => _version.ToString();

        public void Bump() => Interlocked.Exchange(ref _version, DateTime.UtcNow.Ticks);
    }
}
